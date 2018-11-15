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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.CurrentClass = new System.Windows.Forms.Button();
            this.selectedClass = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 41);
            this.label1.TabIndex = 0;
            this.label1.Text = "Home Screen";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(81, 114);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Log Out";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(81, 56);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(82, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Historic Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.HistoricData_Click);
            // 
            // CurrentClass
            // 
            this.CurrentClass.Location = new System.Drawing.Point(12, 85);
            this.CurrentClass.Name = "CurrentClass";
            this.CurrentClass.Size = new System.Drawing.Size(82, 23);
            this.CurrentClass.TabIndex = 4;
            this.CurrentClass.Text = "Current Class";
            this.CurrentClass.UseVisualStyleBackColor = true;
            this.CurrentClass.Click += new System.EventHandler(this.CurrentClass_Click);
            // 
            // selectedClass
            // 
            this.selectedClass.Location = new System.Drawing.Point(101, 86);
            this.selectedClass.Name = "selectedClass";
            this.selectedClass.Size = new System.Drawing.Size(100, 20);
            this.selectedClass.TabIndex = 5;
            // 
            // TeacherHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 203);
            this.Controls.Add(this.selectedClass);
            this.Controls.Add(this.CurrentClass);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Name = "TeacherHome";
            this.Text = "TeacherHome";
            this.Load += new System.EventHandler(this.TeacherHome_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button CurrentClass;
        private System.Windows.Forms.TextBox selectedClass;
    }
}