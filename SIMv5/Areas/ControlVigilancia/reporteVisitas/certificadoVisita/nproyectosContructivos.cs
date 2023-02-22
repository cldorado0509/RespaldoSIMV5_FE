using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita
{
    public partial class nproyectosContructivos : DevExpress.XtraReports.UI.XtraReport
    {
        public nproyectosContructivos()
        {
            InitializeComponent();
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            /*int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_ITEM_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;

            ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = 7;*/

            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_ITEM_ESTADO"));

            if (id_estado > 0)
            {
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;

                int id_formulario = 7;
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = id_formulario;

                var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
                var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                query.Sql = "select * from control.VWR_ENCUESTAS WHERE ID_ESTADO = " + id_estado.ToString() + " And ID_FORMULARIO = 7";

                dataSource.RebuildResultSchema();
            }
            else
            {
                ((XRSubreport)sender).Visible = false;
                ((XRSubreport)sender).ReportSource.DataSource = null;
                e.Cancel = true;
            }
        }

        private void xrSubreport9_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            /*int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_ITEM_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;*/

            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_ITEM_ESTADO"));

            if (id_estado > 0)
            {
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;

                var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
                var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                query.Sql = "select * from control.VWR_FOTOGRAFIAS_PROYECTO WHERE ID_ESTADO = " + id_estado.ToString();

                dataSource.RebuildResultSchema();
            }
            else
            {
                ((XRSubreport)sender).Visible = false;
                ((XRSubreport)sender).ReportSource.DataSource = null;
                e.Cancel = true;
            }
        }

        private void nproyectosContructivos_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrpEncabezado.Image = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/imagenes/cabezoteVisitas.png"));
            xrpUbicacion.Image = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/imagenes/mapa/Procesos_Constructivos.png"));
        }
    }
}
