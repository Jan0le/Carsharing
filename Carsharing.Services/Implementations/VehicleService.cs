using Carsharing.Models.Entities;
using Carsharing.Services.Interfaces;

namespace Carsharing.Services.Implementations;

public class VehicleService : IVehicleService
{
    private List<Vehicle> _vehicles = new();
    private int _nextId = 1;

    public VehicleService()
    {
        // Testdaten
        _vehicles.Add(new Vehicle { 
            VehicleId = _nextId++, 
            Model = "VW Golf", 
            LicensePlate = "M-AB123", 
            Status = "Available", 
            CurrentLocation = "München Zentrum",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        });

        _vehicles.Add(new Vehicle { 
            VehicleId = _nextId++, 
            Model = "BMW i3", 
            LicensePlate = "M-CD456", 
            Status = "Available", 
            CurrentLocation = "München Ost",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        });
    }

    public List<Vehicle> GetAvailableVehicles()
    {
        return _vehicles.Where(v => v.Status == "Available").ToList();
    }

    public List<Vehicle> GetAllVehicles()
    {
        return _vehicles;
    }

    public Vehicle? GetVehicle(int id)
    {
        return _vehicles.FirstOrDefault(v => v.VehicleId == id);
    }

    public void UpdateVehicleStatus(int vehicleId, string status)
    {
        var vehicle = _vehicles.FirstOrDefault(v => v.VehicleId == vehicleId);
        if (vehicle != null)
        {
            vehicle.Status = status;
            vehicle.UpdatedAt = DateTime.Now;
        }
    }

    public bool AddVehicle(Vehicle vehicle)
    {
        vehicle.VehicleId = _nextId++;
        vehicle.CreatedAt = DateTime.Now;
        vehicle.UpdatedAt = DateTime.Now;
        _vehicles.Add(vehicle);
        return true;
    }
}

