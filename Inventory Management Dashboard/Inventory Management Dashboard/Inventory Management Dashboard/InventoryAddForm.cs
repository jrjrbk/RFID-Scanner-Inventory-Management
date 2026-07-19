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
    public partial class InventoryAddForm : Form
    {
        public InventoryAddForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var db = new DatabaseManager();
            db.InsertInventoryItem(textBox1.Text, textBox2.Text,"Available");
            MessageBox.Show("Inventory items added successfully!");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
