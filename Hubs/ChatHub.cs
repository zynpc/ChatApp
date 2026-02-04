using Microsoft.AspNetCore.SignalR;
using ChatApp.Data;
using ChatApp.Models;


namespace ChatApp.Hubs
{

    public class MessageDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = "";
        public string? AgentId { get; set; }
        public string Content { get; set; } = "";
        public string Type { get; set; } = "text";  
        public DateTime Timestamp { get; set; }
        public string? Url { get; set; }

    }
    public class ChatHub:Hub
    {
      
        private readonly AppDbContext _db;
        public ChatHub(AppDbContext db) => _db = db;

        // Agent kendini "agent:all" grubuna ekler
        public async Task RegisterAsAgent(string agentId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "agent:all");
        }

        // Kullanıcı kendi grubuna eklenir
        public async Task RegisterAsUser(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
        }



        // Kullanıcıdan gelen mesaj sadece agent’lara gider

        public async Task SendFromUser(string userId, string content, DateTime timestamp, string type = "text", string? url = null)
        {
            // Mesaj nesnesini oluşturur
            var message = new Message
            {
                UserId = userId,
                Content = content,
                Type = type,
                Url = url,
                Timestamp = timestamp, // Client'tan gelen zaman damgasını kullanır
            };

         
            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            // Mesajı DTO'ya dönüştürür
            var dto = new MessageDto
            {
                Id = message.Id,
                UserId = message.UserId,
                Content = message.Content,
                Type = message.Type,
                Url = message.Url,
                Timestamp = message.Timestamp,
            };

            // Mesajı sadece agent'lara gönderir
            await Clients.Group("agent:all").SendAsync("ReceiveMessage", dto);
        }


        // Agent'tan gelen mesaj sadece ilgili kullanıcıya gider
        public async Task SendFromAgent(string userId, string agentId, string content, DateTime timestamp, string type = "text")
        {
            var message = new Message
            {
                UserId = userId,
                AgentId = agentId,
                Content = content,
                Type = type,
                Timestamp = timestamp,

            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            var dto = new MessageDto
            {
                Id = message.Id,
                UserId = message.UserId,
                AgentId = message.AgentId,
                Content = message.Content,
                Type = message.Type,
                Timestamp = message.Timestamp,

            };

            await Clients.Group($"user:{userId}").SendAsync("ReceiveMessage", dto);
        }


        // Kullanıcı yazıyor bilgisini agent'lara gönder
        public async Task UserTyping(string userId)
        {
            await Clients.Group("agent:all").SendAsync("UserTyping", userId);
        }

        // Agent yazıyor bilgisini ilgili kullanıcıya gönderir
        public async Task AgentTyping(string userId)
        {
            await Clients.Group($"user:{userId}").SendAsync("AgentTyping");
        }


      

    }

}
