using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LectionServer.Contracts;
using LectionServer.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LectionServer.Endpoints;

public static class AuthEndpoints
{
    private const string EndpointsTag = "Auth";

    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("api/auth/login", Login)
            .Accepts<LoginRequest>("application/json")
            .Produces<LoginResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Log in to the system")
            .WithTags(EndpointsTag)
            .WithOpenApi();
    }

    private static IResult Login(IOptions<AuthSettings> authSettings, LoginRequest request, CancellationToken cancellationToken)
    {
        if (request.Email != "test@email.ru" || request.Password != "test-password")
            return Results.BadRequest();
        var result = Authorize(authSettings.Value);
        return Results.Json(result);
    }

    private static LoginResponse Authorize(AuthSettings authSettings)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim("id", Guid.NewGuid().ToString()),
            new Claim("name", "Test User"),
            new Claim("email", "test@email.ru")
        });

        var secret = Encoding.UTF8.GetBytes(authSettings.JWTSecret!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Issuer = authSettings.JWTIssuer,
            Audience = authSettings.JWTAudience,
            Expires = DateTime.UtcNow.AddMinutes(authSettings.JWTTimeToLive),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return new LoginResponse
        {
            AccessToken = tokenHandler.WriteToken(securityToken)
        };
    }
}