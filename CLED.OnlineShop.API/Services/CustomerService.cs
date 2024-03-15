using CLED.OnlineShop.API.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CLED.OnlineShop.API.Services
{
    public class CustomerService
    {
        private readonly string _connectionString;
        public CustomerService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("db");
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryAsync<Customer>(@"SELECT userId as id,
name,
address,
phone
FROM Customers;");
        }
        public async Task<Customer> GetCustomerAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<Customer>(@"SELECT userId as id,
name,
address,
phone
FROM Customers
WHERE userId = @id;", new { id = id });
        }

        // le 3 virgolette nella stringa permettono di identare il testo
        public async Task InsertCustomerAsync(Customer customer)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            await connection.ExecuteAsync("""
                INSERT INTO Customers (name, address, phone) 
                VALUES (@name, @address, @phone)
                """, customer);
        }

        public async Task UpdateCustomerAsync(int id, Customer customer)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            await connection.ExecuteAsync(@"UPDATE Customers SET name = @name, address = @address, phone = @phone where userId = @id", customer);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            await connection.ExecuteAsync(@"DELETE FROM Customers WHERE userId = @id", new { id = id });
        }   
    }
}
