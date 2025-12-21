using Carsharing.Controllers.Mvc;
using Carsharing.Data.DbContext;
using Carsharing.Services.Implementations;
using Carsharing.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.CLI;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        // Services initialisieren
        IVehicleService vehicleService = new VehicleService();
        IPaymentService paymentService = new PaymentService();
        
        // ParticipantService benötigt DbContext
        var options = new DbContextOptionsBuilder<ParticipantDbContext>()
            .UseSqlite("Data Source=cli_participants.db")
            .Options;
        var participantDbContext = new ParticipantDbContext(options);
        IParticipantService participantService = new ParticipantService(participantDbContext);
        
        // BookingService benötigt alle Services
        IBookingService bookingService = new BookingService(vehicleService, participantService, paymentService);
        
        // Controller initialisieren
        VehicleController vehicleController = new VehicleController(vehicleService);
        ParticipantController participantController = new ParticipantController(participantService);
        PaymentController paymentController = new PaymentController(paymentService);
        BookingController bookingController = new BookingController(bookingService);

        bool running = true;
        while (running)
        {
            ShowMainMenu();
            Console.Write("\nAuswahl: ");
            
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Ungültige Eingabe. Bitte eine Zahl eingeben.\n");
                WaitForKey();
                continue;
            }

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Ungültige Eingabe. Bitte eine Zahl eingeben.\n");
                WaitForKey();
                continue;
            }

            switch (choice)
            {
                case 1:
                    HandleVehicleMenu(vehicleController);
                    break;

                case 2:
                    HandleBookingMenu(bookingController);
                    break;

                case 3:
                    HandleParticipantMenu(participantController);
                    break;

                case 4:
                    HandlePaymentMenu(paymentController);
                    break;

                case 5:
                    running = false;
                    Console.WriteLine("\nAuf Wiedersehen! 👋");
                    break;

                default:
                    Console.WriteLine("Ungültige Auswahl. Bitte wählen Sie 1-5.\n");
                    WaitForKey();
                    break;
            }
        }
    }

    static void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("🚗 Carsharing Admin CLI");
        Console.WriteLine("=======================\n");
        Console.WriteLine("Hauptmenü:");
        Console.WriteLine("1. 🚗 VehicleService");
        Console.WriteLine("2. 📅 BookingService");
        Console.WriteLine("3. 👥 ParticipantService");
        Console.WriteLine("4. 💳 PaymentService");
        Console.WriteLine("5. ❌ Beenden");
    }

    static void HandleVehicleMenu(VehicleController controller)
    {
        bool inMenu = true;
        while (inMenu)
        {
            Console.Clear();
            VehicleView.ShowVehicleMenu();
            Console.Write("\nAuswahl: ");

            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int choice))
            {
                Console.WriteLine("Ungültige Eingabe.\n");
                WaitForKey();
                continue;
            }

            switch (choice)
            {
                case 1:
                    Console.Clear();
                    controller.ShowAvailableVehicles();
                    WaitForKey();
                    break;

                case 2:
                    Console.Clear();
                    controller.ShowAllVehicles();
                    WaitForKey();
                    break;

                case 3:
                    Console.Clear();
                    controller.AddNewVehicle();
                    WaitForKey();
                    break;

                case 4:
                    inMenu = false;
                    break;

                default:
                    Console.WriteLine("Ungültige Auswahl.\n");
                    WaitForKey();
                    break;
            }
        }
    }

    static void HandleBookingMenu(BookingController controller)
    {
        bool inMenu = true;
        while (inMenu)
        {
            Console.Clear();
            BookingView.ShowBookingMenu();
            Console.Write("\nAuswahl: ");

            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int choice))
            {
                Console.WriteLine("Ungültige Eingabe.\n");
                WaitForKey();
                continue;
            }

            switch (choice)
            {
                case 1:
                    Console.Clear();
                    controller.CreateNewBooking();
                    WaitForKey();
                    break;

                case 2:
                    Console.Clear();
                    Console.Write("Teilnehmer ID: ");
                    if (int.TryParse(Console.ReadLine(), out int participantId))
                    {
                        controller.ShowUserBookings(participantId);
                    }
                    else
                    {
                        Console.WriteLine("Ungültige Teilnehmer-ID.");
                    }
                    WaitForKey();
                    break;

                case 3:
                    inMenu = false;
                    break;

                default:
                    Console.WriteLine("Ungültige Auswahl.\n");
                    WaitForKey();
                    break;
            }
        }
    }

    static void HandleParticipantMenu(ParticipantController controller)
    {
        bool inMenu = true;
        while (inMenu)
        {
            Console.Clear();
            ParticipantView.ShowParticipantMenu();
            Console.Write("\nAuswahl: ");

            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int choice))
            {
                Console.WriteLine("Ungültige Eingabe.\n");
                WaitForKey();
                continue;
            }

            switch (choice)
            {
                case 1:
                    Console.Clear();
                    controller.ShowAllParticipants();
                    WaitForKey();
                    break;

                case 2:
                    Console.Clear();
                    controller.AddNewParticipant();
                    WaitForKey();
                    break;

                case 3:
                    inMenu = false;
                    break;

                default:
                    Console.WriteLine("Ungültige Auswahl.\n");
                    WaitForKey();
                    break;
            }
        }
    }

    static void HandlePaymentMenu(PaymentController controller)
    {
        bool inMenu = true;
        while (inMenu)
        {
            Console.Clear();
            PaymentView.ShowPaymentMenu();
            Console.Write("\nAuswahl: ");

            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int choice))
            {
                Console.WriteLine("Ungültige Eingabe.\n");
                WaitForKey();
                continue;
            }

            switch (choice)
            {
                case 1:
                    Console.Clear();
                    Console.Write("Teilnehmer ID: ");
                    if (int.TryParse(Console.ReadLine(), out int participantId))
                    {
                        controller.ShowPaymentHistory(participantId);
                    }
                    else
                    {
                        Console.WriteLine("Ungültige Teilnehmer-ID.");
                    }
                    WaitForKey();
                    break;

                case 2:
                    inMenu = false;
                    break;

                default:
                    Console.WriteLine("Ungültige Auswahl.\n");
                    WaitForKey();
                    break;
            }
        }
    }

    static void WaitForKey()
    {
        Console.WriteLine("\nDrücken Sie eine Taste, um fortzufahren...");
        Console.ReadKey();
    }
}
