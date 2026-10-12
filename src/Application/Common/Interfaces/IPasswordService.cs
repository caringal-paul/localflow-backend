namespace Application.Common.Interfaces;


public interface IPasswordService
{
    string Hash(string password);
    bool Verify(string hash, string password);
}