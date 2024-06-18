namespace BetFootballLeague.Application.DTOs
{
    public class CreateUserRequestDto
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public int Status { get; set; }
    }
}
