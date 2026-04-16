using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RESTMusicRecord
{
    /// <summary>
    /// This class starts the API and configures all services and middleware.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method that starts the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // ---------------- JWT SETTINGS ----------------
            // Reads the Jwt section from appsettings.json.
            IConfigurationSection jwtSettings = builder.Configuration.GetSection("Jwt");
            byte[] key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

            // ---------------- SERVICES ----------------

            // Adds support for API controllers.
            builder.Services.AddControllers();

            // Adds database connection.
            builder.Services.AddDbContext<MusicRecordDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // Registers the repository.
            builder.Services.AddScoped<IREPOMusicRecords, MusicRecordRepositoryDatabase>();

            // Adds CORS so frontend is allowed to call the API.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Adds JWT authentication.
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
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            // Adds authorization.
            builder.Services.AddAuthorization();

            // Adds Swagger.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            WebApplication app = builder.Build();

            // ---------------- PIPELINE ----------------

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}