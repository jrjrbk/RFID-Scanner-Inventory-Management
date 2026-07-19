using Inventory_Management_Dashboard.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Inventory_Management_Dashboard
{
    public partial class InventoryList : Form
    {
        public InventoryList()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            var db = new DatabaseManager();
            var inventoryItems = db.selectAllInventoryItems();
            dataGridView1.DataSource = inventoryItems;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InventoryAddForm newWindow = new InventoryAddForm();
            newWindow.ShowDialog();

            var db = new DatabaseManager();
            var inventoryItems = db.selectAllInventoryItems();
            dataGridView1.DataSource = inventoryItems;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0) 
            { 
                int selectedID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                var confirmResult = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo);
                if(confirmResult == DialogResult.Yes)
                {
                    var db = new DatabaseManager();
                    db.RemoveInventoryItem(selectedID);
                    dataGridView1.DataSource = db.selectAllInventoryItems();
                }
            }
        }
    }
}
