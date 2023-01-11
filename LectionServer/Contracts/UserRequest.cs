﻿namespace LectionServer.Contracts;

public record UserRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}