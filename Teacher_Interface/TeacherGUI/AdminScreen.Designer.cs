namespace TeacherGUI
{
    partial class AdminScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.firstName = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SubmitClass = new System.Windows.Forms.Button();
            this.SubmitProfessor = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lastName = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // firstName
            // 
            this.firstName.Location = new System.Drawing.Point(12, 59);
            this.firstName.Name = "firstName";
            this.firstName.Size = new System.Drawing.Size(260, 20);
            this.firstName.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(645, 59);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(264, 20);
            this.textBox1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Add Professor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(641, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Add Class";
            // 
            // SubmitClass
            // 
            this.SubmitClass.Location = new System.Drawing.Point(944, 108);
            this.SubmitClass.Name = "SubmitClass";
            this.SubmitClass.Size = new System.Drawing.Size(79, 23);
            this.SubmitClass.TabIndex = 6;
            this.SubmitClass.Text = "Submit";
            this.SubmitClass.UseVisualStyleBackColor = true;
            this.SubmitClass.Click += new System.EventHandler(this.SubmitClass_Click);
            // 
            // SubmitProfessor
            // 
            this.SubmitProfessor.Location = new System.Drawing.Point(311, 111);
            this.SubmitProfessor.Name = "SubmitProfessor";
            this.SubmitProfessor.Size = new System.Drawing.Size(75, 23);
            this.SubmitProfessor.TabIndex = 7;
            this.SubmitProfessor.Text = "Submit";
            this.SubmitProfessor.UseVisualStyleBackColor = true;
            this.SubmitProfessor.Click += new System.EventHandler(this.SubmitProfessor_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "De",
            "Allan",
            "Zhang",
            "Master Chief"});
            this.comboBox1.Location = new System.Drawing.Point(645, 32);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(264, 21);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.Text = "Professor";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(645, 137);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(622, 280);
            this.dataGridView1.TabIndex = 11;
            // 
            // lastName
            // 
            this.lastName.Location = new System.Drawing.Point(12, 85);
            this.lastName.Name = "lastName";
            this.lastName.Size = new System.Drawing.Size(260, 20);
            this.lastName.TabIndex = 1;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(12, 111);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(260, 20);
            this.password.TabIndex = 2;
            this.password.UseSystemPasswordChar = true;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "De",
            "Allan",
            "Zhang",
            "Master Chief"});
            this.comboBox2.Location = new System.Drawing.Point(645, 110);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(264, 21);
            this.comboBox2.TabIndex = 6;
            this.comboBox2.Text = "Time";
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "De",
            "Allan",
            "Zhang",
            "Master Chief"});
            this.comboBox3.Location = new System.Drawing.Point(645, 85);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.comboBox3.Size = new System.Drawing.Size(264, 21);
            this.comboBox3.TabIndex = 5;
            this.comboBox3.Text = "Day";
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(12, 137);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(627, 280);
            this.dataGridView2.TabIndex = 16;
            // 
            // AdminScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1279, 567);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.password);
            this.Controls.Add(this.lastName);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.SubmitProfessor);
            this.Controls.Add(this.SubmitClass);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.firstName);
            this.Name = "AdminScreen";
            this.Text = "AdminScreen";
            this.Load += new System.EventHandler(this.AdminScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox firstName;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button SubmitClass;
        private System.Windows.Forms.Button SubmitProfessor;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox lastName;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.DataGridView dataGridView2;
    }
}