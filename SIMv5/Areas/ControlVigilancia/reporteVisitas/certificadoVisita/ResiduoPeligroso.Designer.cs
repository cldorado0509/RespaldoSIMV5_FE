namespace SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita
{
    using System;
    using System.Drawing;
    using System.Collections;
    using System.ComponentModel;
    using DevExpress.XtraReports.UI;
    partial class ResiduoPeligroso
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        /// using System;
        /// 

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResiduoPeligroso));
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrpUbicacion = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrRichText1 = new DevExpress.XtraReports.UI.XRRichText();
            this.xrPageBreak1 = new DevExpress.XtraReports.UI.XRPageBreak();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrMapa = new DevExpress.XtraReports.UI.XRPictureBox();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrpEncabezado = new DevExpress.XtraReports.UI.XRPictureBox();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.prm_radicador = new DevExpress.XtraReports.Parameters.Parameter();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.prm_idVisita = new DevExpress.XtraReports.Parameters.Parameter();
            this.prm_formulario = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrSubreport2 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport1 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport3 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport4 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport5 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport6 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport7 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport8 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrSubreport9 = new DevExpress.XtraReports.UI.XRSubreport();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrpUbicacion,
            this.xrRichText1,
            this.xrPageBreak1,
            this.xrLabel19,
            this.xrSubreport2,
            this.xrSubreport1,
            this.xrSubreport3,
            this.xrSubreport4,
            this.xrSubreport5,
            this.xrSubreport6,
            this.xrSubreport7,
            this.xrSubreport8,
            this.xrSubreport9,
            this.xrMapa});
            this.Detail.HeightF = 727.0833F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrpUbicacion
            // 
            this.xrpUbicacion.BackColor = System.Drawing.Color.Transparent;
            this.xrpUbicacion.BorderColor = System.Drawing.Color.Transparent;
            this.xrpUbicacion.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrpUbicacion.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrpUbicacion.BorderWidth = 2F;
            this.xrpUbicacion.ImageUrl = "~/Content/imagenes/mapa/Rediduos_Peligros.png";
            this.xrpUbicacion.LocationFloat = new DevExpress.Utils.PointFloat(410.48F, 201.71F);
            this.xrpUbicacion.Name = "xrpUbicacion";
            this.xrpUbicacion.SizeF = new System.Drawing.SizeF(24.99478F, 27.07813F);
            this.xrpUbicacion.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.xrpUbicacion.StylePriority.UseBackColor = false;
            this.xrpUbicacion.StylePriority.UseBorderColor = false;
            this.xrpUbicacion.StylePriority.UseBorderDashStyle = false;
            this.xrpUbicacion.StylePriority.UseBorders = false;
            this.xrpUbicacion.StylePriority.UseBorderWidth = false;
            this.xrpUbicacion.Visible = false;
            // 
            // xrRichText1
            // 
            this.xrRichText1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrRichText1.ForeColor = System.Drawing.Color.Black;
            this.xrRichText1.LocationFloat = new DevExpress.Utils.PointFloat(53.18747F, 34.83302F);
            this.xrRichText1.Name = "xrRichText1";
            this.xrRichText1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrRichText1.SerializableRtfString = resources.GetString("xrRichText1.SerializableRtfString");
            this.xrRichText1.SizeF = new System.Drawing.SizeF(757.3749F, 22.99994F);
            this.xrRichText1.StylePriority.UseFont = false;
            this.xrRichText1.StylePriority.UseForeColor = false;
            // 
            // xrPageBreak1
            // 
            this.xrPageBreak1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 722.375F);
            this.xrPageBreak1.Name = "xrPageBreak1";
            // 
            // xrLabel19
            // 
            this.xrLabel19.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel19.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(51.02088F, 0F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(757.3748F, 23F);
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.StylePriority.UseForeColor = false;
            this.xrLabel19.Text = "Mapa";
            // 
            // xrMapa
            // 
            this.xrMapa.BackColor = System.Drawing.Color.Transparent;
            this.xrMapa.BorderColor = System.Drawing.Color.Transparent;
            this.xrMapa.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrMapa.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrMapa.BorderWidth = 2F;
            this.xrMapa.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("ImageUrl", null, "CustomSqlQuery.URLMAPA")});
            this.xrMapa.LocationFloat = new DevExpress.Utils.PointFloat(53.18747F, 70.87428F);
            this.xrMapa.Name = "xrMapa";
            this.xrMapa.SizeF = new System.Drawing.SizeF(750F, 315.6674F);
            this.xrMapa.StylePriority.UseBackColor = false;
            this.xrMapa.StylePriority.UseBorderColor = false;
            this.xrMapa.StylePriority.UseBorderDashStyle = false;
            this.xrMapa.StylePriority.UseBorders = false;
            this.xrMapa.StylePriority.UseBorderWidth = false;
            this.xrMapa.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrMapa_BeforePrint);
            this.xrMapa.AfterPrint += new System.EventHandler(this.xrMapa_AfterPrint);
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrpEncabezado});
            this.TopMargin.HeightF = 124F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel3
            // 
            this.xrLabel3.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel3.Font = new System.Drawing.Font("Calibri", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.ForeColor = System.Drawing.Color.White;
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(103.7083F, 0F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(602.2532F, 80.62164F);
            this.xrLabel3.StylePriority.UseBackColor = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseForeColor = false;
            this.xrLabel3.StylePriority.UsePadding = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Residuos [NOMBRE]";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
            // 
            // xrpEncabezado
            // 
            this.xrpEncabezado.BorderWidth = 0F;
            this.xrpEncabezado.ImageUrl = "~/Content/imagenes/cabezoteVisitas.png";
            this.xrpEncabezado.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrpEncabezado.Name = "xrpEncabezado";
            this.xrpEncabezado.SizeF = new System.Drawing.SizeF(849F, 100F);
            this.xrpEncabezado.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.xrpEncabezado.StylePriority.UseBorderWidth = false;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
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
            this.xrPageInfo1,
            this.xrPageInfo2});
            this.PageFooter.HeightF = 28.125F;
            this.PageFooter.Name = "PageFooter";
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
            // xrPageInfo2
            // 
            this.xrPageInfo2.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo2.ForeColor = System.Drawing.Color.Teal;
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(313.4577F, 0F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(313F, 23F);
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseForeColor = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrPageInfo2.TextFormatString = "Página {0} de {1}";
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "172.16.0.100:1521/report_Connection";
            this.sqlDataSource1.Name = "sqlDataSource1";
            customSqlQuery1.Name = "CustomSqlQuery";
            customSqlQuery1.Sql = "SELECT * FROM CONTROL.VWR_RESIDUO";
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
            // prm_formulario
            // 
            this.prm_formulario.Description = "Parameter1";
            this.prm_formulario.Name = "prm_formulario";
            this.prm_formulario.Type = typeof(int);
            this.prm_formulario.ValueInfo = "6";
            this.prm_formulario.Visible = false;
            // 
            // xrSubreport2
            // 
            this.xrSubreport2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 397.9999F);
            this.xrSubreport2.Name = "xrSubreport2";
            this.xrSubreport2.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.caracteristicaGeneral();
            this.xrSubreport2.Scripts.OnBeforePrint = "xrSubreport2_BeforePrint";
            this.xrSubreport2.SizeF = new System.Drawing.SizeF(839F, 23.00003F);
            this.xrSubreport2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrSubreport2_BeforePrint);
            // 
            // xrSubreport1
            // 
            this.xrSubreport1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 431.4167F);
            this.xrSubreport1.Name = "xrSubreport1";
            this.xrSubreport1.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.caracteristicaSeparacion();
            this.xrSubreport1.Scripts.OnBeforePrint = "xrSubreport1_BeforePrint";
            this.xrSubreport1.SizeF = new System.Drawing.SizeF(839F, 22.99997F);
            this.xrSubreport1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrSubreport1_BeforePrint);
            // 
            // xrSubreport3
            // 
            this.xrSubreport3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 467.9583F);
            this.xrSubreport3.Name = "xrSubreport3";
            this.xrSubreport3.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.caracteristicaAlmacenamiento();
            this.xrSubreport3.Scripts.OnBeforePrint = "xrSubreport3_BeforePrint";
            this.xrSubreport3.SizeF = new System.Drawing.SizeF(839F, 23F);
            this.xrSubreport3.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrSubreport3_BeforePrint);
            // 
            // xrSubreport4
            // 
            this.xrSubreport4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 503.4583F);
            this.xrSubreport4.Name = "xrSubreport4";
            this.xrSubreport4.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.carateristicaAprovechamiento();
            this.xrSubreport4.Scripts.OnBeforePrint = "xrSubreport4_BeforePrint";
            this.xrSubreport4.SizeF = new System.Drawing.SizeF(839F, 23.00003F);
            this.xrSubreport4.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrSubreport4_BeforePrint);
            // 
            // xrSubreport5
            // 
            this.xrSubreport5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 536.875F);
            this.xrSubreport5.Name = "xrSubreport5";
            this.xrSubreport5.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.caracteristicaDispFinal();
            this.xrSubreport5.Scripts.OnBeforePrint = "xrSubreport5_BeforePrint";
            this.xrSubreport5.SizeF = new System.Drawing.SizeF(839F, 23F);
            this.xrSubreport5.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrSubreport5_BeforePrint);
            // 
            // xrSubreport6
            // 
            this.xrSubreport6.LocationFloat = new DevExpress.Utils.PointFloat(0.9999275F, 572.375F);
            this.xrSubreport6.Name = "xrSubreport6";
            this.xrSubreport6.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.caracteristicaSoporte();
            this.xrSubreport6.Scripts.OnBeforePrint = "xrSubreport6_BeforePrint";
            this.xrSubreport6.SizeF = new System.Drawing.SizeF(838F, 22.99994F);
            this.xrSubreport6.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrSubreport6_BeforePrint);
            // 
            // xrSubreport7
            // 
            this.xrSubreport7.LocationFloat = new DevExpress.Utils.PointFloat(0.9999275F, 606.8333F);
            this.xrSubreport7.Name = "xrSubreport7";
            this.xrSubreport7.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.caractRespelRua();
            this.xrSubreport7.Scripts.OnBeforePrint = "xrSubreport7_BeforePrint";
            this.xrSubreport7.SizeF = new System.Drawing.SizeF(838F, 23F);
            this.xrSubreport7.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrSubreport7_BeforePrint);
            // 
            // xrSubreport8
            // 
            this.xrSubreport8.LocationFloat = new DevExpress.Utils.PointFloat(0.9998639F, 639.2084F);
            this.xrSubreport8.Name = "xrSubreport8";
            this.xrSubreport8.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.caracteristicaRH1();
            this.xrSubreport8.Scripts.OnBeforePrint = "xrSubreport8_BeforePrint";
            this.xrSubreport8.SizeF = new System.Drawing.SizeF(837.9999F, 23F);
            this.xrSubreport8.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrSubreport8_BeforePrint);
            // 
            // xrSubreport9
            // 
            this.xrSubreport9.LocationFloat = new DevExpress.Utils.PointFloat(0.9998639F, 675.75F);
            this.xrSubreport9.Name = "xrSubreport9";
            this.xrSubreport9.ReportSource = new SIM.Areas.ControlVigilancia.reporte.aguas.fotoresiduo();
            this.xrSubreport9.Scripts.OnBeforePrint = "xrSubreport9_BeforePrint";
            this.xrSubreport9.SizeF = new System.Drawing.SizeF(837.9998F, 23F);
            this.xrSubreport9.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrSubreport9_BeforePrint);
            // 
            // ResiduoPeligroso
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
            this.Margins = new System.Drawing.Printing.Margins(0, 1, 124, 0);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.prm_idVisita,
            this.prm_formulario,
            this.prm_radicador});
            this.Version = "18.1";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.ResiduoPeligroso_BeforePrint);
            this.AfterPrint += new System.EventHandler(this.ResiduoPeligroso_AfterPrint);
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo2;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport2;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport1;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport3;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport4;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport5;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport6;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport7;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport8;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport9;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.Parameters.Parameter prm_idVisita;
        private DevExpress.XtraReports.Parameters.Parameter prm_formulario;
        private DevExpress.XtraReports.Parameters.Parameter prm_radicador;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRPictureBox xrpEncabezado;
        private DevExpress.XtraReports.UI.XRLabel xrLabel19;
        private DevExpress.XtraReports.UI.XRPageBreak xrPageBreak1;
        private DevExpress.XtraReports.UI.XRPictureBox xrMapa;
        private DevExpress.XtraReports.UI.XRPictureBox xrpUbicacion;
        private DevExpress.XtraReports.UI.XRRichText xrRichText1;
    }
}
