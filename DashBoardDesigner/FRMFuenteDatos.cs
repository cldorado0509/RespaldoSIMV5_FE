using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DashBoardDesigner
{
    public partial class FRMFuenteDatos : Form
    {
        public List<object> FuenteDatosSeleccionadas = null;

        public FRMFuenteDatos()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            FuenteDatosSeleccionadas = chkFuentesDatos.Items.GetCheckedValues();
        }

        private void FRMFuenteDatos_Load(object sender, EventArgs e)
        {
            chkFuentesDatos.Items.Add(new FuenteDatos() { ID = 1, Nombre = "PMES", Descripcion = "Consulta de PMES" });
            chkFuentesDatos.Items.Add(new FuenteDatos() { ID = 2, Nombre = "PMES1_SexoEdadPorc", Descripcion = "Cont Población: Sexo y Edad (Porcentaje)" });
            chkFuentesDatos.Items.Add(new FuenteDatos() { ID = 3, Nombre = "PMES1_EstratoPorc", Descripcion = "Cont Población: Estrato (Porcentaje)" });
            chkFuentesDatos.Items.Add(new FuenteDatos() { ID = 4, Nombre = "PMES1_HorarioEntSal", Descripcion = "Cont Población: Horario de Entrada y Salida" });
            chkFuentesDatos.Items.Add(new FuenteDatos() { ID = 5, Nombre = "PMES1_TipoTrabajador", Descripcion = "Cont Población: Tipo Trabajador" });
            chkFuentesDatos.Items.Add(new FuenteDatos() { ID = 6, Nombre = "PMES1_Area", Descripcion = "Cont Población: Area" });
            chkFuentesDatos.Items.Add(new FuenteDatos() { ID = 7, Nombre = "PMES1_Parqueo", Descripcion = "Cont Población: Parqueo" });
            chkFuentesDatos.Items.Add(new FuenteDatos() { ID = 8, Nombre = "PMES1_TeleTrabajo", Descripcion = "Cont Población: TeleTrabajo" });
            chkFuentesDatos.Items.Add(new FuenteDatos() { ID = 9, Nombre = "PMES1_DiasSemana", Descripcion = "Cont Población: Dias de la Semana" });
            chkFuentesDatos.Items.Add(new FuenteDatos() { ID = 10, Nombre = "PMES1_Georeferenciacion", Descripcion = "Cont CO2: Georeferenciación" });
            chkFuentesDatos.Items.Add(new FuenteDatos() { ID = 11, Nombre = "PMES1_DistribucionModal", Descripcion = "Cont CO2: Distribución Modal" });
            chkFuentesDatos.Items.Add(new FuenteDatos() { ID = 12, Nombre = "PMES1_TiempoViaje", Descripcion = "Cont CO2: Tiempo de Viaje" });
            chkFuentesDatos.Items.Add(new FuenteDatos() { ID = 13, Nombre = "PMES1_Distancia", Descripcion = "Cont CO2: Distancia" });
        }
    }
}
