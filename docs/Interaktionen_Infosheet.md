# Interaktionen - Buchungsprozess

## Übersicht

Dieses Dokument beschreibt den vollständigen Buchungsprozess in der Carsharing-Plattform, der die Interaktion zwischen verschiedenen Services koordiniert.

## Ablauf der Interaktionen

### Schritt 1: Nutzer wählt Fahrzeug → VehicleService liefert verfügbare Fahrzeuge

**Beschreibung:**
Der Benutzer öffnet die Buchungsseite und sieht eine Liste aller verfügbaren Fahrzeuge.

**Technische Umsetzung:**
- **Blazor-Seite:** `Pages/Bookings.razor`
- **Service-Aufruf:** `VehicleService.GetAvailableVehicles()`
- **Rückgabe:** `List<Vehicle>` - Liste aller Fahrzeuge mit Status "Available"

**Code-Beispiel:**
```csharp
availableVehicles = VehicleService.GetAvailableVehicles();
```

**UI:**
- Dropdown-Menü mit verfügbaren Fahrzeugen
- Anzeige: Modell, Kennzeichen, Standort

---

### Schritt 2: Buchung wird angelegt → BookingService prüft Fahrzeugstatus

**Beschreibung:**
Nach Auswahl eines Fahrzeugs wird eine Buchung erstellt. Der BookingService prüft, ob das Fahrzeug noch verfügbar ist.

**Technische Umsetzung:**
- **Service:** `BookingService.CreateBooking()`
- **Prüfung:** `VehicleService.GetVehicle(vehicleId)` - Status muss "Available" sein
- **Validierung:** 
  - Fahrzeug existiert
  - Fahrzeug-Status ist "Available"
  - Teilnehmer existiert
  - Endzeit liegt nach Startzeit

**Code-Beispiel:**
```csharp
var vehicle = VehicleService.GetVehicle(vehicleId);
if (vehicle == null || vehicle.Status != "Available")
{
    // Fehler: Fahrzeug nicht verfügbar
    return false;
}
```

**Ergebnis:**
- Bei Erfolg: Buchung wird mit Status "Pending" erstellt
- Bei Fehler: Fehlermeldung wird angezeigt

---

### Schritt 3: Nutzer wird zugeordnet → ParticipantService

**Beschreibung:**
Der ausgewählte Teilnehmer wird der Buchung zugeordnet und validiert.

**Technische Umsetzung:**
- **Service:** `ParticipantService.ParticipantExists(participantId)`
- **Prüfung:** Existiert der Teilnehmer in der Datenbank?
- **Zuordnung:** `Booking.ParticipantId = participantId`

**Code-Beispiel:**
```csharp
if (!_participantService.ParticipantExists(participantId))
{
    Console.WriteLine("Teilnehmer nicht gefunden!");
    return false;
}
```

**Ergebnis:**
- Teilnehmer wird der Buchung zugeordnet
- Buchung enthält Referenz zum Teilnehmer

---

### Schritt 4: Zahlung → PaymentService

**Beschreibung:**
Nach erfolgreicher Validierung wird der Zahlungsprozess gestartet.

**Technische Umsetzung:**
- **Service:** `PaymentService.ProcessPayment(participantId, amount, confirmPayment: true)`
- **Preisberechnung:** `(EndTime - StartTime).TotalHours * 5.0` (5€ pro Stunde)
- **Zahlungsstatus:** 
  - "Completed" bei Erfolg
  - "Failed" bei Fehler

**Code-Beispiel:**
```csharp
decimal price = CalculatePrice(startTime, endTime);
bool paymentSuccess = _paymentService.ProcessPayment(
    participantId, 
    price, 
    confirmPayment: true
);
```

**Zahlungsprozess:**
1. Preis wird berechnet basierend auf Buchungsdauer
2. Payment-Eintrag wird erstellt
3. Status wird gesetzt (Completed/Failed)
4. Rückgabe: `bool` (Erfolg/Fehler)

---

### Schritt 5: Nach erfolgreicher Zahlung → BookingService bestätigt Buchung, VehicleService aktualisiert Status

**Beschreibung:**
Wenn die Zahlung erfolgreich war, wird die Buchung bestätigt und der Fahrzeugstatus aktualisiert.

