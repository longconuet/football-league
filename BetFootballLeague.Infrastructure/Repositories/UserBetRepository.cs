using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Domain.Repositories;
using BetFootballLeague.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace BetFootballLeague.Infrastructure.Repositories
{
    public class UserBetRepository : IUserBetRepository
    {
        private readonly ApplicationDbContext _context;

        public UserBetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddUserBetAsync(UserBet userBet)
        {
            _context.UserBets.Add(userBet);
            await _context.SaveChangesAsync();
        }

        public async Task<UserBet?> GetUserBetByIdAsync(Guid id)
        {
            return await _context.UserBets.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateUserBetAsync(UserBet userBet)
        {
            userBet.UpdatedAt = DateTime.Now;
            _context.UserBets.Update(userBet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserBetsAsync(List<UserBet> userBets)
        {
            _context.UserBets.UpdateRange(userBets);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserBetAsync(Guid id)
        {
            var match = await _context.UserBets.FirstAsync(x => x.Id == id);
            _context.UserBets.Remove(match);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserBet>> GetBetsByUserAsync(Guid userId, Expression<Func<UserBet, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<UserBet> query = _context.UserBets.Include(x => x.Match).Where(x => x.UserId == userId);
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<List<UserBet>> GetBetsByMatchAsync(Guid matchId)
        {
            var userBets = await _context.UserBets
                .AsNoTracking()
                .Include(x => x.User)
                .Where(x => x.MatchId == matchId)
                .ToListAsync();
            return userBets;
        }

        public async Task<UserBet?> GetUserBetByMatchIdAsync(Guid userId, Guid matchId)
        {
            return await _context.UserBets.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId && x.MatchId == matchId);
        }
    }
}
