namespace BetFootballLeague.Application.DTOs
{
    public class UpdateRoundRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public int BetPoint { get; set; }
    }
}
