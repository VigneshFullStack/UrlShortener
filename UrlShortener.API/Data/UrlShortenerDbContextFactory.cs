using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UrlShortener.API.Data
{
    public class UrlShortenerDbContextFactory : IDesignTimeDbContextFactory<UrlShortenerDbContext>
    {
        public UrlShortenerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UrlShortenerDbContext>();
            optionsBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Database=UrlShortenerDB;Integrated Security=True;TrustServerCertificate=True;");

            return new UrlShortenerDbContext(optionsBuilder.Options);
        }
    }
}
