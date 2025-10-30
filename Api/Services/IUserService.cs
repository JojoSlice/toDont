using Api.Models;

namespace Api.Services
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(string userName, string password);
        Task<User?> AuthenticateAsync(string userName, string password);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUserNameAsync(string userName);
    }
}
