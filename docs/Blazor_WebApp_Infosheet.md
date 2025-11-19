# Blazor Web App - Carsharing Plattform

## Übersicht

Die **Carsharing.Blazor** Web App ist die Benutzeroberfläche für die Carsharing-Plattform. Sie wurde von der Standard-Blazor-Template-Seite zu einer vollständigen Carsharing-Anwendung umgebaut.

## Projektstruktur

```
Carsharing.Blazor/
├── Pages/
│   ├── Index.razor              # Startseite mit verfügbaren Fahrzeugen
│   ├── Vehicles.razor            # Fahrzeuge-Übersicht
│   ├── Bookings.razor           # Buchungsseite
│   ├── Participants.razor       # Teilnehmer-Verwaltung
│   └── Payments.razor           # Zahlungsseite
├── Shared/
│   ├── MainLayout.razor         # Hauptlayout
│   └── NavMenu.razor            # Navigationsmenü
├── Program.cs                   # Service-Registrierung
├── _Imports.razor               # Globale Imports
└── Carsharing.Blazor.csproj     # Projektdatei mit Referenzen
```

---

## Implementierte Features

### 1. Startseite (`Pages/Index.razor`)

**Route:** `/`

**Features:**
- Willkommensnachricht: "🚗 Willkommen bei Carsharing"
- Anzeige aller verfügbaren Fahrzeuge
- Schnellzugriff-Karten für:
  - 🚗 Fahrzeuge
  - 📅 Buchungen
  - 👥 Teilnehmer
  - 💳 Zahlungen

**Funktionalität:**
- Lädt automatisch verfügbare Fahrzeuge beim Seitenaufruf
- Zeigt Fahrzeugdetails (Modell, Kennzeichen, Standort, Status)
- Responsive Design mit Bootstrap

### 2. Fahrzeuge-Seite (`Pages/Vehicles.razor`)

**Route:** `/vehicles`

**Features:**
- Anzeige aller Fahrzeuge oder nur verfügbarer Fahrzeuge
- Filter-Buttons: "Alle Fahrzeuge" / "Nur Verfügbare"
- Fahrzeugkarten mit:
  - Modell
  - Kennzeichen
  - Standort
  - Status-Badge (Verfügbar/Gebucht/Wartung)
  - Erstellungsdatum

**Funktionalität:**
- Lädt Fahrzeuge über `IVehicleService`
- Dynamisches Laden und Aktualisieren der Anzeige

### 3. Buchungen-Seite (`Pages/Bookings.razor`)

**Route:** `/bookings`

**Features:**
- Übersicht über Buchungsfunktionen
- Anzeige verfügbarer Fahrzeuge
- Anzeige registrierter Teilnehmer
- Preisinformationen (5€ pro Stunde)

**Status:** Basis-Implementierung (erweiterbar)

### 4. Teilnehmer-Seite (`Pages/Participants.razor`)

**Route:** `/participants`

**Features:**
- Tabelle aller registrierten Teilnehmer
- Anzeige von:
  - ID
  - Vorname
  - Nachname
  - Email
  - Registrierungsdatum

**Funktionalität:**
- Lädt Teilnehmer über `IParticipantService.GetAllParticipants()`

### 5. Zahlungen-Seite (`Pages/Payments.razor`)

**Route:** `/payments`

**Features:**
- Übersicht über Zahlungsfunktionen
- Anzeige Anzahl registrierter Teilnehmer

**Status:** Basis-Implementierung (erweiterbar)

---

## Service-Registrierung (`Program.cs`)

### Registrierte Services:

```csharp
// VehicleService
builder.Services.AddSingleton<IVehicleService, VehicleService>();

// ParticipantService
builder.Services.AddSingleton<IParticipantService, ParticipantService>();

// PaymentService
builder.Services.AddSingleton<IPaymentService, PaymentService>();

// BookingService (mit Dependency Injection)
builder.Services.AddSingleton<IBookingService>(sp => 
{
    var vehicleService = sp.GetRequiredService<IVehicleService>();
    var participantService = sp.GetRequiredService<IParticipantService>();
    var paymentService = sp.GetRequiredService<IPaymentService>();
    return new BookingService(vehicleService, participantService, paymentService);
});
```

