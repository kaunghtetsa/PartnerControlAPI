namespace PartnerControlAPI.Models.DTOs
{
    public class TransactionRequest
    {
        public string PartnerKey { get; set; } = string.Empty;
        public string PartnerRefNo { get; set; } = string.Empty;
        public string PartnerPassword { get; set; } = string.Empty;
        public long TotalAmount { get; set; }
        public List<ItemDetailRequest>? Items { get; set; }
        public string Timestamp { get; set; } = string.Empty;
        public string Sig { get; set; } = string.Empty;
    }

    public class ItemDetailRequest
    {
        public string PartnerItemRef { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Qty { get; set; }
        public long UnitPrice { get; set; }
    }
} 