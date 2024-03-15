
using CLED.OnlineShop.API.Endpoints;
using CLED.OnlineShop.API.Services;

namespace CLED.OnlineShop.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<CustomerService>();
        builder.Services.AddScoped<OrderService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapGet("/", () => "Hello World!");
        // questo lo abbiamo creato noi nella cartella endpoints
        app.MapCustomerEndpoints();
        app.MapOrdersEndpoints();
        app.Run();

    }
}
