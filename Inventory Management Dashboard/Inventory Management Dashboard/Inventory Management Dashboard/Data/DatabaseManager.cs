using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using Inventory_Management_Dashboard.Models;

namespace Inventory_Management_Dashboard.Data
{
    internal class DatabaseManager
    {
        // The string that tells where the file is.
        // If it doesnt exist, SQLites will create the file automatically.
        private readonly string connectionString = @"Data Source=Data\inventory.db";


        // INSERT
        public void InsertInventoryItem(string uid, string name, string status)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Inventory (uid,name,status) VALUES($uid,$name,$status);";

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("$uid", uid);
                    command.Parameters.AddWithValue("$name", name);
                    command.Parameters.AddWithValue("$status", status);

                    // Used for commands that don't return any data, like INSERT, UPDATE, DELETE.
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertStaffMember(string uid, string name)
        {
            using(var connection = new SqliteConnection(connectionString))
            {
                string insertQuery = "INSERT INTO Staff (uid, name) VALUES ($uid,$name)";

                using(var command = new SqliteCommand(insertQuery))
                {
                    command.Parameters.AddWithValue("$uid", uid);
                    command.Parameters.AddWithValue("$name", name);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Uses string instead of DateTime for timestamp because SQLite does not have a native DateTime type.
        public void InsertLogEntry(int staffID, int inventoryID, string borrowedTime)
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO Logs (staffID,inventoryID,borrowedTime) VALUES ($staffID,$inventoryID,$borrowedTime)";

                    using (var command = new SqliteCommand(insertQuery,connection))
                    {
                        command.Parameters.AddWithValue("$staffID", staffID);
                        command.Parameters.AddWithValue("$inventoryID", inventoryID);
                        command.Parameters.AddWithValue("$borrowedTime", borrowedTime);

                        command.ExecuteNonQuery();
                    }
                }
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // UPDATE
        public void updateInventoryItemStatus(string uid, string newStatus)
        { 
            using(var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string updateQuery = "UPDATE Inventory SET status = $newStatus WHERE uid = $uid ";
                using (var command = new SqliteCommand(updateQuery, connection)) 
                {
                    command.Parameters.AddWithValue("$newStatus", newStatus);
                    command.Parameters.AddWithValue("$uid", uid);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void updateLogEntryReturnTime(int logID, string returnTime)
        {
            using(var connection = new SqliteConnection(connectionString)) 
            {
                connection.Open();
                string updateQuery = "UPDATE Logs SET returnTime = $returnTime WHERE logID = $logID";
                using (var command = new SqliteCommand(updateQuery, connection)) 
                {
                    command.Parameters.AddWithValue("$returnTime", returnTime);
                    command.Parameters.AddWithValue("$logID", logID);

                    command.ExecuteNonQuery();
                }
            }
        }

        // SELECT
        public List<InventoryItem> selectAllInventoryItems()
        {
            var inventoryItems = new List<InventoryItem>();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM Inventory";
                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new InventoryItem
                            {
                                inventoryID = reader.GetInt32(0),
                                uid = reader.GetString(1),
                                name = reader.GetString(2),
                                status = reader.GetString(3)
                            };
                            inventoryItems.Add(item);
                        }
                    }
                }
            }
            return inventoryItems;
        }

        // next are ALL logs, and logs by staffID or inventoryID
        public List<StaffMember> selectAllStaffMembers() 
        {
            var staffMembers = new List<StaffMember>();
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT * FROM Staff;";
                    using (var command = new SqliteCommand(selectQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new StaffMember
                                {
                                    staffID = reader.GetInt32(0),
                                    uid = reader.GetString(1),
                                    name = reader.GetString(2)
                                };
                                staffMembers.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Error:{ex.Message}");
            }
            
            
            return staffMembers;
        }

        public List<LogEntry> selectAllLogEntries()
        {
            var logEntries = new List<LogEntry>();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM Logs;";
                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new LogEntry
                            {
                                logID = reader.GetInt32(0),
                                staffID = reader.GetInt32(1),
                                inventoryID = reader.GetInt32(2),
                                borrowedTime = reader.GetString(3),
                                returnedTime = reader.IsDBNull(4) ? null : reader.GetString(4)
                            };
                        }
                    }
                }
            }

            return logEntries;
        }

        public LogEntry selectLogEntryByID(int staffID, int inventoryID) 
        {
            LogEntry logEntry = null;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT logID FROM Logs WHERE staffID = $staffID AND inventoryID = $inventoryID AND returnedTime IS NULL LIMIT 1;";
                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("$staffID", staffID);
                    command.Parameters.AddWithValue("$inventoryID", inventoryID);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            logEntry = new LogEntry();
                            logEntry.logID = reader.GetInt32(0);
                        }
                    }
                }
            }
            return logEntry;
        }

        public StaffMember selectStaffMemberByUID(string uid)
        {
            StaffMember staffMember = null;

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT * FROM Staff WHERE uid = $uid";
                    using (var command = new SqliteCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("$uid", uid);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                staffMember = new StaffMember();
                                staffMember.staffID = reader.GetInt32(0);
                                staffMember.uid = reader.GetString(1);
                                staffMember.name = reader.GetString(2);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error in selectStaffMemberByUID: {ex.Message}");
            }
            
            if (staffMember == null)
            {
                Console.WriteLine("Staff member not found in database.");
            }

            return staffMember; 
        }

        public InventoryItem selectInventoryItemByUID(string uid) 
        {
            InventoryItem inventoryItem = null;
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT * FROM Inventory WHERE uid = $uid";
                    using (var command = new SqliteCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("$uid", uid);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                inventoryItem = new InventoryItem();
                                inventoryItem.inventoryID = reader.GetInt32(0);
                                inventoryItem.uid = reader.GetString(1);
                                inventoryItem.name = reader.GetString(2);
                                inventoryItem.status = reader.GetString(3);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in selectStaffMemberByUID: {ex.Message}");
            }
            
            
            return inventoryItem;
        }
    }
}
