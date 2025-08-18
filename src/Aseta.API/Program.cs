using Aseta.API;
using Aseta.API.Extensions;
using Aseta.Application;
using Aseta.Infrastructure;
using Aseta.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddPresentation(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
app.MapOpenApi();
app.MapScalarApiReference(opt =>
{
    opt.WithTitle("JWT + Refresh Token Auth API");
});
await app.ApplyMigrations();
// }

app.MapIdentityApi<UserApplication>();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();