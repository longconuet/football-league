using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Domain.Repositories;
using BetFootballLeague.Infrastructure.Data;
using BetFootballLeague.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BetFootballLeague.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsersAsync(Expression<Func<User, bool>>? filter = null, bool tracked = true)
        {

            IQueryable<User> query = _context.Users;
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

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid id, bool tracked = true)
        {
            if (tracked)
            {
                return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            }
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetUserByPhoneAsync(string input)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Phone == input);
        }

        public async Task<User?> GetUserByEmailAsync(string input)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == input);
        }

        public async Task<User?> GetUserByUsernameAsync(string input)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == input);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
