using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;

namespace RightOnBoard.JwtAuthTokenServer.Service.Interfaces
{
    public interface ITokenValidatorService
    {
        Task ValidateAsync(TokenValidatedContext context);
    }
}
