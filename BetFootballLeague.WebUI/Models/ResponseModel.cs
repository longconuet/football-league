using BetFootballLeague.Shared.Enums;

namespace BetFootballLeague.WebUI.Models
{
    public class ResponseModel
    {
        public ResponseStatusEnum Status { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }

    public class ResponseModel<T>
    {
        public ResponseStatusEnum Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
