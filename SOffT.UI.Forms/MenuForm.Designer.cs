namespace Sofft.UI.Forms
{
	partial class MenuForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose ();
			}
			base.Dispose (disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof(MenuForm));
			this.flpBotones = new System.Windows.Forms.FlowLayoutPanel ();
			this.SuspendLayout ();
			//
			// flpBotones
			//
			this.flpBotones.AutoSize = true;
			this.flpBotones.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpBotones.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flpBotones.Location = new System.Drawing.Point (0, 0);
			this.flpBotones.Margin = new System.Windows.Forms.Padding (10);
			this.flpBotones.MinimumSize = new System.Drawing.Size (0, 50);
			this.flpBotones.Name = "flpBotones";
			this.flpBotones.Padding = new System.Windows.Forms.Padding (10);
			this.flpBotones.Size = new System.Drawing.Size (232, 113);
			this.flpBotones.TabIndex = 12;
			//
			// frmMenu
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size (232, 113);
			this.Controls.Add (this.flpBotones);
			this.ForeColor = System.Drawing.Color.Navy;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject ("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size (238, 141);
			this.Name = "frmMenu";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "frmMenu";
			this.ResumeLayout (false);
			this.PerformLayout ();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flpBotones;
	}
}