using Microsoft.EntityFrameworkCore;

public class HistoryDbContext : DbContext
{
    public HistoryDbContext(DbContextOptions<HistoryDbContext> options)
        : base(options) { }

    public DbSet<CalculationResult> CalculationResults => Set<CalculationResult>();
}
