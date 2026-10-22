using Application.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class HasPermissionAttribute(string permission) : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var service = context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();

        if (!await service.HasAsync(permission, context.HttpContext.RequestAborted))
            context.Result = new ForbidResult();
    }
}