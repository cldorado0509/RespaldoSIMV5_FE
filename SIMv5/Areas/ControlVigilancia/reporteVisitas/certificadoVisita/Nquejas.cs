using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita
{
    public partial class Nquejas : DevExpress.XtraReports.UI.XtraReport
    {
        public Nquejas()
        {
            InitializeComponent();
        }


        private void xrSubreport2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            /*int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_QUEJA_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;


            ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = 11;*/

            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_QUEJA_ESTADO"));

            if (id_estado > 0)
            {
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;

                ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = 11;

                var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
                var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                query.Sql = "select * from control.VWR_CARATERISTICAGRUPO WHERE ID_ESTADO = " + id_estado.ToString() + " And GRUPO_FORMULARIO = 0 And ID_FORMULARIO = 11";

                dataSource.RebuildResultSchema();
            }
            else
            {
                ((XRSubreport)sender).Visible = false;
                ((XRSubreport)sender).ReportSource.DataSource = null;
                e.Cancel = true;
            }
        }

        private void xrSubreport3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            /*int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_QUEJA_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;*/

            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_QUEJA_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;

            var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
            var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

            query.Sql = "select * from control.VWR_FOTOGRAFIAS_QUEJA WHERE ID_ESTADO = " + id_estado.ToString();

            dataSource.RebuildResultSchema();
        }

        private void Nquejas_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrpEncabezado.Image = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/imagenes/cabezoteVisitas.png"));
            xrpUbicacion.Image = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/imagenes/mapa/Quejas.png"));
        }
    }
}
