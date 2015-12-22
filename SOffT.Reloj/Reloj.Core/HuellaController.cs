//
//  HuellaController.cs
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



using System.Collections.Generic;

namespace Reloj.Core
{
	public class HuellaController
	{
		HuellaData persistencia = new HuellaData ();

		/// <summary>
		/// Crea una nueva instancia de Huella
		/// </summary>
		/// <returns>Huella con valor de propiedades por defecto</returns>
		public Huella GetNew ()
		{
			return new Huella ();
		}

		/// <summary>
		/// Obtiene una instancia especifica de Huella
		/// </summary>
		/// <param name = "legajo"></param>
		/// <param name = "idHuella"></param>
		/// <returns>Instancia de Huella solicitada</returns>
		public Huella getById (int legajo, int idHuella)
		{
			Huella huella;
			huella = persistencia.GetBy (legajo, idHuella);
			return huella;
		}

		public Huella getByHuella (string huellaStr)
		{
			Huella huella;
			huella = persistencia.GetBy (huellaStr);
			return huella;
		}

		/// <summary>
		/// Obtiene la lista de todos las Huella
		/// </summary>
		/// <returns>Lista de Huella</returns>
		public List<Huella> getLista ()
		{
			List<Huella> huellas = persistencia.GetAll ();
			return huellas;
		}

		/// <summary>
		/// Obtiene la lista de todas las Huella de un legajo
		/// </summary>
		/// <returns>Lista de Huella</returns>
		public List<Huella> getByLegajo (int legajo)
		{
			List<Huella> huellas = persistencia.GetBy (legajo);
			return huellas;
		}

		/// <summary>
		/// Accion de eliminar una Huella
		/// </summary>
		/// <param name="huella">Huella a eliminar</param>
		public void Eliminar (Huella huella)
		{
			persistencia.Delete (huella);
		}

		/// <summary>
		/// elimina todas las huellas de un legajo
		/// </summary>
		/// <param name="legajo"></param>
		public void Eliminar (int legajo)
		{
			persistencia.Delete (legajo);
		}

		/// <summary>
		/// Acción de insertar una Huella
		/// </summary>
		/// <param name="huella">Huella a guardar</param>
		public void Guardar (Huella huella)
		{
			Validar (huella);
			if (persistencia.GetBy (huella.Legajo, int.Parse (huella.DedoHuella.Contenido.ToString ())) == null) {
				persistencia.Insert (huella);
			} else {
				persistencia.Update (huella);
			}
		}

		/// <summary>
		/// Valida los datos de Huella
		/// </summary>
		/// <param name="huella">Huella a validar</param>
		static void Validar (Huella huella)
		{
			if (huella.Patron == null || huella.Patron.Equals (string.Empty)) {
				throw new FichadaException ("Falta cargar la huella");
			}
		}
	}
}
