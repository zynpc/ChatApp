using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ChatApp.Data;
using ChatApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Pages
{
    public class AgentPanelModel : PageModel
    {
        private readonly AppDbContext _db;

        public AgentPanelModel(AppDbContext db)
        {
            _db = db;
        }

        // Geçmiþ mesajlarý getirir
        public async Task<IActionResult> OnGetMessagesAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("userId boþ olamaz.");

            var messages = await _db.Messages
                .Where(m => m.UserId == userId)
                .OrderBy(m => m.Timestamp)
                .Select(m => new
                {
                    m.Id,
                    m.UserId,
                    m.AgentId,
                    m.Content,
                    m.Type,
                    m.Timestamp,
                    m.Url
                })
                .ToListAsync();

            return new JsonResult(messages);
        }

        

    }
}
