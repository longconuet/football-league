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
            return await _context.Matches.ToListAsync();
        }

        public async Task<LeagueMatch?> GetMatchByIdAsync(Guid id)
        {
            return await _context.Matches.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddMatchAsync(LeagueMatch match)
        {
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMatchAsync(LeagueMatch match)
        {
            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
        }        

        public async Task UpdateMatchAsync(LeagueMatch match)
        {
            _context.Matches.Update(match);
            await _context.SaveChangesAsync();
        }
    }
}
