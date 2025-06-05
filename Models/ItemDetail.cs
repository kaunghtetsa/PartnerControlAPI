using System.ComponentModel.DataAnnotations;

namespace PartnerControlAPI.Models
{
    public class ItemDetail
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string PartnerItemRef { get; set; } = string.Empty;

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public int Qty { get; set; }

        public long UnitPrice { get; set; }

        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; } = null!;
    }
} 