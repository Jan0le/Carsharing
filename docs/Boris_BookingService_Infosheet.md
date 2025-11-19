# BookingService - Aufgaben von Boris

## Übersicht

Der **BookingService** ist verantwortlich für die Verwaltung von Fahrzeugbuchungen in der Carsharing-Plattform. Er koordiniert die Interaktion zwischen VehicleService, ParticipantService und PaymentService.

## Projektstruktur

```
Carsharing.Models/Entities/
├── Booking.cs          # Booking Entity Model
├── Vehicle.cs          # Vehicle Entity (Abhängigkeit)
├── Participant.cs      # Participant Entity (Abhängigkeit)
└── Payment.cs          # Payment Entity (Abhängigkeit)

Carsharing.Services/
├── Interfaces/
│   ├── IBookingService.cs      # Interface für BookingService
│   ├── IVehicleService.cs      # Interface für VehicleService (Abhängigkeit)
│   ├── IParticipantService.cs  # Interface für ParticipantService (Abhängigkeit)
│   └── IPaymentService.cs      # Interface für PaymentService (Abhängigkeit)
└── Implementations/
    └── BookingService.cs       # Implementierung des BookingService

Carsharing.Controllers/Mvc/
├── BookingController.cs        # Controller für Buchungen
└── BookingView.cs              # View-Helper-Klasse
```

## Implementierte Komponenten

### 1. Booking Model (`Carsharing.Models/Entities/Booking.cs`)

```csharp
public class Booking
{
    public int BookingId { get; set; }
    public int VehicleId { get; set; }
    public int ParticipantId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string BookingStatus { get; set; } // Pending, Confirmed, Cancelled
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

**Eigenschaften:**
- `BookingId`: Eindeutige ID der Buchung
- `VehicleId`: Referenz zum gebuchten Fahrzeug
- `ParticipantId`: Referenz zum Teilnehmer
- `StartTime` / `EndTime`: Buchungszeitraum
- `BookingStatus`: Status der Buchung (Pending, Confirmed, Cancelled)
- `CreatedAt` / `UpdatedAt`: Zeitstempel für Erstellung und Aktualisierung

### 2. BookingService (`Carsharing.Services/Implementations/BookingService.cs`)

**Abhängigkeiten:**
- `IVehicleService`: Prüft Fahrzeugverfügbarkeit und aktualisiert Status
- `IParticipantService`: Validiert Teilnehmer-Existenz
- `IPaymentService`: Verarbeitet Zahlungen für Buchungen

**Hauptmethoden:**

#### `CreateBooking(int vehicleId, int participantId, DateTime startTime, DateTime endTime)`
- Erstellt eine neue Buchung
- Prüft Fahrzeugverfügbarkeit (Status muss "Available" sein)
- Validiert Teilnehmer-Existenz
- Berechnet Preis basierend auf Dauer (5€ pro Stunde)
- Startet automatisch Zahlungsprozess
- Bestätigt Buchung bei erfolgreicher Zahlung
- **Rückgabe:** `bool` (true bei Erfolg, false bei Fehler)

#### `ConfirmBooking(int bookingId)`
- Bestätigt eine Buchung
- Setzt Status auf "Confirmed"
- Aktualisiert Fahrzeugstatus auf "Booked"
- Aktualisiert `UpdatedAt` Zeitstempel

#### `GetUserBookings(int participantId)`
- Gibt alle Buchungen eines Teilnehmers zurück
- **Rückgabe:** `List<Booking>`

**Preisberechnung:**
- Formel: `(EndTime - StartTime).TotalHours * 5.0`
- Beispiel: 2 Stunden = 10€

### 3. BookingController (`Carsharing.Controllers/Mvc/BookingController.cs`)

**Konstruktor:**
- Verwendet Dependency Injection für `IBookingService`

**Methoden:**

#### `CreateNewBooking()`
- Ruft `BookingView.GetBookingDetails()` auf
- Erstellt Buchung über `IBookingService.CreateBooking()`
- Gibt Erfolgs-/Fehlermeldung aus

#### `ShowUserBookings(int participantId)`
- Ruft `IBookingService.GetUserBookings()` auf
- Zeigt Buchungen über `BookingView.DisplayBookings()` an

### 4. BookingView (`Carsharing.Controllers/Mvc/BookingView.cs`)

**Statische Methoden:**

#### `GetBookingDetails()`
- Fragt Benutzer nach Buchungsdetails
- Eingaben: VehicleId, ParticipantId, StartTime, EndTime
- **Rückgabe:** Tuple `(int VehicleId, int ParticipantId, DateTime StartTime, DateTime EndTime)`

#### `DisplayBookings(List<Booking> bookings)`
- Zeigt Liste aller Buchungen an
- Format: `Buchung #ID | Fahrzeug: VehicleId | Status: Status | StartTime - EndTime`

