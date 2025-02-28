namespace TvPlaylistManager.Domain.Models.Errors
{
    public class ErrorResponse
    {
        public string? Instance { get; set; }
        public string? TraceId { get; set; }
        public IEnumerable<Error> Errors { get; set; }
        public ErrorResponse(string? instance, string? traceId)
        {
            Errors = [];
            Instance = instance;
            TraceId = traceId;
        }
    }
}
