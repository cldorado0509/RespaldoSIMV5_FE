using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using SIM.Areas.Tramites.Models;
using SIM.Data.Tramites;

/// <summary>
/// Summary description for DGAReport
/// </summary>
public class CalculoReport : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRPictureBox xrpLogo;
    private XRPanel xrPanel1;
    private XRLabel xrLabel1;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private XRLabel xrLabel4;
    private XRPageInfo xrPageInfo1;
    private XRLabel xrLabel10;
    private XRPanel xrPanel2;
    private XRLabel xrLabel6;
    private XRLabel xrlValorProyecto;
    private XRLabel xrlTipoTramite;
    private XRLabel xrLabel9;
    private XRLabel xrLabel5;
    private XRLabel xrlNit;
    private XRLabel xrlEmpresa;
    private XRLabel xrlEmpresaTit;
    private XRLabel xrLabel11;
    private XRLabel xrlCM;
    private XRLabel xrlProfesionales;
    private XRLabel xrLabel12;
    private XRLabel xrlObservaciones;
    private XRLabel xrLabel3;
    private XRPanel xrPanel3;
    private XRLabel xrlCostosTarifa_E;
    private XRLabel xrLabel8;
    private XRLabel xrLabel21;
    private XRLabel xrlCostosTramite_E;
    private XRLabel xrLabel23;
    private XRLabel xrlCostosTo_E;
    private XRLabel xrlCostosB_E;
    private XRLabel xrLabel14;
    private XRLabel xrLabel15;
    private XRLabel xrlCostosD_E;
    private XRLabel xrLabel17;
    private XRLabel xrlCostosC_E;
    private XRLabel xrlCostosA_E;
    private XRLabel xrLabel20;
    private XRPanel xrPanel4;
    private XRLabel xrlCostosTarifa_S;
    private XRLabel xrLabel13;
    private XRLabel xrLabel16;
    private XRLabel xrlCostosTramite_S;
    private XRLabel xrLabel19;
    private XRLabel xrlCostosTo_S;
    private XRLabel xrlCostosB_S;
    private XRLabel xrLabel25;
    private XRLabel xrLabel26;
    private XRLabel xrlCostosD_S;
    private XRLabel xrLabel28;
    private XRLabel xrlCostosC_S;
    private XRLabel xrlCostosA_S;
    private XRLabel xrLabel31;
    private XRLabel xrLabel32;
    private XRLabel xrLabel2;

    public CalculoReport()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

    public void CargarDatos(TBTARIFAS_CALCULO tramiteE, TBTARIFAS_CALCULO tramiteS, string tipoTramite, string terceroRS)
    {
        SetControlText("xrlEmpresa", terceroRS);
        SetControlText("xrlNit", tramiteE.NIT);

        SetControlText("xrlTipoTramite", tipoTramite);
        SetControlText("xrlProfesionales", tramiteE.NRO_TECNICOS.ToString());
        SetControlText("xrlValorProyecto", tramiteE.VALOR_PROYECTO.ToString("#,##0.00"));
        SetControlText("xrlCM", tramiteE.CM);
        SetControlText("xrlObservaciones", tramiteE.OBSERVACION);

        SetControlText("xrlCostosA_E", tramiteE.COSTOS_A.ToString("#,##0.00"));
        SetControlText("xrlCostosB_E", tramiteE.COSTOS_B.ToString("#,##0.00"));
        SetControlText("xrlCostosC_E", ((decimal)tramiteE.COSTOS_C).ToString("#,##0.00"));
        SetControlText("xrlCostosD_E", tramiteE.COSTOS_D.ToString("#,##0.00"));
        SetControlText("xrlCostosTarifa_E", (tramiteE.COSTOS_A + tramiteE.COSTOS_B + ((decimal)tramiteE.COSTOS_C) + tramiteE.COSTOS_D).ToString("#,##0.00"));
        SetControlText("xrlCostosTo_E", tramiteE.TOPES.ToString("#,##0.00"));
        SetControlText("xrlCostosTramite_E", tramiteE.VALOR.ToString("#,##0.00"));

        SetControlText("xrlCostosA_S", tramiteS.COSTOS_A.ToString("#,##0.00"));
        SetControlText("xrlCostosB_S", tramiteS.COSTOS_B.ToString("#,##0.00"));
        SetControlText("xrlCostosC_S", ((decimal)tramiteS.COSTOS_C).ToString("#,##0.00"));
        SetControlText("xrlCostosD_S", tramiteS.COSTOS_D.ToString("#,##0.00"));
        SetControlText("xrlCostosTarifa_S", (tramiteS.COSTOS_A + tramiteS.COSTOS_B + ((decimal)tramiteS.COSTOS_C) + tramiteS.COSTOS_D).ToString("#,##0.00"));
        SetControlText("xrlCostosTo_S", tramiteS.TOPES.ToString("#,##0.00"));
        SetControlText("xrlCostosTramite_S", tramiteS.VALOR.ToString("#,##0.00"));
    }

    public void SetControlText(string rfstrFieldName, string rfstrText)
    {
        this.FindControl(rfstrFieldName, true).Text = rfstrText;
    }

    public void AddBoundLabel(string rfstrFieldName, string rfstrBindingMember)
    {
        AddBoundLabel(rfstrFieldName, rfstrBindingMember, "");
    }

    public void AddBoundLabel(string rfstrFieldName, string rfstrBindingMember, string rfstrFormato)
    {
        if (rfstrFormato != "")
            this.FindControl(rfstrFieldName, true).DataBindings.Add("Text", DataSource, rfstrBindingMember, "{0:" + rfstrFormato + "}");
        else
            this.FindControl(rfstrFieldName, true).DataBindings.Add("Text", DataSource, rfstrBindingMember);
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
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrlObservaciones = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlProfesionales = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlCM = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlValorProyecto = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlTipoTramite = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlNit = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlEmpresa = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlEmpresaTit = new DevExpress.XtraReports.UI.XRLabel();
            this.xrpLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel3 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrlCostosB_E = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlCostosD_E = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlCostosC_E = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlCostosA_E = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlCostosTarifa_E = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlCostosTramite_E = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlCostosTo_E = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel4 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrlCostosTarifa_S = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlCostosTramite_S = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlCostosTo_S = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlCostosB_S = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlCostosD_S = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlCostosC_S = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlCostosA_S = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel4,
            this.xrLabel32,
            this.xrLabel3,
            this.xrPanel3,
            this.xrLabel10,
            this.xrPanel2,
            this.xrPanel1});
            this.Detail.HeightF = 832.4375F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel10
            // 
            this.xrLabel10.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel10.BorderWidth = 1F;
            this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(0F, 46.79168F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(789.9999F, 23F);
            this.xrLabel10.StylePriority.UseBackColor = false;
            this.xrLabel10.StylePriority.UseBorders = false;
            this.xrLabel10.StylePriority.UseBorderWidth = false;
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "Datos del Proyecto";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPanel2
            // 
            this.xrPanel2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlObservaciones,
            this.xrLabel2,
            this.xrlProfesionales,
            this.xrLabel12,
            this.xrLabel11,
            this.xrlCM,
            this.xrLabel6,
            this.xrlValorProyecto,
            this.xrlTipoTramite,
            this.xrLabel9});
            this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 69.79166F);
            this.xrPanel2.Name = "xrPanel2";
            this.xrPanel2.SizeF = new System.Drawing.SizeF(790F, 188.5417F);
            this.xrPanel2.StylePriority.UseBorders = false;
            // 
            // xrlObservaciones
            // 
            this.xrlObservaciones.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlObservaciones.BorderWidth = 1F;
            this.xrlObservaciones.LocationFloat = new DevExpress.Utils.PointFloat(37.5F, 155.4167F);
            this.xrlObservaciones.Multiline = true;
            this.xrlObservaciones.Name = "xrlObservaciones";
            this.xrlObservaciones.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlObservaciones.SizeF = new System.Drawing.SizeF(744.9996F, 23F);
            this.xrlObservaciones.StylePriority.UseBorders = false;
            this.xrlObservaciones.StylePriority.UseBorderWidth = false;
            this.xrlObservaciones.StylePriority.UseTextAlignment = false;
            this.xrlObservaciones.Text = "[Observaciones]";
            this.xrlObservaciones.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel2.BorderWidth = 1F;
            this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(9.999939F, 132.4167F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(327.1592F, 23F);
            this.xrLabel2.StylePriority.UseBorders = false;
            this.xrLabel2.StylePriority.UseBorderWidth = false;
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Observaciones";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlProfesionales
            // 
            this.xrlProfesionales.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlProfesionales.BorderWidth = 1F;
            this.xrlProfesionales.LocationFloat = new DevExpress.Utils.PointFloat(347.5F, 37.58333F);
            this.xrlProfesionales.Name = "xrlProfesionales";
            this.xrlProfesionales.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlProfesionales.SizeF = new System.Drawing.SizeF(133.4092F, 23F);
            this.xrlProfesionales.StylePriority.UseBorders = false;
            this.xrlProfesionales.StylePriority.UseBorderWidth = false;
            this.xrlProfesionales.StylePriority.UseTextAlignment = false;
            this.xrlProfesionales.Text = "[Profesionales]";
            this.xrlProfesionales.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel12
            // 
            this.xrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel12.BorderWidth = 1F;
            this.xrLabel12.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(10.00005F, 37.58336F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(327.159F, 23F);
            this.xrLabel12.StylePriority.UseBorders = false;
            this.xrLabel12.StylePriority.UseBorderWidth = false;
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = "Profesionales Técnicos:";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel11.BorderWidth = 1F;
            this.xrLabel11.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 99.41668F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(327.1592F, 23F);
            this.xrLabel11.StylePriority.UseBorders = false;
            this.xrLabel11.StylePriority.UseBorderWidth = false;
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "CM:";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlCM
            // 
            this.xrlCM.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlCM.BorderWidth = 1F;
            this.xrlCM.LocationFloat = new DevExpress.Utils.PointFloat(347.5F, 99.41668F);
            this.xrlCM.Multiline = true;
            this.xrlCM.Name = "xrlCM";
            this.xrlCM.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlCM.SizeF = new System.Drawing.SizeF(432.5001F, 23F);
            this.xrlCM.StylePriority.UseBorders = false;
            this.xrlCM.StylePriority.UseBorderWidth = false;
            this.xrlCM.StylePriority.UseTextAlignment = false;
            this.xrlCM.Text = "[CM]";
            this.xrlCM.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel6.BorderWidth = 1F;
            this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 68.41669F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(327.1592F, 23F);
            this.xrLabel6.StylePriority.UseBorders = false;
            this.xrLabel6.StylePriority.UseBorderWidth = false;
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Valor del Proyecto:";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlValorProyecto
            // 
            this.xrlValorProyecto.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlValorProyecto.BorderWidth = 1F;
            this.xrlValorProyecto.LocationFloat = new DevExpress.Utils.PointFloat(347.5F, 68.41667F);
            this.xrlValorProyecto.Multiline = true;
            this.xrlValorProyecto.Name = "xrlValorProyecto";
            this.xrlValorProyecto.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlValorProyecto.SizeF = new System.Drawing.SizeF(432.5001F, 22.99999F);
            this.xrlValorProyecto.StylePriority.UseBorders = false;
            this.xrlValorProyecto.StylePriority.UseBorderWidth = false;
            this.xrlValorProyecto.StylePriority.UseTextAlignment = false;
            this.xrlValorProyecto.Text = "[Valor Proyecto]";
            this.xrlValorProyecto.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlTipoTramite
            // 
            this.xrlTipoTramite.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlTipoTramite.BorderWidth = 1F;
            this.xrlTipoTramite.LocationFloat = new DevExpress.Utils.PointFloat(347.5F, 6.583309F);
            this.xrlTipoTramite.Name = "xrlTipoTramite";
            this.xrlTipoTramite.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlTipoTramite.SizeF = new System.Drawing.SizeF(133.4092F, 23F);
            this.xrlTipoTramite.StylePriority.UseBorders = false;
            this.xrlTipoTramite.StylePriority.UseBorderWidth = false;
            this.xrlTipoTramite.StylePriority.UseTextAlignment = false;
            this.xrlTipoTramite.Text = "[Tipo Tramite]";
            this.xrlTipoTramite.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel9.BorderWidth = 1F;
            this.xrLabel9.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 6.583351F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(327.1591F, 23F);
            this.xrLabel9.StylePriority.UseBorders = false;
            this.xrLabel9.StylePriority.UseBorderWidth = false;
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "Tipo de Trámite:";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPanel1
            // 
            this.xrPanel1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel5,
            this.xrlNit,
            this.xrlEmpresa,
            this.xrlEmpresaTit});
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(790F, 39.58333F);
            this.xrPanel1.StylePriority.UseBorders = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel5.BorderWidth = 1F;
            this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(484.375F, 6.583341F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(35.41669F, 23F);
            this.xrLabel5.StylePriority.UseBorders = false;
            this.xrLabel5.StylePriority.UseBorderWidth = false;
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "Nit:";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlNit
            // 
            this.xrlNit.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlNit.BorderWidth = 1F;
            this.xrlNit.LocationFloat = new DevExpress.Utils.PointFloat(519.7918F, 6.583341F);
            this.xrlNit.Name = "xrlNit";
            this.xrlNit.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlNit.SizeF = new System.Drawing.SizeF(250F, 23F);
            this.xrlNit.StylePriority.UseBorders = false;
            this.xrlNit.StylePriority.UseBorderWidth = false;
            this.xrlNit.StylePriority.UseTextAlignment = false;
            this.xrlNit.Text = "[Nit]";
            this.xrlNit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlEmpresa
            // 
            this.xrlEmpresa.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlEmpresa.BorderWidth = 1F;
            this.xrlEmpresa.LocationFloat = new DevExpress.Utils.PointFloat(75.62499F, 6.583341F);
            this.xrlEmpresa.Name = "xrlEmpresa";
            this.xrlEmpresa.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlEmpresa.SizeF = new System.Drawing.SizeF(376.0417F, 23F);
            this.xrlEmpresa.StylePriority.UseBorders = false;
            this.xrlEmpresa.StylePriority.UseBorderWidth = false;
            this.xrlEmpresa.StylePriority.UseTextAlignment = false;
            this.xrlEmpresa.Text = "[Empresa]";
            this.xrlEmpresa.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlEmpresaTit
            // 
            this.xrlEmpresaTit.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlEmpresaTit.BorderWidth = 1F;
            this.xrlEmpresaTit.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrlEmpresaTit.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 6.583341F);
            this.xrlEmpresaTit.Name = "xrlEmpresaTit";
            this.xrlEmpresaTit.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlEmpresaTit.SizeF = new System.Drawing.SizeF(65.62498F, 23F);
            this.xrlEmpresaTit.StylePriority.UseBorders = false;
            this.xrlEmpresaTit.StylePriority.UseBorderWidth = false;
            this.xrlEmpresaTit.StylePriority.UseFont = false;
            this.xrlEmpresaTit.StylePriority.UseTextAlignment = false;
            this.xrlEmpresaTit.Text = "Empresa:";
            this.xrlEmpresaTit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrpLogo
            // 
            this.xrpLogo.ImageUrl = "~/Content/Images/Logo_Area_2012.png";
            this.xrpLogo.LocationFloat = new DevExpress.Utils.PointFloat(3.416729F, 10.00001F);
            this.xrpLogo.Name = "xrpLogo";
            this.xrpLogo.SizeF = new System.Drawing.SizeF(87.96754F, 104.7917F);
            this.xrpLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrpLogo});
            this.TopMargin.HeightF = 126F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(100F, 44.17591F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(678.5417F, 41.61581F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "CALCULO DEL VALOR DEL TRAMITE AMBIENTAL";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrPageInfo1});
            this.BottomMargin.HeightF = 41F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel4
            // 
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(674.0001F, 7.958285F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(69.79169F, 23F);
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Página";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(743.7918F, 7.958285F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(41.66663F, 23F);
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel3
            // 
            this.xrLabel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel3.BorderWidth = 1F;
            this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 269.3334F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(789.9999F, 23F);
            this.xrLabel3.StylePriority.UseBackColor = false;
            this.xrLabel3.StylePriority.UseBorders = false;
            this.xrLabel3.StylePriority.UseBorderWidth = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Valor del Auto de Inicio";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPanel3
            // 
            this.xrPanel3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlCostosTarifa_E,
            this.xrLabel8,
            this.xrLabel21,
            this.xrlCostosTramite_E,
            this.xrLabel23,
            this.xrlCostosTo_E,
            this.xrlCostosB_E,
            this.xrLabel14,
            this.xrLabel15,
            this.xrlCostosD_E,
            this.xrLabel17,
            this.xrlCostosC_E,
            this.xrlCostosA_E,
            this.xrLabel20});
            this.xrPanel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 292.3333F);
            this.xrPanel3.Name = "xrPanel3";
            this.xrPanel3.SizeF = new System.Drawing.SizeF(790F, 230.2083F);
            this.xrPanel3.StylePriority.UseBorders = false;
            // 
            // xrlCostosB_E
            // 
            this.xrlCostosB_E.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlCostosB_E.BorderWidth = 1F;
            this.xrlCostosB_E.LocationFloat = new DevExpress.Utils.PointFloat(604.7916F, 37.58334F);
            this.xrlCostosB_E.Name = "xrlCostosB_E";
            this.xrlCostosB_E.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlCostosB_E.SizeF = new System.Drawing.SizeF(165.0002F, 23F);
            this.xrlCostosB_E.StylePriority.UseBorders = false;
            this.xrlCostosB_E.StylePriority.UseBorderWidth = false;
            this.xrlCostosB_E.StylePriority.UseTextAlignment = false;
            this.xrlCostosB_E.Text = "[B]";
            this.xrlCostosB_E.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel14
            // 
            this.xrLabel14.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel14.BorderWidth = 1F;
            this.xrLabel14.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(10.00005F, 37.58334F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(532.3675F, 23F);
            this.xrLabel14.StylePriority.UseBorders = false;
            this.xrLabel14.StylePriority.UseBorderWidth = false;
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = "Gastos de Viaje (B):";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel15
            // 
            this.xrLabel15.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel15.BorderWidth = 1F;
            this.xrLabel15.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 99.41669F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(532.3676F, 23F);
            this.xrLabel15.StylePriority.UseBorders = false;
            this.xrLabel15.StylePriority.UseBorderWidth = false;
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.Text = "Gastos de Administración 25% (D):";
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlCostosD_E
            // 
            this.xrlCostosD_E.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlCostosD_E.BorderWidth = 1F;
            this.xrlCostosD_E.LocationFloat = new DevExpress.Utils.PointFloat(604.7916F, 99.41669F);
            this.xrlCostosD_E.Multiline = true;
            this.xrlCostosD_E.Name = "xrlCostosD_E";
            this.xrlCostosD_E.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlCostosD_E.SizeF = new System.Drawing.SizeF(165.0002F, 23F);
            this.xrlCostosD_E.StylePriority.UseBorders = false;
            this.xrlCostosD_E.StylePriority.UseBorderWidth = false;
            this.xrlCostosD_E.StylePriority.UseTextAlignment = false;
            this.xrlCostosD_E.Text = "[D]";
            this.xrlCostosD_E.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel17
            // 
            this.xrLabel17.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel17.BorderWidth = 1F;
            this.xrLabel17.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 68.41672F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(532.3676F, 23F);
            this.xrLabel17.StylePriority.UseBorders = false;
            this.xrLabel17.StylePriority.UseBorderWidth = false;
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            this.xrLabel17.Text = "Gastos Análisis de Laboratorio y Otros Trabajos Técnicos (C):";
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlCostosC_E
            // 
            this.xrlCostosC_E.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlCostosC_E.BorderWidth = 1F;
            this.xrlCostosC_E.LocationFloat = new DevExpress.Utils.PointFloat(604.7916F, 68.41666F);
            this.xrlCostosC_E.Multiline = true;
            this.xrlCostosC_E.Name = "xrlCostosC_E";
            this.xrlCostosC_E.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlCostosC_E.SizeF = new System.Drawing.SizeF(165.0002F, 23F);
            this.xrlCostosC_E.StylePriority.UseBorders = false;
            this.xrlCostosC_E.StylePriority.UseBorderWidth = false;
            this.xrlCostosC_E.StylePriority.UseTextAlignment = false;
            this.xrlCostosC_E.Text = "[C]";
            this.xrlCostosC_E.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrlCostosA_E
            // 
            this.xrlCostosA_E.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlCostosA_E.BorderWidth = 1F;
            this.xrlCostosA_E.LocationFloat = new DevExpress.Utils.PointFloat(604.7916F, 6.583282F);
            this.xrlCostosA_E.Name = "xrlCostosA_E";
            this.xrlCostosA_E.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlCostosA_E.SizeF = new System.Drawing.SizeF(165.0002F, 23F);
            this.xrlCostosA_E.StylePriority.UseBorders = false;
            this.xrlCostosA_E.StylePriority.UseBorderWidth = false;
            this.xrlCostosA_E.StylePriority.UseTextAlignment = false;
            this.xrlCostosA_E.Text = "[A]";
            this.xrlCostosA_E.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel20
            // 
            this.xrLabel20.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel20.BorderWidth = 1F;
            this.xrLabel20.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 6.583344F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(532.3676F, 23F);
            this.xrLabel20.StylePriority.UseBorders = false;
            this.xrLabel20.StylePriority.UseBorderWidth = false;
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.Text = "Gastos Por Sueldos y Honorarios (A):";
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlCostosTarifa_E
            // 
            this.xrlCostosTarifa_E.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlCostosTarifa_E.BorderWidth = 1F;
            this.xrlCostosTarifa_E.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrlCostosTarifa_E.LocationFloat = new DevExpress.Utils.PointFloat(604.7916F, 132.4167F);
            this.xrlCostosTarifa_E.Name = "xrlCostosTarifa_E";
            this.xrlCostosTarifa_E.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlCostosTarifa_E.SizeF = new System.Drawing.SizeF(165.0002F, 23F);
            this.xrlCostosTarifa_E.StylePriority.UseBorders = false;
            this.xrlCostosTarifa_E.StylePriority.UseBorderWidth = false;
            this.xrlCostosTarifa_E.StylePriority.UseFont = false;
            this.xrlCostosTarifa_E.StylePriority.UseTextAlignment = false;
            this.xrlCostosTarifa_E.Text = "[Tarifa]";
            this.xrlCostosTarifa_E.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel8.BorderWidth = 1F;
            this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(10.00012F, 132.4167F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(532.3675F, 23F);
            this.xrLabel8.StylePriority.UseBorders = false;
            this.xrLabel8.StylePriority.UseBorderWidth = false;
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "COSTO TOTAL DE LA TARIFA";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel21
            // 
            this.xrLabel21.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel21.BorderWidth = 1F;
            this.xrLabel21.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(10.00006F, 194.25F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(532.3676F, 23F);
            this.xrLabel21.StylePriority.UseBorders = false;
            this.xrLabel21.StylePriority.UseBorderWidth = false;
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.StylePriority.UseTextAlignment = false;
            this.xrLabel21.Text = "VALOR A CANCELAR POR TRAMITE";
            this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlCostosTramite_E
            // 
            this.xrlCostosTramite_E.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlCostosTramite_E.BorderWidth = 1F;
            this.xrlCostosTramite_E.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrlCostosTramite_E.LocationFloat = new DevExpress.Utils.PointFloat(604.7917F, 194.25F);
            this.xrlCostosTramite_E.Multiline = true;
            this.xrlCostosTramite_E.Name = "xrlCostosTramite_E";
            this.xrlCostosTramite_E.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlCostosTramite_E.SizeF = new System.Drawing.SizeF(165.0001F, 23F);
            this.xrlCostosTramite_E.StylePriority.UseBorders = false;
            this.xrlCostosTramite_E.StylePriority.UseBorderWidth = false;
            this.xrlCostosTramite_E.StylePriority.UseFont = false;
            this.xrlCostosTramite_E.StylePriority.UseTextAlignment = false;
            this.xrlCostosTramite_E.Text = "[Tramite]";
            this.xrlCostosTramite_E.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel23
            // 
            this.xrLabel23.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel23.BorderWidth = 1F;
            this.xrLabel23.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(10.00007F, 163.2501F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(532.3676F, 23F);
            this.xrLabel23.StylePriority.UseBorders = false;
            this.xrLabel23.StylePriority.UseBorderWidth = false;
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.Text = "Determinación de los Topes de las Tarifas (To)";
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlCostosTo_E
            // 
            this.xrlCostosTo_E.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlCostosTo_E.BorderWidth = 1F;
            this.xrlCostosTo_E.LocationFloat = new DevExpress.Utils.PointFloat(604.7917F, 163.25F);
            this.xrlCostosTo_E.Multiline = true;
            this.xrlCostosTo_E.Name = "xrlCostosTo_E";
            this.xrlCostosTo_E.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlCostosTo_E.SizeF = new System.Drawing.SizeF(165.0001F, 23F);
            this.xrlCostosTo_E.StylePriority.UseBorders = false;
            this.xrlCostosTo_E.StylePriority.UseBorderWidth = false;
            this.xrlCostosTo_E.StylePriority.UseTextAlignment = false;
            this.xrlCostosTo_E.Text = "[To]";
            this.xrlCostosTo_E.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrPanel4
            // 
            this.xrPanel4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlCostosTarifa_S,
            this.xrLabel13,
            this.xrLabel16,
            this.xrlCostosTramite_S,
            this.xrLabel19,
            this.xrlCostosTo_S,
            this.xrlCostosB_S,
            this.xrLabel25,
            this.xrLabel26,
            this.xrlCostosD_S,
            this.xrLabel28,
            this.xrlCostosC_S,
            this.xrlCostosA_S,
            this.xrLabel31});
            this.xrPanel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 558.2291F);
            this.xrPanel4.Name = "xrPanel4";
            this.xrPanel4.SizeF = new System.Drawing.SizeF(790F, 230.2083F);
            this.xrPanel4.StylePriority.UseBorders = false;
            // 
            // xrlCostosTarifa_S
            // 
            this.xrlCostosTarifa_S.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlCostosTarifa_S.BorderWidth = 1F;
            this.xrlCostosTarifa_S.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrlCostosTarifa_S.LocationFloat = new DevExpress.Utils.PointFloat(604.7916F, 132.4167F);
            this.xrlCostosTarifa_S.Name = "xrlCostosTarifa_S";
            this.xrlCostosTarifa_S.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlCostosTarifa_S.SizeF = new System.Drawing.SizeF(165.0002F, 23F);
            this.xrlCostosTarifa_S.StylePriority.UseBorders = false;
            this.xrlCostosTarifa_S.StylePriority.UseBorderWidth = false;
            this.xrlCostosTarifa_S.StylePriority.UseFont = false;
            this.xrlCostosTarifa_S.StylePriority.UseTextAlignment = false;
            this.xrlCostosTarifa_S.Text = "[Tarifa]";
            this.xrlCostosTarifa_S.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel13
            // 
            this.xrLabel13.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel13.BorderWidth = 1F;
            this.xrLabel13.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(10.00012F, 132.4167F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(532.3675F, 23F);
            this.xrLabel13.StylePriority.UseBorders = false;
            this.xrLabel13.StylePriority.UseBorderWidth = false;
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = "COSTO TOTAL DE LA TARIFA";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel16
            // 
            this.xrLabel16.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel16.BorderWidth = 1F;
            this.xrLabel16.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(10.00006F, 194.25F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(532.3676F, 23F);
            this.xrLabel16.StylePriority.UseBorders = false;
            this.xrLabel16.StylePriority.UseBorderWidth = false;
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "VALOR A CANCELAR POR TRAMITE";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlCostosTramite_S
            // 
            this.xrlCostosTramite_S.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlCostosTramite_S.BorderWidth = 1F;
            this.xrlCostosTramite_S.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrlCostosTramite_S.LocationFloat = new DevExpress.Utils.PointFloat(604.7917F, 194.25F);
            this.xrlCostosTramite_S.Multiline = true;
            this.xrlCostosTramite_S.Name = "xrlCostosTramite_S";
            this.xrlCostosTramite_S.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlCostosTramite_S.SizeF = new System.Drawing.SizeF(165.0001F, 23F);
            this.xrlCostosTramite_S.StylePriority.UseBorders = false;
            this.xrlCostosTramite_S.StylePriority.UseBorderWidth = false;
            this.xrlCostosTramite_S.StylePriority.UseFont = false;
            this.xrlCostosTramite_S.StylePriority.UseTextAlignment = false;
            this.xrlCostosTramite_S.Text = "[Tramite]";
            this.xrlCostosTramite_S.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel19
            // 
            this.xrLabel19.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel19.BorderWidth = 1F;
            this.xrLabel19.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(10.00007F, 163.2501F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(532.3676F, 23F);
            this.xrLabel19.StylePriority.UseBorders = false;
            this.xrLabel19.StylePriority.UseBorderWidth = false;
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.StylePriority.UseTextAlignment = false;
            this.xrLabel19.Text = "Determinación de los Topes de las Tarifas (To)";
            this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlCostosTo_S
            // 
            this.xrlCostosTo_S.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlCostosTo_S.BorderWidth = 1F;
            this.xrlCostosTo_S.LocationFloat = new DevExpress.Utils.PointFloat(604.7917F, 163.25F);
            this.xrlCostosTo_S.Multiline = true;
            this.xrlCostosTo_S.Name = "xrlCostosTo_S";
            this.xrlCostosTo_S.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlCostosTo_S.SizeF = new System.Drawing.SizeF(165.0001F, 23F);
            this.xrlCostosTo_S.StylePriority.UseBorders = false;
            this.xrlCostosTo_S.StylePriority.UseBorderWidth = false;
            this.xrlCostosTo_S.StylePriority.UseTextAlignment = false;
            this.xrlCostosTo_S.Text = "[To]";
            this.xrlCostosTo_S.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrlCostosB_S
            // 
            this.xrlCostosB_S.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlCostosB_S.BorderWidth = 1F;
            this.xrlCostosB_S.LocationFloat = new DevExpress.Utils.PointFloat(604.7916F, 37.58334F);
            this.xrlCostosB_S.Name = "xrlCostosB_S";
            this.xrlCostosB_S.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlCostosB_S.SizeF = new System.Drawing.SizeF(165.0002F, 23F);
            this.xrlCostosB_S.StylePriority.UseBorders = false;
            this.xrlCostosB_S.StylePriority.UseBorderWidth = false;
            this.xrlCostosB_S.StylePriority.UseTextAlignment = false;
            this.xrlCostosB_S.Text = "[B]";
            this.xrlCostosB_S.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel25
            // 
            this.xrLabel25.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel25.BorderWidth = 1F;
            this.xrLabel25.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(10.00005F, 37.58334F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(532.3675F, 23F);
            this.xrLabel25.StylePriority.UseBorders = false;
            this.xrLabel25.StylePriority.UseBorderWidth = false;
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.StylePriority.UseTextAlignment = false;
            this.xrLabel25.Text = "Gastos de Viaje (B):";
            this.xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel26
            // 
            this.xrLabel26.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel26.BorderWidth = 1F;
            this.xrLabel26.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 99.41669F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(532.3676F, 23F);
            this.xrLabel26.StylePriority.UseBorders = false;
            this.xrLabel26.StylePriority.UseBorderWidth = false;
            this.xrLabel26.StylePriority.UseFont = false;
            this.xrLabel26.StylePriority.UseTextAlignment = false;
            this.xrLabel26.Text = "Gastos de Administración 25% (D):";
            this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlCostosD_S
            // 
            this.xrlCostosD_S.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlCostosD_S.BorderWidth = 1F;
            this.xrlCostosD_S.LocationFloat = new DevExpress.Utils.PointFloat(604.7916F, 99.41669F);
            this.xrlCostosD_S.Multiline = true;
            this.xrlCostosD_S.Name = "xrlCostosD_S";
            this.xrlCostosD_S.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlCostosD_S.SizeF = new System.Drawing.SizeF(165.0002F, 23F);
            this.xrlCostosD_S.StylePriority.UseBorders = false;
            this.xrlCostosD_S.StylePriority.UseBorderWidth = false;
            this.xrlCostosD_S.StylePriority.UseTextAlignment = false;
            this.xrlCostosD_S.Text = "[D]";
            this.xrlCostosD_S.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel28
            // 
            this.xrLabel28.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel28.BorderWidth = 1F;
            this.xrLabel28.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 68.41672F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(532.3676F, 23F);
            this.xrLabel28.StylePriority.UseBorders = false;
            this.xrLabel28.StylePriority.UseBorderWidth = false;
            this.xrLabel28.StylePriority.UseFont = false;
            this.xrLabel28.StylePriority.UseTextAlignment = false;
            this.xrLabel28.Text = "Gastos Análisis de Laboratorio y Otros Trabajos Técnicos (C):";
            this.xrLabel28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlCostosC_S
            // 
            this.xrlCostosC_S.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlCostosC_S.BorderWidth = 1F;
            this.xrlCostosC_S.LocationFloat = new DevExpress.Utils.PointFloat(604.7916F, 68.41666F);
            this.xrlCostosC_S.Multiline = true;
            this.xrlCostosC_S.Name = "xrlCostosC_S";
            this.xrlCostosC_S.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlCostosC_S.SizeF = new System.Drawing.SizeF(165.0002F, 23F);
            this.xrlCostosC_S.StylePriority.UseBorders = false;
            this.xrlCostosC_S.StylePriority.UseBorderWidth = false;
            this.xrlCostosC_S.StylePriority.UseTextAlignment = false;
            this.xrlCostosC_S.Text = "[C]";
            this.xrlCostosC_S.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrlCostosA_S
            // 
            this.xrlCostosA_S.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlCostosA_S.BorderWidth = 1F;
            this.xrlCostosA_S.LocationFloat = new DevExpress.Utils.PointFloat(604.7916F, 6.583282F);
            this.xrlCostosA_S.Name = "xrlCostosA_S";
            this.xrlCostosA_S.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlCostosA_S.SizeF = new System.Drawing.SizeF(165.0002F, 23F);
            this.xrlCostosA_S.StylePriority.UseBorders = false;
            this.xrlCostosA_S.StylePriority.UseBorderWidth = false;
            this.xrlCostosA_S.StylePriority.UseTextAlignment = false;
            this.xrlCostosA_S.Text = "[A]";
            this.xrlCostosA_S.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel31
            // 
            this.xrLabel31.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel31.BorderWidth = 1F;
            this.xrLabel31.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 6.583344F);
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel31.SizeF = new System.Drawing.SizeF(532.3676F, 23F);
            this.xrLabel31.StylePriority.UseBorders = false;
            this.xrLabel31.StylePriority.UseBorderWidth = false;
            this.xrLabel31.StylePriority.UseFont = false;
            this.xrLabel31.StylePriority.UseTextAlignment = false;
            this.xrLabel31.Text = "Gastos Por Sueldos y Honorarios (A):";
            this.xrLabel31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel32
            // 
            this.xrLabel32.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrLabel32.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel32.BorderWidth = 1F;
            this.xrLabel32.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(0F, 535.2291F);
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(789.9999F, 23F);
            this.xrLabel32.StylePriority.UseBackColor = false;
            this.xrLabel32.StylePriority.UseBorders = false;
            this.xrLabel32.StylePriority.UseBorderWidth = false;
            this.xrLabel32.StylePriority.UseFont = false;
            this.xrLabel32.StylePriority.UseTextAlignment = false;
            this.xrLabel32.Text = "Valor de la Resolución";
            this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // CalculoReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Margins = new System.Drawing.Printing.Margins(30, 30, 126, 41);
            this.Version = "14.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
