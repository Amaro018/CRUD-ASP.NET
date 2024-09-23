using Microsoft.EntityFrameworkCore;
using Watch.Models;

namespace Watch.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product>Products { get; set; }
    }
}
