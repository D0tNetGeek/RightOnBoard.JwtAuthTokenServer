using System.Collections.Generic;
using System.Security.Claims;

namespace RightOnBoard.JwtAuthTokenServer.Service.Interfaces
{
    public interface IAntiForgeryCookieService
    {
        void RegenerateAntiForgeryCookies(IEnumerable<Claim> claims);
        void DeleteAntiForgeryCookies();
    }
}
