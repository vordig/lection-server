namespace LectionServer.Contracts;

public record LoginResponse
{
    public required string AccessToken { get; init; }
}