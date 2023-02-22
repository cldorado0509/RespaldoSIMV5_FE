using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Web.Hosting;
using SIM.Utilidades;
using System.Linq;
using SIM.Areas.Models;
using SIM.Areas.Tramites.Models;
using System.Data.Entity;
using System.IO;
using System.Drawing.Imaging;
using System.Globalization;
using SIM.Data;
using SIM.Data.Tramites;

/// <summary>
/// Summary description for CBReport
/// </summary>
public class Radicado01Report: DevExpress.XtraReports.UI.XtraReport, IRadicador
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRBarCode bcCodigo;
    private XRPictureBox xrpLogo;
    private XRLabel xrlTexto1;
    private XRLabel xrlTexto2;
    private XRLabel xrlTexto3;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

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

    public Radicado01Report()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //

        System.Drawing.Text.PrivateFontCollection privateFonts = new System.Drawing.Text.PrivateFontCollection();
        privateFonts.AddFontFile(HostingEnvironment.MapPath(@"~/fonts/OCRAEXT.TTF"));
        System.Drawing.Font font = new Font(privateFonts.Families[0], 12);

        bcCodigo.Font = font;
        xrlTexto1.Font = font;
        xrlTexto2.Font = font;
        xrlTexto3.Font = font;
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
            this.Detail.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlTexto1,
            this.xrlTexto2,
            this.xrpLogo,
            this.xrlTexto3,
            this.bcCodigo});
            this.Detail.Dpi = 96F;
            this.Detail.HeightF = 81.52286F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 96F);
            this.Detail.StylePriority.UseBorders = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrlTexto1
            // 
            this.xrlTexto1.CanGrow = false;
            this.xrlTexto1.Dpi = 96F;
            this.xrlTexto1.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlTexto1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 41.84569F);
            this.xrlTexto1.Name = "xrlTexto1";
            this.xrlTexto1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrlTexto1.SizeF = new System.Drawing.SizeF(302.7877F, 13F);
            this.xrlTexto1.StylePriority.UseFont = false;
            this.xrlTexto1.StylePriority.UseTextAlignment = false;
            this.xrlTexto1.Text = "[Texto 1]";
            this.xrlTexto1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrlTexto1.TextTrimming = System.Drawing.StringTrimming.EllipsisCharacter;
            // 
            // xrlTexto2
            // 
            this.xrlTexto2.CanGrow = false;
            this.xrlTexto2.Dpi = 96F;
            this.xrlTexto2.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlTexto2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 54.18427F);
            this.xrlTexto2.Name = "xrlTexto2";
            this.xrlTexto2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrlTexto2.SizeF = new System.Drawing.SizeF(302.7877F, 13F);
            this.xrlTexto2.StylePriority.UseFont = false;
            this.xrlTexto2.StylePriority.UseTextAlignment = false;
            this.xrlTexto2.Text = "[Texto 2]";
            this.xrlTexto2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrlTexto2.TextTrimming = System.Drawing.StringTrimming.EllipsisCharacter;
            // 
            // xrpLogo
            // 
            this.xrpLogo.Dpi = 96F;
            this.xrpLogo.ImageUrl = "~/Content/Images/LogoAreaEtiqueta.png";
            this.xrpLogo.LocationFloat = new DevExpress.Utils.PointFloat(299.2646F, 1.511813F);
            this.xrpLogo.Name = "xrpLogo";
            this.xrpLogo.SizeF = new System.Drawing.SizeF(66.30234F, 74.34962F);
            this.xrpLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrlTexto3
            // 
            this.xrlTexto3.CanGrow = false;
            this.xrlTexto3.Dpi = 96F;
            this.xrlTexto3.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlTexto3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 67.52286F);
            this.xrlTexto3.Name = "xrlTexto3";
            this.xrlTexto3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrlTexto3.SizeF = new System.Drawing.SizeF(302.7877F, 14F);
            this.xrlTexto3.StylePriority.UseFont = false;
            this.xrlTexto3.StylePriority.UseTextAlignment = false;
            this.xrlTexto3.Text = "[Texto 3]";
            this.xrlTexto3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrlTexto3.TextTrimming = System.Drawing.StringTrimming.EllipsisCharacter;
            // 
            // bcCodigo
            // 
            this.bcCodigo.AutoModule = true;
            this.bcCodigo.Dpi = 96F;
            this.bcCodigo.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.bcCodigo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.bcCodigo.Module = 1.92F;
            this.bcCodigo.Name = "bcCodigo";
            this.bcCodigo.Padding = new DevExpress.XtraPrinting.PaddingInfo(9, 9, 0, 0, 96F);
            this.bcCodigo.SizeF = new System.Drawing.SizeF(297.7528F, 41.84569F);
            this.bcCodigo.StylePriority.UseFont = false;
            this.bcCodigo.StylePriority.UseTextAlignment = false;
            code128Generator1.CharacterSet = DevExpress.XtraPrinting.BarCode.Code128Charset.CharsetAuto;
            this.bcCodigo.Symbology = code128Generator1;
            this.bcCodigo.Text = "1234567890123456789012345";
            this.bcCodigo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 96F;
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 96F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 96F;
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 96F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.BottomMargin.Visible = false;
            // 
            // Radicado01Report
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Dpi = 96F;
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageHeight = 94;
            this.PageWidth = 368;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.Pixels;
            this.Version = "16.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    public MemoryStream GenerarEtiqueta(int idRadicado, string tipoRetorno)
    {
        decimal idRadicadoDec =  Convert.ToDecimal(idRadicado);
        RADICADO_DOCUMENTO radicado = dbSIM.RADICADO_DOCUMENTO.Where(r => r.ID_RADICADODOC == idRadicadoDec).FirstOrDefault();
        TBSERIE unidadDocumental = dbSIM.TBSERIE.Where(ud => ud.CODSERIE == radicado.CODSERIE).FirstOrDefault();

        CultureInfo esCO = new CultureInfo("es-CO");

        bcCodigo.Text = radicado.S_ETIQUETA;
        xrlTexto1.Text = unidadDocumental.NOMBRE;
        xrlTexto2.Text = radicado.D_RADICADO.ToString("MMMM dd yyyy HH:mm", esCO);
        xrlTexto3.Text = "Radicado      00-" + radicado.N_CONSECUTIVO.ToString("000000");

        var stream = new MemoryStream();
        switch (tipoRetorno.ToUpper())
        {
            case "PDF":
                this.ExportToPdf(stream);
                this.Dispose();
                break;
            case "PNG":
                this.ExportToImage(stream, ImageFormat.Png);
                break;
            case "JPG":
                this.ExportToImage(stream, ImageFormat.Jpeg);
                break;
            default:
                this.ExportToImage(stream, ImageFormat.Png);
                break;
        }

        return stream;
    }
}
