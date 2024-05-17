using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;
        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<User> GetUser()
        {
            var result = await context.Users
                .AsNoTracking()
                .Where(u => u.Orders.Any(o => o.Status == OrderStatus.Delivered))
                .Where(u => u.Orders.Any(o => o.CreatedAt.Year == 2003))
                .Select(u => new
                {
                    User = u,
                    TotalSum = u.Orders.Where(o => o.Status == OrderStatus.Delivered)
                .Where(o => o.UserId == u.Id)
                 .Sum(o => o.Price)
                })
                  .OrderByDescending(u => u.TotalSum)
                .FirstAsync();



            if (result == null)
                throw new Exception("User is not found");

            return result.User;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await context.Users.AsNoTracking().
                Where(u => u.Orders.Any(o => o.Status == OrderStatus.Paid)).
                Where(u => u.Orders.Any(o => o.CreatedAt.Year == 2010)).
                ToListAsync();

            return users;
        }
    }
}
