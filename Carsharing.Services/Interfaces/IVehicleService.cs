using Carsharing.Models.Entities;

namespace Carsharing.Services.Interfaces;

public interface IVehicleService
{
    Vehicle? GetVehicle(int id);
    void UpdateVehicleStatus(int vehicleId, string status);
}
