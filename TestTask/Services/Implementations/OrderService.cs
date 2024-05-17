using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext context;
        public OrderService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Order> GetOrder()
        {
            var order = await context.Orders.
                AsNoTracking().
                Where(o => o.Quantity > 1).
                OrderByDescending(o => o.CreatedAt).
                FirstOrDefaultAsync();


            if (order == null)
                throw new Exception("Order is not found");
            return order;
        }

        public async Task<List<Order>> GetOrders()
        {
            var orders = await context.Orders
                 .AsNoTracking()
                 .Where(o => o.User.Status == UserStatus.Active)
                 .OrderByDescending(o => o.CreatedAt)
                 .ToListAsync();

            return orders;
        }
    }
}
