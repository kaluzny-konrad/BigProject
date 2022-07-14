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
            _context.Products.AddRange(products);
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
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
    }
}
