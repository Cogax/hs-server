using Microsoft.EntityFrameworkCore;

namespace FinanceApi.Models
{
    public class FinanceContext : DbContext
    {
        public FinanceContext(DbContextOptions<FinanceContext> options) : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }
    }
}
