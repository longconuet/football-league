using BetFootballLeague.Domain.Entities;
using System.Linq.Expressions;

namespace BetFootballLeague.Domain.Repositories
{
    public interface IRoundRepository
    {
        Task<List<Round>> GetRoundsAsync(Expression<Func<Round, bool>>? predicate = null);
        Task<Round?> GetRoundByIdAsync(Guid id);
        Task AddRoundAsync(Round round);
        Task UpdateRoundAsync(Round round);
        Task DeleteRoundAsync(Round round);
    }
}
