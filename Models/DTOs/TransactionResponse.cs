namespace PartnerControlAPI.Models.DTOs
{
    public class TransactionResponse
    {
        public int Result { get; set; }
        public long? TotalAmount { get; set; }
        public long? TotalDiscount { get; set; }
        public long? FinalAmount { get; set; }
        public string? ResultMessage { get; set; }
        public string? ErrorCode { get; set; }

        public static TransactionResponse Success(long totalAmount, long totalDiscount = 0)
        {
            return new TransactionResponse
            {
                Result = 1,
                TotalAmount = totalAmount,
                TotalDiscount = totalDiscount,
                FinalAmount = totalAmount - totalDiscount,
                ErrorCode = null
            };
        }

        public static TransactionResponse Failure(string message, string errorCode = "ERROR")
        {
            return new TransactionResponse
            {
                Result = 0,
                ResultMessage = message,
                ErrorCode = errorCode,
                TotalAmount = null,
                TotalDiscount = null,
                FinalAmount = null
            };
        }
    }
} 