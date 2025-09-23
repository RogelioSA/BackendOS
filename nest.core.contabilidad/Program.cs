using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using nest.core.aplication.auth;
using nest.core.contabilidad.Extensions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("Iniciando aplicación Contabilidad...");
if (Environment.GetEnvironmentVariable("IS_LAMBDA") != null)
    builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
// Add services custom
builder.Configuration.AddJsonFile("appsettings.json", optional: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                     .AddUserSecrets<Program>()
                     .AddEnvironmentVariables();
DbContextSelector.SelectProvider(builder, true);
builder.Services.ConfigureAplication(builder.Configuration);
builder.Services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
// End services custom

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }); ;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    string proyecto = Assembly.GetExecutingAssembly().GetName().Name.Split('.')[2];
    proyecto = char.ToUpper(proyecto[0]) + proyecto.Substring(1).ToLower();
    string apiName = $"{proyecto} Api";
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = apiName,
        Version = $"v{Assembly.GetExecutingAssembly().GetName().Version}",
        Description = $"La {apiName} permite gestionar el plan contable del sistema. Todos los endpoints requieren autorización.",
        Contact = new OpenApiContact { Email = "gabogth@gmail.com", Name = "Gabriel Rodriguez", Url = new Uri("https://es.stackoverflow.com/users/30423") }
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese el token JWT en este formato: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement(){{
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] {} }
    });
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.SaveToken = true;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        SaveSigninToken = true,
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("BASE_URL")))
    app.UsePathBase(Environment.GetEnvironmentVariable("BASE_URL"));
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = check => check.Tags.Contains("live") });
app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();
app.Run();
