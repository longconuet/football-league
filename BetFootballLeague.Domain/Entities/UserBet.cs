namespace BetFootballLeague.Domain.Entities
{
    public class UserBet : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid MatchId { get; set; }
        public Guid BetTeamId { get; set; }
        public bool IsWin { get; set; }

        public User User { get; set; }
        public LeagueMatch Match { get; set; }
        public Team BetTeam { get; set; }
    }
}
