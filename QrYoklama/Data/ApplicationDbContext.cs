using Microsoft.EntityFrameworkCore;
using QrYoklama.Models; 

namespace QrYoklama.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Teacher> Teachers { get; set; }
    }
}