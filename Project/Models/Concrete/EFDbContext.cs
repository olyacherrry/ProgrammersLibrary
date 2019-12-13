using System.Data.Entity;
using Project.Models.Entities;

namespace Project.Models.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}