using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BetFootballLeague.Domain.Entities;

namespace BetFootballLeague.Infrastructure.FluentConfigurations
{
    internal class Match_FluentConfiguration : IEntityTypeConfiguration<LeagueMatch>
    {
        public void Configure(EntityTypeBuilder<LeagueMatch> modelBuilder)
        {
            //modelBuilder
            //    .HasOne<Team>(b => b.Team1)
            //    .WithMany(b => b.Matches)
            //    .HasForeignKey(b => b.Team1Id)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder
            //    .HasOne<Team>(b => b.Team2)
            //    .WithMany(b => b.Matches)
            //    .HasForeignKey(b => b.Team2Id)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .HasOne(m => m.Team1)
                .WithMany()
                .HasForeignKey(m => m.Team1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .HasOne(m => m.Team2)
                .WithMany()
                .HasForeignKey(m => m.Team2Id)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
