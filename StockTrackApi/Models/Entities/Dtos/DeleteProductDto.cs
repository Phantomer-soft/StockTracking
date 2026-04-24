using System.ComponentModel.DataAnnotations;

namespace StockTrackApi.Models.Entities.Dtos
{
    public class DeleteProductDto
    {
        public Guid Id { get; set; }


        [Required]
        public string ProductName { get; set; }
    }
}
