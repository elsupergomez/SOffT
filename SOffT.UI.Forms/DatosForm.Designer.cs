﻿//
//  DatosForm.Designer.cs
//
//  Author:
//       Hernan Vivani <hernan@vivani.com.ar> - http://hvivani.com.ar
//       Claudio Rodrigo Pereyra Diaz <claudiorodrigo@pereyradiaz.com.ar>
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
	partial class DatosForm
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
			this.aceptarButton = new System.Windows.Forms.Button ();
			this.cancelarButton = new System.Windows.Forms.Button ();
			this.SuspendLayout ();
			//
			// aceptarButton
			//
			this.aceptarButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.aceptarButton.Image = global::Sofft.UI.Forms.Properties.Resources.CheckMark32x322;
			this.aceptarButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.aceptarButton.Location = new System.Drawing.Point (101, 238);
			this.aceptarButton.Name = "aceptarButton";
			this.aceptarButton.Size = new System.Drawing.Size (103, 37);
			this.aceptarButton.TabIndex = 4;
			this.aceptarButton.Text = "&Aceptar";
			this.aceptarButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.aceptarButton.UseVisualStyleBackColor = true;
			this.aceptarButton.Click += new System.EventHandler (this.aceptarButton_Click);
			//
			// cancelarButton
			//
			this.cancelarButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.cancelarButton.Image = global::Sofft.UI.Forms.Properties.Resources.Cancel;
			this.cancelarButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.cancelarButton.Location = new System.Drawing.Point (210, 238);
			this.cancelarButton.Name = "cancelarButton";
			this.cancelarButton.Size = new System.Drawing.Size (103, 37);
			this.cancelarButton.TabIndex = 3;
			this.cancelarButton.Text = "&Cancelar";
			this.cancelarButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cancelarButton.UseVisualStyleBackColor = true;
			this.cancelarButton.Click += new System.EventHandler (this.cancelarButton_Click);
			//
			// FormDatos
			//
			this.ClientSize = new System.Drawing.Size (410, 298);
			this.Controls.Add (this.aceptarButton);
			this.Controls.Add (this.cancelarButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormDatos";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Datos";
			this.ResumeLayout (false);

		}

		#endregion

		protected System.Windows.Forms.Button aceptarButton;
		protected System.Windows.Forms.Button cancelarButton;
	}
}