using System.ComponentModel.DataAnnotations;

namespace PartnerControlAPI.Models
{
    public class Partner
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string PartnerNo { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string PartnerKey { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Password { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
} 