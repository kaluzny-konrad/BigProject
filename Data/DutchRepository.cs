using BigProject.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BigProject.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext context;
        private readonly ILogger<DutchRepository> logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            try
            {
                if (includeItems)
                    return context.Orders.Include(o => o.Items)
                     .ThenInclude(i => i.Product)
                     .ToList();
                else
                    return context.Orders.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to get all orders: {exception}", ex);
                return null;
            }
        }

        public Order GetOrderById(int id)
        {
            try
            {
                return context.Orders.Include(o => o.Items)
                                     .ThenInclude(i => i.Product)
                                     .Where(o => o.Id == id)
                                     .FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to get all orders: {exception}", ex);
                return null;
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                return context.Products.OrderBy(p => p.Title).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to get all products: {exception}", ex);
                return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            try
            {
                return context.Products.Where(p => p.Category == category).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to get all products: {exception}", ex);
                return null;
            }
        }

        public bool SaveAll()
        {
            return context.SaveChanges() > 0;
        }

        public void AddEntity(object model)
        {
            context.Add(model);
        }
    }
}
