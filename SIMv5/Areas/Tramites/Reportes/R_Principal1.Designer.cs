namespace SIM.Areas.Tramites.Reportes
{
    partial class R_Principal1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(R_Principal1));
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageBreak1 = new DevExpress.XtraReports.UI.XRPageBreak();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrPageBreak2 = new DevExpress.XtraReports.UI.XRPageBreak();
            this.prmIdTerceroPrpal = new DevExpress.XtraReports.Parameters.Parameter();
            this.prmIdInstalacionP = new DevExpress.XtraReports.Parameters.Parameter();
            this.prm_Nombre_Tramite = new DevExpress.XtraReports.Parameters.Parameter();
            this.prmidVisitaP = new DevExpress.XtraReports.Parameters.Parameter();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource();
            this.xrCaract_AguasSup1 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrInfoInstalacion1 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrInfoApoderado1 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrInfoGral1 = new DevExpress.XtraReports.UI.XRSubreport();
            this.r_InfoInstalacion11 = new SIM.Areas.Tramites.Reportes.R_InfoInstalacion1();
            this.superficialesAgua11 = new SIM.Areas.Tramites.Reportes.Caracteristicas.AguasSuperficiales.superficialesAgua1();
            ((System.ComponentModel.ISupportInitialize)(this.r_InfoInstalacion11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.superficialesAgua11)).BeginInit();
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
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrCaract_AguasSup1,
            this.xrInfoInstalacion1,
            this.xrInfoApoderado1,
            this.xrInfoGral1,
            this.xrLabel13,
            this.xrLabel11,
            this.xrLabel9,
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel1,
            this.xrPageInfo1,
            this.xrLabel10,
            this.xrLabel12,
            this.xrLabel14,
            this.xrPageBreak1,
            this.xrLabel15,
            this.xrPanel1});
            this.GroupHeader1.HeightF = 941.3764F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel13
            // 
            this.xrLabel13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.ID_INSTALACION")});
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(574.9375F, 772.1293F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel13.Visible = false;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(63.0625F, 167.6292F);
            this.xrLabel11.Multiline = true;
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(719.9375F, 440.4166F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.Text = resources.GetString("xrLabel11.Text");
            // 
            // xrLabel9
            // 
            this.xrLabel9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.S_CORREO")});
            this.xrLabel9.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(212.0209F, 777.1292F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(340.625F, 23F);
            this.xrLabel9.StylePriority.UseFont = false;
            // 
            // xrLabel8
            // 
            this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.N_TELEFONO")});
            this.xrLabel8.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(198.4791F, 743.4207F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(342.7084F, 23F);
            this.xrLabel8.StylePriority.UseFont = false;
            // 
            // xrLabel7
            // 
            this.xrLabel7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.TIPO_TERCERO")});
            this.xrLabel7.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(184.9375F, 710.8373F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(340.625F, 22.99997F);
            this.xrLabel7.StylePriority.UseFont = false;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(63.0625F, 777.1292F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(148.9584F, 23F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "Correo Electrónico:";
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(63.0625F, 743.4207F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(135.4167F, 22.99994F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "Teléfono/ Celular:";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(64.06249F, 710.8373F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(120.875F, 23.00003F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "Tipo Solicitante:";
            // 
            // xrLabel3
            // 
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.N_DOCUMENTO")});
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(174.5208F, 679.2957F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(340.625F, 23F);
            this.xrLabel3.StylePriority.UseFont = false;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(63.0625F, 679.2957F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(111.4583F, 23F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "Identificación:";
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.S_RSOCIAL")});
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(63.0625F, 649.5457F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(570.8333F, 23F);
            this.xrLabel1.StylePriority.UseFont = false;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo1.Format = "{0:dddd d\' de \'MMMM\' de \'yyyy}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(134.8958F, 100.2487F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(280.2083F, 23F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(64.06249F, 100.2487F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(70.83333F, 23F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.Text = "Medellín,";
            // 
            // xrLabel12
            // 
            this.xrLabel12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.ID_TERCERO")});
            this.xrLabel12.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(574.9375F, 749.1292F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.Text = "ID_TERCERO";
            this.xrLabel12.Visible = false;
            // 
            // xrLabel14
            // 
            this.xrLabel14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.ID_VISITA")});
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(574.9375F, 725.1293F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel14.Visible = false;
            // 
            // xrPageBreak1
            // 
            this.xrPageBreak1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 811.3763F);
            this.xrPageBreak1.Name = "xrPageBreak1";
            // 
            // xrLabel15
            // 
            this.xrLabel15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.TRAMITE")});
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(574.9375F, 702.1293F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel15.Visible = false;
            // 
            // xrPanel1
            // 
            this.xrPanel1.BackColor = System.Drawing.Color.LightSeaGreen;
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1});
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 1F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(849.9999F, 75F);
            this.xrPanel1.StylePriority.UseBackColor = false;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageUrl = "~/Content/imagenes/LogosA.png";
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(688.9584F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(151.0416F, 75F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrPageBreak2
            // 
            this.xrPageBreak2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 914.3765F);
            this.xrPageBreak2.Name = "xrPageBreak2";
            // 
            // prmIdTerceroPrpal
            // 
            this.prmIdTerceroPrpal.Description = "prmIdTerceroPrpal";
            this.prmIdTerceroPrpal.Name = "prmIdTerceroPrpal";
            this.prmIdTerceroPrpal.Type = typeof(int);
            this.prmIdTerceroPrpal.ValueInfo = "0";
            // 
            // prmIdInstalacionP
            // 
            this.prmIdInstalacionP.Description = "prmIdInstalacionP";
            this.prmIdInstalacionP.Name = "prmIdInstalacionP";
            this.prmIdInstalacionP.Type = typeof(int);
            this.prmIdInstalacionP.ValueInfo = "0";
            // 
            // prm_Nombre_Tramite
            // 
            this.prm_Nombre_Tramite.Description = "prm_Nombre_Tramite";
            this.prm_Nombre_Tramite.Name = "prm_Nombre_Tramite";
            // 
            // prmidVisitaP
            // 
            this.prmidVisitaP.Description = "prmidVisitaP";
            this.prmidVisitaP.Name = "prmidVisitaP";
            this.prmidVisitaP.Type = typeof(int);
            this.prmidVisitaP.ValueInfo = "0";
            this.prmidVisitaP.Visible = false;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "amdb-cluster-scan.areametro.com:1521/pruebas.areametro.com_Connection";
            this.sqlDataSource1.Name = "sqlDataSource1";
            customSqlQuery1.Name = "CustomSqlQuery";
            customSqlQuery1.Sql = "SELECT * FROM TRAMITES.TL_V_ACTUACION";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // xrCaract_AguasSup1
            // 
            this.xrCaract_AguasSup1.LocationFloat = new DevExpress.Utils.PointFloat(63.06251F, 895.918F);
            this.xrCaract_AguasSup1.Name = "xrCaract_AguasSup1";
            this.xrCaract_AguasSup1.ReportSource = new SIM.Areas.Tramites.Reportes.Caracteristicas.AguasSuperficiales.superficialesAgua1();
            this.xrCaract_AguasSup1.SizeF = new System.Drawing.SizeF(719.9376F, 23F);
            // 
            // xrInfoInstalacion1
            // 
            this.xrInfoInstalacion1.LocationFloat = new DevExpress.Utils.PointFloat(63.06251F, 872.918F);
            this.xrInfoInstalacion1.Name = "xrInfoInstalacion1";
            this.xrInfoInstalacion1.ReportSource = new SIM.Areas.Tramites.Reportes.R_InfoInstalacion1();
            this.xrInfoInstalacion1.SizeF = new System.Drawing.SizeF(719.9376F, 23F);
            // 
            // xrInfoApoderado1
            // 
            this.xrInfoApoderado1.LocationFloat = new DevExpress.Utils.PointFloat(63.06251F, 849.918F);
            this.xrInfoApoderado1.Name = "xrInfoApoderado1";
            this.xrInfoApoderado1.ReportSource = new SIM.Areas.Tramites.Reportes.R_InfoApoderado1();
            this.xrInfoApoderado1.SizeF = new System.Drawing.SizeF(719.9376F, 23F);
            // 
            // xrInfoGral1
            // 
            this.xrInfoGral1.LocationFloat = new DevExpress.Utils.PointFloat(64.06256F, 826.918F);
            this.xrInfoGral1.Name = "xrInfoGral1";
            this.xrInfoGral1.ReportSource = new SIM.Areas.Tramites.Reportes.R_InfoGralSoli1();
            this.xrInfoGral1.SizeF = new System.Drawing.SizeF(718.9375F, 23F);
            // 
            // r_InfoInstalacion11
            // 
            this.r_InfoInstalacion11.DataMember = "CustomSqlQuery";
            this.r_InfoInstalacion11.FilterString = "[ID_INSTALACION] = ?prmIdInstalacion";
            this.r_InfoInstalacion11.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.r_InfoInstalacion11.Name = "r_InfoInstalacion11";
            this.r_InfoInstalacion11.PageHeight = 1100;
            this.r_InfoInstalacion11.PageWidth = 850;
            this.r_InfoInstalacion11.Version = "14.2";
            // 
            // superficialesAgua11
            // 
            this.superficialesAgua11.DataMember = "CustomSqlQuery";
            this.superficialesAgua11.FilterString = "[ID_VISITA] = ?prm_visita";
            this.superficialesAgua11.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 48);
            this.superficialesAgua11.Name = "superficialesAgua11";
            this.superficialesAgua11.PageHeight = 1100;
            this.superficialesAgua11.PageWidth = 850;
            this.superficialesAgua11.ScriptsSource = resources.GetString("superficialesAgua11.ScriptsSource");
            this.superficialesAgua11.Version = "14.2";
            // 
            // R_Principal1
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "CustomSqlQuery";
            this.DataSource = this.sqlDataSource1;
            this.FilterString = "[ID_TERCERO] = ?prmIdTerceroPrpal And [ID_INSTALACION] = ?prmIdInstalacionP And [" +
    "ID_VISITA] = ?prmidVisitaP";
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.prmIdTerceroPrpal,
            this.prmIdInstalacionP,
            this.prm_Nombre_Tramite,
            this.prmidVisitaP});
            this.ScriptsSource = resources.GetString("$this.ScriptsSource");
            this.Version = "14.2";
            ((System.ComponentModel.ISupportInitialize)(this.r_InfoInstalacion11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.superficialesAgua11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRPanel xrPanel1;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.XRLabel xrLabel14;
        private DevExpress.XtraReports.UI.XRPageBreak xrPageBreak1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel15;
        //private DevExpress.XtraReports.UI.XRSubreport xrInfoGral1;
        //private DevExpress.XtraReports.UI.XRSubreport xrInfoApoderado1;
        //private DevExpress.XtraReports.UI.XRSubreport xrInfoInstalacion1;
        //private DevExpress.XtraReports.UI.XRSubreport xrInfoRL1;
        private DevExpress.XtraReports.UI.XRPageBreak xrPageBreak2;
        //private DevExpress.XtraReports.UI.XRSubreport xrCaract_AguasSup1;
        private DevExpress.XtraReports.Parameters.Parameter prmIdTerceroPrpal;
        private DevExpress.XtraReports.Parameters.Parameter prmIdInstalacionP;
        private DevExpress.XtraReports.Parameters.Parameter prm_Nombre_Tramite;
        private DevExpress.XtraReports.Parameters.Parameter prmidVisitaP;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.UI.XRSubreport xrCaract_AguasSup1;
        private DevExpress.XtraReports.UI.XRSubreport xrInfoInstalacion1;
        private DevExpress.XtraReports.UI.XRSubreport xrInfoApoderado1;
        private DevExpress.XtraReports.UI.XRSubreport xrInfoGral1;
        private R_InfoInstalacion1 r_InfoInstalacion11;
        private Caracteristicas.AguasSuperficiales.superficialesAgua1 superficialesAgua11;
    }
}
