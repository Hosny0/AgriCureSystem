using AgriCureSystem.Data;
using AgriCureSystem.Models;
using AgriCureSystem.Repositories.IRepositories;

namespace AgriCureSystem.Repositories
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        private readonly ApplicationDbContext _context;

        public BrandRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
