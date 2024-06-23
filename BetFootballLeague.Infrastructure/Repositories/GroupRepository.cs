using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Domain.Repositories;
using BetFootballLeague.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return await _context.Groups.ToListAsync();
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
