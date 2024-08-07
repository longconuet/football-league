﻿using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Domain.Repositories;
using BetFootballLeague.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BetFootballLeague.Infrastructure.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly ApplicationDbContext _context;

        public MatchRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<List<LeagueMatch>> GetMatchesAsync(Expression<Func<LeagueMatch, bool>>? filter = null, bool tracked = true, bool include = true)
        {
            IQueryable<LeagueMatch> query = _context.Matches;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (include)
            {
                query = query.Include(x => x.Team1)
                    .Include(x => x.Team2)
                    .Include(x => x.Round);
            }

            return await query.Include(x => x.Round).OrderBy(x => x.IndexOrder).ToListAsync();
        }

        public async Task<LeagueMatch?> GetMatchByIdAsync(Guid id)
        {
            return await _context.Matches
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .Include(x => x.Round)
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

        public async Task UpdateMatchListAsync(List<LeagueMatch> matches)
        {
            _context.Matches.UpdateRange(matches);
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
