//
//  ABMForm.cs
//
//  Author:
//       Claudio Rodrigo Pereyra Diaz <claudiorodrigo@pereyradiaz.com.ar>
//       Hernan Vivani <hernan@vivani.com.ar> - http://hvivani.com.ar
//
//  Copyright (c) 2015 Hamekoz - www.hamekoz.com.ar
//  Copyright (c) 2010 SOffT - http://www.sofft.com.ar
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
using System.Data;
using System.Windows.Forms;
using Sofft.Utils;

namespace Sofft.UI.Forms
{
	public partial class ABMForm : Form
	{
		/// <summary>
		/// Arreglo de botones del formulario Buscar
		/// </summary>
		List<Button> botones;

		/// <summary>
		/// Arreglo de Label para identificar el criterio de los filtros definidos filtros
		/// </summary>
		Label[] etiquetas;

		/// <summary>
		/// Arreglo de RadioButton utilizados para criterios de busqueda por texto
		/// </summary>
		RadioButton[] busquedas;

		/// <summary>
		/// Arreglo de ComboBox que se utilizan como filtro de Grilla
		/// </summary>
		ComboBox[] filtros;

		/// <summary>
		/// Enlace para aplicar filtros sobre la consulta
		/// </summary>
		internal BindingSource binding;

		public ABMForm()
		{
			InitializeComponent();
            Icon = Controles.Icono;
            txtBoxBuqueda.KeyDown += txtBoxBuqueda_KeyDown;
			txtBoxBuqueda.Focus();
		}

		void txtBoxBuqueda_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				btnBuscar_Click(sender, e);
		}

		/// <summary>
		/// carga la grilla con un dataset
		/// </summary>
		/// <param name="ds"></param>
		public void CargaGrilla(DataSet ds)
		{
			Controles.CargarDataGridView(dgvDatos, ds, false);
			Controles.DataGridViewEstandar(dgvDatos);
			//esta columna pertenece al id interno del objeto consultado.
			//nunca es visible.
			//Se utiliza solamente para las busquedas
			binding = new BindingSource();
			binding.DataSource = dgvDatos.DataSource;
		}

		public void OcultarColumna(int columna)
		{
			dgvDatos.Columns[columna].Visible = false;
		}

		/// <summary>
		/// Procedimiento que crear los botones de busqueda.
		/// Botones por Defecto Consultar, Nuevo, Modificar, Eliminar, Listado.
		/// Opcional 2 botones adicionales indicar por parametro el nombre de los mismo.
		/// </summary>
		/// <param name="nombres"></param> Opcional Nombre de los Botones adicionales.
		public void CrearBotones(params string[] nombres)
		{
			botones = new List<Button>();

			for (int i = 0; i < nombres.Length + 5; i++)
			{
				var boton = new Button();
				boton.Enabled = !Usuario.Requerir;
				boton.Size = btnCerrar.Size;
				boton.Tag = i;
				boton.Click += botones_Click;
				botones.Add(boton);
				flpBotones.Controls.Add(boton);
			}

			botones[0].Text = "Consultar";
			botones[1].Text = "Nuevo";
			botones[2].Text = "Modificar";
			botones[3].Text = "Eliminar";
			botones[4].Text = "Listado";

			for (int i = 5; i < nombres.Length + 5; i++)
			{
				botones[i].Text = nombres[i - 5];
			}
		}

		public DataTable Lista
		{
			get
			{
				DataTable datos = ((DataTable)dgvDatos.DataSource).Clone();
				DataRow[] rowColl = ((DataTable)dgvDatos.DataSource).Select(binding.Filter);
				foreach (DataRow row in rowColl)
				{
					datos.ImportRow(row);
				}
				return datos;
			}
		}

		/// <summary>
		/// Crea los RadioButton para seleccion de criterio de busqueda por texto.
		/// </summary>
		/// <remarks>Debe invocarse antes el procediminto CargaGrilla</remarks>
		/// <param name="nroColumna">nro de columna de la Tabla de datos que representa el criterio de busqueda</param>
		public void CrearBusquedas(params byte[] nroColumna)
		{
			busquedas = new RadioButton[nroColumna.Length];

			for (int i = 0; i < nroColumna.Length; i++)
			{
				busquedas[i] = new RadioButton();
				busquedas[i].Text = dgvDatos.Columns[nroColumna[i]].Name;
				busquedas[i].GotFocus += frmABM_GotFocus;
				busquedas[i].Width = 150;
				flpBusquedas.Controls.Add(busquedas[i]);
			}
			busquedas[0].Select();
			txtBoxBuqueda.Focus();
		}

		void frmABM_GotFocus(object sender, EventArgs e)
		{
			txtBoxBuqueda.Focus();
		}

		/// <summary>
		/// Crea los controles ComboBox utilizados como filtro de busqueda en la grilla.
		/// Debe invocarse antes el Procedimiento CargaGrilla
		/// </summary>
		/// <param name="DataCombos"></param> 0-8 ComboBox cargados con la informacion de filtro.
		public void CreaFiltros(params ComboBox[] DataCombos)
		{
			etiquetas = new Label[DataCombos.Length];
			filtros = new ComboBox[DataCombos.Length];

			for (int i = 0; i < DataCombos.Length; i++)
			{
				etiquetas[i] = new Label();
				filtros[i] = new ComboBox();

				etiquetas[i].Width = 175;
				etiquetas[i].Height = etiquetas[i].Height - 10;

				filtros[i] = DataCombos[i];
				filtros[i].Width = 175;
				filtros[i].DropDownStyle = ComboBoxStyle.DropDownList;

				flpFiltros.Controls.Add(etiquetas[i]);
				flpFiltros.Controls.Add(filtros[i]);
				filtros[i].SelectedIndex = -1;

				filtros[i].SelectedIndexChanged += AplicarFiltros;
			}
		}

