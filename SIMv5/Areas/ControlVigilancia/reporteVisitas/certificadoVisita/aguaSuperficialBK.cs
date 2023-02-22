using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Linq;
using SIM.Areas.ControlVigilancia.Models;


namespace SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita
{
    public partial class aguaSuperficial : DevExpress.XtraReports.UI.XtraReport
    {
        private DataSet datos = new DataSet();
        private OracleConnection db = new OracleConnection("data source=amdb-cluster-scan.areametro.com:1521/amva.areametro.com;password=simedicion;persist security info=True;user id=sim_edicion;");

        public aguaSuperficial(int idActuacion)
        {
            OracleDataAdapter da;

            InitializeComponent();

            db.Open();

            DataTable principal = new DataTable();
            DataTable usos = new DataTable();
            DataTable variablesUsos = new DataTable();
            DataTable caracteristicas = new DataTable();

            da = new OracleDataAdapter("select * from agua.VWR_CAPTACION where ID_VISITA = " + idActuacion.ToString() + " AND IDTIPOC=1", db);
            da.Fill(principal);
            principal.TableName = "Principal";

            da = new OracleDataAdapter("select * from agua.VWR_USOS where ID_CAPTACION_ESTADO IN (" + Utilidades.Data.ObtenerListaCampo(principal, "ID_CAPTACION_ESTADO") + ") AND TIPO = 2", db);
            da.Fill(usos);
            usos.TableName = "Usos";

            da = new OracleDataAdapter("select * from agua.VWR_USOS_VARIABLES where ID_USO IN (" + Utilidades.Data.ObtenerListaCampo(usos, "ID_USO") + ")", db);
            da.Fill(variablesUsos);
            usos.TableName = "UsosVariables";

            da = new OracleDataAdapter("select * from agua.VWR_CAPTACION where ID_VISITA = " + idActuacion.ToString() + " AND IDTIPOC=1", db);
            da.Fill(caracteristicas);
            caracteristicas.TableName = "Caracteristicas";

            datos.Tables.AddRange(new DataTable[] { principal, usos, caracteristicas });

            this.DataSource = datos.Tables["Principal"];
        }

        private void xsrUsos_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int idCaptacionEstado = Convert.ToInt32(GetCurrentColumnValue("ID_CAPTACION_ESTADO"));

            if (idCaptacionEstado > 0)
            {
                ((XRSubreport)sender).ReportSource.Tag = datos;
                ((XRSubreport)sender).ReportSource.DataSource = (new DataView(datos.Tables["Usos"], "ID_CAPTACION_ESTADO = " + idCaptacionEstado.ToString(), "", DataViewRowState.CurrentRows));

                //((XRSubreport)sender).ReportSource.Parameters["prm_id_estadoUso"].Value = id_estado;
                //((XRSubreport)sender).ReportSource.Parameters["prm_tipo"].Value = 2;
            }
            else
            {
                ((XRSubreport)sender).Visible = false;
            }
        }

        private void xsrCaracteristicas_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int idCaptacionEstado = Convert.ToInt32(GetCurrentColumnValue("ID_CAPTACION_ESTADO"));

            if (idCaptacionEstado > 0)
            {
                ((XRSubreport)sender).ReportSource.Tag = datos;
                ((XRSubreport)sender).ReportSource.DataSource = (new DataView(datos.Tables["Usos"], "ID_CAPTACION_ESTADO = " + idCaptacionEstado.ToString(), "", DataViewRowState.CurrentRows));

                //((XRSubreport)sender).ReportSource.Parameters["prm_id_estadoUso"].Value = id_estado;
                //((XRSubreport)sender).ReportSource.Parameters["prm_tipo"].Value = 2;
            }
            else
            {
                ((XRSubreport)sender).Visible = false;
            }
        }
    }
}
