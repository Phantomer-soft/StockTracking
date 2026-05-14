namespace StockTrackApi.Models.Entities.Dtos
{
    public class UpdateStockDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public DateTime? LastIncomingDate { get; set; }
    }
}
