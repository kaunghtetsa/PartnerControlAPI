using System;
using Microsoft.EntityFrameworkCore;
using PartnerControlAPI.Data;
using PartnerControlAPI.Models;

namespace PartnerControlAPI.Services
{
    public class DiscountService
    {
        private readonly ApplicationDbContext _context;
        private const decimal MAX_DISCOUNT_PERCENTAGE = 20.0m;

        public DiscountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(long discountAmount, decimal appliedPercentage)> CalculateDiscountAsync(long totalAmount)
        {
            var baseDiscount = await _context.DiscountSettings
                .Where(d => d.IsActive && 
                           d.Type == DiscountType.Base &&
                           totalAmount >= d.MinAmount && 
                           totalAmount <= d.MaxAmount)
                .FirstOrDefaultAsync();

            decimal totalDiscountPercentage = baseDiscount?.DiscountPercentage ?? 0;

            if (totalAmount > 500 && IsPrime(totalAmount))
            {
                var primeDiscount = await _context.DiscountSettings
                    .Where(d => d.IsActive && d.Type == DiscountType.PrimeNumber)
                    .FirstOrDefaultAsync();
                
                if (primeDiscount != null)
                {
                    totalDiscountPercentage += primeDiscount.DiscountPercentage;
                }
            }

            if (totalAmount > 900 && totalAmount.ToString().EndsWith("5"))
            {
                var endsWithFiveDiscount = await _context.DiscountSettings
                    .Where(d => d.IsActive && d.Type == DiscountType.EndsWith5)
                    .FirstOrDefaultAsync();
                
                if (endsWithFiveDiscount != null)
                {
                    totalDiscountPercentage += endsWithFiveDiscount.DiscountPercentage;
                }
            }

            decimal appliedPercentage = Math.Min(totalDiscountPercentage, MAX_DISCOUNT_PERCENTAGE);
            long discountAmount = (long)(totalAmount * (appliedPercentage / 100.0m));
            
            return (discountAmount, appliedPercentage);
        }

        private bool IsPrime(long number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (long)Math.Floor(Math.Sqrt(number));

            for (long i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0) return false;
            }

            return true;
        }
    }
} 