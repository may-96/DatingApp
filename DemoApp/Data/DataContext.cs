using DemoApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
    }
}