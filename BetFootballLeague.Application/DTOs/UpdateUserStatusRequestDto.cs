namespace BetFootballLeague.Application.DTOs
{
    public class UpdateUserStatusRequestDto
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
    }
}
