using EmployeeWebAPI.Models.DTOs.Department;
using EmployeeWebAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DepartmentWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IMongoCollection<Department> _departmentCollection;

    public DepartmentController()
    {
        string connectionString = "mongodb://localhost:27017/departmentDB";

        MongoUrl mongoUrl = MongoUrl.Create(connectionString);
        MongoClient mongoClient = new MongoClient(mongoUrl);
        IMongoDatabase database = mongoClient.GetDatabase(mongoUrl.DatabaseName);

        _departmentCollection = database.GetCollection<Department>("Department");
    }

    [HttpGet]
    public async Task<ActionResult<List<DepartmentResult>>> GetDepartments()
    {
        return await _departmentCollection.AsQueryable().Select(s => new DepartmentResult()
        {
            _Id = s._Id,
            Name = s.Name
        }).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentResult>> GetDepartment(string id)
    {
        var filterDefinition = Builders<Department>.Filter.Eq(r => r._Id, id);
        return await _departmentCollection.Find(filterDefinition).As<DepartmentResult>().FirstOrDefaultAsync();
    }

    [HttpPost]
    public async Task<ActionResult> Add(DepartmentInput departmentInput)
    {
        Department department = new Department();
        department.Name = departmentInput.Name;
        department.IsDeleted = false;
        await _departmentCollection.InsertOneAsync(department);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Edit(string id, DepartmentInput departmentInput)
    {
        Department department = new Department();
        department._Id = id;
        department.Name = departmentInput.Name;
        department.IsDeleted = false;

        var filterDefinition = Builders<Department>.Filter.Eq(r => r._Id, id);
        await _departmentCollection.ReplaceOneAsync(filterDefinition, department);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var filterDefinition = Builders<Department>.Filter.Eq(r => r._Id, id);

        UpdateDefinition<Department> department = Builders<Department>.Update.Set(r => r.IsDeleted, true);

        await _departmentCollection.UpdateOneAsync(filterDefinition, department);
        //await _departmentCollection.DeleteOneAsync(filterDefinition);
        return Ok();
    }
}
