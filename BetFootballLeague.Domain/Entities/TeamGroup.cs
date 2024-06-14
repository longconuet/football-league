namespace BetFootballLeague.Domain.Entities
{
    public class TeamGroup : BaseEntity
    {
        public Guid TeamId { get; set; }
        public Guid GroupId { get; set; }
    }
}
