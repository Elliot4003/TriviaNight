namespace TriviaNight.Models;

public sealed class UserModel
{
    public Guid Id { get; set; }

    public required string Username { get; set; }

    public required string PasswordHash { get; set; }
}