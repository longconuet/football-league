using BetFootballLeague.Domain.Entities;
using System.Linq.Expressions;

namespace BetFootballLeague.Domain.Repositories
{
    public interface IMatchRepository
    {
        Task<List<LeagueMatch>> GetMatchesAsync(Expression<Func<LeagueMatch, bool>>? filter = null, bool tracked = true, bool include = true);
        Task<LeagueMatch?> GetMatchByIdAsync(Guid id);
        Task AddMatchAsync(LeagueMatch match);
        Task UpdateMatchAsync(LeagueMatch match);
        Task UpdateMatchListAsync(List<LeagueMatch> matches);
        Task DeleteMatchAsync(Guid id);
        Task<List<LeagueMatch>> GetBetMatchesForUserAsync();
    }
}
