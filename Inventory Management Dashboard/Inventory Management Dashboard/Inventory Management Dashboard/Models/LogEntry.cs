using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Dashboard.Models
{
    internal class LogEntry
    {
        public int logID { get; set; }
        public int staffID { get; set; }
        public int inventoryID { get; set; }
        public string borrowedTime { get; set; }
        public string returnedTime { get; set; }
    }
}
