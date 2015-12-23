//
//  PermisosForm.cs
//
//  Author:
//       Claudio Rodrigo Pereyra Diaz <claudiorodrigo@pereyradiaz.com.ar>
//
//  Copyright (c) 2008 Hamekoz - www.hamekoz.com.ar
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
using System.Data.Common;
using System.Windows.Forms;
using System.Xml;
using Hamekoz.Data;

namespace Seguridad.UI.Forms
{
	public partial class PermisosForm : Form
	{
		//TODO Reemplazar constante por el manejo generico de modulos.
		//idModulo = 2 corresponde al modulo sueldos
		const int ModuloId = 2;
		const string PermisosXML = "Permisos.xml";

		int idUsuario;

		public PermisosForm ()
		{ 
			InitializeComponent ();
		}

		void cargarListaPermisos ()
		{
			try {
				// SECTION 1. Create a DOM Document and load the XML data into it.
				var dom = new XmlDocument ();
				dom.Load (Application.StartupPath + System.IO.Path.DirectorySeparatorChar + PermisosXML);

				// SECTION 2. Initialize the TreeView control.
				treeViewPermisos.Nodes.Clear ();
				treeViewPermisos.Nodes.Add (new TreeNode (dom.DocumentElement.Name));
				TreeNode tNode = treeViewPermisos.Nodes [0];

				// SECTION 3. Populate the TreeView with the DOM nodes.
				agregarNodo (dom.DocumentElement, tNode);
				//treeViewPermisos.ExpandAll();
				treeViewPermisos.Nodes [0].Checked = true;
				treeViewPermisos.Nodes [0].Expand ();
				treeViewPermisos.SelectedNode = treeViewPermisos.Nodes [0];
			} catch (XmlException xmlEx) {
				MessageBox.Show (xmlEx.Message);
			} catch (Exception ex) {
				MessageBox.Show (ex.Message);
			}
		}

		void agregarNodo (XmlNode inXmlNode, TreeNode inTreeNode)
		{
			XmlNode xNode;
			TreeNode tNode;
			XmlNodeList nodeList;
			int i;

			// Loop through the XML nodes until the leaf is reached.
			// Add the nodes to the TreeView during the looping process.
			if (inXmlNode.HasChildNodes) {
				nodeList = inXmlNode.ChildNodes;
				for (i = 0; i <= nodeList.Count - 1; i++) {
					xNode = inXmlNode.ChildNodes [i];
					inTreeNode.Nodes.Add (xNode.Attributes ["Nombre"].Value);
					tNode = inTreeNode.Nodes [i];
					agregarNodo (xNode, tNode);
				}
			} else {
				// Here you need to pull the data from the XmlNode based on the
				// type of node, whether attribute values are required, and so forth.
				inTreeNode.Text = inXmlNode.Attributes ["Nombre"].Value;
			}
		}

		int getIdEmpleado ()
		{
			return cbAsociarEmpleado.Checked ? Convert.ToInt32 (cmbEmpleados.SelectedValue) : 0;
		}

		void btnGrabar_Click (object sender, EventArgs e)
		{
			DB.Instancia.SP ("permisosEliminar", "idUsuario", cmbUsuarios.SelectedValue, "idModulo", ModuloId);
			if (treeViewPermisos.Nodes.Count > 0) {
				foreach (TreeNode nodo in treeViewPermisos.Nodes) {
					grabar (nodo.Nodes, ModuloId.ToString (), ModuloId);
				}
			}
			DB.Instancia.SP ("usuariosActualizar",
				"idUsuario", idUsuario,
				"nombre", cmbUsuarios.Text,
				"login", txtUsuario.Text,
				"password", txtPassword.Text,
				"documentos", cbDocumentos.Checked,
				"seguridad", cbSeguridad.Checked,
				"idEmpleado", getIdEmpleado (),
				"eliminado", cbDeshabilitar.Checked);
			MessageBox.Show ("Ya se pueden utilizar los nuevos permisos");
		}

