using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita
{
    public partial class ResiduoPeligroso : DevExpress.XtraReports.UI.XtraReport
    {
        public ResiduoPeligroso()
        {
            InitializeComponent();
        }

        private void xrSubreport2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_RESIDUO_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;


            ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = 6;
            ((XRSubreport)sender).ReportSource.Parameters["prm_grupo"].Value = 0;

            var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
            var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

            query.Sql = "select * from control.VWR_CARATERISTICAGRUPO WHERE ID_ESTADO = " + id_estado.ToString() + " And GRUPO_FORMULARIO = 0 And ID_FORMULARIO = 6";

            dataSource.RebuildResultSchema();
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_RESIDUO_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;


            ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = 6;
            ((XRSubreport)sender).ReportSource.Parameters["prm_grupo"].Value = 1;

            var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
            var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

            query.Sql = "select * from control.VWR_CARATERISTICAGRUPO WHERE GRUPO_FORMULARIO = 1 And ID_ESTADO = " + id_estado + " And ID_FORMULARIO = 6";

            dataSource.RebuildResultSchema();
        }

        private void xrSubreport3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_RESIDUO_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;


            ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = 6;
            ((XRSubreport)sender).ReportSource.Parameters["prm_grupo"].Value = 2;

            var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
            var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

            query.Sql = "select * from control.VWR_CARATERISTICAGRUPO WHERE ID_ESTADO = " + id_estado.ToString() + " And GRUPO_FORMULARIO = 2 And ID_FORMULARIO = 6";

            dataSource.RebuildResultSchema();
        }

        private void xrSubreport4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_RESIDUO_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;


            ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = 6;
            ((XRSubreport)sender).ReportSource.Parameters["prm_grupo"].Value = 3;
        }

        private void xrSubreport5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_RESIDUO_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;


            ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = 6;
            ((XRSubreport)sender).ReportSource.Parameters["prm_grupo"].Value = 5;

            var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
            var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

            query.Sql = "select * from control.VWR_CARATERISTICAGRUPO WHERE ID_ESTADO = " + id_estado.ToString() + " And GRUPO_FORMULARIO = 5 And ID_FORMULARIO = 6";

            dataSource.RebuildResultSchema();
        }

        private void xrSubreport6_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_RESIDUO_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;


            ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = 6;
            ((XRSubreport)sender).ReportSource.Parameters["prm_grupo"].Value = 6;

            var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
            var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

            query.Sql = "select * from control.VWR_CARATERISTICAGRUPO WHERE ID_ESTADO = " + id_estado.ToString() + " And GRUPO_FORMULARIO = 6 And ID_FORMULARIO = 6";

            dataSource.RebuildResultSchema();
        }

        private void xrSubreport7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_RESIDUO_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;


            ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = 6;
            ((XRSubreport)sender).ReportSource.Parameters["prm_grupo"].Value = 9;

            var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
            var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

            query.Sql = "select * from control.VWR_CARATERISTICAGRUPO WHERE ID_ESTADO = " + id_estado.ToString() + " And GRUPO_FORMULARIO = 9 And ID_FORMULARIO = 6";

            dataSource.RebuildResultSchema();
        }

        private void xrSubreport8_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_RESIDUO_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;


            ((XRSubreport)sender).ReportSource.Parameters["prm_id_formulario"].Value = 6;
            ((XRSubreport)sender).ReportSource.Parameters["prm_grupo"].Value = 10;

            var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
            var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

            query.Sql = "select * from control.VWR_CARATERISTICAGRUPO WHERE ID_ESTADO = " + id_estado.ToString() + " And GRUPO_FORMULARIO = 10 And ID_FORMULARIO = 6";

            dataSource.RebuildResultSchema();
        }

        private void xrSubreport9_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int id_estado = Convert.ToInt32(GetCurrentColumnValue("ID_RESIDUO_ESTADO"));
            ((XRSubreport)sender).ReportSource.Parameters["prm_id_estado"].Value = id_estado;

            var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)(((XRSubreport)sender).ReportSource.DataSource);
            var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

            query.Sql = "select * from control.VWR_FOTOGRAFIAS_RESIDUO WHERE ID_ESTADO = " + id_estado.ToString();

            dataSource.RebuildResultSchema();
        }

        private void ResiduoPeligroso_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrpEncabezado.Image = null;
            xrpUbicacion.Image = null;

            xrpEncabezado.Image = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/imagenes/cabezoteVisitas.png"));
            xrpUbicacion.Image = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/imagenes/mapa/Rediduos_Peligros.png"));
        }

        private void ResiduoPeligroso_AfterPrint(object sender, EventArgs e)
        {

        }

        private void xrMapa_AfterPrint(object sender, EventArgs e)
        {

        }

        private void xrMapa_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (xrMapa.Image != null)
            {
                var width = xrMapa.Image.Width;
                var height = xrMapa.Image.Height;

                var image1 = xrMapa.ToImage();
                var image2 = xrpUbicacion.ToImage();

                xrMapa.Image = null;

                Graphics g = Graphics.FromImage(image1);

                //g.DrawLine(Pens.Black, new Point(0, 0), new Point(200, 200));
                g.DrawImage(image2, new Point(width - width / 2, height - height / 2 - image2.Height));

                xrMapa.Image = image1;
            }
        }
    }
}
