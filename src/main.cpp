#include <Arduino.h>
#include <MFRC522.h>
#include <SPI.h>

// PICC = Proximity Integrated Circuit Card (Fancy Name for RFID)
// PCD = Proximity Coupling Device (Fancy Name for RFID Reader)

// Configuration SPI BUS
#define RST_PIN 9  // Allows Arduino to reset/wake/power off RC522
#define SDA_PIN 10 // Slave Select, enables Arduino to talk to RFID Reader

// MFCR522 Object/Instance
MFRC522 mfrc5222(SDA_PIN, RST_PIN);

// Initialization array that will store new NUID
byte nuidPICC[4];

int myFunction(int, int);
void resetBaudRate(MFRC522 &);
void dump_byte_array(byte *, byte);

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
  // Reset the loop if no new card present on the sensor/reader. This saves the entire process when idle
  if (!mfrc5222.PICC_IsNewCardPresent()) {
    return;
  }

  // Verify if the NUID has been readed

  if (!mfrc5222.PICC_ReadCardSerial()) {
    return;
  }

  Serial.print(F("PICC type: "));
  MFRC522::PICC_Type piccType = mfrc5222.PICC_GetType(mfrc5222.uid.sak);
  Serial.println(mfrc5222.PICC_GetTypeName(piccType));

  // Check is the PICC of Classic MIFARE type
  if (piccType != MFRC522::PICC_TYPE_MIFARE_MINI &&
      piccType != MFRC522::PICC_TYPE_MIFARE_1K &&
      piccType != MFRC522::PICC_TYPE_MIFARE_4K) {
    Serial.println(F("Your tag is not of type MIFARE Classic."));
    return;
  }

  if (mfrc5222.uid.uidByte[0] != nuidPICC[0] ||
      mfrc5222.uid.uidByte[1] != nuidPICC[1] ||
      mfrc5222.uid.uidByte[2] != nuidPICC[2] ||
      mfrc5222.uid.uidByte[3] != nuidPICC[3]) {
    Serial.println("A new card has been detected.");

    // Store NUID into nuidPICC array
    for (byte i = 0; i < 4; i++) {
      nuidPICC[i] = mfrc5222.uid.uidByte[i];
    }

    Serial.println(F("The NUID tag is:"));
    dump_byte_array(mfrc5222.uid.uidByte, mfrc5222.uid.size);
  } else
    Serial.println("Card read previously");

  // Halt PICC (Makes RFID card go to sleep)
  mfrc5222.PICC_HaltA();

  //Stop expecting encrypted data on PCD
  mfrc5222.PCD_StopCrypto1();
}

// put function definitions here:
void resetBaudRate(MFRC522 &mfrc5222) {
  // Resetting the Baud Rate (Low Level operation) - not good to put in looop
  // When an RFID connects to a card, sometimes reader & card will negotiate different speeds
  // This can affect the ability to read a new card, because of wrong speeds.
  // Resetting them forces the reader back to default listening mode
  mfrc5222.PCD_WriteRegister(mfrc5222.TxModeReg, 0x00); // Transmitter Mode Register, reset transmitter speed to default
  mfrc5222.PCD_WriteRegister(mfrc5222.RxModeReg, 0x00); // Receiver Mode Register, reset receiver speed to default
  // Reset ModWidthReg
  mfrc5222.PCD_WriteRegister(mfrc5222.ModWidthReg, 0x26); // Modulation Width Register, Resets wireless pulse width to default
}

/**
 * Helper routine to dump a byte array as hex values to Serial.
 */
void dump_byte_array(byte *buffer, byte bufferSize) {
  for (byte i = 0; i < bufferSize; i++) {
    Serial.print(buffer[i] < 0x10 ? " 0" : " ");
    Serial.print(buffer[i], HEX);
  }
  Serial.println();
}