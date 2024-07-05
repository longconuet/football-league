using AutoMapper;

namespace BetFootballLeague.Application.Mappings
{
    public class NullableDateTimeToStringConverter : ITypeConverter<DateTime?, string?>
    {
        public string? Convert(DateTime? source, string? destination, ResolutionContext context)
        {
            return source?.ToString("HH:mm dd/MM/yyyy");
        }
    }
}
