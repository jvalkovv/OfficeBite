using Microsoft.AspNetCore.SignalR;

public class TwoFactorHub : Hub
{
    public async Task SendLoginRequest(string userId, string token)
    {
        // Notify specific user by userId
        await Clients.User(userId).SendAsync("Receive2FANotification", token);
    }

    public async Task ApproveLogin(string token)
    {
        await Clients.All.SendAsync("LoginApproved", token);
    }
}
