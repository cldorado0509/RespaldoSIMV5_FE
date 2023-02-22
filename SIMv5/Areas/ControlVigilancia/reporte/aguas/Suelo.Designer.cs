namespace SIM.Areas.ControlVigilancia.reporte.aguas
{
    partial class Suelo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Suelo));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLine9 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSubreport9 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport8 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport7 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport6 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport5 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport4 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport3 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport1 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport2 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource();
            this.prm_formulario = new DevExpress.XtraReports.Parameters.Parameter();
            this.prm_idVisita = new DevExpress.XtraReports.Parameters.Parameter();
            this.fotoresiduo1 = new SIM.Areas.ControlVigilancia.reporte.aguas.fotoresiduo();
            ((System.ComponentModel.ISupportInitialize)(this.fotoresiduo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine9,
            this.xrLabel24,
            this.xrSubreport9,
            this.xrSubreport8,
            this.xrSubreport7,
            this.xrSubreport6,
            this.xrSubreport5,
            this.xrSubreport4,
            this.xrSubreport3,
            this.xrSubreport1,
            this.xrSubreport2,
            this.xrLabel1});
            this.Detail.HeightF = 263.5417F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLine9
            // 
            this.xrLine9.ForeColor = System.Drawing.Color.DarkGray;
            this.xrLine9.LocationFloat = new DevExpress.Utils.PointFloat(0F, 229.9999F);
            this.xrLine9.Name = "xrLine9";
            this.xrLine9.SizeF = new System.Drawing.SizeF(633.9999F, 6F);
            this.xrLine9.StylePriority.UseForeColor = false;
            // 
            // xrLabel24
            // 
            this.xrLabel24.Font = new System.Drawing.Font("Franklin Gothic Medium", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel24.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(0F, 207F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(370.8333F, 23F);
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.StylePriority.UseForeColor = false;
            this.xrLabel24.Text = "Fotografías residuo";
            // 
            // xrSubreport9
            // 
            this.xrSubreport9.LocationFloat = new DevExpress.Utils.PointFloat(0F, 236.8958F);
            this.xrSubreport9.Name = "xrSubreport9";
            this.xrSubreport9.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.fotoresiduo();
            this.xrSubreport9.Scripts.OnBeforePrint = "xrSubreport9_BeforePrint";
            this.xrSubreport9.SizeF = new System.Drawing.SizeF(641.0416F, 23F);
            // 
            // xrSubreport8
            // 
            this.xrSubreport8.LocationFloat = new DevExpress.Utils.PointFloat(2.384186E-05F, 184F);
            this.xrSubreport8.Name = "xrSubreport8";
            this.xrSubreport8.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.caracteristicaRH1();
            this.xrSubreport8.Scripts.OnBeforePrint = "xrSubreport8_BeforePrint";
            this.xrSubreport8.SizeF = new System.Drawing.SizeF(641.0416F, 23F);
            // 
            // xrSubreport7
            // 
            this.xrSubreport7.LocationFloat = new DevExpress.Utils.PointFloat(2.384186E-05F, 161F);
            this.xrSubreport7.Name = "xrSubreport7";
            this.xrSubreport7.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.caractRespelRua();
            this.xrSubreport7.Scripts.OnBeforePrint = "xrSubreport7_BeforePrint";
            this.xrSubreport7.SizeF = new System.Drawing.SizeF(641.0416F, 23F);
            // 
            // xrSubreport6
            // 
            this.xrSubreport6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 138F);
            this.xrSubreport6.Name = "xrSubreport6";
            this.xrSubreport6.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.caracteristicaSoporte();
            this.xrSubreport6.Scripts.OnBeforePrint = "xrSubreport6_BeforePrint";
            this.xrSubreport6.SizeF = new System.Drawing.SizeF(641.0416F, 23F);
            // 
            // xrSubreport5
            // 
            this.xrSubreport5.LocationFloat = new DevExpress.Utils.PointFloat(2.384186E-05F, 115F);
            this.xrSubreport5.Name = "xrSubreport5";
            this.xrSubreport5.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.caracteristicaDispFinal();
            this.xrSubreport5.Scripts.OnBeforePrint = "xrSubreport5_BeforePrint";
            this.xrSubreport5.SizeF = new System.Drawing.SizeF(641.0416F, 23F);
            // 
            // xrSubreport4
            // 
            this.xrSubreport4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 92.00001F);
            this.xrSubreport4.Name = "xrSubreport4";
            this.xrSubreport4.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.carateristicaAprovechamiento();
            this.xrSubreport4.Scripts.OnBeforePrint = "xrSubreport4_BeforePrint";
            this.xrSubreport4.SizeF = new System.Drawing.SizeF(641.0416F, 23F);
            // 
            // xrSubreport3
            // 
            this.xrSubreport3.LocationFloat = new DevExpress.Utils.PointFloat(2.384186E-05F, 69.00001F);
            this.xrSubreport3.Name = "xrSubreport3";
            this.xrSubreport3.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.caracteristicaAlmacenamiento();
            this.xrSubreport3.Scripts.OnBeforePrint = "xrSubreport3_BeforePrint";
            this.xrSubreport3.SizeF = new System.Drawing.SizeF(641.0416F, 23F);
            // 
            // xrSubreport1
            // 
            this.xrSubreport1.LocationFloat = new DevExpress.Utils.PointFloat(2.384186E-05F, 46F);
            this.xrSubreport1.Name = "xrSubreport1";
            this.xrSubreport1.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.caracteristicaSeparacion();
            this.xrSubreport1.Scripts.OnBeforePrint = "xrSubreport1_BeforePrint";
            this.xrSubreport1.SizeF = new System.Drawing.SizeF(641.0416F, 23F);
            // 
            // xrSubreport2
            // 
            this.xrSubreport2.LocationFloat = new DevExpress.Utils.PointFloat(2.384186E-05F, 23F);
            this.xrSubreport2.Name = "xrSubreport2";
            this.xrSubreport2.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.caracteristicaGeneral();
            this.xrSubreport2.Scripts.OnBeforePrint = "xrSubreport2_BeforePrint";
            this.xrSubreport2.SizeF = new System.Drawing.SizeF(641.0416F, 23F);
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.NOMBRE")});
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(100F, 23F);
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel13});
            this.TopMargin.HeightF = 23.95833F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel13
            // 
            this.xrLabel13.Font = new System.Drawing.Font("Franklin Gothic Medium", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel13.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel13.LockedInUserDesigner = true;
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(232.2917F, 23F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseForeColor = false;
            this.xrLabel13.Text = "Residuo Peligroso";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "172.16.0.100:1521/report_Connection";
            this.sqlDataSource1.Name = "sqlDataSource1";
            customSqlQuery1.Name = "CustomSqlQuery";
            customSqlQuery1.Sql = "select * from control.VWR_RESIDUO";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // prm_formulario
            // 
            this.prm_formulario.Description = "Parameter1";
            this.prm_formulario.Name = "prm_formulario";
            this.prm_formulario.Type = typeof(int);
            this.prm_formulario.ValueInfo = "0";
            this.prm_formulario.Visible = false;
            // 
            // prm_idVisita
            // 
            this.prm_idVisita.Description = "Parameter1";
            this.prm_idVisita.Name = "prm_idVisita";
            this.prm_idVisita.Type = typeof(int);
            this.prm_idVisita.ValueInfo = "0";
            this.prm_idVisita.Visible = false;
            // 
            // fotoresiduo1
            // 
            this.fotoresiduo1.DataMember = "CustomSqlQuery";
            this.fotoresiduo1.FilterString = "[ID_ESTADO_CAPTACION] = ?prm_id_estado";
            this.fotoresiduo1.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.fotoresiduo1.Name = "fotoresiduo1";
            this.fotoresiduo1.PageHeight = 1100;
            this.fotoresiduo1.PageWidth = 850;
            this.fotoresiduo1.ScriptsSource = resources.GetString("fotoresiduo1.ScriptsSource");
            this.fotoresiduo1.Version = "14.2";
            // 
            // Suelo
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "CustomSqlQuery";
            this.DataSource = this.sqlDataSource1;
            this.FilterString = "[ID_VISITA] = ?prm_idVisita";
            this.Margins = new System.Drawing.Printing.Margins(0, 1, 24, 0);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.prm_formulario,
            this.prm_idVisita});
            this.ReportPrintOptions.PrintOnEmptyDataSource = false;
            this.ScriptsSource = resources.GetString("$this.ScriptsSource");
            this.Version = "14.2";
            ((System.ComponentModel.ISupportInitialize)(this.fotoresiduo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.Parameters.Parameter prm_formulario;
        private DevExpress.XtraReports.Parameters.Parameter prm_idVisita;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport8;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport7;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport6;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport5;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport4;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport3;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport1;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport2;
        private DevExpress.XtraReports.UI.XRLine xrLine9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel24;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport9;
        private fotoresiduo fotoresiduo1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
    }
}
