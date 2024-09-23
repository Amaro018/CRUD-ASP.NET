using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Watch.Models
{
	public class ProductDto
	{
		
		[Required, MaxLength(55)]
		public string Name { get; set; } = "";

		[Required]
		public decimal Price { get; set; }

		[Required, MaxLength(255)]
		public string Description { get; set; } = "";

		[Required]
		public IFormFile? ImageFile { get; set; }
	}
}
