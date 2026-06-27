using CarGalleryHub.Infrastructure.Options;
using CarGalleryHub.Persistence.Extensions;
using CarGalleryHub.Persistence.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore;
using System.Text;
using System.Reflection.Metadata;
using CarGalleryHub.Persistence.UnitOfWork;
using CarGalleryHub.Application.Interfaces;
using CarGalleryHub.Persistence.Services;
using CarGalleryHub.Infrastructure.Services;
using CarGalleryHub.Persistence.Repositories;
using CarGalleryHub.Infrastructure.Extensions;
using System.Security.Claims;
using CarGalleryHub.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})



.AddJwtBearer(options => 
{
    var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
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


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "CarHub", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer {Token}"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});


builder.Services.AddAuthorization();




builder.Services.AddPersistenceService(builder.Configuration);
builder.Services.AddInfrastructureService(builder.Configuration);
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
app.UseMiddleware<GlobalExceptionHandler>();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
