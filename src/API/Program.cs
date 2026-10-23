using API;
using API.Logging;
using API.Middleware;
using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApi(builder.Configuration, builder.Environment);
builder.Host.UseApiLogging();

var app = builder.Build();

if (args.Contains("--seed"))
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();

    await seeder.SeedRolesAsync(CancellationToken.None);

    if (app.Environment.IsDevelopment())
        await seeder.SeedDevelopmentAsync(CancellationToken.None);

    return;   // seed and exit, don't start the server
}

app.UseExceptionHandler();
app.UseSerilogRequestLogging();
app.UseStatusCodePages(StatusCodeResponseWriter.WriteAsync);

app.UseHttpsRedirection();

// CORS, Authentication, Authorization, tenant middleware go here (next steps)
app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

