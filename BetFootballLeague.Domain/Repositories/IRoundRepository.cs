using BetFootballLeague.Domain.Entities;

namespace BetFootballLeague.Domain.Repositories
{
    public interface IRoundRepository
    {
        Task<List<Round>> GetRoundsAsync();
        Task<Round?> GetRoundByIdAsync(Guid id);
        Task AddRoundAsync(Round round);
        Task UpdateRoundAsync(Round round);
        Task DeleteRoundAsync(Round round);
    }
}
