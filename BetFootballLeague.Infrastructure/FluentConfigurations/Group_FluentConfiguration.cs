using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BetFootballLeague.Domain.Entities;

namespace BetFootballLeague.Infrastructure.FluentConfigurations
{
    internal class Group_FluentConfiguration : IEntityTypeConfiguration<LeagueGroup>
    {
        public void Configure(EntityTypeBuilder<LeagueGroup> modelBuilder)
        {
            modelBuilder
                .HasMany<Team>(b => b.Teams)
                .WithOne(b => b.Group)
                .HasForeignKey(b => b.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
