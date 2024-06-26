namespace BetFootballLeague.Application.DTOs
{
    public class CreateMatchRequestDto
    {
        public Guid RoundId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public DateTime DateTime { get; set; }
        public int IndexOrder { get; set; }
        public Guid? Team1Id { get; set; }
        public Guid? Team2Id { get; set; }
    }
}
