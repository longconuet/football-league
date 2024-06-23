namespace BetFootballLeague.Application.DTOs
{
    public class RoundDto : BaseEntityDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Index { get; set; }
        public int BetPoint { get; set; }
    }
}