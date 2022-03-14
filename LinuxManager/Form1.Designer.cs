
namespace LinuxManager
{
	partial class Main
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
			this.button_connect = new System.Windows.Forms.Button();
			this.listBox_output = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// button_connect
			// 
			this.button_connect.Location = new System.Drawing.Point(12, 256);
			this.button_connect.Name = "button_connect";
			this.button_connect.Size = new System.Drawing.Size(239, 43);
			this.button_connect.TabIndex = 0;
			this.button_connect.Text = "Connect";
			this.button_connect.UseVisualStyleBackColor = true;
			this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
			// 
			// listBox_output
			// 
			this.listBox_output.FormattingEnabled = true;
			this.listBox_output.Location = new System.Drawing.Point(12, 12);
			this.listBox_output.Name = "listBox_output";
			this.listBox_output.Size = new System.Drawing.Size(239, 238);
			this.listBox_output.TabIndex = 1;
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(263, 311);
			this.Controls.Add(this.listBox_output);
			this.Controls.Add(this.button_connect);
			this.Name = "Main";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Connect";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button_connect;
		private System.Windows.Forms.ListBox listBox_output;
	}
}

