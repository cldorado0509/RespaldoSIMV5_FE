using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.DataAccess.Sql;

namespace SIM.Areas.ControlVigilancia.reporte.aguas
{
    public partial class usosSubreport : DevExpress.XtraReports.UI.XtraReport
    {
        public usosSubreport()
        {
            InitializeComponent();
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_uso_estado = Convert.ToInt32(GetCurrentColumnValue("ID_CAPTACION_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado_uso"].Value = id_uso_estado;

            int id_uso = Convert.ToInt32(GetCurrentColumnValue("ID_USO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_uso"].Value = id_uso;

            var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
            var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

            query.Sql = "select * from agua.VWR_USOS_VARIABLES WHERE ID_USO = " + id_uso.ToString() + " And ID_CAPTACION_ESTADO = " + id_uso_estado.ToString();

            dataSource.RebuildResultSchema();
        }
    }
}
