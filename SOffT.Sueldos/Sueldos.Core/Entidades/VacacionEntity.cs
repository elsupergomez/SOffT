//
//  VacacionEntity.cs
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

namespace Sueldos.Entidades
{
	public class VacacionEntity
	{
		public int Legajo {
			get;
			set;
		}

		public DateTime Fecha {
			get;
			set;
		}

		int codigo = 45;

		public int Codigo {
			get { return codigo; }
			set { codigo = value; }
		}

		public int AnioVacaciones {
			get;
			set;
		}

		public int DiasTomados {
			get;
			set;
		}

		public int TotalDias {
			get;
			set;
		}
	}
}
