using ChatApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Pages
{
    public class AgentDashboardModel : PageModel
    {
        private readonly AppDbContext _db;

        public int TotalUsers { get; set; }
        public int ActiveSessions { get; set; }
        public List<string> Dates { get; set; } = new();
        public List<int> MessageCounts { get; set; } = new();
        public List<UserStat> UserStats { get; set; } = new();
        public List<MessageTypeStat> TypeDistribution { get; set; } = new();

        public AgentDashboardModel(AppDbContext db)
        {
            _db = db;
        }

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            // Toplam kullanıcı
            TotalUsers = await _db.Messages.Select(m => m.UserId).Distinct().CountAsync();


            //Son 5 dk' da mesaj atan kullanıcılar aktif olarak belirlenir

            var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);
            ActiveSessions = await _db.Messages
                .Where(m => m.Timestamp >= fiveMinutesAgo)
                .Select(m => m.UserId)
                .Distinct()
                .CountAsync();


            // Günlük mesaj sayıları (son 7 gün) - UTC'ye göre
            var todayUtc = DateTime.UtcNow.Date;
            var last7DaysUtc = Enumerable.Range(0, 7)
                .Select(i => todayUtc.AddDays(-i))
                .OrderBy(d => d)
                .ToList();

            Dates = last7DaysUtc.Select(d => d.ToString("yyyy-MM-dd")).ToList();
            MessageCounts = new();
            foreach (var day in last7DaysUtc)
            {
                var nextDay = day.AddDays(1);
                var count = await _db.Messages
                    .Where(m => m.Timestamp >= day && m.Timestamp < nextDay)
                    .CountAsync();
                MessageCounts.Add(count);
            }

            // Kullanıcı bazlı mesaj istatistikleri
            UserStats = await _db.Messages
                .GroupBy(m => m.UserId)
                .Select(g => new UserStat
                {
                    UserId = g.Key,
                    MessageCount = g.Count()
                })
                .OrderByDescending(x => x.MessageCount)
                .ToListAsync();

            // Mesaj tipi dağılımı
            TypeDistribution = await _db.Messages
                .GroupBy(m => m.Type)
                .Select(g => new MessageTypeStat
                {
                    Type = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }

        
        public async Task<IActionResult> OnGetStatsAsync()
        {
            await LoadDataAsync();

            return new JsonResult(new
            {
                totalUsers = TotalUsers,
                activeSessions = ActiveSessions,
                dates = Dates,
                messageCounts = MessageCounts,
                userStats = UserStats,
                typeDistribution = TypeDistribution
            });
        }

        public class UserStat
        {
            public string UserId { get; set; }
            public int MessageCount { get; set; }
        }

        public class MessageTypeStat
        {
            public string Type { get; set; }
            public int Count { get; set; }
        }
    }
}