using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add YARP config from file with hot-reload support
var yarpConfigPath = "/app/yarp/yarp-config.json";
if (File.Exists(yarpConfigPath))
{
    builder.Configuration.AddJsonFile(yarpConfigPath, optional: false, reloadOnChange: true);
}

builder.Services.AddAuthentication("Bearer")
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!
                )
            )
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Authenticated",
        policy => policy.RequireAuthenticatedUser());

    options.AddPolicy("AllowAnonymous",
        policy => policy.RequireAssertion(_ => true));
});

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.Run();
