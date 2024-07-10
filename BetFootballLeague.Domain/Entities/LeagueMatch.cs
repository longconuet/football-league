using BetFootballLeague.Shared.Enums;

namespace BetFootballLeague.Domain.Entities
{
    public class LeagueMatch : BaseEntity
    {
        public Guid? GroupId { get; set; }
        public Guid RoundId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public DateTime DateTime { get; set; }
        public int IndexOrder { get; set; }
        public Guid? Team1Id { get; set; }
        public Guid? Team2Id { get; set; }
        public double? Odds { get; set; }
        public Guid? UpperDoorTeamId { get; set; }
        public int? Team1Score { get; set; }
        public int? Team2Score { get; set; }
        public Guid? WinBetTeamId { get; set; }
        public MatchBetStatusEnum BetStatus { get; set; } = MatchBetStatusEnum.NOT_ALLOWED;
        public bool IsLockedBet { get; set; } = true;

        public Round Round { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public ICollection<UserBet> UserBets { get; set; }
    }
}
