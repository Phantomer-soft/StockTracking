namespace StockTrackApi.Models.Entities
{
    public class Company
    {
        public Guid Id { get; set; }= Guid.NewGuid();
        public string CompanyName { get; set; } = default!;
        public string Mail { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public bool Active { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
    }
}
