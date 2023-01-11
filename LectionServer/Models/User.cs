namespace LectionServer.Models;

public record User
{
    public required Guid Id { get; init; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}