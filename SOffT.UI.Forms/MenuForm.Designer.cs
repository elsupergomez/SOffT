//
//  MenuForm.Designer.cs
//
//  Author:
//       Claudio Rodrigo Pereyra Diaz <claudiorodrigo@pereyradiaz.com.ar>
//       Hernan Vivani <hernan@vivani.com.ar> - http://hvivani.com.ar
//
//  Copyright (c) 2010 SOffT - http://www.sofft.com.ar
//  Copyright (c) 2015 Hamekoz - www.hamekoz.com.ar
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
			this.flpBotones.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpBotones.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flpBotones.Location = new System.Drawing.Point (0, 0);
			this.flpBotones.Margin = new System.Windows.Forms.Padding (10);
			this.flpBotones.MinimumSize = new System.Drawing.Size (0, 0);
			this.flpBotones.Name = "flpBotones";
			this.flpBotones.TabIndex = 12;
			//
			// frmMenu
			//
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add (this.flpBotones);
			this.ForeColor = System.Drawing.Color.Navy;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject ("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size (100, 0);
			this.Name = "MenuForm";
			this.Padding = new System.Windows.Forms.Padding (10);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MenuForm";
			this.ResumeLayout (false);
			this.PerformLayout ();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flpBotones;
	}
}