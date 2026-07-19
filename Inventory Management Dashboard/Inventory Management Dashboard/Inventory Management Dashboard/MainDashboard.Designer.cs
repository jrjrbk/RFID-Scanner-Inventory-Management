namespace Inventory_Management_Dashboard
{
    partial class MainDashboard
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            dataGridView1 = new DataGridView();
            checkInventoryButton = new Button();
            comboBox1 = new ComboBox();
            portsUsed = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            button1.Location = new Point(28, 21);
            button1.Name = "button1";
            button1.Size = new Size(140, 33);
            button1.TabIndex = 0;
            button1.Text = "Check Staff";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(28, 79);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(776, 384);
            dataGridView1.TabIndex = 1;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // checkInventoryButton
            // 
            checkInventoryButton.Location = new Point(664, 21);
            checkInventoryButton.Name = "checkInventoryButton";
            checkInventoryButton.Size = new Size(140, 33);
            checkInventoryButton.TabIndex = 2;
            checkInventoryButton.Text = "Check Inventory";
            checkInventoryButton.UseVisualStyleBackColor = true;
            checkInventoryButton.Click += button2_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(349, 31);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 3;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // portsUsed
            // 
            portsUsed.AutoSize = true;
            portsUsed.Location = new Point(380, 13);
            portsUsed.Name = "portsUsed";
            portsUsed.Size = new Size(62, 15);
            portsUsed.TabIndex = 4;
            portsUsed.Text = "Using Port";
            portsUsed.Click += label1_Click;
            // 
            // MainDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(829, 586);
            Controls.Add(portsUsed);
            Controls.Add(comboBox1);
            Controls.Add(checkInventoryButton);
            Controls.Add(dataGridView1);
            Controls.Add(button1);
            Name = "MainDashboard";
            Text = "RFID App";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private DataGridView dataGridView1;
        private Button checkInventoryButton;
        private ComboBox comboBox1;
        private Label portsUsed;
    }
}
