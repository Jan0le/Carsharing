# ParticipantService & PaymentService - Aufgaben von Maxl

## Übersicht

**Maxl** ist verantwortlich für die Implementierung von zwei zentralen Services:
- **ParticipantService**: Verwaltung von Teilnehmern (Benutzern) der Carsharing-Plattform
- **PaymentService**: Verwaltung von Zahlungen und Zahlungshistorie

Diese Services sind grundlegende Komponenten, die von anderen Services (z.B. BookingService) verwendet werden.

## Projektstruktur

```
Carsharing.Models/Entities/
├── Participant.cs      # Participant Entity Model
└── Payment.cs          # Payment Entity Model

Carsharing.Services/
├── Interfaces/
│   ├── IParticipantService.cs  # Interface für ParticipantService
│   └── IPaymentService.cs      # Interface für PaymentService
└── Implementations/
    ├── ParticipantService.cs   # Implementierung des ParticipantService
    └── PaymentService.cs       # Implementierung des PaymentService

Carsharing.Controllers/Mvc/
├── ParticipantController.cs    # Controller für Teilnehmer
├── ParticipantView.cs          # View-Helper-Klasse für Teilnehmer
├── PaymentController.cs        # Controller für Zahlungen
└── PaymentView.cs              # View-Helper-Klasse für Zahlungen
```

---

## ParticipantService

### 1. Participant Model (`Carsharing.Models/Entities/Participant.cs`)

```csharp
public class Participant
{
    public int ParticipantId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

**Eigenschaften:**
- `ParticipantId`: Eindeutige ID des Teilnehmers
- `FirstName`: Vorname
- `LastName`: Nachname
- `Email`: E-Mail-Adresse
- `CreatedAt`: Zeitstempel der Erstellung

### 2. IParticipantService Interface (`Carsharing.Services/Interfaces/IParticipantService.cs`)

```csharp
public interface IParticipantService
{
    bool ParticipantExists(int participantId);
    Participant? GetParticipant(int participantId);
    List<Participant> GetAllParticipants();
    void AddParticipant(Participant participant);
}
```

### 3. ParticipantService Implementierung (`Carsharing.Services/Implementations/ParticipantService.cs`)

**Testdaten:**
- Max Mustermann (max@example.com)
- Anna Schmidt (anna@example.com)

**Methoden:**

#### `ParticipantExists(int participantId)`
- Prüft, ob ein Teilnehmer mit der gegebenen ID existiert
- **Rückgabe:** `bool`

#### `GetParticipant(int participantId)`
- Gibt einen Teilnehmer anhand der ID zurück
- **Rückgabe:** `Participant?` (null wenn nicht gefunden)

#### `GetAllParticipants()`
- Gibt alle registrierten Teilnehmer zurück
- **Rückgabe:** `List<Participant>`

#### `AddParticipant(Participant participant)`
- Fügt einen neuen Teilnehmer hinzu
- Setzt automatisch `ParticipantId` und `CreatedAt`
- **Parameter:** `Participant` (ohne ID, wird automatisch generiert)

### 4. ParticipantController (`Carsharing.Controllers/Mvc/ParticipantController.cs`)

**Konstruktor:**
- Verwendet Dependency Injection für `IParticipantService`

**Methoden:**

#### `ShowAllParticipants()`
- Zeigt alle Teilnehmer über `ParticipantView.DisplayParticipants()` an

#### `AddNewParticipant()`
- Ruft `ParticipantView.GetNewParticipantDetails()` auf
- Fügt neuen Teilnehmer über `IParticipantService.AddParticipant()` hinzu
- Gibt Erfolgsmeldung aus

### 5. ParticipantView (`Carsharing.Controllers/Mvc/ParticipantView.cs`)

**Statische Methoden:**

#### `DisplayParticipants(List<Participant> participants)`
- Zeigt Liste aller Teilnehmer an
- Format: `ID: X | Vorname Nachname | Email`

#### `GetNewParticipantDetails()`
- Fragt Benutzer nach Teilnehmerdetails
- Eingaben: Vorname, Nachname, Email
- **Rückgabe:** `Participant` (ohne ID)

#### `ShowParticipantMenu()`
- Zeigt Menüoptionen an:
  1. Alle Teilnehmer anzeigen
  2. Neuen Teilnehmer hinzufügen
  3. Zurück zum Hauptmenü

---

## PaymentService

### 1. Payment Model (`Carsharing.Models/Entities/Payment.cs`)

```csharp
public class Payment
{
    public int PaymentId { get; set; }
    public int ParticipantId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } // Pending, Completed, Failed
    public DateTime CreatedAt { get; set; }
}
```

**Eigenschaften:**
- `PaymentId`: Eindeutige ID der Zahlung
- `ParticipantId`: Referenz zum Teilnehmer
- `Amount`: Betrag der Zahlung
- `Status`: Status der Zahlung (Pending, Completed, Failed)
- `CreatedAt`: Zeitstempel der Erstellung

### 2. IPaymentService Interface (`Carsharing.Services/Interfaces/IPaymentService.cs`)

```csharp
public interface IPaymentService
{
    bool ProcessPayment(int participantId, decimal amount);
    List<Payment> GetPaymentHistory(int participantId);
}
```

### 3. PaymentService Implementierung (`Carsharing.Services/Implementations/PaymentService.cs`)

**Methoden:**

#### `ProcessPayment(int participantId, decimal amount)`
- Verarbeitet eine Zahlung für einen Teilnehmer
- Simuliert Zahlungsprozess (fragt Benutzer nach Bestätigung)
- Erstellt Payment-Eintrag mit Status "Completed" oder "Failed"
- **Rückgabe:** `bool` (true bei Erfolg, false bei Fehler)
- **Interaktion:** Fragt Benutzer "Zahlung bestätigen? (j/n)"

#### `GetPaymentHistory(int participantId)`
- Gibt alle Zahlungen eines Teilnehmers zurück
- **Rückgabe:** `List<Payment>`

**Zahlungsprozess:**
1. Zeigt Zahlungsanfrage an: `Zahlungsanfrage für Teilnehmer X: Betrag`
2. Fragt nach Bestätigung: `Zahlung bestätigen? (j/n)`
3. Erstellt Payment-Eintrag mit entsprechendem Status
4. Gibt Erfolgs-/Fehlermeldung aus

### 4. PaymentController (`Carsharing.Controllers/Mvc/PaymentController.cs`)

**Konstruktor:**
- Verwendet Dependency Injection für `IPaymentService`

**Methoden:**

#### `ShowPaymentHistory(int participantId)`
- Ruft `IPaymentService.GetPaymentHistory()` auf
- Zeigt Zahlungshistorie über `PaymentView.DisplayPayments()` an

### 5. PaymentView (`Carsharing.Controllers/Mvc/PaymentView.cs`)

**Statische Methoden:**

#### `DisplayPayments(List<Payment> payments)`
- Zeigt Liste aller Zahlungen an
- Format: `Zahlung #ID | Betrag | Status: Status | Datum`

