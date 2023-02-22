namespace SIM.Areas.Tramites.Reportes.Caracteristicas.FuentesFijas
{
    partial class caracteristicaInfChimeneas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(caracteristicaInfChimeneas));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource();
            this.prm_id_estado = new DevExpress.XtraReports.Parameters.Parameter();
            this.prm_tipo = new DevExpress.XtraReports.Parameters.Parameter();
            this.prm_id_formulario = new DevExpress.XtraReports.Parameters.Parameter();
            this.prm_grupo = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrVariablesSubr = new DevExpress.XtraReports.UI.XRSubreport();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrVariablesSubr,
            this.xrLabel3});
            this.Detail.HeightF = 45.83333F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel3
            // 
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.TITULO")});
            this.xrLabel3.Font = new System.Drawing.Font("Franklin Gothic Medium", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(19.27083F, 0F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(680.0209F, 23.54167F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseForeColor = false;
            this.xrLabel3.Text = "xrLabel3";
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel20,
            this.xrLine3});
            this.TopMargin.HeightF = 27.24995F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel20
            // 
            this.xrLabel20.Font = new System.Drawing.Font("Franklin Gothic Medium", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel20.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(306.6666F, 19.7916F);
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UseForeColor = false;
            this.xrLabel20.Text = " Inf chimeneas";
            // 
            // xrLine3
            // 
            this.xrLine3.ForeColor = System.Drawing.Color.DarkGray;
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 19.7916F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(699.2917F, 7.458344F);
            this.xrLine3.StylePriority.UseForeColor = false;
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
            this.sqlDataSource1.ConnectionName = "amdb-cluster-scan.areametro.com:1521/pruebas.areametro.com_Connection";
            this.sqlDataSource1.Name = "sqlDataSource1";
            customSqlQuery1.Name = "CustomSqlQuery";
            customSqlQuery1.Sql = "select *\r\n  from control.VWR_CARATERISTICAGRUPO\r\n";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // prm_id_estado
            // 
            this.prm_id_estado.Description = "prm_id_estado";
            this.prm_id_estado.Name = "prm_id_estado";
            this.prm_id_estado.Type = typeof(int);
            this.prm_id_estado.ValueInfo = "0";
            this.prm_id_estado.Visible = false;
            // 
            // prm_tipo
            // 
            this.prm_tipo.Description = "prm_tipo";
            this.prm_tipo.Name = "prm_tipo";
            this.prm_tipo.Type = typeof(int);
            this.prm_tipo.ValueInfo = "0";
            this.prm_tipo.Visible = false;
            // 
            // prm_id_formulario
            // 
            this.prm_id_formulario.Description = "prm_id_formulario";
            this.prm_id_formulario.Name = "prm_id_formulario";
            this.prm_id_formulario.Type = typeof(int);
            this.prm_id_formulario.ValueInfo = "0";
            this.prm_id_formulario.Visible = false;
            // 
            // prm_grupo
            // 
            this.prm_grupo.Description = "prm_grupo";
            this.prm_grupo.Name = "prm_grupo";
            this.prm_grupo.Type = typeof(int);
            this.prm_grupo.ValueInfo = "0";
            this.prm_grupo.Visible = false;
            // 
            // xrVariablesSubr
            // 
            this.xrVariablesSubr.LocationFloat = new DevExpress.Utils.PointFloat(19.27083F, 23.54167F);
            this.xrVariablesSubr.Name = "xrVariablesSubr";
            this.xrVariablesSubr.ReportSource = new SIM.Areas.Tramites.Reportes.Caracteristicas.variablesSubreport();
            this.xrVariablesSubr.Scripts.OnBeforePrint = "xrVariablesSubr_BeforePrint";
            this.xrVariablesSubr.SizeF = new System.Drawing.SizeF(680.0209F, 19.875F);
            // 
            // caracteristicaInfChimeneas
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "CustomSqlQuery";
            this.DataSource = this.sqlDataSource1;
            this.FilterString = "[ID_ESTADO] = ?prm_id_estado And [GRUPO_FORMULARIO] = ?prm_grupo And [ID_FORMULAR" +
    "IO] = ?prm_id_formulario";
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 27, 0);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.prm_id_estado,
            this.prm_tipo,
            this.prm_id_formulario,
            this.prm_grupo});
            this.ReportPrintOptions.PrintOnEmptyDataSource = false;
            this.ScriptsSource = resources.GetString("$this.ScriptsSource");
            this.Version = "14.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRSubreport xrVariablesSubr;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel20;
        private DevExpress.XtraReports.UI.XRLine xrLine3;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.Parameters.Parameter prm_id_estado;
        private DevExpress.XtraReports.Parameters.Parameter prm_tipo;
        private DevExpress.XtraReports.Parameters.Parameter prm_id_formulario;
        private DevExpress.XtraReports.Parameters.Parameter prm_grupo;
    }
}
