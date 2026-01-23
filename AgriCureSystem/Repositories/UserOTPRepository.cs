using AgriCureSystem.Data;
using AgriCureSystem.Models;
using AgriCureSystem.Repositories.IRepositories;

namespace AgriCureSystem.Repositories
{
    public class UserOTPRepository : Repository<UserOTP>, IUserOTPRepository
    {
        private readonly ApplicationDbContext _context;

        public UserOTPRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
