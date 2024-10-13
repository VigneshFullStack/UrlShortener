﻿using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Data;
using UrlShortener.API.Models;

namespace UrlShortener.API.Repositories
{
    public class UrlMappingRepository : IUrlMappingRepository
    {
        public readonly UrlShortenerDbContext _dbContext;
        public UrlMappingRepository(UrlShortenerDbContext dbContext)
        { 
            _dbContext = dbContext;
        }

        public async Task AddAsync(URLMapping urlMapping)
        {
            await _dbContext.URLMappings.AddAsync(urlMapping);
        }

        public async Task<URLMapping> GetByShortUrlAsync(string shortUrl)
        {
            return await _dbContext.URLMappings.FirstOrDefaultAsync(u => u.ShortUrl == shortUrl);
        }

        public async Task IncrementAccessCountAsync(string shortUrl)
        {
            var urlMapping = await GetByShortUrlAsync(shortUrl);
            if (urlMapping != null)
            {
                urlMapping.AccessCount += 1;
                _dbContext.URLMappings.Update(urlMapping);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}