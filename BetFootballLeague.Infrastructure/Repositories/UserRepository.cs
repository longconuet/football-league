using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Domain.Repositories;
using BetFootballLeague.Infrastructure.Data;
using BetFootballLeague.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace BetFootballLeague.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<List<User>> GetActiveNormalUsersAsync()
        {
            return await _context.Users.Where(x => x.Status == UserStatusEnum.Active && x.Role == RoleEnum.NORMAL_USER)
                .ToListAsync();
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

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
