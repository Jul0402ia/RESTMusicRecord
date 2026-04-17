using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

namespace RESTMusicRecord
{
    public class Program
    {
        /// <summary>
        /// Main metode som starter hele API'en
        /// </summary>
        /// <param name="args">Det programmet modtager ved opstart</param>
        public static void Main(string[] args)
        {
            // Opretter builder som bruges til at sætte programmet op
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // ---------------- JWT ----------------

            // Henter Jwt sektionen fra appsettings.json
            IConfigurationSection jwtSettings = builder.Configuration.GetSection("Jwt");

            // Laver secret key om til bytes så systemet kan bruge den
            byte[] key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            // ---------------- SERVICES ----------------

            // Tilføjer controller support så endpoints virker
            builder.Services.AddControllers();

            // Tilføjer database forbindelse til SQL Server
            builder.Services.AddDbContext<MusicRecordDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // Registrerer repository så controller kan bruge database metoder
            builder.Services.AddScoped<IREPOMusicRecords, MusicRecordRepositoryDatabase>();

            // Tilføjer CORS så frontend må kontakte API'en
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()   // alle må kalde API
                          .AllowAnyMethod()   // GET POST PUT DELETE osv
                          .AllowAnyHeader();  // alle headers tilladt
                });
            });

            // Tilføjer JWT login system
            builder.Services.AddAuthentication(options =>
            {
                // Sætter JWT som standard login metode
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Regler for hvordan token bliver tjekket
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, // tjek hvem der har lavet token
                    ValidateAudience = true, // tjek hvem token er lavet til
                    ValidateLifetime = true, // tjek om token er udløbet
                    ValidateIssuerSigningKey = true, // tjek secret key

                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    // Fortæller hvad username hedder i token
                    NameClaimType = ClaimTypes.Name,

                    // Fortæller hvad role hedder i token
                    RoleClaimType = ClaimTypes.Role
                };
            });

            // Tilføjer adgangskontrol med Authorize
            builder.Services.AddAuthorization();

            // Tilføjer Swagger så vi kan teste API
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                // Laver Authorize knap i Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Skriv: Bearer dit_token"
                });

                // Gør at Swagger sender token med requests
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            // Bygger selve appen færdig
            WebApplication app = builder.Build();

            // ---------------- PIPELINE ----------------

            // Starter Swagger side
            app.UseSwagger();
            app.UseSwaggerUI();

            // Tvinger HTTPS
            app.UseHttpsRedirection();

            // Aktiverer CORS
            app.UseCors("AllowAll");

            // Tjekker login token
            app.UseAuthentication();

            // Tjekker adgang til endpoints
            app.UseAuthorization();

            // Mapper controllers til endpoints
            app.MapControllers();

            // Starter programmet
            app.Run();
        }
    }
}