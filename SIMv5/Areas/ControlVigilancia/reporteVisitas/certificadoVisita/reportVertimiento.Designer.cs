﻿namespace SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita
{
    partial class reportVertimiento
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(reportVertimiento));
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrPageBreak1 = new DevExpress.XtraReports.UI.XRPageBreak();
            this.xrRichText1 = new DevExpress.XtraReports.UI.XRRichText();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrpUbicacion = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrSubreport1 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport2 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport5 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrpEncabezado = new DevExpress.XtraReports.UI.XRPictureBox();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.prm_radicador = new DevExpress.XtraReports.Parameters.Parameter();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.prm_idVisita = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageBreak1,
            this.xrRichText1,
            this.xrLabel19,
            this.xrpUbicacion,
            this.xrSubreport1,
            this.xrSubreport2,
            this.xrSubreport5,
            this.xrPictureBox2});
            this.Detail.HeightF = 518.75F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageBreak1
            // 
            this.xrPageBreak1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 509.375F);
            this.xrPageBreak1.Name = "xrPageBreak1";
            // 
            // xrRichText1
            // 
            this.xrRichText1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrRichText1.ForeColor = System.Drawing.Color.Black;
            this.xrRichText1.LocationFloat = new DevExpress.Utils.PointFloat(48.39589F, 35.77042F);
            this.xrRichText1.Name = "xrRichText1";
            this.xrRichText1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrRichText1.SerializableRtfString = resources.GetString("xrRichText1.SerializableRtfString");
            this.xrRichText1.SizeF = new System.Drawing.SizeF(757.3749F, 22.99994F);
            this.xrRichText1.StylePriority.UseFont = false;
            this.xrRichText1.StylePriority.UseForeColor = false;
            // 
            // xrLabel19
            // 
            this.xrLabel19.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel19.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(48.39589F, 0F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(757.3748F, 23F);
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.StylePriority.UseForeColor = false;
            this.xrLabel19.Text = "Mapa";
            // 
            // xrpUbicacion
            // 
            this.xrpUbicacion.BackColor = System.Drawing.Color.Transparent;
            this.xrpUbicacion.BorderColor = System.Drawing.Color.Transparent;
            this.xrpUbicacion.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrpUbicacion.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrpUbicacion.BorderWidth = 2F;
            this.xrpUbicacion.ImageUrl = "~/Content/imagenes/mapa/Vertimientos.png";
            this.xrpUbicacion.LocationFloat = new DevExpress.Utils.PointFloat(404.6461F, 198.9787F);
            this.xrpUbicacion.Name = "xrpUbicacion";
            this.xrpUbicacion.SizeF = new System.Drawing.SizeF(24.99478F, 27.07813F);
            this.xrpUbicacion.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.xrpUbicacion.StylePriority.UseBackColor = false;
            this.xrpUbicacion.StylePriority.UseBorderColor = false;
            this.xrpUbicacion.StylePriority.UseBorderDashStyle = false;
            this.xrpUbicacion.StylePriority.UseBorders = false;
            this.xrpUbicacion.StylePriority.UseBorderWidth = false;
            // 
            // xrSubreport1
            // 
            this.xrSubreport1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 400.0833F);
            this.xrSubreport1.Name = "xrSubreport1";
            this.xrSubreport1.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.caracteristicaVertimiento();
            this.xrSubreport1.SizeF = new System.Drawing.SizeF(849.9999F, 23F);
            this.xrSubreport1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrSubreport1_BeforePrint);
            // 
            // xrSubreport2
            // 
            this.xrSubreport2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 437.6666F);
            this.xrSubreport2.Name = "xrSubreport2";
            this.xrSubreport2.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.encuesta();
            this.xrSubreport2.SizeF = new System.Drawing.SizeF(849.9999F, 23F);
            this.xrSubreport2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrSubreport2_BeforePrint);
            // 
            // xrSubreport5
            // 
            this.xrSubreport5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 474.2083F);
            this.xrSubreport5.Name = "xrSubreport5";
            this.xrSubreport5.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.fotoVertimiento();
            this.xrSubreport5.SizeF = new System.Drawing.SizeF(850F, 23F);
            this.xrSubreport5.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrSubreport5_BeforePrint);
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.xrPictureBox2.BorderColor = System.Drawing.Color.Transparent;
            this.xrPictureBox2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrPictureBox2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPictureBox2.BorderWidth = 2F;
            this.xrPictureBox2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("ImageUrl", null, "CustomSqlQuery.URLMAPA")});
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(48.39589F, 58.77037F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(750F, 315.6674F);
            this.xrPictureBox2.StylePriority.UseBackColor = false;
            this.xrPictureBox2.StylePriority.UseBorderColor = false;
            this.xrPictureBox2.StylePriority.UseBorderDashStyle = false;
            this.xrPictureBox2.StylePriority.UseBorders = false;
            this.xrPictureBox2.StylePriority.UseBorderWidth = false;
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrpEncabezado});
            this.TopMargin.HeightF = 123.9583F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel2
            // 
            this.xrLabel2.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel2.Font = new System.Drawing.Font("Calibri", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.ForeColor = System.Drawing.Color.White;
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(105.2083F, 0F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(602.2532F, 80.62164F);
            this.xrLabel2.StylePriority.UseBackColor = false;
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseForeColor = false;
            this.xrLabel2.StylePriority.UsePadding = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Vertimiento [NOMBRE]";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
            // 
            // xrpEncabezado
            // 
            this.xrpEncabezado.BorderWidth = 0F;
            this.xrpEncabezado.ImageUrl = "~/Content/imagenes/cabezoteVisitas.png";
            this.xrpEncabezado.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrpEncabezado.Name = "xrpEncabezado";
            this.xrpEncabezado.SizeF = new System.Drawing.SizeF(850F, 100F);
            this.xrpEncabezado.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.xrpEncabezado.StylePriority.UseBorderWidth = false;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 1.041667F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // prm_radicador
            // 
            this.prm_radicador.Description = "Parameter1";
            this.prm_radicador.Name = "prm_radicador";
            this.prm_radicador.Visible = false;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2,
            this.xrPageInfo1});
            this.PageFooter.HeightF = 25.00001F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo2.ForeColor = System.Drawing.Color.Teal;
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(527F, 2.000014F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(313F, 23F);
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseForeColor = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrPageInfo2.TextFormatString = "Página {0} de {1}";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo1.ForeColor = System.Drawing.Color.Teal;
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(313F, 23F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseForeColor = false;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "172.16.0.100:1521/report_Connection";
            this.sqlDataSource1.Name = "sqlDataSource1";
            customSqlQuery1.Name = "CustomSqlQuery";
            customSqlQuery1.Sql = "select *\r\n  from AGUA.vwr_vertimiento\r\n";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // prm_idVisita
            // 
            this.prm_idVisita.Description = "Parameter1";
            this.prm_idVisita.Name = "prm_idVisita";
            this.prm_idVisita.Type = typeof(int);
            this.prm_idVisita.ValueInfo = "130";
            this.prm_idVisita.Visible = false;
            // 
            // reportVertimiento
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "CustomSqlQuery";
            this.DataSource = this.sqlDataSource1;
            this.FilterString = "[ID_VISITA] = ?prm_idVisita";
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 124, 1);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.prm_radicador,
            this.prm_idVisita});
            this.Version = "18.1";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.reportVertimiento_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport1;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport2;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport5;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo2;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.Parameters.Parameter prm_radicador;
        private DevExpress.XtraReports.Parameters.Parameter prm_idVisita;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox2;
        private DevExpress.XtraReports.UI.XRRichText xrRichText1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel19;
        private DevExpress.XtraReports.UI.XRPictureBox xrpUbicacion;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRPictureBox xrpEncabezado;
        private DevExpress.XtraReports.UI.XRPageBreak xrPageBreak1;
    }
}