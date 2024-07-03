using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Domain.Repositories;
using BetFootballLeague.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
            _context.UserBets.Update(userBet);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserBetAsync(Guid id)
        {
            var match = await _context.UserBets.FirstAsync(x => x.Id == id);
            _context.UserBets.Remove(match);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserBet>> GetBetsByUserAsync(Guid userId)
        {
            var userBets = await _context.UserBets.Where(x => x.UserId == userId).ToListAsync();
            return userBets;
        }
    }
}
