using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SIM.Areas.ControlVigilancia.reporte.aguas
{
    public partial class fotoresiduo : DevExpress.XtraReports.UI.XtraReport
    {
        public fotoresiduo()
        {
            InitializeComponent();
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;

            ((XRSubreport)sender).ReportSource.Parameters["prm_tipo"].Value = 1;

            var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
            var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

            //query.Sql = "select * from control.VWR_FOTOGRAFIAS_RESIDUO WHERE ID_ESTADO = " + id_estado.ToString() + " And TIPO = 1";
            query.Sql = "select * from control.VWR_FOTOGRAFIAS_RESIDUO WHERE ID_ESTADO = " + id_estado.ToString();

            dataSource.RebuildResultSchema();
        }

        private void xrSubreport2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;

            ((XRSubreport)sender).ReportSource.Parameters["prm_tipo"].Value = 2;

            var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
            var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

            //query.Sql = "select * from control.VWR_FOTOGRAFIAS_RESIDUO WHERE ID_ESTADO = " + id_estado.ToString() + " And TIPO = 2";
            query.Sql = "select * from control.VWR_FOTOGRAFIAS_RESIDUO WHERE ID_ESTADO = " + id_estado.ToString();

            dataSource.RebuildResultSchema();
        }
    }
}
