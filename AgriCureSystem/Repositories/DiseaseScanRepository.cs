using AgriCureSystem.Data;
using AgriCureSystem.Models;
using AgriCureSystem.Repositories.IRepositories;

namespace AgriCureSystem.Repositories
{
    public class DiseaseScanRepository : Repository<DiseaseScan>, IDiseaseScanRepository
    {
        private readonly ApplicationDbContext _context;

        public DiseaseScanRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
