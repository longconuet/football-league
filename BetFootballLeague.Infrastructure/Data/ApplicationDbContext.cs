using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Infrastructure.FluentConfigurations;
using Microsoft.EntityFrameworkCore;

namespace BetFootballLeague.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserBet> UserBets { get; set; }
        public DbSet<LeagueGroup> Groups { get; set; }
        public DbSet<LeagueMatch> Matches { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Bet_Football_League;TrustServerCertificate=True;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // config user
            modelBuilder.ApplyConfiguration(new User_FluentConfiguration());
            modelBuilder.Entity<User>().HasData(new User 
            { 
                Id = Guid.NewGuid(),
                FullName = "Nguyen Thanh Long",
                Email = "nice231096@gmail.com",
                Phone = "0348523140",
                Role = Shared.Enums.RoleEnum.ADMIN,
                Status = Shared.Enums.UserStatusEnum.Active
            });

            modelBuilder.ApplyConfiguration(new Group_FluentConfiguration());
            modelBuilder.ApplyConfiguration(new Round_FluentConfiguration());
            modelBuilder.ApplyConfiguration(new Team_FluentConfiguration());
        }
    }
}
