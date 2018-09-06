//
//  PrincipalForm.cs
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
        public PrincipalForm()
        {
            InitializeComponent();
            Icon = Controles.Icono;
            pictureBoxEmpresa.Image = Controles.Banner;
        }

        /// <summary>
        /// Lista de Botones para manejar los indices de los permisos.
        /// </summary>
        List<Button> botones = new List<Button>();

        /// <summary>
        /// Llama al login y verifica los datos ingresados.
        /// hvivani. 20061011.
        /// </summary>
        public void VerificaLogin()
        {
            var f = new LoginForm();
            f.ShowDialog();
            if (Usuario.Actual != null)
                Usuario.SetPermisos(ref botones, Nivel);
        }

        public void SetDatos(string modulo, string sistema)
        {
            SetDatos(0, modulo, sistema);
        }

        public void SetDatos(int id, string modulo, string sistema)
        {
            Usuario.ModuloId = id;
            Icon = Controles.Icono;
            lblVersion.Text = string.Format("versión: {0}", Application.ProductVersion);
            lblSistemaGestion.Text = sistema;
            lblModulo.Text = modulo;
        }

		/// <summary>
		/// Crea los botones en una lista a agregar en el frm.
		/// </summary>
		/// <param name="nombres">Lista de nombre para los botones a crear</param>
		public void CreaBotones(params string[] nombres)
		{
			foreach (string nombre in nombres)
			{
				AgregarBoton(nombre);
			}
		}

		/// <summary>
		/// Agrega un nuevo boton al final de la lista de botones
		/// </summary>
		/// <param name="nombre">Nombre del boton</param>
		public void AgregarBoton(string nombre)
		{
            var boton = new Button
            {
                Enabled = !Usuario.Requerir,
                TabIndex = botones.Count,
                Text = nombre,
                Width = fLPBotones.Width - fLPBotones.Margin.All
            };
            fLPBotones.Controls.Add(boton);
			boton.Click += botones_Click;
			botones.Add(boton);
		}

		/// <summary>
		/// Evento único para el panel de botones. Detecta la pulsacion de un boton
		/// hvivani. 20061011.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void botones_Click(object sender, EventArgs e)
		{
			var b = (Button)sender;
			Boton_Click(b.TabIndex);
		}

		/// <summary>
		/// Metodo para definir la accion de cada boton pulsado
		/// </summary>
		/// <param name="indice">Indice del boton pulsado</param>
		public virtual void Boton_Click(int indice)
		{
		}

		void FormPrincipal_Load(object sender, EventArgs e)
		{
			//si se esta validando el usuario y no se obtuvo uno valido. cierro.
			if (Usuario.Requerir && Usuario.Actual == null || Nivel == null)
				Close();
		}

		#region Miembros de IPermisible

		public string Nivel
		{
			get { return Usuario.ModuloId.ToString(); }
		}

		#endregion
	}
}
