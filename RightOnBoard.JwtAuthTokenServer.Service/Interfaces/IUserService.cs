using Microsoft.AspNetCore.Identity;
using RightOnBoard.JwtAuthTokenServer.Service.Models;
using RightOnBoard.JwtAuthTokenServer.Service.Models.Entities;
using System.Threading.Tasks;

namespace RightOnBoard.JwtAuthTokenServer.Service.Interfaces
{
    public interface IUserService
    {
        Task<string> GetSerialNumberAsync(string userId);
        Task<ApplicationUser> FindUserAsync(string username, string password);
        Task<ApplicationUser> FindUserAsync(string userId);
        Task UpdateUserLastActivityDateAsync(string userId);
        Task<ApplicationUser> GetCurrentUserAsync();
        string GetCurrentUserId();
        //Task<(bool Succeeded, string Error)> ChangePasswordAsync(User user, string currentPassword, string newPassword);
    }
}
