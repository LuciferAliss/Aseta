using Aseta.API;
using Aseta.API.Extensions;
using Aseta.Application;
using Aseta.Domain;
using Aseta.Domain.Entities.Users;
using Aseta.Infrastructure;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration)
    .AddDomain()
    .AddApplication()
    .AddPresentation(builder.Configuration);

builder.Services.AddOpenApi();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt => opt.WithTitle("JWT + Refresh Token Auth API"));
}

await app.ApplyMigrations();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler();

await app.RunAsync();
