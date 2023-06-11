using Main_Branch_Locator_App.Models;

namespace Main_Branch_Locator_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            

            var builder = WebApplication.CreateBuilder(args);

           

            builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
            {
                
                build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
            

            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<MainBranchLocatorDbContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            
            app.UseCors("corspolicy");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}