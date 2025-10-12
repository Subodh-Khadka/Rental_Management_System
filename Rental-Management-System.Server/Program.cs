using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rental_Management_System.Server.Data;
using Rental_Management_System.Server.MappingProfiles;
using Rental_Management_System.Server.Models;
using Rental_Management_System.Server.Repositories.CharegTemplate;
using Rental_Management_System.Server.Repositories.MonthlyCharge;
using Rental_Management_System.Server.Repositories.PaymentTransaction;
using Rental_Management_System.Server.Repositories.RentalContract;
using Rental_Management_System.Server.Repositories.RentPayment;
using Rental_Management_System.Server.Repositories.Room;
using Rental_Management_System.Server.Repositories.Tenant;
using Rental_Management_System.Server.Services.ChargeTemplate;
using Rental_Management_System.Server.Services.MonthlyCharge;
using Rental_Management_System.Server.Services.PaymentTransaction;
using Rental_Management_System.Server.Services.RentalContract;
using Rental_Management_System.Server.Services.RentPayment;
using Rental_Management_System.Server.Services.Room;
using Rental_Management_System.Server.Services.Tenant;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace Rental_Management_System.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add DbContext
            builder.Services.AddDbContext<RentalDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<RentalDbContext>()
            .AddDefaultTokenProviders();

            // Add JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

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



            // Add controllers
            builder.Services.AddControllers();

            // Register repositories
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<ITenantRepository, TenantRepository>();
            builder.Services.AddScoped<IRentalContractRepository, RentalContractRepository>();
            builder.Services.AddScoped<IChargeTemplateRepository, ChargeTemplateRepository>();
            builder.Services.AddScoped<IRentPaymentRepository, RentPaymentRepository>();
            builder.Services.AddScoped<IMonthlyChargeRepository, MonthlyChargeRepository>();
            builder.Services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();

            // Register AutoMapper profiles
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<RoomProfile>();
                cfg.AddProfile<TenantProfile>();
                cfg.AddProfile<RentalContractProfile>();
                cfg.AddProfile<ChargeTemplateProfile>();
                cfg.AddProfile<MonthlyChargeProfile>();
                cfg.AddProfile<RentPaymentProfile>();
                cfg.AddProfile<PaymentTransactionProfile>();
            });

            // Register services
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<ITenantService, TenantService>();
            builder.Services.AddScoped<IRentalContractService, RentalContractService>();
            builder.Services.AddScoped<IChargeTemplateService, ChargeTemplateService>();
            builder.Services.AddScoped<IMonthlyChargeService, MonthlyChargeService>();
            builder.Services.AddScoped<IRentPaymentService, RentPaymentService>();
            builder.Services.AddScoped<IPaymentTransactionService, PaymentTransactionService>();

            // Configure Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new() { Title = "Rental Management API", Version = "v1" });

                // ✅ Add JWT Authorization button in Swagger UI
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter JWT token like: Bearer {your token}"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });


            // Configure CORS for React frontend
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy => policy
                        .WithOrigins("http://localhost:5173") // frontend-url
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            var app = builder.Build();

            // Middleware pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Enable CORS
            app.UseCors("AllowReactApp");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // Serve static files and SPA fallback
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
