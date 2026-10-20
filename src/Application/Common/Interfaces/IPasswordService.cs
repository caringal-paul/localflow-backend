using Domain.Entities;

namespace Application.Common.Interfaces;


public interface IPasswordService
{
    string Hash(string password);
    string Hash(User user, string password);
    bool Verify(string hash, string password);
}