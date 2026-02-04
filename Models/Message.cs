namespace ChatApp.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string? AgentId { get; set; }
        public string Content { get; set; } = null!;
        public string Type { get; set; } = "text"; // text | file | image
        public string? Url { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
    }
}
