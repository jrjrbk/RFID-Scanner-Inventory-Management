#include <Arduino.h>
#include <MFRC522.h>
#include <SPI.h>

// Configuration SPI BUS
#define RST_PIN 9  // Allows Arduino to reset/wake/power off RC522
#define SDA_PIN 10 // Slave Select, enables Arduino to talk to RFID Reader

// MFCR522 Object/Instance
MFRC522 mfrc5222(SDA_PIN, RST_PIN);

// State tracking (Since Arduino runs in a loop)
bool rfid_tag_present_prev = false; // Remembers if a tag was detected in the last loop
bool rfid_tag_present = false;      // Tracks if a tag is detected RIGHT NOW in the current loop

// Error Mitigation
int _rfid_error_counter = 0; // A counter to handle "debouncing". If the read fails and the counter reaches 3, we declare the tag is removed
bool _tag_found = false;     // A temp flag during a scan, to indicate a successful communnication with a tag

//  put function declarations here:
int myFunction(int, int);

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600); // Intialize serial communication with 9600 baud rate aka bits of data per second

  while (!Serial);     // Do nothing if no serial port opened
  SPI.begin();         // Initialize SPI bus (Allows communication of SDA_PIN and RST_PIN)
  mfrc5222.PCD_Init(); // Initialise MFRC522

  // Check to see if working and print version
  mfrc5222.PCD_DumpVersionToSerial();
}

void loop() {
  // put your main code here, to run repeatedly:
  rfid_tag_present_prev = rfid_tag_present;

  _rfid_error_counter += 1;
  if (_rfid_error_counter > 2) {
    _tag_found = false;
  }

  // Detect tag without looking for collisions
  byte bufferATQA[2]; // ATQA - Answer To Request, Type A. Card replies with a 2-byte code, hence SIZE 2 byte array
  byte bufferSize = sizeof(bufferATQA);

  // Resetting the Baud Rate (Low Level operation) - not good to put in looop
  // When an RFID connects to a card, sometimes reader & card will negotiate different speeds
  // This can affect the ability to read a new card, because of wrong speeds.
  // Resetting them forces the reader back to default listening mode
  mfrc5222.PCD_WriteRegister(mfrc5222.TxModeReg, 0x00); // Transmitter Mode Register, reset transmitter speed to default
  mfrc5222.PCD_WriteRegister(mfrc5222.RxModeReg, 0x00); // Receiver Mode Register, reset receiver speed to default
  // Reset ModWidthReg
  mfrc5222.PCD_WriteRegister(mfrc5222.ModWidthReg, 0x26); // Modulation Width Register, Resets wireless pulse width to default

  // Makes the RFID Reader broadcast a wireless signal called REQA (Request Command, Type A)
  // If a tag is in range, saves the 2-byte identification code to bufferATQA (The type of card)
  // result -> STATUS_OK, STATUS_TIMEOUT, STATUS_ERROR
  MFRC522::StatusCode result = mfrc5222.PICC_RequestA(bufferATQA, &bufferSize);

  if (result == mfrc5222.STATUS_OK) {
    // If the reading process fails, exit loop
    if (!mfrc5222.PICC_ReadCardSerial()) {
      return;
    }

    _rfid_error_counter = 0; // Reset error counter
    _tag_found = true;       // tag is found
  }

  rfid_tag_present = _tag_found;

  // rising edge
  if (rfid_tag_present && !rfid_tag_present_prev) {
    Serial.println("Tag Found");
  }

  // falling edge
  if (!rfid_tag_present && rfid_tag_present_prev) {
    Serial.println("Tag gone");
  }
}

// put function definitions here:
