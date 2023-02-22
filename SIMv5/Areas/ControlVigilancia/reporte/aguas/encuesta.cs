using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.DataAccess.Sql;

namespace SIM.Areas.ControlVigilancia.reporte.aguas
{
    public partial class encuesta : DevExpress.XtraReports.UI.XtraReport
    {
        public encuesta()
        {
            InitializeComponent();
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_encuesta = Convert.ToInt32(GetCurrentColumnValue("ID_ENCUESTA"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_encuesta"].Value = id_encuesta;

            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;

            int id_formulario = Convert.ToInt32(GetCurrentColumnValue("ID_FORMULARIO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulrio"].Value = id_formulario;

            var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
            var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

            query.Sql = "select * from control.VWR_ENCUESTAS_PREGUNTAS WHERE ID_ENCUESTA = " + id_encuesta.ToString() + " And ID_FORMULARIO = " + id_formulario.ToString() + " And ID_ESTADO = " + id_estado.ToString();

            dataSource.RebuildResultSchema();
        }
    }
}
