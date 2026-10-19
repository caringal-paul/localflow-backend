using Application.Common.Enums;
using Application.Common.Errors;

namespace Application.Users;

public static class UsersErrors
{
    public static readonly Error NotFound =
        new("users.not_found", "User not found.", ErrorType.NotFound);

    public static readonly Error EmailTaken =
        new("users.email_taken", "Email is already in use.", ErrorType.Conflict);

    public static readonly Error RoleNotFound =
        new("users.role_not_found", "Role not found.", ErrorType.NotFound);

    public static readonly Error CannotRemoveSelf =
        new("users.cannot_remove_self", "You cannot remove your own account.", ErrorType.Conflict);
}