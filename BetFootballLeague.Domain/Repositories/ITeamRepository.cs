using BetFootballLeague.Domain.Entities;

namespace BetFootballLeague.Domain.Repositories
{
    public interface ITeamRepository
    {
        Task<List<Team>> GetTeamsAsync(bool includeGroup = false);
        Task<Team?> GetTeamByIdAsync(Guid id);
        Task AddTeamAsync(Team team);
        Task UpdateTeamAsync(Team team);
        Task DeleteTeamAsync(Team team);
    }
}
