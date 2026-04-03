using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using OfficeBite.Core.Extensions;
using OfficeBite.Core.Extensions.Interfaces;
using OfficeBite.Infrastructure.Data;
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
builder.Services.Configure<ForwardedHeadersOptions>(options => {
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // Доверяваме се само на нашия Nginx контейнер (CT 152)
    options.KnownProxies.Add(System.Net.IPAddress.Parse("192.168.1.152"));
});
var app = builder.Build();

app.UseForwardedHeaders();

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OfficeBiteDbContext>(); // Замени ApplicationDbContext с твоето име на контекста, ако е различно
    db.Database.Migrate();
}

app.Run();