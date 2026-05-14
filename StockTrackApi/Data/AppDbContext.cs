using Microsoft.EntityFrameworkCore;
using StockTrackApi.Models.Entities;

namespace StockTrackApi.Data;

public class AppDbContext : DbContext
{

    public  AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Company>Companies {  get; set; }
    public DbSet<Product> Products   { get; set; }
}