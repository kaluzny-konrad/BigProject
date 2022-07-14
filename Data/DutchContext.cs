using BigProject.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BigProject.Data
{
    public class DutchContext : DbContext
    {
        private readonly IConfiguration config;

        public DutchContext(IConfiguration config)
        {
            this.config = config;
        }

        public DbSet<Product>? Products { get; set; }
        public DbSet<Order>? Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(config["ConnectionStrings:DutchContextDb"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
