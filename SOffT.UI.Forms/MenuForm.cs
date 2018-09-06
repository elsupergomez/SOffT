//
//  MenuForm.cs
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
	public partial class MenuForm : Form
	{
		public MenuForm()
		{
            InitializeComponent();
            Icon = Controles.Icono;
        }

		/// <summary>
		/// Lista de Botones para manejar los indices de los permisos.
		/// </summary>
		List<Button> botones = new List<Button>();

		/// <summary>
		/// Agrega un nuevo boton al final de la lista de botones
		/// </summary>
		/// <param name="nombre">Nombre del boton</param>
		public void AgregarBoton(string nombre)
		{
            var boton = new Button
            {
                AutoSize = true,
                Enabled = !Usuario.Requerir,
                TabIndex = botones.Count,
                Text = nombre,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            flpBotones.Controls.Add(boton);
			boton.Click += botones_Click;
			botones.Add(boton);
		}

		/// <summary>
		/// Evento único para el panel de botones. Detecta la pulsacion de un boton
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
			MessageBox.Show("Es necesario redefinir este metodo en la nueva instancia");
		}

		/// <summary>
		/// el subIndice hace referencia al numero de boton (indiceboton + 1) que se está usando para acceder a
		/// este formulario.
		/// </summary>
		/// <param name="indice"></param>
		public void AplicarPermisos(int indice)
		{
			Usuario.SetPermisosSubIndices(ref botones, indice);
		}

		#region Miembros de IPermisible

		string nivel;

		/// <summary>
		/// Obtinene el nivel de profundidad del formulario. Recorrido de indices desde el fomulario principal hasta el formulario actual.
		/// </summary>
		public string Nivel
		{
			get { return nivel; }
		}

		/// <summary>
		/// Abre el formulario y aplica los permisos correspondientes al nivel e indice.
		/// Es necesario invocar este metodo desde las redefiniciones abrir en los formularios heredados.
		/// La llamda en la redefinicion debe estar al final del metodo
		/// </summary>
		/// <param name="nivel">Nivel del fomulario invocador</param>
		/// <param name="indice">Indice del boton invocador</param>
		public virtual void Abrir(string nivel, int indice)
		{
			this.nivel = string.Concat(nivel, ".", indice.ToString());
			Usuario.SetPermisos(ref botones, Nivel);
			ShowDialog();
		}

		#endregion
	}
}