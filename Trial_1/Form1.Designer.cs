﻿namespace Trial_1
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
        public void InitializeComponent()
        {
            this.RDBtn = new System.Windows.Forms.Button();
            this.Display1 = new System.Windows.Forms.DataGridView();
            this.Opts = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ExcBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Display1)).BeginInit();
            this.SuspendLayout();
            // 
            // RDBtn
            // 
            this.RDBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.ExcBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExcBtn.Location = new System.Drawing.Point(12, 100);
            this.ExcBtn.Name = "ExcBtn";
            this.ExcBtn.Size = new System.Drawing.Size(120, 53);
            this.ExcBtn.TabIndex = 11;
            this.ExcBtn.Text = "Generate PDF";
            this.ExcBtn.UseVisualStyleBackColor = true;
            this.ExcBtn.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ExcBtn_MouseClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Sylfaen", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(423, 317);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(206, 25);
            this.label2.TabIndex = 13;
            this.label2.Text = "Generate PDF Progress :";
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(623, 317);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 24);
            this.label3.TabIndex = 15;
            this.label3.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Sylfaen", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(133, 317);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(169, 25);
            this.label4.TabIndex = 16;
            this.label4.Text = "Loading Customer :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Sylfaen", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(295, 317);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 25);
            this.label5.TabIndex = 17;
            this.label5.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 359);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ExcBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Opts);
            this.Controls.Add(this.Display1);
            this.Controls.Add(this.RDBtn);
            this.ForeColor = System.Drawing.Color.Black;
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