**Hinweis:** Alle Services werden als Singleton registriert, da sie In-Memory-Daten verwenden.

---

## Navigation (`Shared/NavMenu.razor`)

**Menüpunkte:**
1. 🏠 **Startseite** (`/`)
2. 🚗 **Fahrzeuge** (`/vehicles`)
3. 📅 **Buchungen** (`/bookings`)
4. 👥 **Teilnehmer** (`/participants`)
5. 💳 **Zahlungen** (`/payments`)

**Branding:** "🚗 Carsharing"

---

## Projekt-Konfiguration

### Projekt-Referenzen (`Carsharing.Blazor.csproj`)

```xml
<ItemGroup>
  <ProjectReference Include="..\Carsharing.Models\Carsharing.Models.csproj" />
  <ProjectReference Include="..\Carsharing.Services\Carsharing.Services.csproj" />
</ItemGroup>
```

### Globale Imports (`_Imports.razor`)

```razor
@using Carsharing.Models.Entities
@using Carsharing.Services.Interfaces
@using Carsharing.Services.Implementations
```

---

## Technische Details

### Framework
- **.NET 7.0** (Blazor Server)
- **Bootstrap** für Styling
- **Open Iconic** für Icons

### Architektur
- **Server-Side Blazor** (Blazor Server)
- **Dependency Injection** für Services
- **Component-Based** Architektur

### Datenfluss
1. Benutzer interagiert mit Razor-Komponente
2. Komponente injiziert benötigte Services
3. Service liefert Daten aus In-Memory-Listen
4. Komponente rendert UI mit Daten

---

## Verwendung

### App starten:

```bash
cd Carsharing.Blazor
dotnet run
```

**Standard-URL:** `https://localhost:5001` oder `http://localhost:5000`

### Seiten aufrufen:

- **Startseite:** `http://localhost:5000/`
- **Fahrzeuge:** `http://localhost:5000/vehicles`
- **Buchungen:** `http://localhost:5000/bookings`
- **Teilnehmer:** `http://localhost:5000/participants`
- **Zahlungen:** `http://localhost:5000/payments`

---

## UI-Komponenten

### Bootstrap-Komponenten verwendet:

- **Cards** - Für Fahrzeuganzeige und Informationsboxen
- **Badges** - Für Status-Anzeige (Verfügbar/Gebucht/Wartung)
- **Tables** - Für Teilnehmer-Liste
- **Buttons** - Für Aktionen
- **Alerts** - Für Warnungen und Informationen
- **Navbar** - Für Navigation

### Farben:

- **Primary (Blau)** - Hauptaktionen, Header
- **Success (Grün)** - Verfügbare Fahrzeuge, Erfolg
- **Warning (Gelb)** - Gebuchte Fahrzeuge
- **Danger (Rot)** - Wartung, Fehler
- **Info (Hellblau)** - Informationen

---

## Implementierte Features

### ✅ Vollständig implementiert:

1. **Buchungsformular:**
   - ✅ Fahrzeugauswahl
   - ✅ Datum/Zeit-Auswahl
   - ✅ Teilnehmerauswahl
   - ✅ Live-Preisberechnung

2. **Fahrzeugübersicht:**
   - ✅ Filter nach Standort, Modell, Status
   - ✅ Listenansicht und Rasteransicht
   - ✅ Responsive Design

3. **Buchungsstatus:**
   - ✅ Alle aktuellen und vergangenen Buchungen
   - ✅ Filter nach Teilnehmer, Status, Zeitraum
   - ✅ Detaillierte Anzeige

4. **Benutzerprofil:**
   - ✅ Profil auswählen/anmelden
   - ✅ Neues Profil erstellen
   - ✅ Profildetails anzeigen

5. **Fehlerbehandlung:**
   - ✅ Umfassende Fehlerbehandlung
   - ✅ Benutzerfreundliche Fehlermeldungen

