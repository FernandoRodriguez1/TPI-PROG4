using MatchTickets.Application.Interfaces;
using MatchTickets.Application.Services;
using MatchTickets.Domain.Interfaces;
using MatchTickets.Infraestructure.Authentication;
using MatchTickets.Infraestructure.Data;
using MatchTickets.Infraestructure.External_Services;
using MatchTickets.Infraestructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SendGrid.Helpers.Mail;
using System;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


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
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese 'Bearer' seguido de su token JWT. Ejemplo: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new Exception("Jwt:Key no está configurada en Azure App Settings");
}

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
if (string.IsNullOrEmpty(jwtIssuer))
{
    throw new Exception("Jwt:Issuer no está configurada en Azure App Settings");
}

var jwtAudience = builder.Configuration["Jwt:Audience"];
if (string.IsNullOrEmpty(jwtAudience))
{
    throw new Exception("Jwt:Audience no está configurada en Azure App Settings");
}

// Configuración de JWT
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
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        ),
        RoleClaimType = ClaimTypes.Role // necesario para que Authorize(Roles="Client") funcione
    };
});


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
// configura las opciones de SendGrid mediante nuestras credenciales del appsettings.json
builder.Services.Configure<SendGridOptions>(builder.Configuration.GetSection("SendGrid"));

// registra y vincula SendGridService con HttpClient
// evita crear infinitos HttpClient y los maneja internamente
builder.Services.AddHttpClient<IMailService, SendGridService>(client =>
{
    client.BaseAddress = new Uri("https://api.sendgrid.com/v3/"); // URL base de la API de SendGrid
    // configura los headers por defecto para no tener que pasarlos en cada request
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// integra con Dependency Injection que cuando se inyecta IMailService en algún servicio, automáticamente recibe un HttpClient configurado


#endregion


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
    if (dbContext.Database.IsRelational())
    {
        dbContext.Database.Migrate();
    }

}

// middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MatchTickets API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();   
app.UseAuthorization();

app.MapControllers();

app.Run();
