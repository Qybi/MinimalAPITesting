using CLED.OnlineShop.API.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CLED.OnlineShop.API.Services;

public class OrderService
{
    private readonly string _connectionString;
    public OrderService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("db");
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        return await connection.QueryAsync<Order>(@"SELECT orderId as id,
                userId,
                orderDate,
                total
                FROM Orders;");
    }

    public async Task<Order> GetOrderAsync(int id)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<Order>("""
            SELECT orderId as id, 
            userId, 
            orderDate, 
            total 
            FROM Orders 
            WHERE orderId = @id;
            """, new { id = id });
    }

    public async Task InsertOrderAsync(Order order)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(@"INSERT INTO Orders (userId, orderDate, total) 
                VALUES (@userId, @orderDate, @total)", order);
    }

    public async Task UpdateOrderAsync(int id, Order order)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("""
            UPDATE Orders 
            SET userId = @userId, 
            orderDate = @orderDate, 
            total = @total 
            where orderId = @id
            """, order);
    }

    public async Task DeleteOrderAsync(int id)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(@"DELETE FROM Orders WHERE orderId = @id", new { id = id });
    }
}
