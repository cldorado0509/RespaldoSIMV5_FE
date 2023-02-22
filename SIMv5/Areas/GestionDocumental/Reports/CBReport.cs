using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for CBReport
/// </summary>
public class CBReport : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRBarCode bcCodigo;
    private XRPictureBox xrpLogo;
    private XRLabel xrlTexto1;
    private XRLabel xrlTexto2;
    private XRLabel xrlTexto3;
    private XRLabel xrlTexto9;
    private XRLabel xrlTexto10;
    private XRLabel xrlTexto4;
    private XRLabel xrlTexto8;
    private XRLabel xrlTexto5;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public string CB
    {
        set { bcCodigo.Text = value; }
    }

    public string Texto1
    {
        set { xrlTexto1.Text = value; }
    }

    public string Texto2
    {
        set { xrlTexto2.Text = value; }
    }

    public string Texto3
    {
        set { xrlTexto3.Text = value; }
    }

    public string Texto4
    {
        set { xrlTexto4.Text = value; }
    }

    public string Texto5
    {
        set { xrlTexto5.Text = value; }
    }

    public string Texto8
    {
        set { xrlTexto8.Text = value; }
    }

    public string Texto9
    {
        set { xrlTexto9.Text = value; }
    }

    public string Texto10
    {
        set { xrlTexto10.Text = value; }
    }

    public CBReport()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

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
            DevExpress.XtraPrinting.BarCode.Code128Generator code128Generator1 = new DevExpress.XtraPrinting.BarCode.Code128Generator();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrlTexto5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlTexto8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlTexto4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlTexto10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlTexto9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlTexto1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlTexto2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrpLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrlTexto3 = new DevExpress.XtraReports.UI.XRLabel();
            this.bcCodigo = new DevExpress.XtraReports.UI.XRBarCode();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlTexto5,
            this.xrlTexto8,
            this.xrlTexto4,
            this.xrlTexto10,
            this.xrlTexto9,
            this.xrlTexto1,
            this.xrlTexto2,
            this.xrpLogo,
            this.xrlTexto3,
            this.bcCodigo});
            this.Detail.Dpi = 254F;
            this.Detail.HeightF = 218.7167F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrlTexto5
            // 
            this.xrlTexto5.CanGrow = false;
            this.xrlTexto5.Dpi = 254F;
            this.xrlTexto5.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlTexto5.LocationFloat = new DevExpress.Utils.PointFloat(600.3965F, 187.95F);
            this.xrlTexto5.Name = "xrlTexto5";
            this.xrlTexto5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrlTexto5.SizeF = new System.Drawing.SizeF(212.4127F, 30.00005F);
            this.xrlTexto5.StylePriority.UseFont = false;
            this.xrlTexto5.StylePriority.UseTextAlignment = false;
            this.xrlTexto5.Text = "[Texto5]";
            this.xrlTexto5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrlTexto8
            // 
            this.xrlTexto8.CanGrow = false;
            this.xrlTexto8.Dpi = 254F;
            this.xrlTexto8.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlTexto8.LocationFloat = new DevExpress.Utils.PointFloat(534.251F, 127.9459F);
            this.xrlTexto8.Name = "xrlTexto8";
            this.xrlTexto8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrlTexto8.SizeF = new System.Drawing.SizeF(278.5534F, 29.99997F);
            this.xrlTexto8.StylePriority.UseFont = false;
            this.xrlTexto8.StylePriority.UseTextAlignment = false;
            this.xrlTexto8.Text = "[Texto 8]";
            this.xrlTexto8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrlTexto4
            // 
            this.xrlTexto4.CanGrow = false;
            this.xrlTexto4.Dpi = 254F;
            this.xrlTexto4.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlTexto4.LocationFloat = new DevExpress.Utils.PointFloat(0.0001220703F, 187.9459F);
            this.xrlTexto4.Name = "xrlTexto4";
            this.xrlTexto4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrlTexto4.SizeF = new System.Drawing.SizeF(600.3965F, 29.99998F);
            this.xrlTexto4.StylePriority.UseFont = false;
            this.xrlTexto4.StylePriority.UseTextAlignment = false;
            this.xrlTexto4.Text = "[Texto 4]";
            this.xrlTexto4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrlTexto4.TextTrimming = System.Drawing.StringTrimming.EllipsisCharacter;
            // 
            // xrlTexto10
            // 
            this.xrlTexto10.CanGrow = false;
            this.xrlTexto10.Dpi = 254F;
            this.xrlTexto10.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlTexto10.LocationFloat = new DevExpress.Utils.PointFloat(600.3965F, 157.9459F);
            this.xrlTexto10.Name = "xrlTexto10";
            this.xrlTexto10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrlTexto10.SizeF = new System.Drawing.SizeF(212.4077F, 30.00003F);
            this.xrlTexto10.StylePriority.UseFont = false;
            this.xrlTexto10.StylePriority.UseTextAlignment = false;
            this.xrlTexto10.Text = "[Texto10]";
            this.xrlTexto10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrlTexto9
            // 
            this.xrlTexto9.CanGrow = false;
            this.xrlTexto9.Dpi = 254F;
            this.xrlTexto9.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlTexto9.LocationFloat = new DevExpress.Utils.PointFloat(534.2509F, 97.94601F);
            this.xrlTexto9.Name = "xrlTexto9";
            this.xrlTexto9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrlTexto9.SizeF = new System.Drawing.SizeF(278.5534F, 29.99998F);
            this.xrlTexto9.StylePriority.UseFont = false;
            this.xrlTexto9.StylePriority.UseTextAlignment = false;
            this.xrlTexto9.Text = "[Texto 9]";
            this.xrlTexto9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrlTexto1
            // 
            this.xrlTexto1.CanGrow = false;
            this.xrlTexto1.Dpi = 254F;
            this.xrlTexto1.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlTexto1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 97.94598F);
            this.xrlTexto1.Name = "xrlTexto1";
            this.xrlTexto1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrlTexto1.SizeF = new System.Drawing.SizeF(534.2509F, 29.99999F);
            this.xrlTexto1.StylePriority.UseFont = false;
            this.xrlTexto1.StylePriority.UseTextAlignment = false;
            this.xrlTexto1.Text = "[Texto 1]";
            this.xrlTexto1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrlTexto1.TextTrimming = System.Drawing.StringTrimming.EllipsisCharacter;
            // 
            // xrlTexto2
            // 
            this.xrlTexto2.CanGrow = false;
            this.xrlTexto2.Dpi = 254F;
            this.xrlTexto2.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlTexto2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 127.946F);
            this.xrlTexto2.Name = "xrlTexto2";
            this.xrlTexto2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrlTexto2.SizeF = new System.Drawing.SizeF(534.2509F, 30F);
            this.xrlTexto2.StylePriority.UseFont = false;
            this.xrlTexto2.StylePriority.UseTextAlignment = false;
            this.xrlTexto2.Text = "[Texto 2]";
            this.xrlTexto2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrlTexto2.TextTrimming = System.Drawing.StringTrimming.EllipsisCharacter;
            // 
            // xrpLogo
            // 
            this.xrpLogo.Dpi = 254F;
            this.xrpLogo.Image = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Images/LogoAreaEtiqueta.png"));
        //this.xrpLogo.ImageUrl = "~/Content/Images/LogoAreaEtiqueta.png";
            this.xrpLogo.LocationFloat = new DevExpress.Utils.PointFloat(823.3926F, 0F);
            this.xrpLogo.Name = "xrpLogo";
            this.xrpLogo.SizeF = new System.Drawing.SizeF(176.6074F, 218.7167F);
            this.xrpLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrlTexto3
            // 
            this.xrlTexto3.CanGrow = false;
            this.xrlTexto3.Dpi = 254F;
            this.xrlTexto3.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlTexto3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 157.9459F);
            this.xrlTexto3.Name = "xrlTexto3";
            this.xrlTexto3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrlTexto3.SizeF = new System.Drawing.SizeF(600.3966F, 30F);
            this.xrlTexto3.StylePriority.UseFont = false;
            this.xrlTexto3.StylePriority.UseTextAlignment = false;
            this.xrlTexto3.Text = "[Texto 3]";
            this.xrlTexto3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrlTexto3.TextTrimming = System.Drawing.StringTrimming.EllipsisCharacter;
            // 
            // bcCodigo
            // 
            this.bcCodigo.Alignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.bcCodigo.AutoModule = true;
            this.bcCodigo.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.bcCodigo.Dpi = 254F;
            this.bcCodigo.Font = new System.Drawing.Font("Arial Narrow", 6F);
            this.bcCodigo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.bcCodigo.Module = 5.08F;
            this.bcCodigo.Name = "bcCodigo";
            this.bcCodigo.Padding = new DevExpress.XtraPrinting.PaddingInfo(13, 5, 0, 0, 254F);
            this.bcCodigo.SizeF = new System.Drawing.SizeF(823.3926F, 98F);
            this.bcCodigo.StylePriority.UseBorderDashStyle = false;
            this.bcCodigo.StylePriority.UseFont = false;
            this.bcCodigo.StylePriority.UsePadding = false;
            this.bcCodigo.StylePriority.UseTextAlignment = false;
            this.bcCodigo.Symbology = code128Generator1;
            this.bcCodigo.Text = "0030034141010000000113";
            this.bcCodigo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 254F;
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 254F;
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // CBReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Dpi = 254F;
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageHeight = 250;
            this.PageWidth = 1000;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.Version = "14.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
