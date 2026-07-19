using Inventory_Management_Dashboard.Data;
using Inventory_Management_Dashboard.Models;
using Inventory_Management_Dashboard.Services;
using System.Security.Cryptography;

namespace Inventory_Management_Dashboard
{
    public partial class MainDashboard : Form
    {
        // The list of UIDs that have been read from the RFID reader.
        SerialReader RFIDReader = new SerialReader();
        static List<string> UID = new List<string>();
        static StaffMember staff = null;
        static InventoryItem item = null;
        static string dateTime = "";

        public MainDashboard()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialisation of the window
            var db = new DatabaseManager();

            var logEntries = db.selectAllLogEntries();
            dataGridView1.DataSource = logEntries;

            
            RFIDReader.RFIDDataReceived += RFIDDataReceivedHandler;

            RFIDReader.StartReading("COM14", 9600);
            //Console.WriteLine("Press any key to stop");
            //Console.ReadKey();
            //RFIDReader.StopReading();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            StaffList newWindow = new StaffList();
            newWindow.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InventoryList newWindow = new InventoryList();
            newWindow.ShowDialog();
        }

        void RFIDDataReceivedHandler(string data)
        {
            DatabaseManager db = new DatabaseManager();
            Console.WriteLine($"Data received: {data}");

            if (data.Contains(":"))
            {
                if (data.Contains("NUID"))
                {
                    string newUID = data.Split(':')[1].Trim();
                    if (UID.Count > 0 && UID[0] == newUID)
                    {
                        Console.WriteLine("Detected Duplicate!");
                        return; // Ignore if the new UID is the same as the first UID
                    }
                    UID.Add(newUID);
                    //Console.WriteLine($"Successfully added UID: {newUID}");
                    Console.WriteLine($"UID COUNT: {UID.Count}");

                    // Check if UID 1 is a staff UID
                    if (UID.Count == 1)
                    {
                        staff = db.selectStaffMemberByUID(UID[0]);
                        if (staff == null)
                        {
                            MessageBox.Show($"The UID {UID[0]} is not a valid staff UID. Please scan a valid staff card.");
                            UID.Clear(); // Clear the list if the first UID is not a staff UID
                            dateTime = "";
                            RFIDReader.SendCommand("ERROR");
                        }
                        else
                        {
                            Console.WriteLine($"The UID {UID[0]} is valid with staff {staff.name}");
                            RFIDReader.SendCommand("SUCCESS");
                        }
                    }

                    // Check if there is enough UID to process a transaction
                    else if (UID.Count == 2)
                    {
                        item = db.selectInventoryItemByUID(UID[1]);
                        if (item == null)
                        {
                            MessageBox.Show($"The UID {UID[1]} is not a valid inventory UID. Please scan a valid inventory item.");
                            UID.RemoveAt(1); // Remove the second UID if it's not a valid inventory UID
                            RFIDReader.SendCommand("ERROR");
                            return; // Exit the method to wait for a new inventory UID
                        }
                        else
                        {
                            Console.WriteLine($"The UID {UID[1]} is valid with inventory {item.name}");
                            ProcessTransaction(staff.staffID, item.inventoryID, dateTime);

                            // Clear the list after processing the transaction
                            UID.Clear();
                            dateTime = ""; // Reset the dateTime after processing the transaction
                            RFIDReader.SendCommand("SUCCESS");
                        }
                    }
                }
                else if (data.Contains("Time") && string.IsNullOrEmpty(dateTime))
                {
                    dateTime = data.Replace("Time:", "").Trim();
                }

            }
        }

        void ProcessTransaction(int staffID, int inventoryID, string dateTime)
        {
            // Here you would implement the logic to process the transaction
            // For example, you might check if the staff member is authorized to borrow the item,
            // update the database to reflect that the item has been borrowed, etc.
            // Call the database manager to insert new log entry

            // Check if 

            DatabaseManager db = new DatabaseManager();
            LogEntry log = db.selectLogEntryByID(staffID, inventoryID);
            // 1. Check whether the item is currently borrowed or not
            if (log != null)
            {
                MessageBox.Show($"Log entry exists! ID: {log.logID}");
                // 2. if exist, update the log entry with current timestamp as returned time
                db.updateLogEntryReturnTime(log.logID, dateTime);
                db.updateInventoryItemStatus(inventoryID, "Available");
                RFIDReader.SendCommand("COMPLETE");
            }
            // 2. If no log entry exists, then create a new log entry with current timestamp
            else
            {
                MessageBox.Show("Log entry does not exists!");
                Console.WriteLine($"Creating entry with:\nstaffID: {staffID}\ninventoryID: {inventoryID}\ndateTime: {dateTime}");
                db.InsertLogEntry(staffID, inventoryID, dateTime);
                db.updateInventoryItemStatus(inventoryID, "Borrowed");
                RFIDReader.SendCommand("COMPLETE");
            }

            var logEntries = db.selectAllLogEntries();

            this.Invoke(new Action(() =>
            {
                dataGridView1.DataSource = logEntries;
            }));
        }

        
    }
}
