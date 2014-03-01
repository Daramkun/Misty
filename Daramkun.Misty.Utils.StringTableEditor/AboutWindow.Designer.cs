namespace Daramkun.Misty.Utils.StringTableEditor
{
	partial class AboutWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose ( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose ();
			}
			base.Dispose ( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.labelToolVersion = new System.Windows.Forms.Label();
			this.labelBlockarVersion = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.labelDotNet = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(82, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(79, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "Tool version:";
			// 
			// labelToolVersion
			// 
			this.labelToolVersion.AutoSize = true;
			this.labelToolVersion.Location = new System.Drawing.Point(167, 26);
			this.labelToolVersion.Name = "labelToolVersion";
			this.labelToolVersion.Size = new System.Drawing.Size(38, 12);
			this.labelToolVersion.TabIndex = 1;
			this.labelToolVersion.Text = "label2";
			// 
			// labelBlockarVersion
			// 
			this.labelBlockarVersion.AutoSize = true;
			this.labelBlockarVersion.Location = new System.Drawing.Point(167, 53);
			this.labelBlockarVersion.Name = "labelBlockarVersion";
			this.labelBlockarVersion.Size = new System.Drawing.Size(38, 12);
			this.labelBlockarVersion.TabIndex = 3;
			this.labelBlockarVersion.Text = "label2";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(65, 53);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 12);
			this.label3.TabIndex = 2;
			this.label3.Text = "Blockar version:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(49, 106);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(223, 12);
			this.label2.TabIndex = 4;
			this.label2.Text = "Powered by .NET Framework, Blockar";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(60, 132);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(201, 12);
			this.label4.TabIndex = 5;
			this.label4.Text = "Copyright (C) 2013-2014 Daramkun";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(125, 166);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 6;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// labelDotNet
			// 
			this.labelDotNet.AutoSize = true;
			this.labelDotNet.Location = new System.Drawing.Point(167, 80);
			this.labelDotNet.Name = "labelDotNet";
			this.labelDotNet.Size = new System.Drawing.Size(38, 12);
			this.labelDotNet.TabIndex = 8;
			this.labelDotNet.Text = "label2";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(78, 80);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(83, 12);
			this.label6.TabIndex = 7;
			this.label6.Text = ".NET version:";
			// 
			// AboutWindow
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(324, 201);
			this.Controls.Add(this.labelDotNet);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.labelBlockarVersion);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.labelToolVersion);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "AboutWindow";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Misty Framework String Table Editor";
			this.Load += new System.EventHandler(this.AboutWindow_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelToolVersion;
		private System.Windows.Forms.Label labelBlockarVersion;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label labelDotNet;
		private System.Windows.Forms.Label label6;
	}
}