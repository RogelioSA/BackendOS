using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi;
using nest.core.aplication.auth;
using nest.core.dominio.Contabilidad.CuentaContableEntities;
using nest.core.security.Extensions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var assembly = Assembly.GetExecutingAssembly();

bool hasBaseUrl = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("BASE_URL"));

if (Environment.GetEnvironmentVariable("IS_LAMBDA") != null)
    builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

// Add services custom
builder.Configuration.AddJsonFile("appsettings.json", optional: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                     .AddUserSecrets<Program>()
                     .AddEnvironmentVariables();

DbContextSelector.SelectProvider(builder, !MigrationService.IsMigration());

builder.Services.ConfigureAplication(builder.Configuration);
builder.Services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("Content-Disposition", "X-Custom-Header", "Authorization");
        builder.SetIsOriginAllowed(_ => true);
    });
});
builder.Services.AddHttpClient($"admService", client =>
{
    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("URL_ENDPOINT") ?? "");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});
// End services custom

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }); ;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    string product = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
    c.SwaggerDoc("/", new OpenApiInfo
    {
        Title = $"{product} - Principal",
        Version = assembly.GetName().Version?.ToString(),
        Contact = new OpenApiContact
        {
            Name = "Gabriel Rodriguez Riveros",
            Email = "gabogth@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/gabogth")
        },
        Description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description
    });
    foreach (var endpoint in ConfigureEndpoints.Endpoints)
    {
        c.SwaggerDoc(endpoint.Key, new OpenApiInfo
        {
            Title = $"{product} - {endpoint.Value.Title}",
            Version = assembly.GetName().Version?.ToString(),
            Contact = new OpenApiContact
            {
                Name = "Gabriel Rodriguez Riveros",
                Email = "gabogth@gmail.com",
                Url = new Uri("https://www.linkedin.com/in/gabogth")
            },
            Description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description
        });
    }
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        if (docName == "main") return true;
        var relativePath = apiDesc.RelativePath;
        if (string.IsNullOrEmpty(relativePath)) return false;
        return relativePath.StartsWith(docName, StringComparison.OrdinalIgnoreCase);
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese el token JWT en este formato: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement((document) => new OpenApiSecurityRequirement()
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
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
builder.Services
    .AddGraphQLServer()
    .AddDataSources()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddErrorFilter(err =>
    {
        if (err.Exception is not null)
            return err.WithMessage(err.Exception.ToString());
        return err;
    });
builder.Services
    .AddControllers()
    .AddOData(options =>
        options.AddRouteComponents("odata", GetEdmModel())
               .Select().Filter().OrderBy().Expand().Count().SetMaxTop(null));

var app = builder.Build();
//await MigrationResolver.ExecuteMigration(app);
if(hasBaseUrl) app.UsePathBase(Environment.GetEnvironmentVariable("BASE_URL"));
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    foreach (var endpoint in ConfigureEndpoints.Endpoints)
        c.SwaggerEndpoint($"./{endpoint.Key}/swagger.json", endpoint.Value.Title);
});
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapGraphQL($"/graphql");
app.MapNitroApp($"/my-graphql-ui");
app.MapHealthChecks("/health/live", new HealthCheckOptions {
    ResponseWriter = async (ctx, report) =>
    {
        ctx.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            results = report.Entries.Select(e => new
            {
                key = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        });
        await ctx.Response.WriteAsync(result);
    }
});
app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();
app.Run();

static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<CuentaContable>("CuentaContable");
    return builder.GetEdmModel();
}