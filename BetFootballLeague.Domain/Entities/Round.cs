namespace BetFootballLeague.Domain.Entities
{
    public class Round : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Index { get; set; }
        public int BetPoint { get; set; }
    }
}
