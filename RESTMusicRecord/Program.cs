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

        }
    }
}




