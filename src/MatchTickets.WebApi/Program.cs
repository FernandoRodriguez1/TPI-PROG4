using MatchTickets.Application.Interfaces;
using MatchTickets.Application.Services;
using MatchTickets.Domain.Interfaces;
using MatchTickets.Infraestructure;
using MatchTickets.Infraestructure.Authentication;
using MatchTickets.Infraestructure.Data;
using MatchTickets.Infraestructure.External_Services;
using MatchTickets.Infraestructure.Repositories;
using MatchTickets.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using Polly;
using Polly.Extensions.Http;
using Polly.CircuitBreaker;

var builder = WebApplication.CreateBuilder(args);

//asegura que se lean correctamente las variables de entorno de Azure
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();

builder.Services.AddAuthorization(options =>
{
    // politicas para los roles

    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("ClientPolicy", policy =>
        policy.RequireRole("Client"));

    // ambos roles
    options.AddPolicy("BothPolicy", policy =>
        policy.RequireRole("Admin", "Client"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContextCR>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));




builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MatchTickets API", Version = "v1" });

    // configuracion para JWT Bearer
    c.AddSecurityDefinition("MatchTicketsAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Description = "Ingrese el Token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "MatchTicketsAuth"
                }
            },
            new string[] {}
        }
    });
});

// configuracion de JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
           Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])
        ),
        RoleClaimType = ClaimTypes.Role // necesario para que Authorize(Roles="Client") funcione
    };
});

#region
builder.Services.AddTransient<GlobalExceptionMiddleware>();
#endregion
#region
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClubRepository, ClubRepository>();
builder.Services.AddScoped<IMembershipCardRepository, MembershipCardRepository>();
builder.Services.AddScoped<ISoccerMatchRepository, SoccerMatchRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
#endregion

#region
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IClubService , ClubService>();
builder.Services.AddScoped<IMembershipCardService, MembershipCardService>();
builder.Services.AddScoped<ISoccerMatchService, SoccerMatchService>();
builder.Services.AddScoped<ITicketService, TicketService>();
#endregion



#region 

// vincula las opciones de SendGrid con las de appsettings IOptions Pattern
builder.Services.Configure<SendGridOptions>(
    builder.Configuration.GetSection("SendGrid"));

// 2) configuracion de resiliencia para SendGrid
var sendGridResilienceConfig = new ApiClientConfiguration
{
    RetryCount = 3,
    RetryAttemptInSeconds = 1,
    HandledEventsAllowedBeforeBreaking = 2,
    DurationOfBreakInSeconds = 30
};

// registra IMailService y SendGridService
// y crea un HttpClient administrado por IHttpClientFactory
builder.Services.AddHttpClient<IMailService, SendGridService>(client =>
{
    client.BaseAddress = new Uri("https://api.sendgrid.com/v3/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})

// patrones Retry y Circuit Breaker
.AddPolicyHandler(PollyResiliencePolicies.GetRetryPolicy(sendGridResilienceConfig))
.AddPolicyHandler(PollyResiliencePolicies.GetCircuitBreakerPolicy(sendGridResilienceConfig));

#endregion



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DbContextCR>();
    if (dbContext.Database.IsRelational())
    {
        dbContext.Database.Migrate();
    }

}

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthentication();   
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();
