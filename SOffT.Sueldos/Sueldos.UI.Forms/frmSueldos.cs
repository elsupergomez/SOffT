using System.Windows.Forms;
using Sofft.Utils;
using Sofft.UI.Forms;

namespace Sueldos.View
{
	public partial class frmSueldos : PrincipalForm
	{
		public frmSueldos ()
		{
			InitializeComponent ();
			Modulo.SeteaCultura ();
			Modulo.ValidaLogin = true;
			AgregarBoton ("A.B.M. Empleados");
			AgregarBoton ("Reloj y Asistencia");
			AgregarBoton ("Anticipos");
			AgregarBoton ("Liquidacion");
			AgregarBoton ("Cargas Sociales");
			AgregarBoton ("Informes");
			AgregarBoton ("Herramientas");
			setDatos ("", "", "Gestion de Asistencia, Remuneraciones y Cargas Sociales", "", "version: " + Application.ProductVersion);
			Text = "SOffT Sueldos";
			if (Modulo.ValidaLogin)
				verificaLogin ();
		}

		public override void boton_Click (int indice)
		{
			switch (indice) {
			case 0: //A.B.M. Empleados
				var frmEmple = new frmABMempleados ();
				frmEmple.abrir (Nivel, indice);
				break;
			case 1: //Reloj y Asistencia
				var frmAsi = new frmMnuAsistencia ();
				frmAsi.abrir (Nivel, indice);
				break;
			case 2: //Anticipios
				var frmAnti = new frmMnuAnticipos ();
				frmAnti.abrir (Nivel, indice);
				break;
			case 3: //Liquidacion
				var frmLiq = new frmMnuLiquidacion ();
				frmLiq.abrir (Nivel, indice);
				break;
			case 4: //Cargas Sociales
				var frmCargas = new frmMnuCargasSociales ();
				frmCargas.abrir (Nivel, indice);
				break;
			case 5: //Informes
				var frmInf = new frmMnuInformes ();
				frmInf.abrir (Nivel, indice);
				break;
			case 6: //Herramientas
				var frmHerr = new frmMnuHerramientas ();
				frmHerr.abrir (Nivel, indice);
				break;
			}
		}
	}
}

