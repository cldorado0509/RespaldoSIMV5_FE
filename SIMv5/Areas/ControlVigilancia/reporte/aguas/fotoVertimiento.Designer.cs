namespace SIM.Areas.ControlVigilancia.reporte.aguas
{
    partial class fotoVertimiento
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fotoVertimiento));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSubreport1 = new DevExpress.XtraReports.UI.XRSubreport();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource();
            this.prm_id_estado = new DevExpress.XtraReports.Parameters.Parameter();
            this.fotoVertSub1 = new SIM.Areas.ControlVigilancia.reporte.aguas.fotoVertSub();
            ((System.ComponentModel.ISupportInitialize)(this.fotoVertSub1)).BeginInit();
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
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 4.166667F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel24,
            this.xrSubreport1});
            this.ReportHeader.HeightF = 63.66666F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel24
            // 
            this.xrLabel24.Font = new System.Drawing.Font("Calibri", 17.25F, System.Drawing.FontStyle.Bold);
            this.xrLabel24.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(50F, 0F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(370.8333F, 23F);
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.StylePriority.UseForeColor = false;
            this.xrLabel24.Text = "Fotografías Vertimientos";
            // 
            // xrSubreport1
            // 
            this.xrSubreport1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 34.49995F);
            this.xrSubreport1.Name = "xrSubreport1";
            this.xrSubreport1.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.fotoVertSub();
            this.xrSubreport1.Scripts.OnBeforePrint = "xrSubreport1_BeforePrint";
            this.xrSubreport1.SizeF = new System.Drawing.SizeF(849F, 20.54168F);
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "172.16.0.100:1521/report_Connection";
            this.sqlDataSource1.Name = "sqlDataSource1";
            customSqlQuery1.Name = "CustomSqlQuery";
            customSqlQuery1.Sql = "select * from agua.VWR_FOTOGRAFIAS_VERTIMIENTO";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // prm_id_estado
            // 
            this.prm_id_estado.Description = "Parameter1";
            this.prm_id_estado.Name = "prm_id_estado";
            this.prm_id_estado.Type = typeof(int);
            this.prm_id_estado.ValueInfo = "0";
            this.prm_id_estado.Visible = false;
            // 
            // fotoVertSub1
            // 
            this.fotoVertSub1.DataMember = "CustomSqlQuery";
            this.fotoVertSub1.FilterString = "[ID_ESTADO_CAPTACION] = ?prm_id_estado And [TIPO] = ?prm_tipo";
            this.fotoVertSub1.Margins = new System.Drawing.Printing.Margins(0, 1, 0, 2);
            this.fotoVertSub1.Name = "fotoVertSub1";
            this.fotoVertSub1.PageHeight = 1100;
            this.fotoVertSub1.PageWidth = 850;
            this.fotoVertSub1.Version = "14.2";
            // 
            // fotoVertimiento
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "CustomSqlQuery";
            this.DataSource = this.sqlDataSource1;
            this.FilterString = "[ID_ESTADO] = ?prm_id_estado";
            this.Margins = new System.Drawing.Printing.Margins(0, 1, 0, 4);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.prm_id_estado});
            this.ReportPrintOptions.PrintOnEmptyDataSource = false;
            this.ScriptsSource = resources.GetString("$this.ScriptsSource");
            this.Version = "14.2";
            ((System.ComponentModel.ISupportInitialize)(this.fotoVertSub1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport1;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.Parameters.Parameter prm_id_estado;
        private fotoVertSub fotoVertSub1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel24;
    }
}
