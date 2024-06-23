using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Domain.Repositories;
using BetFootballLeague.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BetFootballLeague.Infrastructure.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<List<LeagueGroup>> GetLeagueGroupsAsync()
        {
            return await _context.Groups.Include(x => x.Teams).ToListAsync();
        }

        public async Task<LeagueGroup?> GetLeagueGroupByIdAsync(Guid id)
        {
            return await _context.Groups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddLeagueGroupAsync(LeagueGroup group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLeagueGroupAsync(LeagueGroup group)
        {
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }        

        public async Task UpdateLeagueGroupAsync(LeagueGroup group)
        {
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
        }
    }
}
