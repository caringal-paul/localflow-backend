using API;
using API.Middleware;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApi(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages(StatusCodeResponseWriter.WriteAsync);

app.UseHttpsRedirection();

// CORS, Authentication, Authorization, tenant middleware go here (next steps)
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();