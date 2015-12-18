using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Sofft.Utils;

namespace Sofft.UI.Forms
{
	/// <summary>
	/// Formulario Principal de Modulos, Heredable, con metodos redefinibles.
	/// </summary>
	public partial class PrincipalForm : Form
	{
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public PrincipalForm ()
		{
			InitializeComponent ();
			pictureBoxEmpresa.Image = Controles.CargarImagen (Application.StartupPath + @"/imagenes/logoGrande.jpg");
		}

		/// <summary>
		/// Lista de Botones para manejar los indices de los permisos.
		/// </summary>
		List<Button> botones = new List<Button> ();

		/// <summary>
		/// Llama al login y verifica los datos ingresados.
		/// hvivani. 20061011.
		/// </summary>
		public void verificaLogin ()
		{
			var f = new LoginForm ();
			f.ShowDialog ();
			if (Modulo.Usuario != null) {
				//this.lblSistemaGestion.Text = Modulo.NombreSistema;
				//this.lblModulo.Text = Modulo.NombreModulo;
				//Usuario.setPermisosIndices(ref botones);
				//actualizarDatosDocumentoModulo Me
				//administrarPermisosIndices Me
				Usuario.SetPermisos (ref botones, Nivel);
			}
		}

		public void setDatos (string servidor, string DB, string nombreModulo, string nombreSistema, string version)
		{
			Modulo.ServidorDB = servidor;
			Modulo.DB = DB;
			Modulo.NombreModulo = nombreModulo;
			Modulo.NombreSistema = nombreSistema;
			Modulo.IdModulo = 2;
			Icon = Modulo.cargaIcono ();
			lblVersion.Text = version;
			lblSistemaGestion.Text = Modulo.NombreSistema;
			lblModulo.Text = Modulo.NombreModulo;
		}

		/// <summary>
		/// Crea los botones en una lista a agregar en el frm.
		/// </summary>
		/// <param name="nombres">Lista de nombre para los botones a crear</param>
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
			boton.Width = fLPBotones.Width - fLPBotones.Margin.All;
			fLPBotones.Controls.Add (boton);
			boton.Click += botones_Click;
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
		}

		void FormPrincipal_Load (object sender, EventArgs e)
		{
			//si se esta validando el usuario y no se obtuvo uno valido. cierro.
			if (Modulo.ValidaLogin && Modulo.Usuario == null || Nivel == null)
				Close ();
		}

		#region Miembros de IPermisible

		public string Nivel {
			get { return Modulo.IdModulo.ToString (); }
		}

		#endregion
	}
}