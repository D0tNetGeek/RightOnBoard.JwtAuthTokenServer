namespace RightOnBoard.JwtAuthTokenServer.Service.Interfaces
{
    public interface ISecurityService
    {
        string GetSha256Hash(string input);
    }
}
