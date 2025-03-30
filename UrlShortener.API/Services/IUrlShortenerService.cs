using UrlShortener.API.Models;

namespace UrlShortener.API.Services
{
    public interface IUrlShortenerService
    {
        Task<List<URLMapping>> GetAllUrlsAsync();
        Task<string> ShortenUrlAsync(string longUrl);
        Task<URLMapping> GetLongUrlAsync(string shortUrl);
        Task IncrementAccessCountAsync(string shortUrl);
    }
}