using CLED.OnlineShop.API.Models;
using CLED.OnlineShop.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CLED.OnlineShop.API.Endpoints
{
    public static class OrdersEndpoint
    {
        public static IEndpointRouteBuilder MapOrdersEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("api/v1/orders")
                .WithTags("Orders")
                .WithOpenApi();

            group.MapGet("/", async (OrderService data) => await GetOrdersAsync(data)).WithName("GetOrders")
            .WithSummary("Get all Orders")
            .WithDescription("Returns the complete Order list");

            group.MapGet("/{id:int}", GetOrderAsync)
                .WithName("GetOrder");

            group.MapPost("/", InsertOrderAsync)
                .WithName("InsertOrder")
                .WithSummary("Creates the Order");

            group.MapPut("/{id:int}", UpdateOrderAsync)
                .WithName("UpdateOrder")
                .WithSummary("Updates the Order");

            group.MapDelete("/{id:int}", DeleteOrderAsync)
                .WithName("DeleteOrder")
                .WithSummary("Deletes the Order");

            return builder;
        }
        private static async Task<Ok<IEnumerable<Order>>> GetOrdersAsync(OrderService data)
        {
            // typed results è più comodo perchè ci permette di restituire un oggetto con un messaggio di errore
            return TypedResults.Ok(await data.GetOrdersAsync());
        }
        private static async Task<Results<Ok<Order>, NotFound>> GetOrderAsync(int id, OrderService data)
        {
            var Order = await data.GetOrderAsync(id);
            if (Order is null)
                return TypedResults.NotFound();
            return TypedResults.Ok(Order);
        }
        private static async Task<Created> InsertOrderAsync(Order Order, OrderService data)
        {
            await data.InsertOrderAsync(Order);
            return TypedResults.Created();
        }

        // Results mi permette di elencare le possibili risposte che posso ottenere
        private static async Task<Results<NoContent, NotFound>> UpdateOrderAsync(int id, Order Order, OrderService data)
        {
            if (await data.GetOrderAsync(id) is null)
                return TypedResults.NotFound();

            await data.UpdateOrderAsync(id, Order);
            return TypedResults.NoContent();
        }
        private static async Task<Results<NoContent, NotFound>> DeleteOrderAsync(int id, OrderService data)
        {
            if (await data.GetOrderAsync(id) is null)
                return TypedResults.NotFound();
            await data.DeleteOrderAsync(id);
            return TypedResults.NoContent();
        }
    }
}
