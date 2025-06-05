using System.ComponentModel.DataAnnotations;

namespace PartnerControlAPI.Models
{
    public enum DiscountType
    {
        Base,
        PrimeNumber,
        EndsWith5
    }

    public class DiscountSetting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DiscountType Type { get; set; }

        [Required]
        public long MinAmount { get; set; }

        [Required]
        public long MaxAmount { get; set; }

        [Required]
        [Range(0, 100)]
        public int DiscountPercentage { get; set; }

        public bool IsActive { get; set; } = true;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
} 