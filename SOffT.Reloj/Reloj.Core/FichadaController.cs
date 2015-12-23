//
//  FichadaController.cs
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
using System.Diagnostics;
using ExcelLibrary;
using Sueldos.Modelo;

namespace Reloj.Core
{
	public class FichadaController
	{
		TablaController tablaController = new TablaController ();

		public Fichada UltimaFichada {
			get;
			private set;
		}

		FichadaData persistencia = new FichadaData ();

		/// <summary>
		/// Crea una nueva instancia de Fichada
		/// </summary>
		/// <returns>Fichada con valor de propiedades por defecto</returns>
		public Fichada GetNew ()
		{
			return new Fichada ();
		}

		/// <summary>
		/// Obtiene una instancia especifica de Fichada
		/// </summary>
		/// <param name="id">Identificador unico de Fichada</param>
		/// <returns>Instancia de Fichada solicitada</returns>
		public Fichada getById (short id)
		{
			Fichada fichada;
			fichada = persistencia.GetBy (id);
			return fichada;
		}

		/// <summary>
		/// Obtiene la lista de todos las Fichada
		/// </summary>
		/// <returns>Lista de Fichada</returns>
		public List<Fichada> getLista ()
		{
			List<Fichada> fichadas = persistencia.GetAll ();
			return fichadas;
		}

		/// <summary>
		/// Obtiene la lista de todos las Fichadas entre fechas
		/// </summary>
		/// <returns>Lista de Fichada</returns>
		public List<Fichada> getListaEntreFechas (DateTime desde, DateTime hasta)
		{
			List<Fichada> fichadas = persistencia.GetEntreFechas (desde, hasta);
			return fichadas;
		}

		[Obsolete ("Usar metodo con otros parametros")]
		public DataSet ExportarXLS (DateTime desde, DateTime hasta)
		{
			DataSet ds;
			ds = persistencia.GetAll (desde, hasta);
			return ds;
		}

		public void ExportarXLS (DateTime desde, DateTime hasta, string archivo)
		{
			DataSet fichadas = persistencia.GetAll (desde, hasta);
			DataSetHelper.CreateWorkbook (archivo, fichadas);
			Process.Start (archivo);
		}

		/// <summary>
		/// Accion de eliminar una Fichada
		/// </summary>
		/// <param name="fichada">Fichada a eliminar</param>
		/*public void Eliminar(FichadaEntity fichada)
        {
            using (var fichadaData = new FichadaData())
            {
                FichadaData.Delete(fichada);
            }
        }*/

		/// <summary>
		/// Acción de guardar un Fichada
		/// </summary>
		/// <param name="fichada">Fichada a guardar</param>
		public void Guardar (Fichada fichada)
		{
			fichada.FechaHora = DateTime.Now;

			//verifica tiempos entre fichadas y legajo repetido
			//si la fichada es del mismo legajo y está dentro de los 30 segundos se descarta.
			if (UltimaFichada != null) {
				var timeStamp = new TimeSpan (fichada.FechaHora.Ticks - UltimaFichada.FechaHora.Ticks);
				var segundosEntreFichada = timeStamp.TotalSeconds;
				if (UltimaFichada.Legajo == fichada.Legajo && segundosEntreFichada < 30)
					throw new Exception ("Debe esperar al menos 30 segundos entre cada fichada");
			}

			fichada.ApellidoYnombres = ConsultaEmpleados.consultarApellidoYnombres (fichada.Legajo);

			if (fichada.ApellidoYnombres != "SIN DATO") {
				fichada.Foto = ConsultaEmpleados.consultarFoto (fichada.Legajo);
				fichada.Foto = fichada.Foto.Replace ("\\\\curie\\Sistemas\\Personal\\Fotos\\", "./fotos/");
				fichada.Reloj = tablaController.getById ("reloj", 3, fichada.RelojId);
			} else {
				throw new Exception ("Empleado no encontrado");
			}

			if (fichada.Id == 0) {
				persistencia.Insert (fichada);
			}

			UltimaFichada = fichada;
		}

		/// <summary>
		/// Valida los datos de Fichada
		/// </summary>
		/// <param name="fichada">Fichada a validar</param>
		static void Validar (Fichada fichada)
		{
			if (fichada.Fecha == null || fichada.Fecha.Equals (string.Empty)) {
				throw new FichadaException ("Falta cargar la fecha");
			}
		}
	}
}

//TODO Remover
namespace Reloj.Negocio
{
	[Obsolete ("Usar Reloj.Core.FichadaController")]
	public class FichadasNegocio : Reloj.Core.FichadaController
	{
		public DataSet getAll (DateTime desde, DateTime hasta)
		{
			return ExportarXLS (desde, hasta);
		}
	}
}
