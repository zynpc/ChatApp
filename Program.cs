using Microsoft.EntityFrameworkCore;
using ChatApp.Data;
using ChatApp.Hubs;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(_ => true));
});

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseDefaultFiles();
app.UseRouting();
app.MapRazorPages();
app.MapHub<ChatHub>("/chathub");

// Text mesajlarýný getirir
app.MapGet("/api/messages/{userId}", async (string userId, AppDbContext db) =>
{
   
        var messages = await db.Messages
            .Where(m => m.UserId == userId)
            .OrderBy(m => m.Timestamp)
            .ToListAsync();
        return Results.Ok(messages);
   
});


// Dosya yükleme endpoint'i
app.MapPost("/api/messages/upload", async (HttpRequest request, AppDbContext db) =>
{
    var form = await request.ReadFormAsync();
    var file = form.Files.GetFile("file");
    var userId = form["userId"].ToString();

    if (file == null || string.IsNullOrEmpty(userId))
        return Results.BadRequest();

    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
    var filePath = Path.Combine(app.Environment.WebRootPath, "uploads", fileName);

    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
    await using var stream = File.Create(filePath);
    await file.CopyToAsync(stream);

    var fileUrl = $"{request.Scheme}://{request.Host}/uploads/{fileName}";

    // Mesaj veritabanýna kaydedilmeden, sadece gerekli bilgiler dönülüyor.
    // Bu, mesajý göndermek için widget.js tarafýndan kullanýlacak.
    return Results.Ok(new
    {
        userId = userId,
        content = file.FileName,
        type = file.ContentType.StartsWith("image/") ? "image" : "file",
        url = fileUrl,
    });
});

app.MapGet("/", async context =>
{
    context.Response.Redirect("/testhtml.html");
});

app.Run();
