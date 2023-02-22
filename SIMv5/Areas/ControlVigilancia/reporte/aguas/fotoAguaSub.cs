using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SIM.Areas.ControlVigilancia.reporte.aguas
{
    public partial class fotoAguaSub : DevExpress.XtraReports.UI.XtraReport
    {
        public fotoAguaSub()
        {
            InitializeComponent();
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_ESTADO"));

            if (id_estado > 0)
            {
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;
                ((XRSubreport)sender).ReportSource.Parameters["prm_tipo"].Value = 1;

                var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);

                if (dataSource == null)
                {
                    fotoSubterranea reporte = (fotoSubterranea)(((XRSubreport)sender).ReportSource);

                    reporte.DataSource = reporte.sqlDataSource1;
                    dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
                }

                var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                //query.Sql = "select * from agua.VWR_FOTOGRAFIAS_AGUASSUBT WHERE ID_ESTADO = " + id_estado.ToString() + " And TIPO = 1";
                query.Sql = "select * from agua.VWR_FOTOGRAFIAS_AGUASSUBT WHERE ID_ESTADO = " + id_estado.ToString();

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
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_ESTADO"));

            if (id_estado > 0)
            {
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;
                ((XRSubreport)sender).ReportSource.Parameters["prm_tipo"].Value = 2;

                var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);

                if (dataSource == null)
                {
                    fotoSubterranea reporte = (fotoSubterranea)(((XRSubreport)sender).ReportSource);

                    reporte.DataSource = reporte.sqlDataSource1;
                    dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
                }

                var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                //query.Sql = "select * from agua.VWR_FOTOGRAFIAS_AGUASSUBT WHERE ID_ESTADO = " + id_estado.ToString() + " And TIPO = 2";
                query.Sql = "select * from agua.VWR_FOTOGRAFIAS_AGUASSUBT WHERE ID_ESTADO = " + id_estado.ToString();

                dataSource.RebuildResultSchema();
            }
            else
            {
                ((XRSubreport)sender).Visible = false;
                ((XRSubreport)sender).ReportSource.DataSource = null;
                e.Cancel = true;
            }

        }
    }
}
