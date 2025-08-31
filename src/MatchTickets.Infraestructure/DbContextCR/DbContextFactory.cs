using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace MatchTickets.Infraestructure.Data
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DbContextCR>
    {
        public DbContextCR CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContextCR>();
            optionsBuilder.UseSqlite("Data Source=miBaseEF.db"); // o tu cadena de conexión real

            return new DbContextCR(optionsBuilder.Options);
        }
    }
}




