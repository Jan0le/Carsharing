using Carsharing.Models.Entities;

namespace Carsharing.Controllers.Mvc;

public static class VehicleView
{
    public static void DisplayVehicles(List<Vehicle> vehicles)
    {
        Console.WriteLine("\n=== Verfügbare Fahrzeuge ===");
        foreach (var vehicle in vehicles)
        {
            Console.WriteLine($"ID: {vehicle.VehicleId} | {vehicle.Model} | {vehicle.LicensePlate} | " +
                            $"Status: {vehicle.Status} | Standort: {vehicle.CurrentLocation}");
        }
    }

    public static Vehicle GetNewVehicleDetails()
    {
        Console.WriteLine("\n=== Neues Fahrzeug hinzufügen ===");
        Console.Write("Modell: ");
        string model = Console.ReadLine() ?? string.Empty;
        Console.Write("Kennzeichen: ");
        string licensePlate = Console.ReadLine() ?? string.Empty;
        Console.Write("Standort: ");
        string location = Console.ReadLine() ?? string.Empty;

        return new Vehicle 
        { 
            Model = model, 
            LicensePlate = licensePlate, 
            CurrentLocation = location,
            Status = "Available"
        };
    }

    public static void ShowVehicleMenu()
    {
        Console.WriteLine("\n=== VehicleService Menu ===");
        Console.WriteLine("1. Verfügbare Fahrzeuge anzeigen");
        Console.WriteLine("2. Alle Fahrzeuge anzeigen");
        Console.WriteLine("3. Neues Fahrzeug hinzufügen");
        Console.WriteLine("4. Zurück zum Hauptmenü");
    }
}

