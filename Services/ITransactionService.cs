using PartnerControlAPI.Models.DTOs;

namespace PartnerControlAPI.Services
{
    public interface ITransactionService
    {
        Task<TransactionResponse> ProcessTransactionAsync(TransactionRequest request);
    }
} 