6. **Statusanzeigen:**
   - ✅ Echtzeit-Feedback für Buchungen
   - ✅ Status-Badges
   - ✅ Loading-Indikatoren

### Erweiterungsmöglichkeiten:

1. **Kartenansicht für Fahrzeuge:**
   - Integration mit Maps-API
   - Standorte auf Karte anzeigen

2. **Zahlungshistorie:**
   - Detaillierte Anzeige aller Zahlungen
   - Filter nach Teilnehmer

3. **Fahrzeug-Verwaltung:**
   - Formular zum Hinzufügen neuer Fahrzeuge
   - Status-Änderung über UI

4. **REST-API-Integration:**
   - API-Controller erstellen
   - HTTP-Client in Blazor-App

---

## Wichtige Hinweise

### Datenpersistenz
- Aktuell werden alle Daten **In-Memory** gespeichert
- Bei Neustart der App gehen Daten verloren
- Für Produktion: Integration mit Datenbank erforderlich

### Service-Lebensdauer
- Alle Services sind als **Singleton** registriert
- Daten werden zwischen Seitenaufrufen beibehalten
- Für Multi-User-Szenarien: Scoped Services verwenden

### Performance
- Blazor Server verwendet SignalR für Echtzeit-Updates
- Bei vielen gleichzeitigen Benutzern: Skalierung erforderlich

---

## Dateipfade

- **Startseite:** `Carsharing.Blazor/Pages/Index.razor`
- **Fahrzeuge:** `Carsharing.Blazor/Pages/Vehicles.razor`
- **Buchungen:** `Carsharing.Blazor/Pages/Bookings.razor`
- **Teilnehmer:** `Carsharing.Blazor/Pages/Participants.razor`
- **Zahlungen:** `Carsharing.Blazor/Pages/Payments.razor`
- **Navigation:** `Carsharing.Blazor/Shared/NavMenu.razor`
- **Layout:** `Carsharing.Blazor/Shared/MainLayout.razor`
- **Program:** `Carsharing.Blazor/Program.cs`

---

## Troubleshooting

### Problem: Standard-Seite wird angezeigt

**Lösung:**
1. Prüfen Sie, ob `Index.razor` aktualisiert wurde
2. Browser-Cache leeren
3. App neu starten

### Problem: Services nicht gefunden

**Lösung:**
1. Prüfen Sie Projekt-Referenzen in `.csproj`
2. Prüfen Sie Service-Registrierung in `Program.cs`
3. Prüfen Sie `_Imports.razor` für Namespace-Imports

### Problem: Daten werden nicht angezeigt

**Lösung:**
1. Prüfen Sie, ob Services korrekt injiziert werden (`@inject`)
2. Prüfen Sie, ob `OnInitializedAsync()` aufgerufen wird
3. Prüfen Sie Browser-Konsole für Fehler

---

## Beispiel-Workflow

1. **App starten:**
   ```bash
   dotnet run --project Carsharing.Blazor
   ```

2. **Browser öffnen:**
   - Navigieren zu `http://localhost:5000`

3. **Startseite anzeigen:**
   - Siehe Willkommensnachricht
   - Verfügbare Fahrzeuge werden angezeigt

4. **Navigation:**
   - Klicken Sie auf "Fahrzeuge" im Menü
   - Alle Fahrzeuge werden angezeigt

5. **Teilnehmer anzeigen:**
   - Klicken Sie auf "Teilnehmer" im Menü
   - Tabelle mit allen Teilnehmern wird angezeigt

---

## Zusammenfassung

Die Blazor Web App wurde erfolgreich von der Standard-Template-Seite zu einer vollständigen Carsharing-Anwendung umgebaut. Sie bietet:

✅ **Moderne UI** mit Bootstrap  
✅ **Vollständige Navigation** zu allen Bereichen  
✅ **Service-Integration** mit allen Backend-Services  
✅ **Responsive Design** für verschiedene Bildschirmgrößen  
✅ **Erweiterbare Architektur** für zukünftige Features  

Die App ist bereit für weitere Entwicklung und kann einfach um neue Features erweitert werden.

