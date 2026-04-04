namespace SalaryApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Salary.Models;

[ApiController]
[Route("api/[controller]")]
public class SalaryController : ControllerBase
{
        private readonly string _connectionString;

    public SalaryController(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Default")!;
    }
    //GET /api/salary
 [HttpGet]
public IActionResult GetSalary([FromQuery] int empId)
{
    var salaries = new List<Salary>();

    try
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var command = new SqlCommand(
            "SELECT * FROM Salary WHERE EmpId = @EmpId", connection);

        command.Parameters.AddWithValue("@EmpId", empId);

        var reader = command.ExecuteReader();

 while (reader.Read())
{
    salaries.Add(new Salary
    {
        Id     = (int)reader["Id"],
        EmpId  = (int)reader["EmpId"],
        Amount = (decimal)reader["Amount"], // ✅ decimal + capital A
    });
}
        if (!salaries.Any())
            return NotFound(new { message = "No records found" });

        return Ok(salaries);
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { error = ex.Message });
    }
}

    //POST /api/salary
    [HttpPost]
    public IActionResult CreateSalary([FromBody] Salary salary){
  using var connection = new SqlConnection(_connectionString);
        connection.Open();
var command = new SqlCommand(
    "INSERT INTO Salary (EmpId, Amount) VALUES (@EmpId, @Amount)", connection);

command.Parameters.AddWithValue("@EmpId",  salary.EmpId);
command.Parameters.AddWithValue("@Amount", salary.Amount);
        command.ExecuteNonQuery();
        return Ok("Salary Added");
    }
    
}