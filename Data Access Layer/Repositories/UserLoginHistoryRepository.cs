using CoreLayer.Models;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces.IRepositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    internal class UserLoginHistoryRepository : Repository<UserLoginHistory> , IUserLoginHistoryRepository
    {

        private readonly ApplicationDbContext _context;
        public UserLoginHistoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
