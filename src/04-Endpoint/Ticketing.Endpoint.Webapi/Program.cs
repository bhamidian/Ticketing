using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Ticketing.Domain.Entites;
using Ticketing.Infrastructure.Efcore;
using Ticketing.Infrastructure.Efcore.Persistence;
using Ticketing.Infrastructure.JwtService;
using Ticketing.Infrastructure.JwtService.Options;
using Ticketing.Application;
using Ticketing.Endpoint.Webapi.Middleware;
using Ticketing.Endpoint.Webapi.Mapping;
using Ticketing.Endpoint.Webapi.Filters;
using Serilog;
using Ticketing.Framework;
using Ticketing.Infrastructure.FileService.Contracts;
using Ticketing.Infrastructure.FileService.Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddControllers(options => options.Filters.Add<ApiActionLoggingFilter>())
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var problem = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "اطلاعات واردشده معتبر نیست",
                Detail = "لطفاً اطلاعات را بررسی و دوباره تلاش کنید.",
                Instance = context.HttpContext.Request.Path
            };
            return new BadRequestObjectResult(problem);
        };
    });
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ticketing API",
        Version = "v1"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter the access token returned by the login endpoint.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    options.OperationFilter<AuthorizeOperationFilter>();
});
builder.Services.AddApplication();
builder.Services.AddJwtService(builder.Configuration);
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
    .Configure<IOptions<JwtOptions>>((options, jwt) =>
    {
        var jwtOptions = jwt.Value;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };
    });
builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrWhiteSpace(connectionString))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
    builder.Services.AddIdentityCore<AppUser>()
        .AddRoles<IdentityRole<Guid>>()
        .AddErrorDescriber<PersianIdentityErrorDescriber>()
        .AddEntityFrameworkStores<ApplicationDbContext>();
    builder.Services.AddEfCoreRepositories();
}

var app = builder.Build();

if (app.Environment.IsDevelopment() && builder.Configuration.GetValue<bool>("SeedUsers:Enabled"))
{
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Configure ConnectionStrings:DefaultConnection before enabling runtime seeding.");

    await using var scope = app.Services.CreateAsyncScope();
    await scope.ServiceProvider.GetRequiredService<DatabaseSeeder>().SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApplicationExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
