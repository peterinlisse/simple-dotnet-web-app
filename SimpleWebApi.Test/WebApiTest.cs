namespace SimpleWebApi.Test;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Testing;


public class WebApiTest : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly WebApplicationFactory<Program> _factory;

    public WebApiTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task TestWebApi()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/weatherforecast");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task TestSwagger()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/swagger");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("text/html; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task TestFretchSQL()
    {
        bool testRun = false;
        //Connection string to SQL Server on port 1433
        string connectionString = "Server=localhost,1433;Database=testDB;User Id=sa;Password=Password1!;Encrypt=true;TrustServerCertificate=true;";

        // SQL query to fetch all data from the User table
        string query = "SELECT * FROM [User]";

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Connected to SQL Server.");

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Print column names
                    for (int i = 0; i<reader.FieldCount; i++)
                    {
                        string name = reader.GetName(i);
                        Console.Write(name + "\t");
                        testRun = true;
                    }
                    Console.WriteLine();

                    // Print rows
                    while (reader.Read())
                    {
                        for (int i = 0; i<reader.FieldCount; i++)
                        {
                            Console.Write(reader[i] + "\t");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        Assert.True(testRun);
    }
}