#### `ShowBookingMenu()`
- Zeigt Menüoptionen an:
  1. Neue Buchung erstellen
  2. Meine Buchungen anzeigen
  3. Zurück zum Hauptmenü

## Abhängigkeiten zu anderen Services

### VehicleService
- **Benötigt:** `GetVehicle(int id)` - Prüft Fahrzeugverfügbarkeit
- **Benötigt:** `UpdateVehicleStatus(int vehicleId, string status)` - Aktualisiert Status auf "Booked"

### ParticipantService
- **Benötigt:** `ParticipantExists(int participantId)` - Validiert Teilnehmer

### PaymentService
- **Benötigt:** `ProcessPayment(int participantId, decimal amount)` - Verarbeitet Zahlung

## Buchungsablauf

1. **Validierung:**
   - Fahrzeug existiert und ist verfügbar (Status = "Available")
   - Teilnehmer existiert

2. **Buchungserstellung:**
   - Neue Booking-Instanz mit Status "Pending"
   - Berechnung des Preises (5€ pro Stunde)

3. **Zahlungsprozess:**
   - Automatischer Aufruf von `PaymentService.ProcessPayment()`
   - Bei Erfolg: Buchung wird bestätigt
   - Bei Fehler: Buchung bleibt "Pending"

4. **Bestätigung:**
   - Status wird auf "Confirmed" gesetzt
   - Fahrzeugstatus wird auf "Booked" gesetzt

## Verwendung

```csharp
// Services initialisieren
var vehicleService = new VehicleService();
var participantService = new ParticipantService();
var paymentService = new PaymentService();

// BookingService mit Dependencies erstellen
var bookingService = new BookingService(vehicleService, participantService, paymentService);

// Controller erstellen
var bookingController = new BookingController(bookingService);

// Neue Buchung erstellen
bookingController.CreateNewBooking();

// Buchungen eines Teilnehmers anzeigen
bookingController.ShowUserBookings(participantId: 1);
```

## Status-Werte

- **Pending**: Buchung wurde erstellt, Zahlung läuft noch
- **Confirmed**: Buchung wurde bestätigt und bezahlt
- **Cancelled**: Buchung wurde storniert

## Wichtige Hinweise

- Der BookingService verwendet **Dependency Injection** für alle Abhängigkeiten
- Alle Services müssen über **Interfaces** implementiert werden
- Die Preisberechnung erfolgt automatisch basierend auf der Buchungsdauer
- Bei fehlgeschlagener Zahlung bleibt die Buchung im Status "Pending"
- Der Fahrzeugstatus wird automatisch auf "Booked" gesetzt bei erfolgreicher Buchung

## Dateipfade

- **Model:** `Carsharing.Models/Entities/Booking.cs`
- **Interface:** `Carsharing.Services/Interfaces/IBookingService.cs`
- **Service:** `Carsharing.Services/Implementations/BookingService.cs`
- **Controller:** `Carsharing.Controllers/Mvc/BookingController.cs`
- **View:** `Carsharing.Controllers/Mvc/BookingView.cs`

