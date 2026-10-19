using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;


// TODO CREATE LOGGER FOR DEVELOPMENT PURPOSES
// TODO Build and fix compile errors.
// TODO TEST
// TODO UniqueConstraintErrors.
// TODO ROLE BASED IF POSSIBLE

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<User> _hasher = new();

    public string Hash(string password) => _hasher.HashPassword(null!, password);

    public bool Verify(string hash, string password)
        => _hasher.VerifyHashedPassword(null!, hash, password) != PasswordVerificationResult.Failed;
}