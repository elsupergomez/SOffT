//
//  AcreditacionBapro.cs
//
//  Author:
//       Claudio Rodrigo Pereyra Diaz <claudiorodrigo@pereyradiaz.com.ar>
//
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

namespace Sueldos.Entidades
{
	public class AcreditacionBapro
	{
		public string CBU { get; set; }

		public decimal Importe { get; set; }

		public string ImporteString {
			get { return string.Format ("{0:F2}", Importe); }
		}

		public string CUIL { get; set; }

		public string Referencia { get; set; }

		public string ApellidosNombres { get; set; }
	}
}
