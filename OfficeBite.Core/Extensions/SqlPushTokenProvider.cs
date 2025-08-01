using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OfficeBite.Infrastructure.Data.Models;
using OfficeBite.Infrastructure.Data;

public class SqlPushTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser> where TUser : class
{
    private readonly OfficeBiteDbContext _db;
    private readonly IHubContext<TwoFactorHub> _hub;

    public SqlPushTokenProvider(OfficeBiteDbContext db, IHubContext<TwoFactorHub> hub)
    {
        _db = db;
        _hub = hub;
    }

    public async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        var userId = await manager.GetUserIdAsync(user);
        var token = Guid.NewGuid().ToString();

        _db.TwoFactorPushChallenges.Add(new TwoFactorPushChallenge
        {
            UserId = int.Parse(userId),
            Token = token,
            Status = "Pending"
        });
        await _db.SaveChangesAsync();

        // Send SignalR push notification
        await _hub.Clients.User(userId).SendAsync("Receive2FANotification", token);
        return token;
    }

    public async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
    {
        var challenge = await _db.TwoFactorPushChallenges.FirstOrDefaultAsync(x => x.Token == token);
        return challenge != null && challenge.Status == "Approved";
    }

    public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        => Task.FromResult(true);
}
