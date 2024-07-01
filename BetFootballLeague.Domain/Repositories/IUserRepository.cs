using BetFootballLeague.Domain.Entities;

namespace BetFootballLeague.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<List<User>> GetActiveNormalUsersAsync();
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByPhoneAsync(string input);
        Task<User?> GetUserByEmailAsync(string input);
        Task<User?> GetUserByUsernameAsync(string input);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}
