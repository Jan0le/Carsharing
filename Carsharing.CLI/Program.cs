using Carsharing.Controllers.Mvc;
using Carsharing.Services.Implementations;
using Carsharing.Services.Interfaces;

namespace Carsharing.CLI;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("🚗 Carsharing Admin CLI");
        Console.WriteLine("=======================\n");

        // Services initialisieren
        IVehicleService vehicleService = new VehicleService();
        VehicleController vehicleController = new VehicleController(vehicleService);

        bool running = true;
        while (running)
        {
            VehicleView.ShowVehicleMenu();
            Console.Write("\nAuswahl: ");
            
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Ungültige Eingabe. Bitte eine Zahl eingeben.\n");
                continue;
            }

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Ungültige Eingabe. Bitte eine Zahl eingeben.\n");
                continue;
            }

            switch (choice)
            {
                case 1:
                    Console.Clear();
                    vehicleController.ShowAvailableVehicles();
                    Console.WriteLine("\nDrücken Sie eine Taste, um fortzufahren...");
                    Console.ReadKey();
                    Console.Clear();
                    break;

                case 2:
                    Console.Clear();
                    vehicleController.ShowAllVehicles();
                    Console.WriteLine("\nDrücken Sie eine Taste, um fortzufahren...");
                    Console.ReadKey();
                    Console.Clear();
                    break;

                case 3:
                    Console.Clear();
                    vehicleController.AddNewVehicle();
                    Console.WriteLine("\nDrücken Sie eine Taste, um fortzufahren...");
                    Console.ReadKey();
                    Console.Clear();
                    break;

                case 4:
                    running = false;
                    Console.WriteLine("\nAuf Wiedersehen! 👋");
                    break;

                default:
                    Console.WriteLine("Ungültige Auswahl. Bitte wählen Sie 1-4.\n");
                    break;
            }
        }
    }
}
