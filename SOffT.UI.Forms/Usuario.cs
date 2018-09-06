//
//  Usuario.cs
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Hamekoz.Data;

namespace Sofft.Utils
{
	public class Usuario
	{
        public static bool Requerir { get; set; }

        public int Id
		{
			get;
			set;
		}

		public string Login
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

        public static int ModuloId { get; set; }

        public static Usuario Actual { get; set; }

        public int ExisteUsuario(string usr, string pwd)
		{
			int idUsuario;
			idUsuario = Convert.ToInt32(DB.Instancia.SPToScalar("usuariosConsultarSiExiste", "usr", usr, "pwd", pwd));
			return idUsuario > 0 ? idUsuario : 0;
		}

		public static void SetPermisos(ref List<Button> botones, string nivel)
		{
			if (Actual != null)
			{
				var ds = DB.Instancia.SPToDataSet("permisosConsultarNivel", "@idUsuario", Actual.Id, "@idModulo", ModuloId, "nivel", nivel);
				foreach (DataRow dr in ds.Tables["permisosConsultarNivel"].Rows)
				{
					try
					{
						botones[Convert.ToInt32(dr["indice"])].Enabled = (bool)dr["acceso"];
					}
					catch (Exception)
					{
						Console.WriteLine("Eliminar Permisos del Nivel: " + nivel + " indice: " + dr["indice"] + " por no corresponder a ningun boton");
					}
				}
			}
		}


		public static void SetPermisosSubIndices(ref List<Button> botones, int indice)
		{
			if (Actual != null)
			{
				var ds = DB.Instancia.SPToDataSet("permisosUsuariosConsultarSubIndice", "idUsuario", Actual.Id, "idModulo", ModuloId, "indice", indice);
				foreach (DataRow dr in ds.Tables["permisosUsuariosConsultarSubIndice"].Rows)
				{
					botones[(int)dr["subIndice"] - 1].Enabled = (bool)dr["acceso"];
				}
			}
		}
	}
}
