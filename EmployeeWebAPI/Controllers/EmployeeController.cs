using EmployeeWebAPI.Models.DTOs.Department;
using EmployeeWebAPI.Models.DTOs.Employee;
using EmployeeWebAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EmployeeWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IMongoCollection<Employee> _employeeCollection;
    private readonly IMongoCollection<Department> _departmentCollection;

    public EmployeeController()
    {
        string connectionString = "mongodb://localhost:27017/employeeDB";

        MongoUrl mongoUrl = MongoUrl.Create(connectionString);
        MongoClient mongoClient = new MongoClient(mongoUrl);
        IMongoDatabase database = mongoClient.GetDatabase(mongoUrl.DatabaseName);

        _employeeCollection = database.GetCollection<Employee>("Employee");
        _departmentCollection = database.GetCollection<Department>("Department");
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResult>>> GetEmployees()
    {
        return await _employeeCollection.AsQueryable().Where(r => r.IsDeleted == false).Select(s => new EmployeeResult()
        {
            Id = s._Id,
            FullName = s.FullName,
            Email = s.Email,
            CellphoneNumber = s.CellphoneNumber,
            PersonalPictureFileName = s.PersonalPictureFileName,
            IsActive = s.IsActive
        }).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeCompleteResult>> GetEmployee(string id)
    {
        EmployeeCompleteResult employeeCompleteResult = await _employeeCollection.AsQueryable().Where(r => r._Id == id).Select(s => new EmployeeCompleteResult()
        {
            Id = s._Id,
            FullName = s.FullName,
            Email = s.Email,
            CellphoneNumber = s.CellphoneNumber,
            PersonalPictureFileName = s.PersonalPictureFileName,
            IsActive = s.IsActive
        }).FirstOrDefaultAsync();

        var filterDefinition = Builders<Employee>.Filter.Eq(r => r._Id, id);
        var temp = _employeeCollection.Find(filterDefinition).Project(Builders<Employee>.Projection
                                                    .Include(r => r.DepartmentIds)
                                                    .Exclude("_id")).FirstOrDefault();
        List<string> departmentIds = ((BsonArray)temp["DepartmentIds"]).Values
            .Select(x => x.AsString).ToList();
        employeeCompleteResult.Departments = _departmentCollection.AsQueryable().Where(r => departmentIds.Contains(r._Id) && r.IsDeleted == false)
            .Select(s => new DepartmentResult()
            {
                _Id = s._Id,
                Name = s.Name
            }).ToList();

        //return await _employeeCollection.Aggregate().Lookup("Department", r => r.DepartmentIds, "_Id", "asDepartment").Find(filterDefinition).As<EmployeeResult>().FirstOrDefaultAsync();
        return employeeCompleteResult;
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
