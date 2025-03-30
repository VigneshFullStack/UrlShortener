using Newtonsoft.Json;
using StackExchange.Redis;
using UrlShortener.API.Models;
using UrlShortener.API.Repositories;

namespace UrlShortener.API.Services
{
    public class UrlShortenerService(
        IUrlMappingRepository repository
            //, IConnectionMultiplexer redis
            ) : IUrlShortenerService
    {
        private readonly IUrlMappingRepository _repository = repository;

        public async Task<List<URLMapping>> GetAllUrlsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<string> ShortenUrlAsync(string longUrl)
        {
            // Generate a unique short URL using Base62 encoding
            var id = Math.Abs(Guid.NewGuid().GetHashCode()); // Ensure id is non-negative
            var shortUrl = ConvertToBase62(id);

            var urlMapping = new URLMapping
            {
                ShortUrl = shortUrl,
                LongUrl = longUrl,
                CreatedDate = DateTime.UtcNow,
            };

            await _repository.AddAsync(urlMapping);
            await _repository.SaveChangesAsync();

            return $"{shortUrl}";
        }

        public async Task<URLMapping> GetLongUrlAsync(string shortUrl)
        {
            //// Check cache first
            //var cachedData = await _cache.StringGetAsync(shortUrl);
            //if (!cachedData.IsNullOrEmpty)
            //{
            //    return JsonConvert.DeserializeObject<URLMapping>(cachedData);
            //}

            // Fetch from Database
            var urlMapping = await _repository.GetByShortUrlAsync(shortUrl);
            //if (urlMapping != null)
            //{
            //    // cache the result
            //    await _cache.StringSetAsync(shortUrl, JsonConvert.SerializeObject(urlMapping));
            //}

            return urlMapping;
        }

        public async Task IncrementAccessCountAsync(string shortUrl)
        {
            await _repository.IncrementAccessCountAsync(shortUrl);
            await _repository.SaveChangesAsync();

            // Optionally, update cache
            //var urlMapping = _repository.GetByShortUrlAsync(shortUrl);
            //if(urlMapping != null)
            //{
            //    await _cache.StringSetAsync(shortUrl, JsonConvert.SerializeObject(urlMapping));
            //}
        }

        private static string ConvertToBase62(int number)
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var base62 = string.Empty;
            var targetBase = chars.Length;

            if (number == 0) return chars[0].ToString(); // Handle the case when the number is 0

            do
            {
                base62 = chars[number % targetBase] + base62;
                number /= targetBase;
            } while (number > 0);

            return base62;
        }
    }
}