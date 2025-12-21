# Demo-Skript (PaymentService Microservice)

## Ziel
- Live-Demo zeigt die echte View (`WebGUI`) und die API (`RestApi`/Swagger).
- Falls etwas nicht läuft: ausgeblendete Folien in der PowerPoint zeigen Screenshots.

## Ports (Default)
- RestApi: `http://localhost:5261`
  - Swagger: `http://localhost:5261/swagger`
- WebGUI: `http://localhost:5019`
  - Payment View: `http://localhost:5019/Payment`

## Setup (vor der Präsentation)
1. Terminal 1 (RestApi)
   - `cd PaymentService/RestApi`
   - `dotnet run`
2. Terminal 2 (WebGUI)
   - `cd PaymentService/WebGUI`
   - `dotnet run`

Hinweis:
- Die DB ist SQLite und wird automatisch erstellt (`EnsureCreated`) → keine extra Installation notwendig.

## Live-Demo Ablauf (10–12 Minuten)

### A) Swagger (RestApi)
1. Browser öffnen: `http://localhost:5261/swagger`
2. Endpoint testen:
   - `POST /api/payment`
   - Beispiel-Body:
     ```json
     {
       "participantId": 1,
       "amount": 12.5,
       "confirmPayment": true
     }
     ```
3. Ergebnis zeigen:
   - Response enthält `paymentId`, `status`, `createdAt`
4. History testen:
   - `GET /api/payment?participantId=1`

### B) WebGUI
1. Browser öffnen: `http://localhost:5019/Payment`
2. Payment anlegen (Form):
   - ParticipantId: `1`
   - Amount: `12.50`
   - ConfirmPayment: aktiv → “Completed”
3. History laden:
   - oben rechts `participantId=1` → `Load`
4. Optional: Fehlfall zeigen:
   - ConfirmPayment deaktivieren → Status “Failed”

### C) Tests (kurz)
1. `dotnet test PaymentService/MicroservicePayment.sln`
2. Kurz erwähnen:
   - Validation Tests
   - Status-Regel Tests
   - History Tests

## Backup (Fallback)
- Wenn Swagger nicht erreichbar:
  - In der PowerPoint die ausgeblendete Folie `restapi_swagger.png` einblenden/anzeigen.
- Wenn WebGUI nicht erreichbar:
  - In der PowerPoint die ausgeblendete Folie `webgui_payment_index.png` einblenden/anzeigen.
- Wenn Tests nicht laufen:
  - In der PowerPoint `unit_tests.png` zeigen.

# Live-Demo Skript (PaymentService Microservice)

## Ziel
- Live-Demo der **echten Views** (WebGUI) und **Swagger** (RestApi)
- Falls etwas nicht läuft: **Fallback** über ausgeblendete Screenshot-Folien in der PowerPoint

## Setup (vor Präsentation)

### Terminal 1 – RestApi starten
```powershell
cd PaymentService\RestApi
dotnet run
```
- Erwartet: läuft auf `http://localhost:5261`
- Swagger: `http://localhost:5261/swagger`

### Terminal 2 – WebGUI starten
```powershell
cd PaymentService\WebGUI
dotnet run
```
- Erwartet: läuft auf `http://localhost:5019`
- Payment View: `http://localhost:5019/Payment`

## Demo-Ablauf (während Präsentation)

### 1) Swagger zeigen (RestApi)
1. Öffne `http://localhost:5261/swagger`
2. Endpoint testen: `POST /api/payment`
3. Beispiel-Body:
```json
{
  "participantId": 1,
  "amount": 12.50,
  "confirmPayment": true
}
```
4. Ergebnis kurz erklären (Created, ReadPaymentDto mit Status)

### 2) WebGUI zeigen (View)
1. Öffne `http://localhost:5019/Payment`
2. Create payment:
   - ParticipantId: `1`
   - Amount: `9.99`
   - Confirm payment: checked
3. Danach History laden:
   - oben rechts `participantId=1` → Load
4. Ergebnis: Tabelle mit Zahlungen (CreatedAt UTC, Status)

### 3) Tests zeigen
1. (Optional live) `dotnet test PaymentService\MicroservicePayment.sln`
2. Kurz erklären: Validation + Status-Regeln + History

## Fallback (wenn Live-Demo nicht läuft)
- Wenn Swagger nicht erreichbar ist:
  - in der PPTX auf die **ausgeblendete** Folie mit `restapi_swagger.png` wechseln
- Wenn WebGUI nicht erreichbar ist:
  - in der PPTX auf die **ausgeblendete** Folie mit `webgui_payment_index.png` wechseln
- Wenn Tests nicht laufen:
  - in der PPTX auf die **ausgeblendete** Folie mit `unit_tests.png` wechseln

## Hinweise (für „läuft überall“)
- PaymentService nutzt lokale SQLite-Datei (wird beim Start automatisch erstellt).
- Es ist keine externe DB-Installation nötig.


