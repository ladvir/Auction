using System.Text.Json;
using System.Text.Json.Serialization;
using Auction.Components;
using Auction.Database;
using Auction.Repositories;
using Auction.Services;
using Blazored.LocalStorage;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

builder.Configuration.AddJsonFile("appsettingssecrets.json", optional: true, reloadOnChange: true);

builder.Services.AddDbContextFactory<AuctionDbContext>(options =>
{
    if (builder.Environment.IsEnvironment("TEST") || builder.Environment.IsDevelopment())
    {
        options
            .UseSqlServer(configuration.GetConnectionString("Development"),
                opt => opt
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                    .UseRelationalNulls());
        
        options.EnableSensitiveDataLogging();
    }
    else
    {
        options

            .UseSqlServer(configuration.GetConnectionString("Production"),
                opt => opt
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                    .UseRelationalNulls());
    }
}, ServiceLifetime.Scoped);

builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<AuctionService>();
builder.Services.AddBlazoredLocalStorage(config =>
{
    config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    config.JsonSerializerOptions.WriteIndented = false;
});

builder.Services.AddCors(o =>
    {
        o.AddPolicy("CorsPolicy", policy => policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
        );
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UsePathBase("/aukce/");
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();