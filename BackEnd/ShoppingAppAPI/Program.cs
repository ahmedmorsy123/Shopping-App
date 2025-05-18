using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using ShoppingAppBussiness;
using ShoppingAppDB;
using ShoppingAppDB.Services;
using System.Text;

public class Program
{
    public static int Main(string[] args)
    {
        // Configure Serilog first
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.File(
                path: "C:/Users/ENG Ahmed/source/repos/ShoppingApp/Logs/Server Logs/app.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .CreateLogger();

        try
        {
            Log.Information("Starting web host");

            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog as the logging provider for the host
            builder.Host.UseSerilog();

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<UsersService>();
            builder.Services.AddScoped<ProductsService>();
            builder.Services.AddScoped<OrdersService>();
            builder.Services.AddScoped<CartsService>();
            builder.Services.AddScoped<AuthService>();

            builder.Services.AddScoped<UserData>();
            builder.Services.AddScoped<ProductData>();
            builder.Services.AddScoped<OrderData>();
            builder.Services.AddScoped<CartData>();

            builder.Services.AddScoped<Auth>();
            builder.Services.AddScoped<Password>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Auth:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Auth:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Auth:Token"]!)),
                    ValidateIssuerSigningKey = true
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();

            return 0; // Indicate successful execution
        }
        catch (Exception ex)
        {
            Console.WriteLine("Application startup failed:");
            Console.WriteLine(ex.ToString());
            Log.Fatal(ex, "Host terminated unexpectedly");
            return 1; // Indicate an error occurred
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}