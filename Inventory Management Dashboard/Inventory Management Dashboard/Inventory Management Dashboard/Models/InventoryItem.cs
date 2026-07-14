using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Dashboard.Models
{
    internal class InventoryItem
    {
        public int inventoryID { get; set; }
        public string uid { get; set; }
        public string name { get; set; }
        public string status { get; set; }
    }
}
