namespace GuiDownloader
{
	partial class MainForm
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
			this.button1 = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.progressBar2 = new System.Windows.Forms.ProgressBar();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.progressBar3 = new System.Windows.Forms.ProgressBar();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.progressBar4 = new System.Windows.Forms.ProgressBar();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.progressBar5 = new System.Windows.Forms.ProgressBar();
			this.textBox6 = new System.Windows.Forms.TextBox();
			this.progressBar6 = new System.Windows.Forms.ProgressBar();
			this.button6 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(239, 15);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(50, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = ">>>";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.DownloadSingleFile);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(298, 15);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(292, 23);
			this.progressBar1.TabIndex = 1;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(12, 15);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(221, 20);
			this.textBox1.TabIndex = 2;
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(12, 44);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(221, 20);
			this.textBox2.TabIndex = 5;
			// 
			// progressBar2
			// 
			this.progressBar2.Location = new System.Drawing.Point(298, 44);
			this.progressBar2.Name = "progressBar2";
			this.progressBar2.Size = new System.Drawing.Size(292, 23);
			this.progressBar2.TabIndex = 4;
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(12, 73);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(221, 20);
			this.textBox3.TabIndex = 8;
			// 
			// progressBar3
			// 
			this.progressBar3.Location = new System.Drawing.Point(298, 73);
			this.progressBar3.Name = "progressBar3";
			this.progressBar3.Size = new System.Drawing.Size(292, 23);
			this.progressBar3.TabIndex = 7;
			// 
			// textBox4
			// 
			this.textBox4.Location = new System.Drawing.Point(12, 102);
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(221, 20);
			this.textBox4.TabIndex = 11;
			// 
			// progressBar4
			// 
			this.progressBar4.Location = new System.Drawing.Point(298, 102);
			this.progressBar4.Name = "progressBar4";
			this.progressBar4.Size = new System.Drawing.Size(292, 23);
			this.progressBar4.TabIndex = 10;
			// 
			// textBox5
			// 
			this.textBox5.Location = new System.Drawing.Point(12, 131);
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new System.Drawing.Size(221, 20);
			this.textBox5.TabIndex = 14;
			// 
			// progressBar5
			// 
			this.progressBar5.Location = new System.Drawing.Point(298, 131);
			this.progressBar5.Name = "progressBar5";
			this.progressBar5.Size = new System.Drawing.Size(292, 23);
			this.progressBar5.TabIndex = 13;
			// 
			// textBox6
			// 
			this.textBox6.Location = new System.Drawing.Point(12, 160);
			this.textBox6.Name = "textBox6";
			this.textBox6.Size = new System.Drawing.Size(221, 20);
			this.textBox6.TabIndex = 17;
			// 
			// progressBar6
			// 
			this.progressBar6.Location = new System.Drawing.Point(298, 160);
			this.progressBar6.Name = "progressBar6";
			this.progressBar6.Size = new System.Drawing.Size(292, 23);
			this.progressBar6.TabIndex = 16;
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(472, 192);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(118, 36);
			this.button6.TabIndex = 15;
			this.button6.Text = "Download all";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.DownloadAllFiles);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(600, 236);
			this.Controls.Add(this.textBox6);
			this.Controls.Add(this.progressBar6);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.textBox5);
			this.Controls.Add(this.progressBar5);
			this.Controls.Add(this.textBox4);
			this.Controls.Add(this.progressBar4);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.progressBar3);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.progressBar2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "MainForm";
			this.Text = "File downloader";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.ProgressBar progressBar2;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.ProgressBar progressBar3;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.ProgressBar progressBar4;
		private System.Windows.Forms.TextBox textBox5;
		private System.Windows.Forms.ProgressBar progressBar5;
		private System.Windows.Forms.TextBox textBox6;
		private System.Windows.Forms.ProgressBar progressBar6;
		private System.Windows.Forms.Button button6;
	}
}

