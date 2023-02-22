using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita
{
    public partial class NfuentesFijas : DevExpress.XtraReports.UI.XtraReport
    {
        public NfuentesFijas()
        {
            InitializeComponent();
        }

        private void xrSubreport2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            /*int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_FUENTE_FIJA_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;


            ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = 9;
            ((XRSubreport)sender).ReportSource.Parameters["prm_grupo"].Value = 0;*/

            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_FUENTE_FIJA_ESTADO"));

            if (id_estado > 0)
            {
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = 9;
                ((XRSubreport)sender).ReportSource.Parameters["prm_grupo"].Value = 0;

                var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
                var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                query.Sql = "select * from control.VWR_CARATERISTICAGRUPO WHERE ID_ESTADO = " + id_estado.ToString() + " And GRUPO_FORMULARIO = 0 And ID_FORMULARIO = 9";

                dataSource.RebuildResultSchema();

                ((XRSubreport)sender).Visible = false;
            }
            else
            {
                ((XRSubreport)sender).Visible = false;
                ((XRSubreport)sender).ReportSource.DataSource = null;
                e.Cancel = true;
            }
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            /*int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_FUENTE_FIJA_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;


            ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = 9;
            ((XRSubreport)sender).ReportSource.Parameters["prm_grupo"].Value = 1;*/

            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_FUENTE_FIJA_ESTADO"));

            if (id_estado > 0)
            {
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = 9;
                ((XRSubreport)sender).ReportSource.Parameters["prm_grupo"].Value = 1;

                var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
                var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                query.Sql = "select * from control.VWR_CARATERISTICAGRUPO WHERE ID_ESTADO = " + id_estado.ToString() + " And GRUPO_FORMULARIO = 1 And ID_FORMULARIO = 9";

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
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_FUENTE_FIJA_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;

        }

        private void NfuentesFijas_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrpEncabezado.Image = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/imagenes/cabezoteVisitas.png"));
            xrpUbicacion.Image = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/imagenes/mapa/Fuentes_Fijas.png"));
        }
    }
}
