using AgriCureSystem.Models;

namespace AgriCureSystem.Repositories.IRepositories
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task CreateRangeAsync(List<OrderItem> orderItems);
    }
}
