namespace SIM.Areas.Tramites.Reportes.Caracteristicas.FuentesFijas
{
    partial class fuentesFijas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fuentesFijas));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrCaractFuentes = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrChimeneas = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource();
            this.prm_idVisita = new DevExpress.XtraReports.Parameters.Parameter();
            this.prm_formulario = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel8});
            this.TopMargin.HeightF = 33F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new System.Drawing.Font("Franklin Gothic Medium", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel8.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(10F, 10F);
            this.xrLabel8.LockedInUserDesigner = true;
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(232.2917F, 23F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseForeColor = false;
            this.xrLabel8.Text = "Fuentes Fijas";
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrCaractFuentes,
            this.xrChimeneas,
            this.xrLabel1});
            this.BottomMargin.HeightF = 90F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrCaractFuentes
            // 
            this.xrCaractFuentes.LocationFloat = new DevExpress.Utils.PointFloat(7.479191F, 64.58334F);
            this.xrCaractFuentes.Name = "xrCaractFuentes";
            this.xrCaractFuentes.ReportSource = new SIM.Areas.Tramites.Reportes.Caracteristicas.FuentesFijas.caracteristicaFuentes();
            this.xrCaractFuentes.Scripts.OnBeforePrint = "xrCaractFuentes_BeforePrint";
            this.xrCaractFuentes.SizeF = new System.Drawing.SizeF(691.8958F, 23F);
            // 
            // xrChimeneas
            // 
            this.xrChimeneas.LocationFloat = new DevExpress.Utils.PointFloat(7.479191F, 41.58332F);
            this.xrChimeneas.Name = "xrChimeneas";
            this.xrChimeneas.ReportSource = new SIM.Areas.Tramites.Reportes.Caracteristicas.FuentesFijas.caracteristicaInfChimeneas();
            this.xrChimeneas.Scripts.OnBeforePrint = "xrChimeneas_BeforePrint";
            this.xrChimeneas.SizeF = new System.Drawing.SizeF(691.8958F, 23F);
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.NOMBRE")});
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(4.479187F, 17.58333F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(100F, 23F);
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "amdb-cluster-scan.areametro.com:1521/pruebas.areametro.com_Connection";
            this.sqlDataSource1.Name = "sqlDataSource1";
            customSqlQuery1.Name = "CustomSqlQuery";
            customSqlQuery1.Sql = "select *\r\n  from control.VWR_FUENTE_FIJA\r\n";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // prm_idVisita
            // 
            this.prm_idVisita.Description = "prm_idVisita";
            this.prm_idVisita.Name = "prm_idVisita";
            this.prm_idVisita.Type = typeof(int);
            this.prm_idVisita.ValueInfo = "0";
            this.prm_idVisita.Visible = false;
            // 
            // prm_formulario
            // 
            this.prm_formulario.Description = "prm_formulario";
            this.prm_formulario.Name = "prm_formulario";
            this.prm_formulario.Type = typeof(int);
            this.prm_formulario.ValueInfo = "0";
            this.prm_formulario.Visible = false;
            // 
            // fuentesFijas
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "CustomSqlQuery";
            this.DataSource = this.sqlDataSource1;
            this.FilterString = "[ID_VISITA] = ?prm_idVisita And [ID_FORMULARIO] = ?prm_formulario";
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 33, 90);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.prm_idVisita,
            this.prm_formulario});
            this.ReportPrintOptions.PrintOnEmptyDataSource = false;
            this.ScriptsSource = resources.GetString("$this.ScriptsSource");
            this.Version = "14.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRSubreport xrCaractFuentes;
        private DevExpress.XtraReports.UI.XRSubreport xrChimeneas;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.Parameters.Parameter prm_idVisita;
        private DevExpress.XtraReports.Parameters.Parameter prm_formulario;
    }
}
