using Application.Common.Responses;
using Application.Users;
using Application.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IUserService users) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ListAsync(CancellationToken ct) =>
        Ok(ApiResponse.Ok(await users.ListAsync(ct)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken ct) =>
        Ok(ApiResponse.Ok(await users.GetByIdAsync(id, ct)));

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateUserRequest request, CancellationToken ct) =>
        Ok(ApiResponse.Ok(await users.CreateAsync(request, ct)));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken ct) =>
        Ok(ApiResponse.Ok(await users.UpdateAsync(id, request, ct)));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken ct)
    {
        await users.DeleteAsync(id, ct);
        return Ok(ApiResponse.Ok());
    }
}