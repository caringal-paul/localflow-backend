using API.Auth;
using Application.Auth;
using Application.Common.Responses;
using Application.Customers;
using Application.Customers.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("customers")]
public class CustomersController(ICustomerService customers) : ControllerBase
{
    [HttpGet, HasPermission(Permissions.CustomersView)]
    public async Task<IActionResult> List([FromQuery] ListCustomersQuery query, CancellationToken ct) =>
        Ok(ApiResponse.Ok(await customers.ListAsync(query, ct)));

    [HttpGet("{code}"), HasPermission(Permissions.CustomersView)]
    public async Task<IActionResult> Get(string code, CancellationToken ct) =>
        Ok(ApiResponse.Ok(await customers.GetByCodeAsync(code, ct)));

    [HttpPost, HasPermission(Permissions.CustomersManage)]
    public async Task<IActionResult> Create(SaveCustomerRequest request, CancellationToken ct) =>
        Ok(ApiResponse.Ok(await customers.CreateAsync(request, ct)));

    [HttpPut("{code}"), HasPermission(Permissions.CustomersManage)]
    public async Task<IActionResult> Update(string code, SaveCustomerRequest request, CancellationToken ct) =>
        Ok(ApiResponse.Ok(await customers.UpdateAsync(code, request, ct)));

    [HttpDelete("{code}"), HasPermission(Permissions.CustomersManage)]
    public async Task<IActionResult> Delete(string code, CancellationToken ct)
    {
        await customers.DeleteAsync(code, ct);
        return Ok(ApiResponse.Ok());
    }

    [HttpPost("{code}/addresses"), HasPermission(Permissions.CustomersManage)]
    public async Task<IActionResult> AddAddress(string code, SaveCustomerAddressRequest request, CancellationToken ct) =>
        Ok(ApiResponse.Ok(await customers.AddAddressAsync(code, request, ct)));

    [HttpPut("{code}/addresses/{addressId:guid}"), HasPermission(Permissions.CustomersManage)]
    public async Task<IActionResult> UpdateAddress(string code, Guid addressId, SaveCustomerAddressRequest request, CancellationToken ct) =>
        Ok(ApiResponse.Ok(await customers.UpdateAddressAsync(code, addressId, request, ct)));

    [HttpPut("{code}/preferences"), HasPermission(Permissions.CustomersManage)]
    public async Task<IActionResult> UpdatePreferences(string code, SaveCustomerPreferenceRequest request, CancellationToken ct) =>
        Ok(ApiResponse.Ok(await customers.UpdatePreferencesAsync(code, request, ct)));

    [HttpDelete("{code}/addresses/{addressId:guid}"), HasPermission(Permissions.CustomersManage)]
    public async Task<IActionResult> DeleteAddress(string code, Guid addressId, CancellationToken ct)
    {
        await customers.DeleteAddressAsync(code, addressId, ct);
        return Ok(ApiResponse.Ok());
    }
}