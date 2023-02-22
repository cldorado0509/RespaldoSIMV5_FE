using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SIM.Areas.ControlVigilancia.reporte.aguas
{
    public partial class caracteristicaVertimiento : DevExpress.XtraReports.UI.XtraReport
    {
        public caracteristicaVertimiento()
        {
            InitializeComponent();
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_caraterist_estado = Convert.ToInt32(GetCurrentColumnValue("ID_CARACTERISTICA_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_caracte_estado"].Value = id_caraterist_estado;

            var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
            var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

            query.Sql = "select * from control.VWR_CARACTERISTICAS_VARIABLES WHERE ID_ESTRUCTURA_ESTADO = " + id_caraterist_estado.ToString(); ;

            dataSource.RebuildResultSchema();
        }
    }
}
