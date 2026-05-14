using System.ComponentModel.DataAnnotations;

namespace StockTrackApi.Models.Entities.Dtos
{
    public class RegisterProductDto
    {
        [Required]
        public string ProductName { get; set; }


        [Required]
        public decimal Price { get; set; }


        [Required]
        public int Stock { get; set; }

        public string Explanation { get; set; }

        public string? Barcode { get; set; }


    }
}
