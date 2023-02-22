using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SIM.Areas.ControlVigilancia.reporte.aguas
{
    public partial class caracteristicaInfChimeneas : DevExpress.XtraReports.UI.XtraReport
    {
        public caracteristicaInfChimeneas()
        {
            InitializeComponent();
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            /*int Codestado = Convert.ToInt32(GetCurrentColumnValue("ID_CARACTERISTICA_ESTADO"));

            ((XRSubreport)sender).ReportSource.Parameters["prm_id_caracte_estado"].Value = Codestado;*/

            int Codestado = Convert.ToInt32(GetCurrentColumnValue("ID_CARACTERISTICA_ESTADO"));

            if (Codestado > 0)
            {
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_caracte_estado"].Value = Codestado;

                var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
                var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                query.Sql = "select * from control.VWR_CARACTERISTICAS_VARIABLES WHERE ID_ESTRUCTURA_ESTADO = " + Codestado.ToString();

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
