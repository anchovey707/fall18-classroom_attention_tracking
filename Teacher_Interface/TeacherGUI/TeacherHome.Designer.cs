namespace TeacherGUI
{
    partial class TeacherHome
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
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.CurrentClass = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.selectedClass = new System.Windows.Forms.ComboBox();
            this.adminbtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(60, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 41);
            this.label1.TabIndex = 0;
            this.label1.Text = "Home Screen";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(177, 125);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(82, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Historic Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.HistoricData_Click);
            // 
            // CurrentClass
            // 
            this.CurrentClass.Location = new System.Drawing.Point(89, 125);
            this.CurrentClass.Name = "CurrentClass";
            this.CurrentClass.Size = new System.Drawing.Size(82, 23);
            this.CurrentClass.TabIndex = 2;
            this.CurrentClass.Text = "Stream Class";
            this.CurrentClass.UseVisualStyleBackColor = true;
            this.CurrentClass.Click += new System.EventHandler(this.CurrentClass_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(103, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Class:";
            // 
            // selectedClass
            // 
            this.selectedClass.FormattingEnabled = true;
            this.selectedClass.Location = new System.Drawing.Point(144, 86);
            this.selectedClass.Name = "selectedClass";
            this.selectedClass.Size = new System.Drawing.Size(121, 21);
            this.selectedClass.TabIndex = 1;
            // 
            // adminbtn
            // 
            this.adminbtn.Location = new System.Drawing.Point(131, 163);
            this.adminbtn.Name = "adminbtn";
            this.adminbtn.Size = new System.Drawing.Size(75, 23);
            this.adminbtn.TabIndex = 5;
            this.adminbtn.Text = "Admin";
            this.adminbtn.UseVisualStyleBackColor = true;
            this.adminbtn.Click += new System.EventHandler(this.adminbtn_Click);
            // 
            // TeacherHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 214);
            this.Controls.Add(this.adminbtn);
            this.Controls.Add(this.selectedClass);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CurrentClass);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Name = "TeacherHome";
            this.Text = "TeacherHome";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TeacherHome_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button CurrentClass;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox selectedClass;
        private System.Windows.Forms.Button adminbtn;
    }
}