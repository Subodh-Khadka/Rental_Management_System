
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rental_Management_System.Server.Data;
using Rental_Management_System.Server.MappingProfiles;
using Rental_Management_System.Server.Middleware;
using Rental_Management_System.Server.Repositories.Room;
using Rental_Management_System.Server.Services.Room;

namespace Rental_Management_System.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<RentalDbContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddControllers();

            //added
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<RoomProfile>();
            });
            builder.Services.AddScoped<IRoomService, RoomService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            using var app = builder.Build();

            // middleware
            app.UseMiddleware<ExceptionMiddleware>();


            // seeding data
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<RentalDbContext>();
                db.Database.Migrate(); // Ensure DB is up-to-date
                db.SeedData();         // Seed initial data
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
