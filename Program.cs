using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using WebApi_money_management.Data;
using WebApi_money_management.Middleware;
using WebApi_money_management.Repositories;
using WebApi_money_management.Repositories.Auth;
using WebApi_money_management.Repositories.Dashboard;
using WebApi_money_management.Repositories.Statements;
using WebApi_money_management.Repositories.Statistics;
using WebApi_money_management.Repositories.Transactions;
using WebApi_money_management.Services;
using WebApi_money_management.Services.Auth;
using WebApi_money_management.Services.Dashboard;
using WebApi_money_management.Services.Statements;
using WebApi_money_management.Services.Statistics;
using WebApi_money_management.Services.Transactions;
using WebApi_money_management.Repositories.UserPreferences;
using WebApi_money_management.Services.UserPreferences;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers ──────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddMemoryCache();

// Return { error, message } on model-validation failure (consistent with AppException shape)
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var details = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(
                e => e.Key,
                e => e.Value!.Errors.Select(err => err.ErrorMessage).ToArray());

        return new BadRequestObjectResult(new
        {
            error = "VALIDATION_ERROR",
            message = "One or more validation errors occurred.",
            details
        });
    };
});

// ── OpenAPI ───────────────────────────────────────────────────────────────────
builder.Services.AddOpenApi();

// ── CORS ──────────────────────────────────────────────────────────────────────
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
    ?? ["http://localhost:5173"];

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()));

// ── JWT Authentication ────────────────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("Jwt");
var secret = jwtSection["Secret"]
    ?? throw new InvalidOperationException("Jwt:Secret is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateIssuer = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSection["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async ctx =>
            {
                try
                {
                    var blacklist = ctx.HttpContext.RequestServices
                        .GetRequiredService<ITokenBlacklistRepository>();

                    var raw = ctx.Request.Headers.Authorization.ToString();
                    var token = raw.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                        ? raw[7..] : raw;

                    if (await blacklist.IsRevokedAsync(token))
                        ctx.Fail("Token has been revoked.");
                }
                catch (Exception ex)
                {
                    var logger = ctx.HttpContext.RequestServices
                        .GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Token blacklist check failed");
                    ctx.Fail("Token validation error.");
                }
            },
            OnAuthenticationFailed = ctx =>
            {
                var logger = ctx.HttpContext.RequestServices
                    .GetRequiredService<ILogger<Program>>();
                logger.LogWarning("JWT authentication failed: {Error}", ctx.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });

// ── Database ──────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Repositories ──────────────────────────────────────────────────────────────
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenBlacklistRepository, TokenBlacklistRepository>();

// ── Services ──────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IStatisticsRepository, StatisticsRepository>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IStatementRepository, StatementRepository>();
builder.Services.AddScoped<IStatementService, StatementService>();
builder.Services.AddScoped<IUserPreferenceRepository, UserPreferenceRepository>();
builder.Services.AddScoped<IUserPreferenceService, UserPreferenceService>();

// ─────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DataSeeder.SeedAsync(db);
}

app.Run();
