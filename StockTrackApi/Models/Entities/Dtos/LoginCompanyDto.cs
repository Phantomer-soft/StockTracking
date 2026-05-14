using System.ComponentModel.DataAnnotations;

namespace StockTrackApi.Models.Entities.Dtos
{
    public class LoginCompanyDto
    {
        [Required]
        [EmailAddress]
        public string Mail { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;
    }
}
