//
//  ConsultaEmpleados.cs
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
using Sueldos.Data;
using Sueldos.Entidades;

namespace Sueldos.Modelo
{
	public class ConsultaEmpleados
	{
		readonly EmpleadoData persistencia = new EmpleadoData ();

		/// <summary>
		/// Graba Empleado (inserta/actualiza)
		/// </summary>
		/// <param name="empleado"></param>
		/// <returns></returns>
		public int grabar (EmpleadoEntity empleado)
		{
			return persistencia.insert (empleado);
		}

		/// <summary>
		/// Consulta el legajo de un empleado a partir del cuil.
		/// El cuil debe pasarse sin guiones
		/// </summary>
		/// <param name="cuil"></param>
		/// <returns></returns>
		public static int consultarLegajoEmpleado (string cuil)
		{
			int legajo;
			legajo = new EmpleadoData ().getLegajoByCUIL (cuil);
			return legajo;
		}

		/// <summary>
		/// Dado un legajo, consulta el apellido y nombres del empleado.
		/// </summary>
		/// <param name="legajo"></param>
		/// <returns></returns>
		public static string consultarApellidoYnombres (int legajo)
		{
			string nombre;
			nombre = new EmpleadoData ().getNombresByLegajo (legajo);
			return nombre;
		}

		/// <summary>
		/// Dado un legajo, consulta el apellido y nombres del empleado.
		/// </summary>
		/// <param name="legajo"></param>
		/// <returns></returns>
		public static string consultarApellidoYnombresConBajas (int legajo)
		{
			string nombre;
			nombre = new EmpleadoData ().getNombresByLegajoConBajas (legajo);
			return nombre;
		}

		/// <summary>
		/// Dado un legajo, consulta la foto del empleado.
		/// </summary>
		/// <param name="legajo"></param>
		/// <returns></returns>
		public static string consultarFoto (int legajo)
		{
			string foto;
			foto = new EmpleadoData ().getFotoByLegajo (legajo);
			return foto;
		}

		[Obsolete ("Usar ListaDatosBasicos ()")]
		public List<EmpleadoEntity> GetAlldatosBasicos ()
		{
			return ListaDatosBasicos ();
		}

		public List<EmpleadoEntity> ListaDatosBasicos ()
		{
			var empleados = new List<EmpleadoEntity> ();
			DataSet ds = persistencia.GetAlldatosBasicos ();
			foreach (DataRow item in ds.Tables[0].Rows) {
				var empleado = new EmpleadoEntity {
					Nombre = item.Field<string> ("Nombres"),
					Legajo = item.Field<int> ("Legajo"),
				};
				empleados.Add (empleado);
			}
			return empleados;
		}

		/// <summary>
		/// Verifica si existe un legajo
		/// </summary>
		/// <param name="legajo"></param>
		/// <returns></returns>
		public bool existeLegajo (int legajo)
		{
			bool existe;
			existe = persistencia.existeLegajo (legajo);
			return existe;
		}

		public void actualizarFechas (EmpleadoEntity empleado)
		{
			persistencia.cargarFechas (empleado);
		}

		/// <summary>
		/// Crea empleado y carga datos del empleado previamente existente.
		/// </summary>
		/// <param name="idEmpleado"></param>
		/// <returns></returns>
		public EmpleadoEntity getEmpleado (int idEmpleado)
		{
			EmpleadoEntity emp;
			emp = persistencia.getById (idEmpleado);
			return emp;
		}
	}
}
