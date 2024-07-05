namespace BetFootballLeague.Application.DTOs
{
    public abstract class BaseEntityDto
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedAtStr { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedAtStr { get; set; }
    }
}
