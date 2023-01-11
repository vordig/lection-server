using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LectionServer.Contracts;
using LectionServer.Models;
using LectionServer.Services;
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
            .AllowAnonymous()
            .Accepts<LoginRequest>("application/json")
            .Produces<LoginResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Log in to the system")
            .WithTags(EndpointsTag)
            .WithOpenApi();
        
        app.MapPost("api/auth/register", Register)
            .AllowAnonymous()
            .Accepts<UserRequest>("application/json")
            .Produces(StatusCodes.Status200OK)
            .WithDescription("Register into the system")
            .WithTags(EndpointsTag)
            .WithOpenApi();
    }

    private static IResult Login(UserService userService, IOptions<AuthSettings> authSettings, LoginRequest request, CancellationToken cancellationToken)
    {
        var user = userService.GetUser(request.Email, request.Password);
        if (user is null)
            return Results.BadRequest();
        var result = Authorize(user, authSettings.Value);
        return Results.Json(result);
    }
    
    private static IResult Register(UserService userService, UserRequest request, CancellationToken cancellationToken)
    {
        var user = userService.AddUser(request);
        return Results.Ok();
    }

    private static LoginResponse Authorize(User user, AuthSettings authSettings)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim("id", user.Id.ToString()),
            new Claim("name", user.Name),
            new Claim("email", user.Email)
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