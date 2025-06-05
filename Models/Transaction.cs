using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartnerControlAPI.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string PartnerKey { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string PartnerRefNo { get; set; } = string.Empty;

        [Required]
        public long TotalAmount { get; set; }

        public long TotalDiscount { get; set; }

        public long FinalAmount { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public string Signature { get; set; } = string.Empty;

        public virtual ICollection<ItemDetail>? Items { get; set; }
    }
} 