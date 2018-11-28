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
            this.CRNTxtBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SubmitClass = new System.Windows.Forms.Button();
            this.SubmitProfessor = new System.Windows.Forms.Button();
            this.TeacherListBox = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lastName = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.EndTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartTimePicker = new System.Windows.Forms.DateTimePicker();
            this.CourseNameTextBox = new System.Windows.Forms.TextBox();
            this.LoginTxtBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // firstName
            // 
            this.firstName.Location = new System.Drawing.Point(12, 35);
            this.firstName.Name = "firstName";
            this.firstName.Size = new System.Drawing.Size(179, 20);
            this.firstName.TabIndex = 0;
            // 
            // CRNTxtBox
            // 
            this.CRNTxtBox.Location = new System.Drawing.Point(392, 108);
            this.CRNTxtBox.Name = "CRNTxtBox";
            this.CRNTxtBox.Size = new System.Drawing.Size(264, 20);
            this.CRNTxtBox.TabIndex = 4;
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
            this.label2.Location = new System.Drawing.Point(388, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Add Class";
            // 
            // SubmitClass
            // 
            this.SubmitClass.Location = new System.Drawing.Point(662, 106);
            this.SubmitClass.Name = "SubmitClass";
            this.SubmitClass.Size = new System.Drawing.Size(143, 23);
            this.SubmitClass.TabIndex = 6;
            this.SubmitClass.Text = "Submit";
            this.SubmitClass.UseVisualStyleBackColor = true;
            this.SubmitClass.Click += new System.EventHandler(this.SubmitClass_Click);
            // 
            // SubmitProfessor
            // 
            this.SubmitProfessor.Location = new System.Drawing.Point(197, 106);
            this.SubmitProfessor.Name = "SubmitProfessor";
            this.SubmitProfessor.Size = new System.Drawing.Size(143, 23);
            this.SubmitProfessor.TabIndex = 7;
            this.SubmitProfessor.Text = "Submit";
            this.SubmitProfessor.UseVisualStyleBackColor = true;
            this.SubmitProfessor.Click += new System.EventHandler(this.SubmitProfessor_Click);
            // 
            // TeacherListBox
            // 
            this.TeacherListBox.FormattingEnabled = true;
            this.TeacherListBox.Items.AddRange(new object[] {
            "De",
            "Allan",
            "Zhang",
            "Master Chief"});
            this.TeacherListBox.Location = new System.Drawing.Point(392, 32);
            this.TeacherListBox.Name = "TeacherListBox";
            this.TeacherListBox.Size = new System.Drawing.Size(264, 21);
            this.TeacherListBox.TabIndex = 3;
            this.TeacherListBox.Text = "Professor";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(392, 137);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(519, 280);
            this.dataGridView1.TabIndex = 11;
            // 
            // lastName
            // 
            this.lastName.Location = new System.Drawing.Point(12, 72);
            this.lastName.Name = "lastName";
            this.lastName.Size = new System.Drawing.Size(179, 20);
            this.lastName.TabIndex = 1;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(12, 108);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(179, 20);
            this.password.TabIndex = 2;
            this.password.UseSystemPasswordChar = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(12, 137);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(374, 280);
            this.dataGridView2.TabIndex = 16;
            // 
            // EndTimePicker
            // 
            this.EndTimePicker.Location = new System.Drawing.Point(662, 69);
            this.EndTimePicker.Name = "EndTimePicker";
            this.EndTimePicker.Size = new System.Drawing.Size(143, 20);
            this.EndTimePicker.TabIndex = 17;
            // 
            // StartTimePicker
            // 
            this.StartTimePicker.Location = new System.Drawing.Point(662, 32);
            this.StartTimePicker.Name = "StartTimePicker";
            this.StartTimePicker.Size = new System.Drawing.Size(143, 20);
            this.StartTimePicker.TabIndex = 18;
            // 
            // CourseNameTextBox
            // 
            this.CourseNameTextBox.Location = new System.Drawing.Point(392, 72);
            this.CourseNameTextBox.Name = "CourseNameTextBox";
            this.CourseNameTextBox.Size = new System.Drawing.Size(264, 20);
            this.CourseNameTextBox.TabIndex = 19;
            // 
            // LoginTxtBox
            // 
            this.LoginTxtBox.Location = new System.Drawing.Point(197, 35);
            this.LoginTxtBox.Name = "LoginTxtBox";
            this.LoginTxtBox.Size = new System.Drawing.Size(143, 20);
            this.LoginTxtBox.TabIndex = 20;
            // 
            // AdminScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 567);
            this.Controls.Add(this.LoginTxtBox);
            this.Controls.Add(this.CourseNameTextBox);
            this.Controls.Add(this.StartTimePicker);
            this.Controls.Add(this.EndTimePicker);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.password);
            this.Controls.Add(this.lastName);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.TeacherListBox);
            this.Controls.Add(this.SubmitProfessor);
            this.Controls.Add(this.SubmitClass);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CRNTxtBox);
            this.Controls.Add(this.firstName);
            this.Name = "AdminScreen";
            this.Text = "AdminScreen";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AdminScreen_FormClosing);
            this.Load += new System.EventHandler(this.AdminScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox firstName;
        private System.Windows.Forms.TextBox CRNTxtBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button SubmitClass;
        private System.Windows.Forms.Button SubmitProfessor;
        private System.Windows.Forms.ComboBox TeacherListBox;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox lastName;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DateTimePicker EndTimePicker;
        private System.Windows.Forms.DateTimePicker StartTimePicker;
        private System.Windows.Forms.TextBox CourseNameTextBox;
        private System.Windows.Forms.TextBox LoginTxtBox;
    }
}