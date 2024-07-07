using BetFootballLeague.Domain.Entities;
using System.Linq.Expressions;

namespace BetFootballLeague.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync(Expression<Func<User, bool>>? filter = null, bool tracked = true);
        Task<User?> GetUserByIdAsync(Guid id, bool tracked = true);
        Task<User?> GetUserByPhoneAsync(string input);
        Task<User?> GetUserByEmailAsync(string input);
        Task<User?> GetUserByUsernameAsync(string input);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}
