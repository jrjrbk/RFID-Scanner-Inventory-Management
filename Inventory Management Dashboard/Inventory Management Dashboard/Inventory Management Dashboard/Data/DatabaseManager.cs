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
        private readonly string connectionString = "Data Source=inventory.db";


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
            using (var connection = new SqliteConnection(connectionString))
            {
                string insertQuery = "INSERT INTO Logs (staffID,inventoryID,borrowedTime) VALUES ($staffID,$inventoryID,$borrowedTime)";
                
                using(var command = new SqliteCommand(insertQuery))
                {
                    command.Parameters.AddWithValue("$staffID", staffID);
                    command.Parameters.AddWithValue("$inventoryID", inventoryID);
                    command.Parameters.AddWithValue("$borrowedTime", borrowedTime);

                    command.ExecuteNonQuery();
                }
            }
        }

        // UPDATE
        public void updateInventoryItemStatus(string uid, string newStatus)
        { 
            using(var connection = new SqliteConnection(connectionString))
            {
                string updateQuery = "UPDATE Inventory SET status = $newStatus WHERE uid = $uid ";
            }
        }

        public void updateLogEntryReturnTime(int logID, string returnTime)
        {
            using(var connection = new SqliteConnection(connectionString)) 
            {
                string updateQuery = "UPDATE Logs SET returnTime = $returnTime WHERE logID = $logID";
            }
        }

        // SELECT
        public List<InventoryItem> GetAllInventoryItems()
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
                                uid = reader.GetString(0),
                                name = reader.GetString(1),
                                status = reader.GetString(2)
                            };
                            inventoryItems.Add(item);
                        }
                    }
                }
            }
            return inventoryItems;
        }
    }
}
