using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Domain.Repositories;
using BetFootballLeague.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BetFootballLeague.Infrastructure.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly ApplicationDbContext _context;

        public MatchRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<List<LeagueMatch>> GetMatchesAsync()
        {
            return await _context.Matches
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .Include(x => x.Round)
                .OrderBy(x => x.IndexOrder)
                .ToListAsync();
        }

        public async Task<LeagueMatch?> GetMatchByIdAsync(Guid id)
        {
            return await _context.Matches
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddMatchAsync(LeagueMatch match)
        {
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMatchAsync(Guid id)
        {
            var match = await _context.Matches.FirstAsync(x => x.Id == id);
            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
        }        

        public async Task UpdateMatchAsync(LeagueMatch match)
        {
            _context.Matches.Update(match);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LeagueMatch>> GetBetMatchesForUserAsync()
        {
            return await _context.Matches
               .Include(x => x.Team1)
               .Include(x => x.Team2)
               .Include(x => x.Round)
               .OrderBy(x => x.IndexOrder)
               .ToListAsync();
        }
    }
}
