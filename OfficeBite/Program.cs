﻿using Microsoft.AspNetCore.Localization;
using OfficeBite.Extensions;
using OfficeBite.Infrastructure.Data.Seeds.Interfaces;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOfficeBiteDbContext(builder.Configuration);
builder.Services.AddOfficeBiteServices();
builder.Services.AddApplicationIdentity(builder.Configuration);
builder.Services.AddSeedDataLoader();
builder.Services.AddScoped<HelperMethods>();
builder.Services.AddControllersWithViews();
builder.Services.ConfigureCookie(builder.Configuration);

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var seedDataLoader = serviceProvider.GetRequiredService<ISeedDataLoader>();
    seedDataLoader.InitializeSeedData();
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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();


app.Run();
