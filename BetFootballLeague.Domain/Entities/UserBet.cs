namespace BetFootballLeague.Domain.Entities
{
    public class UserBet : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid MatchId { get; set; }
        public Guid BetTeamId { get; set; }
    }
}
