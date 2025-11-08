using Aseta.API;
using Aseta.API.Extensions;
using Aseta.Application;
using Aseta.Domain;
using Aseta.Domain.Entities.Users;
using Aseta.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDomain()
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddPresentation(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer(); //
builder.Services.AddSwaggerGen(); // 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.WithTitle("JWT + Refresh Token Auth API");
    });
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.ApplyMigrations();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapGroup("/auth").MapIdentityApi<UserApplication>();

app.MapControllers();

await app.RunAsync();