using BetFootballLeague.Domain.Entities;

namespace BetFootballLeague.Domain.Repositories
{
    public interface IUserBetRepository
    {
        Task<List<UserBet>> GetBetsByUserAsync(Guid userId);
        Task<List<UserBet>> GetBetsByMatchAsync(Guid matchId);
        Task<UserBet?> GetUserBetByIdAsync(Guid id);
        Task<UserBet?> GetUserBetByMatchIdAsync(Guid userId, Guid matchId);
        Task AddUserBetAsync(UserBet userBet);
        Task UpdateUserBetAsync(UserBet userBet);
        Task UpdateUserBetsAsync(List<UserBet> userBets);
        Task DeleteUserBetAsync(Guid id);
    }
}
