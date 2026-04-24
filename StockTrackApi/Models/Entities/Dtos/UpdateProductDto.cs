using System.ComponentModel.DataAnnotations;

namespace StockTrackApi.Models.Entities.Dtos
{
    public class UpdateProductDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur")]
        [MinLength(2, ErrorMessage = "Ürün adı 2 karakterden uzun olmalıdır.")]
        public string ProductName { get; set; }


        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0, double.MaxValue, ErrorMessage = "Fiyat negatif olamaz")]
        public decimal Price { get; set; }


        public string Explanation { get; set; }


        [Required(ErrorMessage = "Stok sayısı zorunludur.")]
        [Range(0, double.MaxValue, ErrorMessage = "Stok sayısı 0'dan küçük olamaz")]
        public int Stock { get; set; }


        public string[]? Barcode { get; set; }

        public bool IsDeleted { get; set; } = false;

        [DataType(DataType.DateTime)]
        public DateTime? LastIncomingDate { get; set; }
    }
}
