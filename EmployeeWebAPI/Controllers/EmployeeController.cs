using EmployeeWebAPI.Models.DTOs.Employee;
using EmployeeWebAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EmployeeWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IMongoCollection<Employee> _employeeCollection;

    public EmployeeController()
    {
        string connectionString = "mongodb://localhost:27017/employeeDB";

        MongoUrl mongoUrl = MongoUrl.Create(connectionString);
        MongoClient mongoClient = new MongoClient(mongoUrl);
        IMongoDatabase database = mongoClient.GetDatabase(mongoUrl.DatabaseName);

        _employeeCollection = database.GetCollection<Employee>("Employee");
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResult>>> GetEmployees()
    {
        return await _employeeCollection.AsQueryable().Select(s => new EmployeeResult()
        {
            FullName = s.FullName,
            Email = s.Email,
            Password = s.Password,
            CellphoneNumber = s.CellphoneNumber,
            PersonalPictureFileName = s.PersonalPictureFileName,
            IsActive = s.IsActive,
        }).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeResult>> GetEmployee(string id)
    {
        var filterDefinition = Builders<Employee>.Filter.Eq(r => r._Id, id);
        return await _employeeCollection.Find(filterDefinition).As<EmployeeResult>().FirstOrDefaultAsync();
    }

    [HttpPost]
    public async Task<ActionResult> Add(AddEmployeeInput addEmployeeInput)
    {
        Employee employee = new Employee();
        employee.FullName = addEmployeeInput.FullName;
        employee.Email = addEmployeeInput.Email;
        employee.Password = addEmployeeInput.Password;
        employee.CellphoneNumber = addEmployeeInput.CellphoneNumber;
        employee.PersonalPictureFileName = addEmployeeInput.PersonalPictureFileName;
        employee.DepartmentIds = addEmployeeInput.DepartmentIds;
        employee.IsActive = addEmployeeInput.IsActive;
        employee.IsDeleted = false;
        await _employeeCollection.InsertOneAsync(employee);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Edit(string id, EditEmployeeInput editEmployeeInput)
    {
        Employee employee = new Employee();
        employee._Id = id;
        employee.FullName = editEmployeeInput.FullName;
        employee.Email = editEmployeeInput.Email;
        employee.CellphoneNumber = editEmployeeInput.CellphoneNumber;
        employee.PersonalPictureFileName = editEmployeeInput.PersonalPictureFileName;
        employee.DepartmentIds = editEmployeeInput.DepartmentIds;
        employee.IsActive = editEmployeeInput.IsActive;
        employee.IsDeleted = false;

        var filterDefinition = Builders<Employee>.Filter.Eq(r => r._Id, id);
        await _employeeCollection.ReplaceOneAsync(filterDefinition, employee);
        return Ok();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> ChangePassword(string id, ChangePasswordInput changePasswordInput)
    {
        var filterDefinition = Builders<Employee>.Filter.Eq(r => r._Id, id);

        UpdateDefinition<Employee> employee = Builders<Employee>.Update.Set(r => r.Password, changePasswordInput.Password);

        await _employeeCollection.UpdateOneAsync(filterDefinition, employee);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var filterDefinition = Builders<Employee>.Filter.Eq(r => r._Id, id);

        UpdateDefinition<Employee> employee = Builders<Employee>.Update.Set(r => r.IsDeleted, true);

        await _employeeCollection.UpdateOneAsync(filterDefinition, employee);
        //await _employeeCollection.DeleteOneAsync(filterDefinition);
        return Ok();
    }
}
