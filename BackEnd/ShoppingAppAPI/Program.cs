using Serilog;
using Serilog.Events;
using ShoppingAppBussiness;
using ShoppingAppDB;

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
                path: "C:/Users/ENG Ahmed/source/repos/ShoppingAppDB/Logs/Server Logs/app.txt",
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

            builder.Services.AddScoped<Users>();
            builder.Services.AddScoped<Products>();
            builder.Services.AddScoped<Orders>();
            builder.Services.AddScoped<Carts>();

            builder.Services.AddScoped<UserData>();
            builder.Services.AddScoped<ProductData>();
            builder.Services.AddScoped<OrderData>();
            builder.Services.AddScoped<CartData>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();

            return 0; // Indicate successful execution
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            return 1; // Indicate an error occurred
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}