namespace Trial_1
{
    partial class Form1
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
            this.RDBtn = new System.Windows.Forms.Button();
            this.Display1 = new System.Windows.Forms.DataGridView();
            this.Opts = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ExcBtn = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Display1)).BeginInit();
            this.SuspendLayout();
            // 
            // RDBtn
            // 
            this.RDBtn.Font = new System.Drawing.Font("Modern No. 20", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RDBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RDBtn.Location = new System.Drawing.Point(12, 42);
            this.RDBtn.Name = "RDBtn";
            this.RDBtn.Size = new System.Drawing.Size(120, 52);
            this.RDBtn.TabIndex = 7;
            this.RDBtn.Text = "Import Raw Data Files (*.txt)";
            this.RDBtn.UseVisualStyleBackColor = true;
            this.RDBtn.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RDBtn_MouseClick);
            // 
            // Display1
            // 
            this.Display1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Display1.Location = new System.Drawing.Point(138, 42);
            this.Display1.Name = "Display1";
            this.Display1.Size = new System.Drawing.Size(650, 264);
            this.Display1.TabIndex = 8;
            // 
            // Opts
            // 
            this.Opts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Opts.FormattingEnabled = true;
            this.Opts.Location = new System.Drawing.Point(417, 12);
            this.Opts.Name = "Opts";
            this.Opts.Size = new System.Drawing.Size(121, 21);
            this.Opts.TabIndex = 9;
            this.Opts.SelectedIndexChanged += new System.EventHandler(this.Opts_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(376, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Client";
            // 
            // ExcBtn
            // 
            this.ExcBtn.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExcBtn.Location = new System.Drawing.Point(12, 100);
            this.ExcBtn.Name = "ExcBtn";
            this.ExcBtn.Size = new System.Drawing.Size(120, 53);
            this.ExcBtn.TabIndex = 11;
            this.ExcBtn.Text = "Generate PDF";
            this.ExcBtn.UseVisualStyleBackColor = true;
            this.ExcBtn.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ExcBtn_MouseClick);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(260, 313);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(528, 34);
            this.progressBar1.TabIndex = 12;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(135, 325);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Generate PDF Progress";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 160);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 46);
            this.button1.TabIndex = 14;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button1_MouseClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 359);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.ExcBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Opts);
            this.Controls.Add(this.Display1);
            this.Controls.Add(this.RDBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Display1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button RDBtn;
        private System.Windows.Forms.DataGridView Display1;
        private System.Windows.Forms.ComboBox Opts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ExcBtn;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}

