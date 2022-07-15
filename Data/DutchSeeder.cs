using BigProject.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace BigProject.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext context;
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<StoreUser> userManager;

        public DutchSeeder(DutchContext context,
                           IWebHostEnvironment environment,
                           UserManager<StoreUser> userManager)
        {
            this.context = context;
            this.environment = environment;
            this.userManager = userManager;
        }

        public async Task SeedAsync()
        {
            context.Database.EnsureCreated();

            StoreUser user = await userManager.FindByEmailAsync("konrad.kaluzny@hotmail.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Konrad",
                    LastName = "Kaluzny",
                    Email = "konrad.kaluzny@hotmail.com",
                    UserName = "konrad.kaluzny@hotmail.com"
                };

                var result = await userManager.CreateAsync(user, "P@ssw0rd!");
                if (!result.Succeeded) 
                    throw new InvalidOperationException("Could not create new user in seeder");
            }

            if (!context.Products.Any())
                CreateSampleData(user);
        }

        private void CreateSampleData(StoreUser user)
        {
            var filePath = Path.Combine(environment.ContentRootPath, "Data/art.json");
            var json = File.ReadAllText(filePath);
            var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);
            if (products is null)
                return;
            context.Products?.AddRange(products);

            var order = context.Orders.Where(o => o.Id == 1).FirstOrDefault();
            if (order == null)
            {
                order = new Order()
                {
                    OrderDate = DateTime.Now,
                    OrderNumber = "1000"
                };
            }

            order.User = user;
            order.Items = new List<OrderItem>()
            {
                new OrderItem()
                {
                    Product = products.First(),
                    Quantity = 5,
                    UnitPrice = products.First().Price
                }
            };

            context.Orders?.Add(order);
            context.SaveChanges();
        }

        public static bool IsSeeding(string[] args)
            => (args.Length == 1 && args[0].ToLower() == "/seed");
    }
}
