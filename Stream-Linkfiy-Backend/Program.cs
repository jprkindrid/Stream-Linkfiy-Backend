using Scalar.AspNetCore;
using Stream_Linkfiy_Backend.Interfaces;
using Stream_Linkfiy_Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// built services
builder.Services.AddScoped<ISpotifyTokenService, SpotifyTokenService>();
builder.Services.AddHttpClient<ISpotifyTokenService, SpotifyTokenService>();
builder.Services.AddScoped<ISpotifyTrackService, SpotifyTrackService>();
builder.Services.AddHttpClient<ISpotifyTrackService, SpotifyTrackService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

//app.UseCors(x => x
//.AllowAnyMethod()
//.AllowAnyHeader()
//.AllowCredentials()
////.WithOrigins("http://localhost:44351")
//.SetIsOriginAllowed(origin => true));

app.UseAuthorization();

app.MapControllers();

app.Run();
