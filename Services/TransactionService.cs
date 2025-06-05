using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PartnerControlAPI.Data;
using PartnerControlAPI.Models;
using PartnerControlAPI.Models.DTOs;

namespace PartnerControlAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TransactionService> _logger;
        private readonly DiscountService _discountService;

        public TransactionService(ApplicationDbContext context, ILogger<TransactionService> logger, DiscountService discountService)
        {
            _context = context;
            _logger = logger;
            _discountService = discountService;
        }

        public async Task<TransactionResponse> ProcessTransactionAsync(TransactionRequest request)
        {
            try
            {
                _logger.LogInformation("Processing transaction for partner {PartnerKey}, reference {PartnerRefNo}", 
                    request.PartnerKey, request.PartnerRefNo);

                if (!await ValidatePartnerAsync(request))
                {
                    _logger.LogWarning("Access denied for partner {PartnerKey}", request.PartnerKey);
                    return TransactionResponse.Failure("Access Denied!", "E001");
                }

                var validationResult = ValidateRequest(request);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Invalid request data for partner {PartnerKey}: {ErrorMessage}", 
                        request.PartnerKey, validationResult.ErrorMessage);
                    return TransactionResponse.Failure(validationResult.ErrorMessage, validationResult.ErrorCode);
                }

                try
                {
                    if (!ValidateSignature(request))
                    {
                        _logger.LogWarning("Invalid signature for partner {PartnerKey}", request.PartnerKey);
                        return TransactionResponse.Failure("Access Denied!", "E001");
                    }
                }
                catch (FormatException ex)
                {
                    _logger.LogError(ex, "Base64 decoding error for partner {PartnerKey}", request.PartnerKey);
                    return TransactionResponse.Failure("Invalid password format. Password must be Base64 encoded.", "E004");
                }

                var transaction = new Transaction
                {
                    PartnerKey = request.PartnerKey,
                    PartnerRefNo = request.PartnerRefNo,
                    TotalAmount = request.TotalAmount,
                    Timestamp = DateTime.Parse(request.Timestamp),
                    Signature = request.Sig,
                    Items = request.Items?.Select(i => new ItemDetail
                    {
                        PartnerItemRef = i.PartnerItemRef,
                        Name = i.Name,
                        Qty = i.Qty,
                        UnitPrice = i.UnitPrice
                    }).ToList() ?? new List<ItemDetail>()
                };

                // Calculate discount
                var (discountAmount, appliedPercentage) = await _discountService.CalculateDiscountAsync(request.TotalAmount);
                transaction.TotalDiscount = discountAmount;
                transaction.FinalAmount = request.TotalAmount - discountAmount;

                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully processed transaction for partner {PartnerKey}, reference {PartnerRefNo}, Discount: {DiscountPercentage}%", 
                    request.PartnerKey, request.PartnerRefNo, appliedPercentage);

                return TransactionResponse.Success(request.TotalAmount, discountAmount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing transaction for partner {PartnerKey}", request.PartnerKey);
                
                // Provide user-friendly error messages based on exception type
                string userMessage = ex switch
                {
                    DbUpdateException _ => "Transaction could not be processed. This might be due to a duplicate transaction reference number. Please try again with a different reference number.",
                    InvalidOperationException _ => "Transaction processing failed. Please verify your request data and try again.",
                    _ => "We're experiencing technical difficulties. Please try again in a few moments. If the problem persists, contact support."
                };

                return TransactionResponse.Failure(userMessage);
            }
        }

        private async Task<bool> ValidatePartnerAsync(TransactionRequest request)
        {
            var partner = await _context.Partners
                .FirstOrDefaultAsync(p => p.PartnerKey == request.PartnerKey && p.IsActive);

            if (partner == null)
            {
                _logger.LogWarning("Partner not found or inactive: {PartnerKey}", request.PartnerKey);
                return false;
            }

            var decodedPassword = Encoding.UTF8.GetString(Convert.FromBase64String(request.PartnerPassword));
            var isValid = partner.Password == decodedPassword;

            if (!isValid)
            {
                _logger.LogWarning("Invalid password for partner: {PartnerKey}", request.PartnerKey);
            }

            return isValid;
        }

        private (bool IsValid, string ErrorMessage, string ErrorCode) ValidateRequest(TransactionRequest request)
        {
            // Required field validations
            if (string.IsNullOrEmpty(request.PartnerKey))
                return (false, "PartnerKey is Required.", "E004");

            if (string.IsNullOrEmpty(request.PartnerRefNo))
                return (false, "PartnerRefNo is Required.", "E004");

            if (string.IsNullOrEmpty(request.PartnerPassword))
                return (false, "PartnerPassword is Required.", "E004");

            if (string.IsNullOrEmpty(request.Timestamp))
                return (false, "Timestamp is Required.", "E004");

            if (string.IsNullOrEmpty(request.Sig))
                return (false, "Signature is Required.", "E004");

            // Timestamp validation (±60 minutes)
            if (!DateTime.TryParse(request.Timestamp, out DateTime timestamp))
                return (false, "Invalid timestamp format.", "E004");

            var serverTime = DateTime.Now;
            var timeDiff = (serverTime - timestamp).TotalMinutes;
            if (Math.Abs(timeDiff) > 60)
                return (false, "Expired.", "E003");

            // Amount validations
            if (request.TotalAmount <= 0)
                return (false, "Total amount must be greater than 0.", "E002");

            if (request.Items == null || !request.Items.Any())
                return (false, "Items are Required.", "E004");

            foreach (var item in request.Items)
            {
                if (string.IsNullOrEmpty(item.PartnerItemRef))
                    return (false, "PartnerItemRef is Required for all items.", "E004");

                if (string.IsNullOrEmpty(item.Name))
                    return (false, "Item Name is Required for all items.", "E004");

                if (item.Qty <= 0 || item.Qty > 5)
                    return (false, "Item quantity must be between 1 and 5.", "E005");

                if (item.UnitPrice <= 0)
                    return (false, "Item unit price must be greater than 0.", "E005");
            }

            var calculatedTotal = request.Items.Sum(i => i.UnitPrice * i.Qty);
            if (calculatedTotal != request.TotalAmount)
            {
                return (false, $"Invalid Total Amount.", "E002");
            }

            return (true, string.Empty, string.Empty);
        }

        private bool ValidateSignature(TransactionRequest request)
        {
            var decodedPassword = Encoding.UTF8.GetString(Convert.FromBase64String(request.PartnerPassword));
            var signatureData = $"{request.Timestamp}{request.PartnerKey}{request.PartnerRefNo}{request.TotalAmount}{decodedPassword}";
            
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(signatureData));
            var calculatedSignature = Convert.ToBase64String(hash);

            return calculatedSignature == request.Sig;
        }
    }
} 