# VehicleService - Aufgaben von Ole

## Übersicht

Der **VehicleService** ist verantwortlich für die Verwaltung von Fahrzeugen in der Carsharing-Plattform. Er stellt die grundlegende Funktionalität bereit, um Fahrzeuge zu verwalten, deren Verfügbarkeit zu prüfen und Statusänderungen vorzunehmen.

## Projektstruktur

```
Carsharing.Models/Entities/
└── Vehicle.cs              # Vehicle Entity Model

Carsharing.Services/
├── Interfaces/
│   └── IVehicleService.cs  # Interface für VehicleService
└── Implementations/
    └── VehicleService.cs   # Implementierung des VehicleService

Carsharing.Controllers/Mvc/
├── VehicleController.cs    # Controller für Fahrzeuge
└── VehicleView.cs          # View-Helper-Klasse
```

---

## Implementierte Komponenten

### 1. Vehicle Model (`Carsharing.Models/Entities/Vehicle.cs`)

```csharp
public class Vehicle
{
    public int VehicleId { get; set; }
    public string Model { get; set; }
    public string LicensePlate { get; set; }
    public string Status { get; set; } // Available, Booked, Maintenance
    public string CurrentLocation { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

**Eigenschaften:**
- `VehicleId`: Eindeutige ID des Fahrzeugs
- `Model`: Modellbezeichnung (z.B. "VW Golf", "BMW i3")
- `LicensePlate`: Kennzeichen des Fahrzeugs
- `Status`: Aktueller Status (Available, Booked, Maintenance)
- `CurrentLocation`: Aktueller Standort des Fahrzeugs
- `CreatedAt`: Zeitstempel der Erstellung
- `UpdatedAt`: Zeitstempel der letzten Aktualisierung

### 2. IVehicleService Interface (`Carsharing.Services/Interfaces/IVehicleService.cs`)

```csharp
public interface IVehicleService
{
    List<Vehicle> GetAvailableVehicles();
    List<Vehicle> GetAllVehicles();
    Vehicle? GetVehicle(int id);
    void UpdateVehicleStatus(int vehicleId, string status);
    bool AddVehicle(Vehicle vehicle);
}
```

### 3. VehicleService Implementierung (`Carsharing.Services/Implementations/VehicleService.cs`)

**Testdaten:**
- **Fahrzeug 1:** VW Golf (M-AB123) - Status: Available - Standort: München Zentrum
- **Fahrzeug 2:** BMW i3 (M-CD456) - Status: Available - Standort: München Ost

**Methoden:**

#### `GetAvailableVehicles()`
- Gibt alle verfügbaren Fahrzeuge zurück (Status = "Available")
- **Rückgabe:** `List<Vehicle>`
- **Verwendung:** Wird verwendet, um Benutzern nur verfügbare Fahrzeuge anzuzeigen

#### `GetAllVehicles()`
- Gibt alle Fahrzeuge zurück, unabhängig vom Status
- **Rückgabe:** `List<Vehicle>`
- **Verwendung:** Für administrative Zwecke oder Übersichten

#### `GetVehicle(int id)`
- Gibt ein spezifisches Fahrzeug anhand der ID zurück
- **Parameter:** `int id` - Die VehicleId
- **Rückgabe:** `Vehicle?` (null wenn nicht gefunden)
- **Verwendung:** Wird von anderen Services verwendet, um Fahrzeugdetails zu prüfen

#### `UpdateVehicleStatus(int vehicleId, string status)`
- Aktualisiert den Status eines Fahrzeugs
- Aktualisiert automatisch `UpdatedAt` Zeitstempel
- **Parameter:** 
  - `int vehicleId` - Die VehicleId
  - `string status` - Neuer Status (z.B. "Available", "Booked", "Maintenance")
- **Verwendung:** Wird verwendet, wenn ein Fahrzeug gebucht wird oder in Wartung geht

#### `AddVehicle(Vehicle vehicle)`
- Fügt ein neues Fahrzeug hinzu
- Setzt automatisch `VehicleId`, `CreatedAt` und `UpdatedAt`
- **Parameter:** `Vehicle vehicle` - Fahrzeug ohne ID (wird automatisch generiert)
- **Rückgabe:** `bool` (immer true)
- **Verwendung:** Zum Hinzufügen neuer Fahrzeuge zur Flotte

**Datenverwaltung:**
- Verwendet **In-Memory-Liste** (`List<Vehicle>`) für Datenspeicherung
- Automatische ID-Generierung mit `_nextId` Counter
- Testdaten werden im Konstruktor initialisiert

### 4. VehicleController (`Carsharing.Controllers/Mvc/VehicleController.cs`)

**Konstruktor:**
- Verwendet Dependency Injection für `IVehicleService`

**Methoden:**

#### `ShowAvailableVehicles()`
- Ruft `IVehicleService.GetAvailableVehicles()` auf
- Zeigt verfügbare Fahrzeuge über `VehicleView.DisplayVehicles()` an

#### `ShowAllVehicles()`
- Ruft `IVehicleService.GetAllVehicles()` auf
- Zeigt alle Fahrzeuge über `VehicleView.DisplayVehicles()` an

#### `UpdateVehicleStatus(int vehicleId, string status)`
- Ruft `IVehicleService.UpdateVehicleStatus()` auf
- Gibt Bestätigungsmeldung aus: `Fahrzeug {vehicleId} Status aktualisiert auf: {status}`

#### `AddNewVehicle()`
- Ruft `VehicleView.GetNewVehicleDetails()` auf
- Fügt Fahrzeug über `IVehicleService.AddVehicle()` hinzu
- Gibt Erfolgsmeldung aus: `Neues Fahrzeug hinzugefügt!`

### 5. VehicleView (`Carsharing.Controllers/Mvc/VehicleView.cs`)

**Statische Methoden:**

#### `DisplayVehicles(List<Vehicle> vehicles)`
- Zeigt Liste aller Fahrzeuge an
- Format: `ID: X | Modell | Kennzeichen | Status: Status | Standort: Standort`
- Beispiel: `ID: 1 | VW Golf | M-AB123 | Status: Available | Standort: München Zentrum`

#### `GetNewVehicleDetails()`
- Fragt Benutzer nach Fahrzeugdetails
- Eingaben: Modell, Kennzeichen, Standort
- Setzt automatisch Status auf "Available"
- **Rückgabe:** `Vehicle` (ohne ID, wird vom Service gesetzt)

#### `ShowVehicleMenu()`
- Zeigt Menüoptionen an:
  1. Verfügbare Fahrzeuge anzeigen
  2. Alle Fahrzeuge anzeigen
  3. Neues Fahrzeug hinzufügen
  4. Zurück zum Hauptmenü

---

## Status-Werte

- **Available**: Fahrzeug ist verfügbar und kann gebucht werden
- **Booked**: Fahrzeug ist aktuell gebucht
- **Maintenance**: Fahrzeug ist in Wartung und nicht verfügbar

---

## Verwendung

### Basis-Verwendung

```csharp
// Service initialisieren
var vehicleService = new VehicleService();

