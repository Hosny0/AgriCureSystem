using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using AgriCureSystem.Models;

namespace AgriCureSystem.Repositories.IRepositories
{
    public interface IProductRepository : IRepository<Product>
    {
    }
}
