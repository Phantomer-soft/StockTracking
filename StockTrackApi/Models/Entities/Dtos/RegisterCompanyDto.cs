using System.ComponentModel.DataAnnotations;

namespace StockTrackApi.Models.Entities.Dtos
{
    public class RegisterCompanyDto
    {
        [Required(ErrorMessage ="Company name is required")]
        [MaxLength(100)]
        public string CompanyName { get; set; } =default!;

        [Required]
        [EmailAddress]
        public string Mail {  get; set; } =default!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = default!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = default!;
    }
}
