using GamesGlobal.Infrastructure.Models;

namespace GamesGlobal.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(string username, User user);
        Task DeleteUserAsync(string username);
    }
}
