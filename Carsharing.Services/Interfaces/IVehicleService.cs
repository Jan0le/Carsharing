using Carsharing.Models.Entities;

namespace Carsharing.Services.Interfaces;

public interface IVehicleService
{
    List<Vehicle> GetAvailableVehicles();
    List<Vehicle> GetAllVehicles();
    Vehicle? GetVehicle(int id);
    void UpdateVehicleStatus(int vehicleId, string status);
    bool AddVehicle(Vehicle vehicle);
}