**Technische Umsetzung:**
- **Buchungsbestätigung:** `BookingService.ConfirmBooking(bookingId)`
  - Status wird auf "Confirmed" gesetzt
  - `UpdatedAt` wird aktualisiert
- **Fahrzeugstatus:** `VehicleService.UpdateVehicleStatus(vehicleId, "Booked")`
  - Status wird auf "Booked" gesetzt
  - `UpdatedAt` wird aktualisiert

**Code-Beispiel:**
```csharp
if (paymentSuccess)
{
    ConfirmBooking(booking.BookingId);
}

// In ConfirmBooking():
booking.BookingStatus = "Confirmed";
booking.UpdatedAt = DateTime.Now;
_vehicleService.UpdateVehicleStatus(booking.VehicleId, "Booked");
```

**Ergebnis:**
- Buchung ist bestätigt (Status: "Confirmed")
- Fahrzeug ist gebucht (Status: "Booked")
- Fahrzeug erscheint nicht mehr in der Liste verfügbarer Fahrzeuge

---

## Vollständiger Ablauf (Sequenzdiagramm)

```
Benutzer          Blazor UI          BookingService      VehicleService      ParticipantService      PaymentService
   |                  |                      |                    |                    |                      |
   |-- Wählt Fahrzeug |                      |                    |                    |                      |
   |----------------->|                      |                    |                    |                      |
   |                  |-- GetAvailableVehicles()                  |                    |                      |
   |                  |------------------------------------------>|                    |                      |
   |                  |<-- List<Vehicle>                          |                    |                      |
   |<-- Fahrzeuge angezeigt                  |                    |                    |                      |
   |                  |                      |                    |                    |                      |
   |-- Füllt Formular aus                    |                    |                    |                      |
   |-- Klickt "Buchen"|                      |                    |                    |                      |
   |----------------->|                      |                    |                    |                      |
   |                  |-- CreateBooking()    |                    |                    |                      |
   |                  |--------------------->|                    |                    |                      |
   |                  |                      |-- GetVehicle()     |                    |                      |
   |                  |                      |------------------->|                    |                      |
   |                  |                      |<-- Vehicle         |                    |                      |
   |                  |                      |                    |                    |                      |
   |                  |                      |-- ParticipantExists()                    |                      |
   |                  |                      |----------------------------------------->|                    |
   |                  |                      |<-- true/false                           |                      |
   |                  |                      |                    |                    |                      |
   |                  |                      |-- ProcessPayment()                      |                      |
   |                  |                      |----------------------------------------->|                      |
   |                  |                      |<-- true/false                           |                      |
   |                  |                      |                    |                    |                      |
   |                  |                      |-- ConfirmBooking()|                    |                      |
   |                  |                      |-- UpdateVehicleStatus()                 |                      |
   |                  |                      |------------------->|                    |                      |
   |                  |<-- true/false       |                    |                    |                      |
   |<-- Erfolgsmeldung|                      |                    |                    |                      |
```

---

## Implementierung in Blazor

### Buchungsformular (`Pages/Bookings.razor`)

**Formularfelder:**
1. **Fahrzeugauswahl:** Dropdown mit verfügbaren Fahrzeugen
2. **Teilnehmerauswahl:** Dropdown mit registrierten Teilnehmern
3. **Startdatum/Startzeit:** Datum und Uhrzeit
4. **Enddatum/Endzeit:** Datum und Uhrzeit
5. **Preisanzeige:** Automatische Berechnung (5€/Stunde)

**Validierung:**
- Fahrzeug muss ausgewählt sein
- Teilnehmer muss ausgewählt sein
- Endzeit muss nach Startzeit liegen
- Fahrzeug muss verfügbar sein

**Submit-Handler:**
```csharp
private async Task HandleSubmit()
{
    // Schritt 1: Fahrzeug prüfen
    var vehicle = VehicleService.GetVehicle(bookingModel.VehicleId);
    if (vehicle == null || vehicle.Status != "Available")
    {
        // Fehler
        return;
    }
    
    // Schritt 2-5: Buchung erstellen (alle Schritte werden automatisch durchgeführt)
    bool success = BookingService.CreateBooking(
        bookingModel.VehicleId,
        bookingModel.ParticipantId,
        bookingModel.StartTime,
        bookingModel.EndTime
    );
    
    if (success)
    {
        // Erfolgsmeldung
    }
}
```

---

