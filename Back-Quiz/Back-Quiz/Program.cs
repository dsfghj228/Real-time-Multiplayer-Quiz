

using System.Text.Json.Serialization;
using Back_Quiz.Data;
using Back_Quiz.Enums;
using Back_Quiz.Interfaces;
using Back_Quiz.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonNumberEnumConverter<Difficulty>());
    });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo {Title = "Quiz", Version = "v1"});
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"));
});

builder.Services.AddHttpClient();

builder.Services.AddHangfire(config => 
    config.UsePostgreSqlStorage(c =>
        c.UseNpgsqlConnection(builder.Configuration.GetConnectionString("PostgreSQL"))));
builder.Services.AddHangfireServer();

builder.Services.AddScoped<IImportDataService, ImportDataService>();

var app = builder.Build();

app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<IImportDataService>(
    "ImportDataJob",
    service => service.ImportDataAsync(),
    Cron.Minutely
);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("v1/swagger.json", "Quiz V1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();