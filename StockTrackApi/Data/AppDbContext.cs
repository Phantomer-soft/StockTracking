using Microsoft.EntityFrameworkCore;

namespace StockTrackApi.Data;

public class AppDbContext : DbContext
{
    public  AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
}