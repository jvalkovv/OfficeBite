using Microsoft.AspNetCore.Localization;
using OfficeBite.Core.Extensions;
using OfficeBite.Core.Extensions.Interfaces;
using OfficeBite.Infrastructure.Data.Seeds.Interfaces;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOfficeBiteDbContext(builder.Configuration);
builder.Services.AddOfficeBiteServices();
builder.Services.AddApplicationIdentity(builder.Configuration);
builder.Services.AddSeedDataLoader();
builder.Services.AddScoped<IHelperMethods, HelperMethods>();
builder.Services.AddControllersWithViews();
builder.Services.ConfigureCookie(builder.Configuration);
//builder.Services.AddApplicationExternalFbIdentity(builder.Configuration);
//builder.Services.AddMvc();
builder.Services.AddProgressiveWebApp();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var seedDataLoader = serviceProvider.GetRequiredService<ISeedDataLoader>();
    await seedDataLoader.InitializeSeedData();
}


var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("bg-BG")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("bg-BG"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();

}

app.UseStatusCodePagesWithReExecute("/error/{0}");

app.Use((httpsScheme, next) =>
{
    httpsScheme.Request.Scheme = "https";

    return next();
});
app.UseStatusCodePagesWithReExecute("/error/{0}");

app.Use((httpsScheme, next) =>
{
    httpsScheme.Request.Scheme = "https";
    return next();
});

// --- Add IP blocking middleware here ---
app.Use(async (context, next) =>
{
    var blockedIp = "176.222.15.61";
    var remoteIp = context.Connection.RemoteIpAddress?.ToString();

    if (remoteIp == blockedIp)
    {
        context.Response.StatusCode = 403; // Forbidden
        await context.Response.WriteAsync("Nice try");
        return;
    }

    await next();
});
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();


app.Run();
