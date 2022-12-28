namespace LectionServer.Contracts;

public record BookRequest
{
    public required string Author { get; init; }
    public required string Name { get; init; }
}