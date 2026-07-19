using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Dashboard.Models
{
    internal class FormLogEntries
    {
        public int logID { get; set; }
        public string staffName { get; set; }
        public string inventoryName { get; set; }
        public string borrowedTime { get; set; }
        public string returnedTime { get; set; }
    }
}
