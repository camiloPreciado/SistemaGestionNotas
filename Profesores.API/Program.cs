using System.Text;
using Profesores.API.Middlewares;
using Profesores.Application.Interfaces;
using Profesores.Application.Services;
using Profesores.Infrastructure.Persistence;
using Profesores.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFile =
        $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";

    var xmlPath = Path.Combine(
        AppContext.BaseDirectory,
        xmlFile);

    options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "Ingrese el token JWT. Ejemplo: Bearer {token}"
        });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(
                "Bearer",
                document)] = new List<string>()
        });
});


var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "La clave JWT no está configurada.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException(
        "El issuer JWT no está configurado.");

builder.Services.AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtIssuer,
                ValidAudience = jwtIssuer,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey))
            };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "DefaultConnection"),
            sqlOptions =>
            {
                sqlOptions.MigrationsHistoryTable(
                    "__EFMigrationsHistoryProfesores");
            }));


builder.Services.AddScoped<IProfesorRepository, ProfesorRepository>();

builder.Services.AddScoped<IProfesorService, ProfesorService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();