		void grabar (TreeNodeCollection nodo, string nivel, int modulo)
		{
			if (nodo.Count > 0) {
				foreach (TreeNode nodoHijo in nodo) {
					DB.Instancia.SP ("permisosActualizar",
						"idModulo", modulo,
						"idUsuario", cmbUsuarios.SelectedValue,
						"nivel", nivel,
						"indice", nodoHijo.Index,
						"acceso", nodoHijo.Checked);
					grabar (nodoHijo.Nodes, string.Concat (nivel, ".", nodoHijo.Index), modulo);
				}
			}
		}

		void btnCerrar_Click (object sender, EventArgs e)
		{
			Close ();
		}

		void frmPermisos_Load (object sender, EventArgs e)
		{
			cargarListaPermisos ();
			var empleados = DB.Instancia.SPToDataSet ("empleadosConsultarParaCombo");
			cmbEmpleados.ValueMember = "idEmpleado";
			cmbEmpleados.DisplayMember = "Apellido y Nombres";
			cmbEmpleados.DataSource = empleados.Tables [0];
			cmbEmpleados.DropDownStyle = ComboBoxStyle.DropDownList;

			var usuarios = DB.Instancia.SPToDataSet ("usuariosConsultarParaCombo");
			cmbUsuarios.ValueMember = "idusuario";
			cmbUsuarios.DisplayMember = "nombre";
			cmbUsuarios.DataSource = usuarios.Tables [0];
			cmbUsuarios.DropDownStyle = ComboBoxStyle.DropDownList;
		}

		void cbAsociarEmpleado_CheckedChanged (object sender, EventArgs e)
		{
			cmbEmpleados.Enabled = cbAsociarEmpleado.Checked;
		}

		void cmbUsuarios_SelectedIndexChanged (object sender, EventArgs e)
		{
			idUsuario = Convert.ToInt32 (cmbUsuarios.SelectedValue);
			//Consulto los datos del usuario, esto deberia estar encapsulado en el objeto usuario
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("usuariosConsultar", "idUsuario", cmbUsuarios.SelectedValue)) {
				if (rs.Read ()) {
					cbAsociarEmpleado.Checked = (Convert.ToInt32 (rs ["idEmpleado"]) > 0);
					if (cbAsociarEmpleado.Checked)
						cmbEmpleados.SelectedValue = Convert.ToInt32 (rs ["idEmpleado"]);
					cbDocumentos.Checked = Convert.ToBoolean (rs ["documentos"]);
					cbSeguridad.Checked = Convert.ToBoolean (rs ["seguridad"]);
					txtPassword.Text = Convert.ToString (rs ["password"]);
					txtUsuario.Text = Convert.ToString (rs ["login"]);
					cbDeshabilitar.Checked = Convert.ToBoolean (rs ["eliminado"]);
				}
			}

			//Consulto los permisos del usuario, esto deberia estar encapsulado en el objeto usuario
			cargarListaPermisos ();
			using (DbDataReader rs = DB.Instancia.SPToDbDataReader ("permisosConsultar", "idUsuario", cmbUsuarios.SelectedValue, "idModulo", ModuloId)) {
				while (rs.Read ())
					cargarPermisos (Convert.ToString (rs ["nivel"]), Convert.ToInt32 (rs ["indice"]), Convert.ToBoolean (rs ["acceso"]));
			}
		}

		void cargarPermisos (string nivel, int indice, Boolean valor)
		{
			string[] id = nivel.Split (".".ToCharArray ());
			TreeNode nodo = treeViewPermisos.Nodes [0];
			for (int i = 1; i < id.Length; i++) {
				nodo = getNodo (nodo, Convert.ToInt32 (id [i]));
			}
			try {
				nodo.Nodes [indice].Checked = valor;
			} catch (Exception) {
				MessageBox.Show ("Revisar permisos del Nivel: " + nivel + " " + nodo.Text + ". El indice: " + indice + " no definido en el XML");
			}
		}

		TreeNode getNodo (TreeNode nodo, int indice)
		{
			return nodo.Nodes [indice];
		}
	}
}

//TODO Remover
namespace Seguridad.View
{
	[Obsolete ("Usar Seguridad.UI.Forms.PermisosForm")]
	public class frmPermisos : Seguridad.UI.Forms.PermisosForm
	{
	}
}