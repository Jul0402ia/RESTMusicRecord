using Microsoft.AspNetCore.Builder;

namespace RESTMusicRecord
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var builder = WebApplication.CreateBuilder(args);

            // adds controller support 
            builder.Services.AddControllers();

            // add cors policy
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

            // Add swagger services 
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Enables Swagger in development
            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enables CORS
            app.UseCors("AllowAll");

            //app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}




