namespace SIM.Areas.ControlVigilancia.reporte.aguas
{
    partial class agua
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(agua));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSubreport9 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport8 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSubreport7 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSubreport6 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport5 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSubreport4 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport3 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport2 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport1 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine8 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.prm_idVisitaAgua = new DevExpress.XtraReports.Parameters.Parameter();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrSubreport9,
            this.xrSubreport8,
            this.xrLabel11,
            this.xrSubreport7,
            this.xrLabel10,
            this.xrSubreport6,
            this.xrSubreport5,
            this.xrLabel7,
            this.xrSubreport4,
            this.xrSubreport3,
            this.xrSubreport2,
            this.xrSubreport1,
            this.xrLabel12,
            this.xrLine8,
            this.xrLabel1});
            this.Detail.HeightF = 368.7501F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Franklin Gothic Medium", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 329.7084F);
            this.xrLabel2.LockedInUserDesigner = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(232.2917F, 23F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseForeColor = false;
            this.xrLabel2.Text = "GESTIÓN RIESGO";
            // 
            // xrSubreport9
            // 
            this.xrSubreport9.LocationFloat = new DevExpress.Utils.PointFloat(1.589457E-05F, 306.7084F);
            this.xrSubreport9.Name = "xrSubreport9";
            this.xrSubreport9.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.quejas();
            this.xrSubreport9.Scripts.OnBeforePrint = "xrSubreport9_BeforePrint";
            this.xrSubreport9.SizeF = new System.Drawing.SizeF(394.7917F, 23F);
            // 
            // xrSubreport8
            // 
            this.xrSubreport8.LocationFloat = new DevExpress.Utils.PointFloat(0F, 283.7084F);
            this.xrSubreport8.Name = "xrSubreport8";
            this.xrSubreport8.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.proyectoContruct();
            this.xrSubreport8.Scripts.OnBeforePrint = "xrSubreport8_BeforePrint";
            this.xrSubreport8.SizeF = new System.Drawing.SizeF(394.7917F, 23F);
            // 
            // xrLabel11
            // 
            this.xrLabel11.Font = new System.Drawing.Font("Franklin Gothic Medium", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel11.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(7.947286E-06F, 214.7084F);
            this.xrLabel11.LockedInUserDesigner = true;
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(232.2917F, 23F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseForeColor = false;
            this.xrLabel11.Text = "FLORA";
            // 
            // xrSubreport7
            // 
            this.xrSubreport7.LocationFloat = new DevExpress.Utils.PointFloat(7.947286E-06F, 191.7084F);
            this.xrSubreport7.Name = "xrSubreport7";
            this.xrSubreport7.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.CDA();
            this.xrSubreport7.Scripts.OnBeforePrint = "xrSubreport7_BeforePrint";
            this.xrSubreport7.SizeF = new System.Drawing.SizeF(394.7917F, 23F);
            // 
            // xrLabel10
            // 
            this.xrLabel10.Font = new System.Drawing.Font("Franklin Gothic Medium", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel10.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(1.589457E-05F, 237.7084F);
            this.xrLabel10.LockedInUserDesigner = true;
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(232.2917F, 23F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseForeColor = false;
            this.xrLabel10.Text = "SUELO";
            // 
            // xrSubreport6
            // 
            this.xrSubreport6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 260.7084F);
            this.xrSubreport6.Name = "xrSubreport6";
            this.xrSubreport6.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.Suelo();
            this.xrSubreport6.Scripts.OnBeforePrint = "xrSubreport5_BeforePrint_1";
            this.xrSubreport6.SizeF = new System.Drawing.SizeF(394.7917F, 23F);
            // 
            // xrSubreport5
            // 
            this.xrSubreport5.LocationFloat = new DevExpress.Utils.PointFloat(7.947286E-06F, 168.7084F);
            this.xrSubreport5.Name = "xrSubreport5";
            this.xrSubreport5.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.fuentesFijas();
            this.xrSubreport5.Scripts.OnBeforePrint = "xrSubreport5_BeforePrint_1";
            this.xrSubreport5.SizeF = new System.Drawing.SizeF(394.7917F, 23F);
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Franklin Gothic Medium", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel7.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(7.947286E-06F, 145.7084F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseForeColor = false;
            this.xrLabel7.Text = "AIRE";
            // 
            // xrSubreport4
            // 
            this.xrSubreport4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 122.7084F);
            this.xrSubreport4.Name = "xrSubreport4";
            this.xrSubreport4.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.Vertimiento();
            this.xrSubreport4.Scripts.OnBeforePrint = "xrSubreport3_BeforePrint_1";
            this.xrSubreport4.SizeF = new System.Drawing.SizeF(394.7917F, 23F);
            // 
            // xrSubreport3
            // 
            this.xrSubreport3.LocationFloat = new DevExpress.Utils.PointFloat(7.947286E-06F, 99.70838F);
            this.xrSubreport3.Name = "xrSubreport3";
            this.xrSubreport3.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.ocupacionCause();
            this.xrSubreport3.Scripts.OnBeforePrint = "xrSubreport3_BeforePrint_1";
            this.xrSubreport3.SizeF = new System.Drawing.SizeF(394.7917F, 23F);
            // 
            // xrSubreport2
            // 
            this.xrSubreport2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 76.7084F);
            this.xrSubreport2.Name = "xrSubreport2";
            this.xrSubreport2.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.subterraneasAgua();
            this.xrSubreport2.Scripts.OnBeforePrint = "xrSubreport1_BeforePrint";
            this.xrSubreport2.SizeF = new System.Drawing.SizeF(394.7917F, 22.99999F);
            // 
            // xrSubreport1
            // 
            this.xrSubreport1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 53.70835F);
            this.xrSubreport1.Name = "xrSubreport1";
            this.xrSubreport1.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.superficialesAgua();
            this.xrSubreport1.Scripts.OnBeforePrint = "xrSubreport1_BeforePrint_1";
            this.xrSubreport1.SizeF = new System.Drawing.SizeF(394.7917F, 23F);
            // 
            // xrLabel12
            // 
            this.xrLabel12.Font = new System.Drawing.Font("Franklin Gothic Medium", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel12.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel12.LockedInUserDesigner = true;
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseForeColor = false;
            this.xrLabel12.Text = "Formulario";
            // 
            // xrLine8
            // 
            this.xrLine8.ForeColor = System.Drawing.Color.DarkGray;
            this.xrLine8.LocationFloat = new DevExpress.Utils.PointFloat(0F, 23.00002F);
            this.xrLine8.Name = "xrLine8";
            this.xrLine8.SizeF = new System.Drawing.SizeF(462.2917F, 7.708324F);
            this.xrLine8.StylePriority.UseForeColor = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Franklin Gothic Medium", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 30.70835F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseForeColor = false;
            this.xrLabel1.Text = "AGUA";
            // 
            // prm_idVisitaAgua
            // 
            this.prm_idVisitaAgua.Description = "Parameter1";
            this.prm_idVisitaAgua.Name = "prm_idVisitaAgua";
            this.prm_idVisitaAgua.Type = typeof(int);
            this.prm_idVisitaAgua.ValueInfo = "0";
            this.prm_idVisitaAgua.Visible = false;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 1.041349F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "172.16.0.100:1521/report_Connection";
            this.sqlDataSource1.Name = "sqlDataSource1";
            customSqlQuery1.Name = "CustomSqlQuery";
            customSqlQuery1.Sql = "select*\r\n  from control.VWR_VISITA ";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // agua
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "CustomSqlQuery";
            this.DataSource = this.sqlDataSource1;
            this.FilterString = "[ID_VISITA] = ?prm_idVisitaAgua";
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 1);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.prm_idVisitaAgua});
            this.ScriptsSource = resources.GetString("$this.ScriptsSource");
            this.Version = "14.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.XRLine xrLine8;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport1;
        private DevExpress.XtraReports.Parameters.Parameter prm_idVisitaAgua;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport2;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport3;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport6;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport7;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport8;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
    }
}
