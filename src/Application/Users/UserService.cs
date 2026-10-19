using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Roles;
using Application.Users.Dtos;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Users;

public class UserService(
    IUserRepository users,
    IRoleRepository roles,
    IPasswordService passwords,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork,
    ILogger<UserService> logger) : IUserService
{
    public async Task<IReadOnlyList<UserDto>> ListAsync(CancellationToken ct)
    {
        var members = await users.ListMembersAsync(ct);
        return members.Select(ToDto).ToList();
    }

    public async Task<UserDto> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var member = await users.GetMemberAsync(id, ct) ?? throw new AppException(UsersErrors.NotFound);
        return ToDto(member);
    }

    public async Task<UserDto> CreateAsync(CreateUserRequest request, CancellationToken ct)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        if (await users.GetByEmailAsync(email, ct) is not null)
            throw new AppException(UsersErrors.EmailTaken);

        if (!await roles.ExistsAsync(request.RoleId, ct))
            throw new AppException(UsersErrors.RoleNotFound);

        var user = new User
        {
            Email = email,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Phone = request.Phone?.Trim(),
            IsActive = true,
            SecurityStamp = Guid.NewGuid().ToString("N"),
            LastTenantId = currentUser.TenantId
        };
        user.PasswordHash = passwords.Hash(request.Password);

        var member = new TenantMember
        {
            User = user,
            IsActive = true,
            JoinedAt = DateTimeOffset.UtcNow,
            InvitedBy = currentUser.UserId,
            MemberRoles = [new MemberRole { RoleId = request.RoleId }]
        };

        await users.AddMemberAsync(member, ct);   // stages user, member, role
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("User {Email} created", email);
        return ToDto(member);
    }

    public async Task<UserDto> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken ct)
    {
        var member = await users.GetMemberAsync(id, ct) ?? throw new AppException(UsersErrors.NotFound);

        member.User.FirstName = request.FirstName.Trim();
        member.User.LastName = request.LastName.Trim();
        member.User.Phone = request.Phone?.Trim();
        member.IsActive = request.IsActive;

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("User {UserId} updated", id);
        return ToDto(member);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        if (id == currentUser.UserId)
            throw new AppException(UsersErrors.CannotRemoveSelf);

        var member = await users.GetMemberAsync(id, ct) ?? throw new AppException(UsersErrors.NotFound);

        users.RemoveMember(member);   // soft delete
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("User {UserId} removed from tenant", id);
    }

    private static UserDto ToDto(TenantMember m) => new()
    {
        Id = m.UserId,
        Email = m.User.Email,
        FirstName = m.User.FirstName,
        LastName = m.User.LastName,
        Phone = m.User.Phone,
        IsActive = m.IsActive,
        RoleIds = m.MemberRoles.Select(r => r.RoleId).ToList()
    };
}