using BigProject.Data.Entities;
using System.Text.Json;

namespace BigProject.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _context;
        private readonly IWebHostEnvironment _environment;

        public DutchSeeder(DutchContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();

            if (!_context.Products.Any())
                CreateSampleData();
        }

        private void CreateSampleData()
        {
            var filePath = Path.Combine(_environment.ContentRootPath, "Data/art.json");
            var json = File.ReadAllText(filePath);
            var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);
            if (products is null)
                return;
            _context.Products?.AddRange(products);
            var order = new Order()
            {
                OrderDate = DateTime.Now,
                OrderNumber = "1000",
                Items = new List<OrderItem>()
                {
                    new OrderItem()
                    {
                        Product = products.First(),
                        Quantity = 5,
                        UnitPrice = products.First().Price
                    }
                }
            };
            _context.Orders?.Add(order);
            _context.SaveChanges();
        }

        public static bool IsSeeding(string[] args)
            => (args.Length == 1 && args[0].ToLower() == "/seed");

        public static void SeedDb(IServiceProvider serviceProvider)
        {
            var scopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory?.CreateScope();
            var seeder = scope?.ServiceProvider.GetService<DutchSeeder>();
            seeder?.Seed();
        }
    }
}
