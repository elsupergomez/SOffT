using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Sofft.Utils;

namespace Sofft.UI.Forms
{
	public partial class MenuForm : Form
	{
		public MenuForm ()
		{
			InitializeComponent ();
			Icon = Modulo.cargaIcono ();
		}

		/// <summary>
		/// Lista de Botones para manejar los indices de los permisos.
		/// </summary>
		List<Button> botones = new List<Button> ();

		/// <summary>
		/// Crea los botones en una lista a agregar en el frm.
		/// </summary>
		/// <param name="nombres">Lista de nombre para los botones a crear</param>
		[Obsolete ("Usar metodo AgregarBoton()")]
		public void CreaBotones (params string[] nombres)
		{
			foreach (string nombre in nombres) {
				AgregarBoton (nombre);
			}
		}

		/// <summary>
		/// Agrega un nuevo boton al final de la lista de botones
		/// </summary>
		/// <param name="nombre">Nombre del boton</param>
		public void AgregarBoton (string nombre)
		{
			var boton = new Button ();
			boton.Enabled = !Modulo.ValidaLogin;
			boton.TabIndex = botones.Count;
			boton.Text = nombre;
			boton.Width = flpBotones.Width - flpBotones.Padding.Left - flpBotones.Padding.Right - boton.Margin.Horizontal;
			flpBotones.Controls.Add (boton);
			boton.Click += botones_Click;
			boton.AutoSize = true;
			botones.Add (boton);
		}

		/// <summary>
		/// Evento único para el panel de botones. Detecta la pulsacion de un boton
		/// hvivani. 20061011.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void botones_Click (object sender, EventArgs e)
		{
			var b = (Button)sender;
			boton_Click (b.TabIndex);
		}


		/// <summary>
		/// Metodo para definir la accion de cada boton pulsado
		/// </summary>
		/// <param name="indice">Indice del boton pulsado</param>
		public virtual void boton_Click (int indice)
		{
			MessageBox.Show ("Es necesario redefinir este metodo en la nueva instancia");
		}

		/// <summary>
		/// el subIndice hace referencia al numero de boton (indiceboton + 1) que se está usando para acceder a
		/// este formulario.
		/// </summary>
		/// <param name="indice"></param>
		public void aplicaPermisos (int indice)
		{
			Usuario.SetPermisosSubIndices (ref botones, indice);
		}

		#region Miembros de IPermisible

		string nivel;

		/// <summary>
		/// Obtinene el nivel de profundidad del formulario. Recorrido de indices desde el fomulario principal hasta el formulario actual.
		/// </summary>
		public string Nivel {
			get { return nivel; }
		}

		/// <summary>
		/// Abre el formulario y aplica los permisos correspondientes al nivel e indice.
		/// Es necesario invocar este metodo desde las redefiniciones abrir en los formularios heredados.
		/// La llamda en la redefinicion debe estar al final del metodo
		/// </summary>
		/// <param name="nivel">Nivel del fomulario invocador</param>
		/// <param name="indice">Indice del boton invocador</param>
		public virtual void abrir (string nivel, int indice)
		{
			this.nivel = string.Concat (nivel, ".", indice.ToString ());
			Usuario.SetPermisos (ref botones, Nivel);
			ShowDialog ();
		}

		#endregion
	}
}