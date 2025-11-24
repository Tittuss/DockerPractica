using System.Text;
using backend.Data;
using backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Conexión a BD SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        connectionString,
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            );
        }
    )
);

// Inyeccion de dependencias
builder.Services.AddScoped<IAuthService, AuthService>();

// configuracion de JWT
var key = builder.Configuration.GetValue<string>("Jwt:Key");
var keyBytes = Encoding.ASCII.GetBytes(key!);

builder
    .Services.AddAuthentication(config =>
    {
        config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(config =>
    {
        config.RequireHttpsMetadata = false; // False porque estamos en entorno de desarrollo/docker interno
        config.SaveToken = true;
        config.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateIssuer = false, // Simplificado
            ValidateAudience = false, // Simplificado
        };
    });

// Servicios basicos Controladores y CORS
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Habilitar CORS para que Svelte (puerto 3000) pueda hablar con .NET (puerto 5000)
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );
});

var app = builder.Build();

//Migración Automática al iniciar
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<ApplicationDbContext>();

    try
    {
        logger.LogInformation("--> Inicializando base de datos...");

        // Bucle de reintento para esperar a SQL Server
        var maxRetries = 7;
        var delay = TimeSpan.FromSeconds(5);

        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                context.Database.EnsureCreated();
                logger.LogInformation("--> ¡Base de datos creada/verificada exitosamente!");
                break;
            }
            catch (Exception ex)
            {
                if (i == maxRetries - 1)
                {
                    logger.LogError(ex, "--> Se agotaron los reintentos. SQL Server no respondió.");
                    throw;
                }

                logger.LogWarning(
                    $"--> SQL Server no está listo o error de conexión. Reintentando en {delay.Seconds}s... (Intento {i + 1}/{maxRetries})"
                );
                System.Threading.Thread.Sleep(delay);
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "--> Ocurrió un error grave al iniciar la base de datos.");
    }
}

app.UseCors("AllowFrontend"); // Activar CORS

app.UseAuthentication(); // Verificar "quién eres"
app.UseAuthorization(); // Verificar "qué puedes hacer"

app.MapControllers();

app.Run();
