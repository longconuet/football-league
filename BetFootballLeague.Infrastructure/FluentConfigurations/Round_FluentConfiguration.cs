using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BetFootballLeague.Domain.Entities;

namespace BetFootballLeague.Infrastructure.FluentConfigurations
{
    internal class Round_FluentConfiguration : IEntityTypeConfiguration<Round>
    {
        public void Configure(EntityTypeBuilder<Round> modelBuilder)
        {
            modelBuilder
                .HasMany<LeagueMatch>(b => b.Matches)
                .WithOne(b => b.Round)
                .HasForeignKey(b => b.RoundId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
