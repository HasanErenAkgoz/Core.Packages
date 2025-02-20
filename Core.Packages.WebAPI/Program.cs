using Core.Packages.Application;
using Core.Packages.Persistence;
using Core.Packages.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});


ConfigureServices(builder);
var app = builder.Build();
ConfigureMiddleware(app);
app.Run();

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();
    builder.Services.AddCoreApplicationServices();
    builder.Services.AddCoreInfrastructureServices(builder.Configuration);
    builder.Services.AddCorePersistenceServices<BaseDbContext>(builder.Configuration);
    builder.Services.AddJWTSettingsService(builder.Configuration);
    builder.Services.AddSwaggerServices(builder.Configuration);

}

void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseCors("AllowAllOrigins");
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(endpoints => endpoints.MapControllers());
}



