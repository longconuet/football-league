namespace BetFootballLeague.Domain.Entities
{
    public class LeagueGroup : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Team> Teams { get; set; }
    }
}
