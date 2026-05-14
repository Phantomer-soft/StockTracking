namespace StockTrackApi.Models.Entities.Dtos
{
    public class ProductListDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Explanation { get; set; }
        public int Stock { get; set; }
        public string[]? Barcode { get; set; }
    }
}
