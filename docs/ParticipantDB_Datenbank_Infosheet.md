# ParticipantDB - Datenbank-Implementierung

## Übersicht

Der **ParticipantService** verwendet eine SQL Server Datenbank zur persistenten Speicherung von Teilnehmerdaten.

## Datenbank-Struktur

### Datenbank: ParticipantDB

**Connection String:**
```
Data Source=ParticipantDB.db
```

**Hinweis:** Die Datenbank verwendet SQLite (keine Installation erforderlich). Die Datenbankdatei `ParticipantDB.db` wird automatisch im Projektverzeichnis erstellt.

### Tabelle: Participants

**Spalten:**

| Spalte | Datentyp | Beschreibung | Constraints |
|--------|----------|---------------|-------------|
| `ParticipantId` | int | Eindeutige ID des Teilnehmers | Primary Key, Identity |
| `FirstName` | nvarchar(100) | Vorname | Required, MaxLength: 100 |
| `LastName` | nvarchar(100) | Nachname | Required, MaxLength: 100 |
| `Email` | nvarchar(255) | E-Mail-Adresse | Required, MaxLength: 255 |
| `BirthDate` | date | Geburtsdatum | Nullable |
| `Weight` | decimal(5,2) | Gewicht in kg | Nullable |
| `Height` | decimal(5,2) | Größe in cm | Nullable |
| `CreatedAt` | datetime | Erstellungszeitpunkt | Required |
| `UpdatedAt` | datetime | Letzte Aktualisierung | Required |

## Entity Framework Core Konfiguration

### DbContext: ParticipantDbContext

**Datei:** `Carsharing.Data/DbContext/ParticipantDbContext.cs`

**Konfiguration:**
```csharp
public class ParticipantDbContext : DbContext
{
    public DbSet<Participant> Participants { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tabellen- und Spalten-Konfiguration
    }
}
```

**Features:**
- Automatische Tabellenerstellung (`EnsureCreated()`)
- Fluent API Konfiguration
- Spalten-Definitionen
- Constraints und Datentypen

## Service-Implementierung

### ParticipantService mit Datenbank

**Änderungen:**
- Verwendet `ParticipantDbContext` statt In-Memory-Liste
- Alle CRUD-Operationen über Entity Framework
- Automatische Seed-Daten beim ersten Start
- `SaveChanges()` nach jeder Änderung

**Methoden:**
- `ParticipantExists()` - Prüft Existenz über Datenbankabfrage
- `GetParticipant()` - Lädt Teilnehmer aus Datenbank
- `GetAllParticipants()` - Lädt alle Teilnehmer aus Datenbank
- `AddParticipant()` - Fügt neuen Teilnehmer hinzu (mit `CreatedAt` und `UpdatedAt`)
- `UpdateParticipant()` - Aktualisiert Teilnehmer (setzt `UpdatedAt`)
- `DeleteParticipant()` - Löscht Teilnehmer aus Datenbank

## Registrierung

### Program.cs

```csharp
// Database Context registrieren
builder.Services.AddDbContext<ParticipantDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ParticipantDB")));

// Service als Scoped registrieren (für DbContext)
builder.Services.AddScoped<IParticipantService, ParticipantService>();
```

**Wichtig:** ParticipantService ist als **Scoped** registriert (nicht Singleton), da DbContext Scoped sein muss.

## Connection String Konfiguration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "ParticipantDB": "Data Source=ParticipantDB.db"
  }
}
```

### appsettings.Development.json

```json
{
  "ConnectionStrings": {
    "ParticipantDB": "Data Source=ParticipantDB.db"
  }
}
```

## Datenbank-Initialisierung

### Automatische Erstellung

Beim ersten Start des ParticipantService:
1. `EnsureDatabaseCreated()` wird aufgerufen
2. Datenbank wird erstellt, falls nicht vorhanden
3. Tabellen werden erstellt
4. Seed-Daten werden eingefügt (falls Tabelle leer)

### Seed-Daten

**Testdaten:**
- Max Mustermann (max@example.com)
  - Geburtsdatum: 15.05.1990
  - Gewicht: 75.5 kg
  - Größe: 180.0 cm
  
- Anna Schmidt (anna@example.com)
  - Geburtsdatum: 22.08.1992
  - Gewicht: 65.0 kg
  - Größe: 165.0 cm

## Verwendung

### In Blazor-App

```csharp
@inject IParticipantService ParticipantService

// Teilnehmer laden
var participants = ParticipantService.GetAllParticipants();

// Neuen Teilnehmer hinzufügen
var newParticipant = new Participant 
{ 
    FirstName = "John",
    LastName = "Doe",
    Email = "john@example.com",
    BirthDate = new DateTime(1995, 1, 1),
    Weight = 80.0m,
    Height = 175.0m
};
ParticipantService.AddParticipant(newParticipant);
```

## Migrationen (Optional)

Für Produktionsumgebung können Migrationen erstellt werden:

```bash
cd Carsharing.Data
dotnet ef migrations add InitialCreate --startup-project ../Carsharing.Blazor
dotnet ef database update --startup-project ../Carsharing.Blazor
```

## Wichtige Hinweise

### Datenpersistenz
- ✅ Daten werden **persistent** in SQL Server gespeichert
- ✅ Daten bleiben nach Neustart erhalten
- ✅ Mehrere Benutzer können gleichzeitig zugreifen

### Service-Lebensdauer
- ParticipantService ist als **Scoped** registriert
- DbContext wird pro Request erstellt
- Thread-safe durch DbContext-Isolation

### Datenbankanforderungen
- **SQLite** wird verwendet (keine Installation erforderlich)
- Die Datenbankdatei `ParticipantDB.db` wird automatisch erstellt
- Für Produktion: Kann einfach auf SQL Server umgestellt werden (Connection String ändern)

## Troubleshooting

### Problem: Datenbank kann nicht erstellt werden

**Lösung:**
1. Prüfen Sie Schreibrechte im Projektverzeichnis
2. Prüfen Sie Connection String
3. Die Datenbankdatei wird automatisch erstellt beim ersten Start

### Problem: "Cannot access database"

**Lösung:**
1. Prüfen Sie, ob die Datei `ParticipantDB.db` im Projektverzeichnis existiert
2. Prüfen Sie Connection String
3. Löschen Sie die Datei `ParticipantDB.db` und starten Sie die App neu (wird neu erstellt)

### Problem: Seed-Daten werden nicht geladen

**Lösung:**
- Seed-Daten werden nur geladen, wenn die Tabelle leer ist
- Bei vorhandenen Daten werden keine Seed-Daten eingefügt

## Dateipfade

- **DbContext:** `Carsharing.Data/DbContext/ParticipantDbContext.cs`
- **Model:** `Carsharing.Models/Entities/Participant.cs`
- **Service:** `Carsharing.Services/Implementations/ParticipantService.cs`
- **Connection String:** `Carsharing.Blazor/appsettings.json`

