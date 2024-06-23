using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Domain.Repositories;
using BetFootballLeague.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BetFootballLeague.Infrastructure.Repositories
{
    public class RoundRepository : IRoundRepository
    {
        private readonly ApplicationDbContext _context;

        public RoundRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Round>> GetRoundsAsync(Expression<Func<Round, bool>>? predicate = null)
        {
            if (predicate != null)
            {
                return await _context.Rounds.Where(predicate).OrderBy(x => x.Index).ToListAsync();
            }
            return await _context.Rounds.OrderBy(x => x.Index).ToListAsync();
        }

        public async Task<Round?> GetRoundByIdAsync(Guid id)
        {
            return await _context.Rounds.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddRoundAsync(Round round)
        {
            _context.Rounds.Add(round);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoundAsync(Round round)
        {
            _context.Rounds.Remove(round);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoundAsync(Round round)
        {
            _context.Rounds.Update(round);
            await _context.SaveChangesAsync();
        }
    }
}
