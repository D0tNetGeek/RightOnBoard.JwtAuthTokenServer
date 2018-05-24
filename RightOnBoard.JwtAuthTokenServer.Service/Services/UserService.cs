using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RightOnBoard.JwtAuthTokenServer.Service.DbContext;
using RightOnBoard.JwtAuthTokenServer.Service.Interfaces;
using RightOnBoard.JwtAuthTokenServer.Service.Models;
using RightOnBoard.JwtAuthTokenServer.Service.Models.Entities;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RightOnBoard.JwtAuthTokenServer.Service.Services
{
    public class UserService : IUserService
    {
        //private readonly IUnitOfWork _uow;
        //private readonly DbSet<User> _users;
        private readonly ISecurityService _securityService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public UserService(
            //IUnitOfWork uow,
            ISecurityService securityService,
            IHttpContextAccessor contextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            //_uow = uow;
            //_uow.CheckArgumentIsNull(nameof(_uow));

            //_users = _uow.Set<User>();

            _securityService = securityService;
            //_securityService.CheckArgumentIsNull(nameof(_securityService));

            _contextAccessor = contextAccessor;
            //_contextAccessor.CheckArgumentIsNull(nameof(_contextAccessor));

            _userManager = userManager;
        }

        public async Task<ApplicationUser> FindUserAsync(string userId)
        {
            var user = await _userManager.FindByNameAsync(userId);

            return user;
        }

        //Task<IdentityResult> IUserService.FindUserAsync(string username, string password)
        //{
        //    throw new System.NotImplementedException();
        //}

        public async Task<ApplicationUser> FindUserAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            var passwordHash = _securityService.GetSha256Hash(password);

            var checkPass =  await _userManager.CheckPasswordAsync(user, password);

            user.SerialNumber = user.Id;
            user.Issuer = "Insypher";

            if (checkPass)
                return user;
            else
                return null;
        }

        //public async Task<string> GetSerialNumberAsync(int userId)
        //{
        //    var user = await FindUserAsync(userId);
        //    return user.SerialNumber;
        //}

        public async Task UpdateUserLastActivityDateAsync(string userId)
        {
            var user = await FindUserAsync(userId);
            if (user.LastLoggedIn != null)
            {
                var updateLastActivityDate = TimeSpan.FromMinutes(2);
                var currentUtc = DateTimeOffset.UtcNow;
                var timeElapsed = currentUtc.Subtract(user.LastLoggedIn.Value); 

                if (timeElapsed < updateLastActivityDate)
                {
                    return;
                }
            }

            user.LastLoggedIn = DateTimeOffset.UtcNow;
            //await _uow.SaveChangesAsync();

            await _applicationDbContext.SaveChangesAsync();
        }

        public string GetCurrentUserId()
        {
            var claimsIdentity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userDataClaim = claimsIdentity?.FindFirst(ClaimTypes.UserData);

            var userId = userDataClaim?.Value;

            return string.IsNullOrWhiteSpace(userId) ? null : userId; //int.Parse(userId);
        }

        public Task<ApplicationUser> GetCurrentUserAsync()
        {
            var userId = GetCurrentUserId();

            return FindUserAsync(userId);
        }

        public async Task<string> GetSerialNumberAsync(string userId)
        {
            var user = await FindUserAsync(userId);

            return user.SerialNumber;
        }

        //public async Task<(bool Succeeded, string Error)> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        //{
        //    var currentPasswordHash = _securityService.GetSha256Hash(currentPassword);
        //    if (user.Password != currentPasswordHash)
        //    {
        //        return (false, "Current password is wrong.");
        //    }

        //    user.Password = _securityService.GetSha256Hash(newPassword);
        //    // user.SerialNumber = Guid.NewGuid().ToString("N"); // To force other logins to expire.
        //    await _uow.SaveChangesAsync();
        //    return (true, string.Empty);
        //}
    }
}
