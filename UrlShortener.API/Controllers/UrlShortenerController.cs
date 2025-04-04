﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.API.Models;
using UrlShortener.API.Services;

namespace UrlShortener.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlShortenerController(IUrlShortenerService service) : ControllerBase
    {
        private readonly IUrlShortenerService _service = service;

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUrls()
        {
            var urls = await _service.GetAllUrlsAsync();
            return Ok(urls);
        }

        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenUrl([FromBody] UrlRequest request)
        {
            if (string.IsNullOrEmpty(request.LongUrl))
            {
                return BadRequest("LongUrl is Required.");
            }
            var shortUrl = await _service.ShortenUrlAsync(request.LongUrl);
            return Ok(new { ShortUrl = shortUrl });
        }

        [HttpGet("{shortUrl}")]
        public async Task<IActionResult> RedirectToLongUrl(string shortUrl)
        {
            var urlMapping = await _service.GetLongUrlAsync(shortUrl);
            if(urlMapping == null)
            {
                return NotFound("Short Url Not Found.");
            }

            await _service.IncrementAccessCountAsync(shortUrl);
            return Redirect(urlMapping.LongUrl);
        }
    }
}