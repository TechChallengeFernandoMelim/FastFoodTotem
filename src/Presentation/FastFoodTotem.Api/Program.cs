using FastFoodTotem.Api.HealthCheck;
using FastFoodTotem.Api.Middlewares;
using FastFoodTotem.Domain;
using FastFoodTotem.Infra.IoC;
using FastFoodTotem.Infra.SqlServer.Database;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostContext, config) =>
{
    const string fileSettingsName = "appsettings";
    config
        .AddUserSecrets<Program>(optional: true)
        .AddJsonFile($"{fileSettingsName}.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"{fileSettingsName}.{hostContext.HostingEnvironment.EnvironmentName}.json")
        .AddEnvironmentVariables();
});

builder.Host.ConfigureLogging(logging =>
{
    logging
        .ClearProviders()
        .AddConsole();
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var conStr = Environment.GetEnvironmentVariable("SqlServerConnection");
if (string.IsNullOrWhiteSpace(conStr))
    throw new InvalidOperationException(
        $"Could not find a connection string named 'SqlServerConnection'.");

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(setup =>
    {
        setup.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Fast Food Totem Api",
                Version = "v1",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                {
                    //TODO - Add e-mail and name
                    Email = "",
                    Name = "",
                }
            });

        var filePath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
        setup.IncludeXmlComments(filePath);
    })
    .AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    })
    .AddHealthChecks()
    .AddCheck<SimpleHealthCheck>(
        "HealthCheck",
        tags: new[] { "HealthCheck" })
    .AddSqlServer(conStr);

builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FastFoodContext>();
    db.Database.Migrate();
}

//if (app.Environment.IsDevelopment())
app.UseSwagger().UseSwaggerUI();

app.UseRouting();

app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseEndpoints(endpoints =>
{
    app.MapHealthChecks("health/readiness", new HealthCheckOptions
    {
        Predicate = healthCheck => healthCheck.Tags.Contains("HealthCheck"),
    });

    app.MapHealthChecks("health/liveness", new HealthCheckOptions
    {
        Predicate = healthCheck => healthCheck.Tags.Contains("HealthCheck"),
    });

    endpoints.MapControllers();
});

app.UseHttpsRedirection();

app.MapControllerRoute(name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();