using RightOnBoard.JwtAuthTokenServer.Service.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RightOnBoard.JwtAuthTokenServer.Service.Interfaces
{
    public interface IRolesService
    {
        Task<List<Role>> FindUserRolesAsync(string userId);
        Task<bool> IsUserInRole(string userId, string roleName);
        Task<List<ApplicationUser>> FindUsersInRoleAsync(string roleName);
    }
}