## Service-Interaktionen

### BookingService als Orchestrator

Der `BookingService` koordiniert alle anderen Services:

```csharp
public class BookingService : IBookingService
{
    private readonly IVehicleService _vehicleService;
    private readonly IParticipantService _participantService;
    private readonly IPaymentService _paymentService;
    
    public bool CreateBooking(...)
    {
        // 1. Prüft Fahrzeug über VehicleService
        var vehicle = _vehicleService.GetVehicle(vehicleId);
        
        // 2. Prüft Teilnehmer über ParticipantService
        if (!_participantService.ParticipantExists(participantId))
            return false;
        
        // 3. Erstellt Buchung
        var booking = new Booking { ... };
        
        // 4. Startet Zahlung über PaymentService
        bool paymentSuccess = _paymentService.ProcessPayment(...);
        
        // 5. Bestätigt Buchung und aktualisiert Fahrzeugstatus
        if (paymentSuccess)
        {
            ConfirmBooking(booking.BookingId);
            // Intern: _vehicleService.UpdateVehicleStatus(...)
        }
        
        return paymentSuccess;
    }
}
```

---

## Fehlerbehandlung

### Mögliche Fehler:

1. **Fahrzeug nicht verfügbar:**
   - Meldung: "Das ausgewählte Fahrzeug ist nicht verfügbar."
   - Aktion: Buchung wird nicht erstellt

2. **Teilnehmer nicht gefunden:**
   - Meldung: "Teilnehmer nicht gefunden!"
   - Aktion: Buchung wird nicht erstellt

3. **Ungültige Zeitangaben:**
   - Meldung: "Die Endzeit muss nach der Startzeit liegen."
   - Aktion: Formular bleibt offen

4. **Zahlung fehlgeschlagen:**
   - Meldung: "Buchung konnte nicht erstellt werden."
   - Aktion: Buchung bleibt im Status "Pending"

---

## Status-Übergänge

### Fahrzeug-Status:
```
Available → Booked (bei erfolgreicher Buchung)
```

### Buchungs-Status:
```
Pending → Confirmed (bei erfolgreicher Zahlung)
Pending → (bleibt Pending bei fehlgeschlagener Zahlung)
```

### Zahlungs-Status:
```
- → Completed (bei erfolgreicher Zahlung)
- → Failed (bei fehlgeschlagener Zahlung)
```

---

## Preisberechnung

**Formel:**
```
Preis = (EndTime - StartTime).TotalHours × 5.0 €
```

**Beispiel:**
- Start: 10:00 Uhr
- Ende: 14:00 Uhr
- Dauer: 4 Stunden
- Preis: 4 × 5€ = 20€

**Implementierung:**
```csharp
private decimal CalculatePrice(DateTime start, DateTime end)
{
    TimeSpan duration = end - start;
    return (decimal)(duration.TotalHours * 5.0);
}
```

---

## Zusammenfassung

Der Buchungsprozess folgt einem klaren 5-Schritte-Ablauf:

1. ✅ **Fahrzeugauswahl** - VehicleService liefert verfügbare Fahrzeuge
2. ✅ **Buchungserstellung** - BookingService prüft Fahrzeugstatus
3. ✅ **Teilnehmerzuordnung** - ParticipantService validiert Teilnehmer
4. ✅ **Zahlungsprozess** - PaymentService verarbeitet Zahlung
5. ✅ **Bestätigung** - BookingService bestätigt, VehicleService aktualisiert Status

Alle Schritte sind vollständig implementiert und funktionieren nahtlos zusammen.

## UI-Implementierung

### Buchungsformular (`Pages/Bookings.razor`)
- ✅ Vollständiges Formular mit Validierung
- ✅ Live-Preisberechnung
- ✅ Echtzeit-Feedback
- ✅ Fehlerbehandlung

### Buchungsstatus (`Pages/BookingStatus.razor`)
- ✅ Alle Buchungen anzeigen
- ✅ Filter nach Teilnehmer, Status, Zeitraum
- ✅ Detaillierte Informationen
- ✅ Responsive Tabelle

### Fahrzeugübersicht (`Pages/Vehicles.razor`)
- ✅ Filter nach Standort, Modell, Status
- ✅ Listenansicht und Rasteransicht
- ✅ Responsive Design
- ✅ Fehlerbehandlung

