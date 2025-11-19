using Carsharing.Services.Interfaces;

namespace Carsharing.Controllers.Mvc;

public class VehicleController
{
    private readonly IVehicleService _vehicleService;

    public VehicleController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    public void ShowAvailableVehicles()
    {
        var vehicles = _vehicleService.GetAvailableVehicles();
        VehicleView.DisplayVehicles(vehicles);
    }

    public void ShowAllVehicles()
    {
        var vehicles = _vehicleService.GetAllVehicles();
        VehicleView.DisplayVehicles(vehicles);
    }

    public void UpdateVehicleStatus(int vehicleId, string status)
    {
        _vehicleService.UpdateVehicleStatus(vehicleId, status);
        Console.WriteLine($"Fahrzeug {vehicleId} Status aktualisiert auf: {status}");
    }

    public void AddNewVehicle()
    {
        var vehicle = VehicleView.GetNewVehicleDetails();
        _vehicleService.AddVehicle(vehicle);
        Console.WriteLine("Neues Fahrzeug hinzugefügt!");
    }
}

