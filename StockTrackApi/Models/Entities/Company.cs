namespace StockTrackApi.Models.Entities
{
    public class Company
    {
        public Guid Id { get; set; }= Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool Active { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
    }
}
