using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Inventory_Management_Dashboard.Data;

namespace Inventory_Management_Dashboard
{
    public partial class StaffList : Form
    {
        public StaffList()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            var db = new DatabaseManager();
            var StaffMembers = db.selectAllStaffMembers();
            dataGridView1.DataSource = StaffMembers;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StaffAddForm newWindow = new StaffAddForm();
            newWindow.ShowDialog();

            var db = new DatabaseManager();
            var StaffMembers = db.selectAllStaffMembers();
            dataGridView1.DataSource = StaffMembers;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Check if a user is selected in the DataGridView
            if (dataGridView1.SelectedRows.Count > 0) 
            {
                int selectedID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

                var confirmResult = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo);

                if(confirmResult == DialogResult.Yes)
                {
                    var db = new DatabaseManager();
                    db.RemoveStaffMember(selectedID);
                    dataGridView1.DataSource = db.selectAllStaffMembers();
                }
            }
        }
    }
}
