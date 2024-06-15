namespace BetFootballLeague.Domain.Entities
{
    public class Team : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public Guid GroupId { get; set; }
        public int IsEliminated { get; set; } = 0;

        public LeagueGroup Group { get; set; }
        public ICollection<LeagueMatch> Matches { get; set; }
    }
}
