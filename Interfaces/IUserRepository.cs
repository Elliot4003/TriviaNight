using TriviaNight.Models;

namespace TriviaNight.Interfaces;

public interface IUserRepository
{
    Task AddUser(UserModel user);
    Task<bool> Exists(string userName);
    Task<UserModel?> GetUser(string userName);
}