using BetFootballLeague.Domain.Entities;

namespace BetFootballLeague.Domain.Repositories
{
    public interface IUserBetRepository
    {
        Task<List<UserBet>> GetBetsByUserAsync(Guid userId);
        Task<UserBet?> GetUserBetByIdAsync(Guid id);
        Task<UserBet?> GetUserBetByMatchIdAsync(Guid userId, Guid matchId);
        Task AddUserBetAsync(UserBet userBet);
        Task UpdateUserBetAsync(UserBet userBet);
        Task DeleteUserBetAsync(Guid id);
    }
}
