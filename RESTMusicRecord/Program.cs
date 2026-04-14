using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RESTMusicRecord
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // ---------------- SERVICES ----------------
            // Her registrerer vi de services, som programmet skal bruge

            // Tilføjer controller support
            // Det gør, at vi kan bruge API controllers i projektet
            builder.Services.AddControllers();

            // Opretter forbindelse til databasen via connection string
            // DefaultConnection skal findes i appsettings.json
            builder.Services.AddDbContext<MusicRecordDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Fortæller systemet, hvilken repository der skal bruges,
            // når nogen beder om IREPOMusicRecords
            builder.Services.AddScoped<IREPOMusicRecords, MusicRecordRepositoryDatabase>();

            // Tilføjer CORS policy
            // Det gør, at frontend må kalde API et
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });

            // Tilføjer JWT authentication
            // Her fortæller vi programmet, hvordan det skal validere token
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Vi holder det simpelt i denne opgave
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        // Hemmelig nøgle som bruges til at validere token
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("THIS_IS_A_SECRET_KEY_12345678901234567890"))
                    };
                });

            // Tilføjer Swagger
            // Bruges til at teste API et i browseren
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Bygger appen
            WebApplication app = builder.Build();

            // ---------------- PIPELINE ----------------
            // Pipeline er rækkefølgen af middleware

            // Starter Swagger
            // Så vi kan åbne /swagger og teste endpoints
            app.UseSwagger();
            app.UseSwaggerUI();

            // Aktiverer CORS policy
            app.UseCors("AllowAll");

            // Kunne bruges hvis vi ville tvinge HTTPS
            // app.UseHttpsRedirection();

            // Tjekker om brugeren sender gyldigt token
            app.UseAuthentication();

            // Tjekker om brugeren har adgang til endpointet
            app.UseAuthorization();

            // Mapper controllers til endpoints
            app.MapControllers();

            // Starter API et
            app.Run();
        }
    }
}