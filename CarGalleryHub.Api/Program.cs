using CarGalleryHub.Infrastructure.Options;
using CarGalleryHub.Persistence.Extensions;
using CarGalleryHub.Persistence.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => 
{
    var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
    var key = Encoding.UTF8.GetBytes(jwtOptions!.Secret);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});


builder.Services.AddPersistenceService(builder.Configuration);

builder.Services.AddScoped<DataSeeder>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope()) 
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();

    await seeder.SeedAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
