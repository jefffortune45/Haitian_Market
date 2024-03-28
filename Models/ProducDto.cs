using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Haitian_Market.Models
{
    public class ProducDto
    {
        public int Id { get; set; }
        [Required,MaxLength(100)]
        public string Name { get; set; } = "";
        [Required,MaxLength(100)]
        public string Brand { get; set; } = "";
        [Required,MaxLength(100)]
        public string Category { get; set; } = "";
        [Required]

        public decimal Price { get; set; }

        public string? Description { get; set; } 
        
        public IFormFile? ImageFile { get; set; } 
    }
}
