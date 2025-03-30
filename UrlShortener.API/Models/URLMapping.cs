using System.ComponentModel.DataAnnotations;

namespace UrlShortener.API.Models
{
    public class URLMapping
    {
        [Key]
        public string ShortUrl { get; set; }
        [Required]
        public string LongUrl { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int AccessCount { get; set; } = 0;
    }
}