		/// <summary>
		/// Establece el nombre de filtro, Label arriba de cada ComboBox filtro,
		/// a partir de los Header de la Tabla de informacion.
		/// Debe invocarse antes el procedimiento CreaFiltros
		/// </summary>
		/// <param name="nroColumna">0-8, nro de columna de la Tabla de datos que representa el filtro. </param>
		public void NombrarFiltros(params byte[] nroColumna)
		{
			for (int i = 0; i < filtros.Length; i++)
			{
				etiquetas[i].Text = dgvDatos.Columns[nroColumna[i]].Name;
			}
		}

		/// <summary>
		/// Evento Click para los botones. Ejecuta los eventos redefinibles de acuerdo al boton presionado.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void botones_Click(object sender, EventArgs e)
		{
			var b = (Button)sender;
			switch ((int)b.Tag)
			{
				case 0:
					Consultar();
					break;
				case 1:
					Nuevo(sender, e);
					break;
				case 2:
					Modificar(sender, e);
					break;
				case 3:
					Eliminar(sender, e);
					break;
				case 4:
					Listar();
					break;
				case 5:
					OtrosBotones_Click(sender, e);
					break;
			}
		}

		/// <summary>
		/// Procedimiento del boton Consultar. Definir en formulario heredado.
		/// </summary>
		protected virtual void Consultar()
		{
			MessageBox.Show("No Implementado. Ingresar codigo en formulario heredado");
		}

		/// <summary>
		/// Procedimiento del boton Nuevo. Definir en formulario heredado.
		/// </summary>
		protected virtual void Nuevo(object sender, EventArgs e)
		{
			MessageBox.Show("No Implementado. Ingresar codigo en formulario heredado");
		}

		/// <summary>
		/// Procedimiento del boton Modificar. Definir en formulario heredado.
		/// </summary>
		protected virtual void Modificar(object sender, EventArgs e)
		{
			MessageBox.Show("No Implementado. Ingresar codigo en formulario heredado");
		}

		/// <summary>
		/// Procedimiento del boton Eliminar. Definir en formulario heredado.
		/// </summary>
		protected virtual void Eliminar(object sender, EventArgs e)
		{
			MessageBox.Show("No Implementado. Ingresar codigo en formulario heredado");
		}

		/// <summary>
		/// Procedimiento del boton Listado. Definir en formulario heredado.
		/// </summary>
		protected virtual void Listar()
		{
			MessageBox.Show("No Implementado. Ingresar codigo en formulario heredado");
		}

		/// <summary>
		/// Procedimiento que se invoca al presionar uno de los botones opcionales.
		/// Se pasa el objeto tipo boton para trabajarlo en el formulario heredado.
		/// </summary>
		/// <param name="sender"></param> Tipo Button
		/// <param name="e"></param>
		protected virtual void OtrosBotones_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Evento del boton Cerrar. Cierra el formulario Buscar.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btnCerrar_Click(object sender, EventArgs e)
		{
			Close();
		}

		protected virtual void btnTodos_Click(object sender, EventArgs e)
		{
			txtBoxBuqueda.Text = "";
			for (int i = 0; i < filtros.Length; i++)
			{
				filtros[i].SelectedIndex = -1;
			}
			txtBoxBuqueda.Focus();
		}

		protected virtual void btnBuscar_Click(object sender, EventArgs e)
		{
			AplicarFiltros(sender, e);
			txtBoxBuqueda.SelectAll();
		}


		/// <summary>
		/// Evento que aplica los filtros y busquedas elegidas para reducir la
		/// informacion mostrada en la grilla
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void AplicarFiltros(object sender, EventArgs e)
		{
			string cadfitro = "";
			for (int i = 0; i < filtros.Length; i++)
			{
				if (filtros[i].SelectedIndex >= 0)
					cadfitro = cadfitro + " and " + etiquetas[i].Text + " like '*" + filtros[i].Text + "*'";
			}
			if (txtBoxBuqueda.TextLength > 0)
				if (Controles.IsNumeric(txtBoxBuqueda.Text))
					cadfitro = cadfitro + " and [" + busquedas[getBusquedaSeleccionada()].Text + "] = " + txtBoxBuqueda.Text;
				else {
					if (getBusquedaSeleccionada() != 0)
						cadfitro = cadfitro + " and [" + busquedas[getBusquedaSeleccionada()].Text + "] LIKE '*" + txtBoxBuqueda.Text + "*'";
				}
			if (cadfitro.Length > 0)
				cadfitro = cadfitro.Remove(0, 5);
			binding.Filter = cadfitro;
		}



		/// <summary>
		/// consulta el contenido de un campo de un renglon de un dgv.
		/// </summary>
		/// <param name="idColumna">id de columna de la grilla</param>
		/// <returns></returns>
		public string ConsultaCampoRenglon(int idColumna)
		{
			return dgvDatos.SelectedRows[0].Cells[idColumna].Value.ToString();
		}

		/// <summary>
		/// Devuelve la posicion activa de los criterios del busqueda del arreglo Busquedas
		/// </summary>
		/// <returns></returns>
		int getBusquedaSeleccionada()
		{
			int i = -1;
			do
				i++;
			while (!busquedas[i].Checked);
			return i;
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

		public string Nivel
		{
			get { return nivel; }
		}

		/// <summary>
		/// Abre el formulario y aplica los permisos correspondientes al nivel e indice.
		/// Es necesario invocar este metodo desde las redefiniciones abrir en los formularios heredados.
		/// La llamada en la redefinicion debe estar al final del metodo
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