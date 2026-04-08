using Microsoft.EntityFrameworkCore;

public class CurrencyRates: DbContext
{
    public DbSet<Snapshot> Snapshots { get; set; }
    public DbSet<Rate> Rates { get; set; }
    public CurrencyRates()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=currency.db");
    }   
}