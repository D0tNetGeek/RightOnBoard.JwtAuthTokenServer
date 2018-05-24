using Microsoft.EntityFrameworkCore;
using RightOnBoard.JwtAuthTokenServer.Service.DbContext;
using RightOnBoard.JwtAuthTokenServer.Service.Interfaces;
using RightOnBoard.JwtAuthTokenServer.Service.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RightOnBoard.JwtAuthTokenServer.Service.Services
{
    public class RolesService : IRolesService
    {
        //private readonly IUnitOfWork _uow;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DbSet<Role> _roles;
        private readonly DbSet<ApplicationUser> _users;

        public RolesService(ApplicationDbContext applicationDbContext) //IUnitOfWork uow
        {
            //_uow = uow;
            //_uow.CheckArgumentIsNull(nameof(_uow));

            _applicationDbContext = applicationDbContext;

            _roles = _applicationDbContext.Set<Role>();
            _users = _applicationDbContext.Set<ApplicationUser>();
        }

        public Task<List<Role>> FindUserRolesAsync(string userId)
        {
            var userRolesQuery = from role in _roles
                                 from userRoles in role.UserRoles
                                 where userRoles.UserId.ToString() == userId
                                 select role;

            return userRolesQuery.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<bool> IsUserInRole(string userId, string roleName)
        {
            var userRolesQuery = from role in _roles
                                 where role.Name == roleName
                                 from user in role.UserRoles
                                 where user.UserId.ToString() == userId
                                 select role;

            var userRole = await userRolesQuery.FirstOrDefaultAsync();
            return userRole != null;
        }

        public Task<List<ApplicationUser>> FindUsersInRoleAsync(string roleName)
        {
            var roleUserIdsQuery = from role in _roles
                                   where role.Name == roleName
                                   from user in role.UserRoles
                                   select user.UserId;

            return _users.Where(user => roleUserIdsQuery.Contains(user.Id)).ToListAsync();
        }
    }
}