// Controller erstellen
var vehicleController = new VehicleController(vehicleService);

// Verfügbare Fahrzeuge anzeigen
vehicleController.ShowAvailableVehicles();

// Alle Fahrzeuge anzeigen
vehicleController.ShowAllVehicles();

// Neues Fahrzeug hinzufügen
vehicleController.AddNewVehicle();

// Fahrzeugstatus aktualisieren
vehicleController.UpdateVehicleStatus(vehicleId: 1, status: "Booked");
```

### Integration mit anderen Services

```csharp
// Beispiel: BookingService verwendet VehicleService
var vehicleService = new VehicleService();
var bookingService = new BookingService(vehicleService, participantService, paymentService);

// Prüft Fahrzeugverfügbarkeit
var vehicle = vehicleService.GetVehicle(vehicleId);
if (vehicle != null && vehicle.Status == "Available")
{
    // Fahrzeug kann gebucht werden
    bookingService.CreateBooking(vehicleId, participantId, startTime, endTime);
    
    // Status auf "Booked" setzen
    vehicleService.UpdateVehicleStatus(vehicleId, "Booked");
}
```

---

## Wichtige Hinweise

### Datenverwaltung
- Verwendet **In-Memory-Liste** für Datenspeicherung
- Automatische ID-Generierung (`_nextId`)
- Testdaten werden im Konstruktor initialisiert
- `CreatedAt` und `UpdatedAt` werden automatisch gesetzt

### Dependency Injection
- VehicleService verwendet **Interface-basierte Architektur**
- Controller verwendet Dependency Injection über `IVehicleService`
- Ermöglicht einfaches Mocking für Tests

### Status-Management
- Status wird manuell über `UpdateVehicleStatus()` gesetzt
- Keine automatische Status-Validierung
- Status sollte konsistent gehalten werden (z.B. durch BookingService)

### Erweiterungsmöglichkeiten
- Integration mit Datenbank für persistente Speicherung
- Validierung von Fahrzeugdaten (z.B. Kennzeichen-Format)
- Standortverwaltung mit GPS-Koordinaten
- Wartungsplanung und -historie
- Fahrzeugbilder und Details

---

## Dateipfade

- **Model:** `Carsharing.Models/Entities/Vehicle.cs`
- **Interface:** `Carsharing.Services/Interfaces/IVehicleService.cs`
- **Service:** `Carsharing.Services/Implementations/VehicleService.cs`
- **Controller:** `Carsharing.Controllers/Mvc/VehicleController.cs`
- **View:** `Carsharing.Controllers/Mvc/VehicleView.cs`

---

## Abhängigkeiten

### Von anderen Services verwendet:
- **BookingService** verwendet VehicleService:
  - `GetVehicle(int id)` - Prüft Fahrzeugverfügbarkeit
  - `UpdateVehicleStatus(int vehicleId, string status)` - Aktualisiert Status bei Buchung

### Keine Abhängigkeiten zu anderen Services:
- VehicleService ist eigenständig und hat keine Abhängigkeiten zu anderen Services

---

## Testdaten

### Fahrzeug 1
- **ID:** 1
- **Modell:** VW Golf
- **Kennzeichen:** M-AB123
- **Status:** Available
- **Standort:** München Zentrum

### Fahrzeug 2
- **ID:** 2
- **Modell:** BMW i3
- **Kennzeichen:** M-CD456
- **Status:** Available
- **Standort:** München Ost

---

## Beispiel-Workflow

1. **Fahrzeuge anzeigen:**
   ```csharp
   vehicleController.ShowAvailableVehicles();
   // Ausgabe: ID: 1 | VW Golf | M-AB123 | Status: Available | Standort: München Zentrum
   ```

2. **Neues Fahrzeug hinzufügen:**
   ```csharp
   vehicleController.AddNewVehicle();
   // Eingabe: Modell: "Tesla Model 3", Kennzeichen: "M-EF789", Standort: "München Süd"
   // Ausgabe: Neues Fahrzeug hinzugefügt!
   ```

3. **Status aktualisieren:**
   ```csharp
   vehicleController.UpdateVehicleStatus(vehicleId: 1, status: "Booked");
   // Ausgabe: Fahrzeug 1 Status aktualisiert auf: Booked
   ```

4. **Fahrzeug für Wartung markieren:**
   ```csharp
   vehicleController.UpdateVehicleStatus(vehicleId: 2, status: "Maintenance");
   // Ausgabe: Fahrzeug 2 Status aktualisiert auf: Maintenance
   ```


