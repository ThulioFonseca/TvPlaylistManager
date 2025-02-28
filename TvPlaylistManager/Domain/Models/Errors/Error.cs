namespace TvPlaylistManager.Domain.Models.Errors
{
    public class Error
    {
        public string? Type { get; set; }
        public string? Message { get; set; }

        public Error(string? message, string? errorType)
        {
            Message = message;
            Type = errorType;
        }
    }    
}
