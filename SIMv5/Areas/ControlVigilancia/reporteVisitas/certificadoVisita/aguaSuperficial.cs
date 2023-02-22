using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using DevExpress.DataAccess.Sql;

namespace SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita
{
    public partial class aguaSuperficial : DevExpress.XtraReports.UI.XtraReport
    {
        public aguaSuperficial()
        {
            InitializeComponent();
        }

        private void xrSubreport3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_CAPTACION_ESTADO"));

            if (id_estado > 0)
            {
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_estadoUso"].Value = id_estado;
                ((XRSubreport)sender).ReportSource.Parameters["prm_tipo"].Value = 2;

                var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
                var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                query.Sql = "select * from agua.VWR_USOS WHERE ID_CAPTACION_ESTADO = " + id_estado.ToString();

                dataSource.RebuildResultSchema();
            }
            else
            {
                ((XRSubreport)sender).Visible = false;
                ((XRSubreport)sender).ReportSource.DataSource = null;
                e.Cancel = true;
            }
        }

        private void xrSubreport2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_CAPTACION_ESTADO"));

            if (id_estado > 0)
            {
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;

                int id_formulario = Convert.ToInt32(GetCurrentColumnValue("ID_FORMULARIO"));
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = id_formulario;
                ((XRSubreport)sender).ReportSource.Parameters["prm_tipo"].Value = 1;

                var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
                var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                query.Sql = "select * from control.VWR_CARACTERISTICAS WHERE ID_ESTADO = " + id_estado.ToString() + " And ID_FORMULARIO = " + id_formulario.ToString();

                dataSource.RebuildResultSchema();
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
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_CAPTACION_ESTADO"));

            if (id_estado > 0)
            {
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;

                int id_formulario = Convert.ToInt32(GetCurrentColumnValue("ID_FORMULARIO"));
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = id_formulario;

                var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
                var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                query.Sql = "select * from control.VWR_ENCUESTAS WHERE ID_ESTADO = " + id_estado.ToString() + " And ID_FORMULARIO = " + id_formulario.ToString();

                dataSource.RebuildResultSchema();
            }
            else
            {
                ((XRSubreport)sender).Visible = false;
                ((XRSubreport)sender).ReportSource.DataSource = null;
                e.Cancel = true;
            }
        }

        private void xrSubreport5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_CAPTACION_ESTADO"));

            if (id_estado > 0)
            {
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;

                var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
                var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                query.Sql = "select * from agua.VWR_FOTOGRAFIAS_AGUAS WHERE ID_ESTADO = " + id_estado.ToString();

                dataSource.RebuildResultSchema();
            }
            else
            {
                ((XRSubreport)sender).Visible = false;
                ((XRSubreport)sender).ReportSource.DataSource = null;
                e.Cancel = true;
            }
        }

        private void aguaSuperficial_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrpEncabezado.Image = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/imagenes/cabezoteVisitas.png"));
            xrpUbicacion.Image = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/imagenes/mapa/Captaciones_Superficiales.png"));
        }
    }
}
