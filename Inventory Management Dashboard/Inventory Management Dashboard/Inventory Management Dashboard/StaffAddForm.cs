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
    public partial class StaffAddForm : Form
    {
        public StaffAddForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var db = new DatabaseManager();
            db.InsertStaffMember(textBox1.Text, textBox2.Text);
            MessageBox.Show("Staff member added successfully!");
        }
    }
}
