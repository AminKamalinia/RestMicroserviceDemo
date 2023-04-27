using System.Linq.Expressions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VehicleWebAPI.Models.DTOs;
using VehicleWebAPI.Models.Entities;

namespace VehicleWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors("_myAllowSpecificOrigins")]
public class VehicleController : ControllerBase
{
    private readonly DataContext _dataContext;

    public VehicleController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpGet]
    public ActionResult<List<VehicleResult>> Get()
    {
        List<VehicleResult> data = _dataContext.Vehicles.Select(ToResult).ToList();
        return Ok(data);
    }

    [HttpGet("{vin}")]
    public ActionResult<VehicleResult> GetByVIN(string vin)
    {
        VehicleResult data = _dataContext.Vehicles.Where(r => r.VIN == vin).Select(ToResult).FirstOrDefault();
        return Ok(data);
    }

    [HttpPost()]
    public async Task<ActionResult> Add(AddVehicleInput addVehicleInput)
    {
        Vehicle vehicle = new Vehicle();
        vehicle.VIN = addVehicleInput.VIN;
        vehicle.VehicleMaker = addVehicleInput.VehicleMaker;
        vehicle.VehicleYear = addVehicleInput.VehicleYear;
        vehicle.VehicleModel = addVehicleInput.VehicleModel;
        vehicle.InspectionDate = addVehicleInput.InspectionDate;
        vehicle.InspectorName = addVehicleInput.InspectorName;
        vehicle.InspectionLocation = addVehicleInput.InspectionLocation;
        vehicle.PassOrFail = addVehicleInput.PassOrFail;
        vehicle.Notes = addVehicleInput.Notes;

        await _dataContext.Vehicles.AddAsync(vehicle);
        await _dataContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{vin}")]
    public async Task<ActionResult> Edit(string vin, [FromBody] EditVehicleInput editVehicleInput)
    {
        Vehicle vehicle = _dataContext.Vehicles.FirstOrDefault(r => r.VIN == vin);
        if (vehicle == null)
            return NotFound();

        vehicle.VehicleMaker = editVehicleInput.VehicleMaker;
        vehicle.VehicleYear = editVehicleInput.VehicleYear;
        vehicle.VehicleModel = editVehicleInput.VehicleModel;
        vehicle.InspectionDate = editVehicleInput.InspectionDate;
        vehicle.InspectorName = editVehicleInput.InspectorName;
        vehicle.InspectionLocation = editVehicleInput.InspectionLocation;
        vehicle.PassOrFail = editVehicleInput.PassOrFail;
        vehicle.Notes = editVehicleInput.Notes;

        await _dataContext.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{vin}")]
    public async Task<ActionResult> Delete(string vin)
    {
        Vehicle vehicle = _dataContext.Vehicles.FirstOrDefault(r => r.VIN == vin);
        if (vehicle == null)
            return NotFound();

        _dataContext.Vehicles.Remove(vehicle);
        await _dataContext.SaveChangesAsync();
        return Ok();
    }

    private Expression<Func<Vehicle, VehicleResult>> ToResult => vehicle =>
        new VehicleResult()
        {
            VIN = vehicle.VIN,
            VehicleMaker = vehicle.VehicleMaker,
            VehicleYear = vehicle.VehicleYear,
            VehicleModel = vehicle.VehicleModel,
            InspectionDate = vehicle.InspectionDate,
            InspectorName = vehicle.InspectorName,
            InspectionLocation = vehicle.InspectionLocation,
            PassOrFail = vehicle.PassOrFail,
            Notes = vehicle.Notes
        };
}
