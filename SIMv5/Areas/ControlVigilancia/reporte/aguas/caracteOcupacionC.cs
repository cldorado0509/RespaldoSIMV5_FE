using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SIM.Areas.ControlVigilancia.reporte.aguas
{
    public partial class caracteOcupacionC : DevExpress.XtraReports.UI.XtraReport
    {
        public caracteOcupacionC()
        {
            InitializeComponent();
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_encuesta = Convert.ToInt32(GetCurrentColumnValue("ID_ENCUESTA"));
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_ESTADO"));
            int id_formulario = Convert.ToInt32(GetCurrentColumnValue("ID_FORMULARIO"));

            if (id_estado > 0)
            {
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_encuesta"].Value = id_encuesta;
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;
                ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulrio"].Value = id_formulario;

                var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
                var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                query.Sql = "select * from control.VWR_ENCUESTAS_PREGUNTAS WHERE ID_ENCUESTA = " + id_encuesta.ToString() + " And ID_FORMULARIO = " + id_formulario.ToString() + " And ID_ESTADO = " + id_estado.ToString();

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
