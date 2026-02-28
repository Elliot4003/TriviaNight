namespace TriviaNight.Interfaces;

public interface IPasswordHelper
{
    string Hash(string password);
}