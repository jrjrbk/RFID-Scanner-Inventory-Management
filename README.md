# RFID-Scanner-Inventory-Management

An RFID Scanner to manage inventory. 

## Features
* **Real-Time Tracking:** Captures RFID Tags and instantly updates the UI dashboard
* **Hardware Feedback:** Incorporates a buzzer for instant feedback and an RTC Module for accurate timestamping
* **Local Storage:** Utilises a local SQLite database for persistent inventory data and staff records

## Usage
1. Open the desktop app
2. Connect the Arduino to identify COM Port
3. Scan a Staff RFID Card for authentication
4. Scan an Inventory RFID tag to log an item as borrowed or returned.

## Installation & Setup
### Hardware
1. Ensure PlatformIO is installed and Arduino is connected (Needs an RFID Module and a RTC Module)
2. Build and upload to Arduino using
 > pio run -t upload

### Software
1. Open the .sln file in Visual Studio
2. Install dependencies using NuGet
3. Build and run the code

## Environment & Dependencies

* **Editor:** VS Code, Visual Studio
* **Extension:** PlatformIO
* **Library Dependencies:**
  * `MFRC522 @ 1.4.12`
  * `makuna/RTC @ ^2.5.0`
  * `SPI @ 1.0`
* **Framework:** .NET Framework WinForm 

## Components Used

* Arduino UNO R3
* DS1302 RTC Module
* RC522 RFID Module
* White Card & Key Chain RFID
* MB-102 Breadboard
* Jumper Wires & Resistors
* USB Cable

## Wiring & Pin Connections

### RC522 Module (3.3V)
| Component Pin | Arduino Digital Pin | Function | Wire Color |
| :--- | :--- | :--- | :--- |
| **RST** | PIN 9 | Reset | Yellow |
| **SDA** | PIN 10 | Slave Select | Green |
| **MOSI** | PIN 11 | Master Out, Slave In | Blue |
| **MISO** | PIN 12 | Master In, Slave Out | Orange |
| **SCK** | PIN 13 | Serial Clock | Gray |

### DS1302 RTC Module (5V)
| Component Pin | Arduino Pin / Breadboard | Function | Wire Color |
| :--- | :--- | :--- | :--- |
| **GND** | Breadboard (5B -> 22B) | Ground | Black |
| **CLK** | PIN 5 | Clock | Gray |
| **DAT** | PIN 6 | Data/IO | Orange |
| **RST** | PIN 7 | Reset | Yellow |

## Media & References

**Source / Core Library:** [miguelbalboa/rfid](https://github.com/miguelbalboa/rfid)

<img width="850" height="462" alt="image" src="https://github.com/user-attachments/assets/0a320017-6444-419c-8f27-0609c3548e66" />

<img width="866" height="281" alt="image" src="https://github.com/user-attachments/assets/06068ca1-1748-4e06-81f5-c315fff129f3" />
