using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BetFootballLeague.Domain.Entities;

namespace BetFootballLeague.Infrastructure.FluentConfigurations
{
    internal class Team_FluentConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> modelBuilder)
        {
            modelBuilder
                .HasMany<LeagueMatch>(b => b.Matches)
                .WithOne(b => b.Team1)
                .HasForeignKey(b => b.Team1Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
               .HasMany<LeagueMatch>(b => b.Matches)
               .WithOne(b => b.Team2)
               .HasForeignKey(b => b.Team2Id)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
