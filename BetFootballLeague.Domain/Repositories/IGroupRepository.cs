using BetFootballLeague.Domain.Entities;

namespace BetFootballLeague.Domain.Repositories
{
    public interface IGroupRepository
    {
        Task<List<LeagueGroup>> GetLeagueGroupsAsync();
        Task<LeagueGroup?> GetLeagueGroupByIdAsync(Guid id);
        Task AddLeagueGroupAsync(LeagueGroup group);
        Task UpdateLeagueGroupAsync(LeagueGroup group);
        Task DeleteLeagueGroupAsync(LeagueGroup group);
    }
}
