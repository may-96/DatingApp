using DemoApp.Entities;

namespace DemoApp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}