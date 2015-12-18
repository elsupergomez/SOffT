using System;
using System.Windows.Forms;

namespace Sofft.UI.Forms
{
	public partial class DatosForm : Form
	{
		public DatosForm ()
		{
			InitializeComponent ();
		}

		protected virtual void aceptarButton_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close ();
		}

		protected virtual void cancelarButton_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close ();
		}
	}
}