#### `ShowPaymentMenu()`
- Zeigt Menüoptionen an:
  1. Zahlungshistorie anzeigen
  2. Zurück zum Hauptmenü

---

## Verwendung

### ParticipantService

```csharp
// Service initialisieren
var participantService = new ParticipantService();

// Controller erstellen
var participantController = new ParticipantController(participantService);

// Alle Teilnehmer anzeigen
participantController.ShowAllParticipants();

// Neuen Teilnehmer hinzufügen
participantController.AddNewParticipant();

// Teilnehmer prüfen (für andere Services)
bool exists = participantService.ParticipantExists(participantId: 1);
var participant = participantService.GetParticipant(participantId: 1);
```

### PaymentService

```csharp
// Service initialisieren
var paymentService = new PaymentService();

// Controller erstellen
var paymentController = new PaymentController(paymentService);

// Zahlung verarbeiten
bool success = paymentService.ProcessPayment(participantId: 1, amount: 50.00m);

// Zahlungshistorie anzeigen
paymentController.ShowPaymentHistory(participantId: 1);
```

### Integration mit anderen Services

```csharp
// Beispiel: BookingService verwendet beide Services
var participantService = new ParticipantService();
var paymentService = new PaymentService();
var bookingService = new BookingService(vehicleService, participantService, paymentService);

// BookingService prüft Teilnehmer-Existenz
if (participantService.ParticipantExists(participantId))
{
    // Startet Zahlungsprozess
    bool paymentSuccess = paymentService.ProcessPayment(participantId, amount);
}
```

---

## Status-Werte

### Payment Status
- **Pending**: Zahlung wurde initiiert, aber noch nicht abgeschlossen
- **Completed**: Zahlung wurde erfolgreich abgeschlossen
- **Failed**: Zahlung ist fehlgeschlagen

---

## Wichtige Hinweise

### ParticipantService
- Verwendet **In-Memory-Liste** für Datenspeicherung
- Automatische ID-Generierung (`_nextId`)
- Testdaten werden im Konstruktor initialisiert
- `CreatedAt` wird automatisch beim Hinzufügen gesetzt

### PaymentService
- Verwendet **In-Memory-Liste** für Datenspeicherung
- Simuliert Zahlungsprozess über Konsoleneingabe
- Automatische ID-Generierung (`_nextId`)
- Zahlungshistorie kann nach Teilnehmer gefiltert werden

### Allgemein
- Beide Services verwenden **Dependency Injection** über Interfaces
- Alle Services sind **stateless** (keine persistente Speicherung)
- Für Produktionsumgebung: Integration mit Datenbank erforderlich

---

## Dateipfade

### ParticipantService
- **Model:** `Carsharing.Models/Entities/Participant.cs`
- **Interface:** `Carsharing.Services/Interfaces/IParticipantService.cs`
- **Service:** `Carsharing.Services/Implementations/ParticipantService.cs`
- **Controller:** `Carsharing.Controllers/Mvc/ParticipantController.cs`
- **View:** `Carsharing.Controllers/Mvc/ParticipantView.cs`

### PaymentService
- **Model:** `Carsharing.Models/Entities/Payment.cs`
- **Interface:** `Carsharing.Services/Interfaces/IPaymentService.cs`
- **Service:** `Carsharing.Services/Implementations/PaymentService.cs`
- **Controller:** `Carsharing.Controllers/Mvc/PaymentController.cs`
- **View:** `Carsharing.Controllers/Mvc/PaymentView.cs`

---

## Abhängigkeiten

### Von anderen Services verwendet:
- **BookingService** verwendet beide Services:
  - `IParticipantService.ParticipantExists()` - Validiert Teilnehmer
  - `IPaymentService.ProcessPayment()` - Verarbeitet Zahlungen

### Keine Abhängigkeiten zu anderen Services:
- ParticipantService ist eigenständig
- PaymentService ist eigenständig

---

## Testdaten

### ParticipantService
- **Teilnehmer 1:** Max Mustermann (max@example.com)
- **Teilnehmer 2:** Anna Schmidt (anna@example.com)

### PaymentService
- Keine vordefinierten Testdaten
- Zahlungen werden dynamisch erstellt

