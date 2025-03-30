using UrlShortener.API.Models;

namespace UrlShortener.API.Repositories
{
    public interface IUrlMappingRepository
    {
        Task<List<URLMapping>> GetAllAsync();
        Task<URLMapping> GetByShortUrlAsync(string shortUrl);
        Task AddAsync(URLMapping urlMapping);
        Task IncrementAccessCountAsync(string shortUrl);
        Task SaveChangesAsync();
    }
}