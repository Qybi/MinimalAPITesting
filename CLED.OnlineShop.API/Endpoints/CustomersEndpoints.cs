using CLED.OnlineShop.API.Models;
using CLED.OnlineShop.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CLED.OnlineShop.API.Endpoints
{
    public static class CustomersEndpoints
    {
        // extension method per spostare fuori dal program.cs la configurazione degli endpoint
        public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("api/v1/customers")
                .WithTags("Customers")
                .WithOpenApi();

            group.MapGet("/", async (CustomerService data) => await GetCustomersAsync(data)).WithName("GetCustomers")
            .WithSummary("Get all customers")
            .WithDescription("Returns the complete customer list");

            //group.MapGet("/{id:int}", async (int id, CustomerService data) => await GetCustomerAsync(id, data));\
            group.MapGet("/{id:int}", GetCustomerAsync)
                .WithName("GetCustomer");

            group.MapPost("/", InsertCustomerAsync)
                .WithName("InsertCustomer")
                .WithSummary("Creates the customer");

            group.MapPut("/{id:int}", UpdateCustomerAsync)
                .WithName("UpdateCustomer")
                .WithSummary("Updates the customer");

            group.MapDelete("/{id:int}", DeleteCustomerAsync)
                .WithName("DeleteCustomer")
                .WithSummary("Deletes the customer");

            return builder;
        }

        private static async Task<Ok<IEnumerable<Customer>>> GetCustomersAsync(CustomerService data)
        { 
            // typed results è più comodo perchè ci permette di restituire un oggetto con un messaggio di errore
            return TypedResults.Ok(await data.GetCustomersAsync());
        }
        private static async Task<Results<Ok<Customer>, NotFound>> GetCustomerAsync(int id, CustomerService data)
        {
            var customer = await data.GetCustomerAsync(id);
            if (customer is null)
                return TypedResults.NotFound();
            return TypedResults.Ok(customer);
        }
        private static async Task<Created> InsertCustomerAsync(Customer customer, CustomerService data)
        {
            await data.InsertCustomerAsync(customer);
            return TypedResults.Created();
        }

        // Results mi permette di elencare le possibili risposte che posso ottenere
        private static async Task<Results<NoContent, NotFound>> UpdateCustomerAsync(int id, Customer customer, CustomerService data)
        {
            if (await data.GetCustomerAsync(id) is null) 
                return TypedResults.NotFound();
            
            await data.UpdateCustomerAsync(id, customer);
            return TypedResults.NoContent();
        }
        private static async Task<Results<NoContent, NotFound>> DeleteCustomerAsync(int id, CustomerService data)
        {
            if (await data.GetCustomerAsync(id) is null)
                return TypedResults.NotFound();
            await data.DeleteCustomerAsync(id);
            return TypedResults.NoContent();
        }
    }
}
