using LectionServer.Contracts;
using LectionServer.Models;

namespace LectionServer.Services;

public class UserService
{
    private readonly List<User> _users = new();

    public User? GetUser(string email, string password) =>
        _users.SingleOrDefault(x => x.Email == email && x.Password == password);


    public User AddUser(UserRequest request)
    {
        var book = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Password = request.Password
        };
        _users.Add(book);
        return book;
    }
}