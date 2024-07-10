using BetFootballLeague.Shared.Enums;

namespace BetFootballLeague.Application.DTOs
{
    public class MatchDto : BaseEntityDto
    {
        public Guid? GroupId { get; set; }
        public Guid RoundId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string DateTime { get; set; }
        public int IndexOrder { get; set; }
        public Guid? Team1Id { get; set; }
        public Guid? Team2Id { get; set; }
        public double? Odds { get; set; }
        public Guid? UpperDoorTeamId { get; set; }
        public int? Team1Score { get; set; }
        public int? Team2Score { get; set; }
        public Guid? WinBetTeamId { get; set; }
        public MatchBetStatusEnum BetStatus { get; set; }
        public bool IsLockedBet { get; set; }

        public RoundDto Round { get; set; }
        public TeamDto Team1 { get; set; }
        public TeamDto Team2 { get; set; }
    }
}
