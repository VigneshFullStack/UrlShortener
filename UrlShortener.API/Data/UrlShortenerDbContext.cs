using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Models;

namespace UrlShortener.API.Data
{
    public class UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : DbContext(options)
    {
        public DbSet<URLMapping> URLMappings { get; set; }
    }
}