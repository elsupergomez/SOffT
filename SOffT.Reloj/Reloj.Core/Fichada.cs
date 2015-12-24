//
//  Fichada.cs
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

using Sueldos.Entidades;
using System;

namespace Reloj.Core
{
	public class Fichada
	{
		public int Id { get; set; }

		public int Legajo { get; set; }

		public string ApellidoYnombres { get; set; }

		public string Foto { get; set; }

		public DateTime FechaHora {
			get;
			set;
		}

		public string Fecha {
			get {
				return FechaHora.ToShortDateString ();
			}
		}

		public string Hora {
			get {
				return string.Format ("{0:D2}:{1:D2}:{2:D2}", FechaHora.Hour, FechaHora.Minute, FechaHora.Second);
			}
		}

		public int RelojId {
			get;
			set;
		}

		public TablaEntity Reloj{ get; set; }

		public Fichada ()
		{
		}

		public Fichada (int id)
		{
			Id = id;
		}

		public Fichada (int id, int legajo)
		{

			Id = id;
			Legajo = legajo;
		}

		public string ToCustomString ()
		{
			return string.Format ("{0} {1} -> {2:D5} {3}", Fecha, Hora, Legajo, ApellidoYnombres);
		}
	}
}
