namespace EmployeeApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using EmployeeApi.Models;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly string _connectionString;

    public EmployeeController(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Default")!;
    }

    // GET /api/employee
    [HttpGet]
    public IActionResult GetEmployee()
    {
        var employees = new List<Employee>();
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var command = new SqlCommand(
            "SELECT Id, Name FROM Employees", connection);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            employees.Add(new Employee
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }
        return Ok(employees);
    }

    // POST /api/employee
    [HttpPost]
    public IActionResult CreateEmployee([FromBody] Employee employee)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var command = new SqlCommand(
            "INSERT INTO Employees (Name) VALUES (@Name)", connection);
        command.Parameters.AddWithValue("@Name", employee.Name);
        command.ExecuteNonQuery();
        return Ok("Employee created successfully");
    }

// DELETE /api/employee/1
[HttpDelete("{id}")]
public IActionResult DeleteEmployee(int id)
{
    using var connection = new SqlConnection(_connectionString);
    connection.Open();
    var command = new SqlCommand(
        "DELETE FROM Employees WHERE Id = @Id", connection);
    command.Parameters.AddWithValue("@Id", id);
    command.ExecuteNonQuery();
    return Ok("Employee deleted successfully");
}}