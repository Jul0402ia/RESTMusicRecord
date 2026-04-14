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

            // Tilføjer controller support (så vi kan bruge API controllers)
            builder.Services.AddControllers();

            // Opretter forbindelse til database via connection string
            builder.Services.AddDbContext<MusicRecordDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Fortæller systemet at vores interface bruger denne repository
            builder.Services.AddScoped<IREPOMusicRecords, MusicRecordRepositoryDatabase>();

            // CORS så frontend må snakke med vores API
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
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Vi holder det simpelt 
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        // Hemmelig nøgle til token
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("THIS_IS_A_SECRET_KEY_12345678901234567890"))
                    };
                });

            // Swagger til at teste vores API i browser
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            WebApplication app = builder.Build();


            // ---------------- PIPELINE ----------------
            // Pipeline er rækkefølgen af middleware

            // Starter Swagger UI
            app.UseSwagger();
            app.UseSwaggerUI();

            // Aktiverer CORS
            app.UseCors("AllowAll");

            // app.UseHttpsRedirection();

            // Tjekker om brugeren er logget ind
            app.UseAuthentication();

            // Tjekker om brugeren har adgang
            app.UseAuthorization();

            // Mapper vores controllers til endpoints
            app.MapControllers();

            // Starter API'et
            app.Run();
        }
    }
}