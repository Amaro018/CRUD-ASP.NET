using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Watch.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(55)]
        public string Name { get; set; } = "";

        [Precision(16,2)]
        public decimal Price { get; set; }

        [MaxLength(255)]
        public string Description { get; set; } = "";

        [MaxLength(255)]
        public string ImageFileName { get; set; } = "";

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
