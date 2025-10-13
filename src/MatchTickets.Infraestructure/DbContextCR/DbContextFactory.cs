using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MatchTickets.Infraestructure.Data
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DbContextCR>
    {
        public DbContextCR CreateDbContext(string[] args)
        {
           
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<DbContextCR>();
            optionsBuilder.UseSqlServer(connectionString);

            return new DbContextCR(optionsBuilder.Options);
        }
    }
}




