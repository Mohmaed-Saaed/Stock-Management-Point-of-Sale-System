using CoreLayer.Models;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces.IRepositories;


namespace InfrastructureLayer.Repositories
{
    public class RolePermissionRepository : Repository<RolePermission> , IRolePermissionRepository
    {

        private readonly ApplicationDbContext _context;
        public RolePermissionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
