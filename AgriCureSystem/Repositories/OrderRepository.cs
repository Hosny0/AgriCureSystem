using AgriCureSystem.Data;
using AgriCureSystem.Models;
using AgriCureSystem.Repositories.IRepositories;

namespace AgriCureSystem.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
