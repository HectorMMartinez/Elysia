using Elysia.Core.Application;
using Elysia.Infraestructure.Identity;
using Elysia.Infraestructure.persistences;
using Elysia.Infraestructure.Shared;
using Elysia.Presentation.WebApi.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));


builder.Services.AddOpenApi();
builder.Services.AddEmailServicesIOC(builder.Configuration);
builder.Services.AddIdentityLayerIOCForWebApi(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddPersistencesLayerIOC(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning();
builder.Services.AddSwaggerExtension();
builder.Services.AddVersioningExtensions();
builder.Services.AddServicesLayerIOC();


builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend",
        policy =>
        {
            policy
                 .WithOrigins("http://localhost:5173") //para que funcione el despligue del entorno dev
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});


builder.Services.AddControllers();

var app = builder.Build();
await app.Services.RunIdentitySeed();
await app.Services.RunPersistenceSeed();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExtension(app);
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("PermitirFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks("/health");
app.MapControllers();


app.Run();
