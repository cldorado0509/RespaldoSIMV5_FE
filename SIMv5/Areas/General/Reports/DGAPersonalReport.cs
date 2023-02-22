using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using SIM.Areas.General.Models;

/// <summary>
/// Summary description for DGAPersonalReport
/// </summary>
public class DGAPersonalReport : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private XRLabel xrlEmail;
    private XRLabel xrlTelefono;
    private XRLabel xrlResponsable;
    private XRLabel xrlExperiencia;
    private XRLabel xrlDedicacion;
    private XRLabel xrlProfesion;
    private XRLabel xrlDocumento;
    private XRLabel xrlNombre;

    public DGAPersonalReport()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

    public void CargarDatos()
    {
        AddBoundLabel("xrlDocumento", "N_DOCUMENTO");
        AddBoundLabel("xrlNombre", "RAZON_SOCIAL");
        AddBoundLabel("xrlProfesion", "PROFESION");
        AddBoundLabel("xrlDedicacion", "N_DEDICACION");
        AddBoundLabel("xrlExperiencia", "N_EXPERIENCIA");
        AddBoundLabel("xrlResponsable", "S_ESRESPONSABLE");
        AddBoundLabel("xrlEmail", "CORREO_ELECTRONICO");
        AddBoundLabel("xrlTelefono", "TELEFONO");
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
            this.xrlEmail = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlNombre = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrlDocumento = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlProfesion = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlDedicacion = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlExperiencia = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlResponsable = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlTelefono = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlTelefono,
            this.xrlResponsable,
            this.xrlExperiencia,
            this.xrlDedicacion,
            this.xrlProfesion,
            this.xrlDocumento,
            this.xrlEmail,
            this.xrlNombre});
            this.Detail.HeightF = 23F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrlEmail
            // 
            this.xrlEmail.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlEmail.BorderWidth = 1F;
            this.xrlEmail.LocationFloat = new DevExpress.Utils.PointFloat(558.4167F, 0F);
            this.xrlEmail.Name = "xrlEmail";
            this.xrlEmail.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlEmail.SizeF = new System.Drawing.SizeF(143.5833F, 23F);
            this.xrlEmail.StylePriority.UseBorders = false;
            this.xrlEmail.StylePriority.UseBorderWidth = false;
            this.xrlEmail.StylePriority.UseTextAlignment = false;
            this.xrlEmail.Text = "[e-mail]";
            this.xrlEmail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlNombre
            // 
            this.xrlNombre.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlNombre.BorderWidth = 1F;
            this.xrlNombre.LocationFloat = new DevExpress.Utils.PointFloat(88.54166F, 0F);
            this.xrlNombre.Name = "xrlNombre";
            this.xrlNombre.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlNombre.SizeF = new System.Drawing.SizeF(166.6666F, 23F);
            this.xrlNombre.StylePriority.UseBorders = false;
            this.xrlNombre.StylePriority.UseBorderWidth = false;
            this.xrlNombre.StylePriority.UseTextAlignment = false;
            this.xrlNombre.Text = "[Nombre]";
            this.xrlNombre.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 23F;
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
            // xrlDocumento
            // 
            this.xrlDocumento.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlDocumento.BorderWidth = 1F;
            this.xrlDocumento.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrlDocumento.Name = "xrlDocumento";
            this.xrlDocumento.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlDocumento.SizeF = new System.Drawing.SizeF(88.54166F, 23F);
            this.xrlDocumento.StylePriority.UseBorders = false;
            this.xrlDocumento.StylePriority.UseBorderWidth = false;
            this.xrlDocumento.StylePriority.UseTextAlignment = false;
            this.xrlDocumento.Text = "[Documento]";
            this.xrlDocumento.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlProfesion
            // 
            this.xrlProfesion.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlProfesion.BorderWidth = 1F;
            this.xrlProfesion.LocationFloat = new DevExpress.Utils.PointFloat(255.2083F, 0F);
            this.xrlProfesion.Name = "xrlProfesion";
            this.xrlProfesion.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlProfesion.SizeF = new System.Drawing.SizeF(109.0832F, 23F);
            this.xrlProfesion.StylePriority.UseBorders = false;
            this.xrlProfesion.StylePriority.UseBorderWidth = false;
            this.xrlProfesion.StylePriority.UseTextAlignment = false;
            this.xrlProfesion.Text = "[Profesion]";
            this.xrlProfesion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlDedicacion
            // 
            this.xrlDedicacion.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlDedicacion.BorderWidth = 1F;
            this.xrlDedicacion.LocationFloat = new DevExpress.Utils.PointFloat(366.2915F, 0F);
            this.xrlDedicacion.Name = "xrlDedicacion";
            this.xrlDedicacion.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlDedicacion.SizeF = new System.Drawing.SizeF(68.74994F, 23F);
            this.xrlDedicacion.StylePriority.UseBorders = false;
            this.xrlDedicacion.StylePriority.UseBorderWidth = false;
            this.xrlDedicacion.StylePriority.UseTextAlignment = false;
            this.xrlDedicacion.Text = "[% Dedic]";
            this.xrlDedicacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlExperiencia
            // 
            this.xrlExperiencia.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlExperiencia.BorderWidth = 1F;
            this.xrlExperiencia.LocationFloat = new DevExpress.Utils.PointFloat(435.0415F, 0F);
            this.xrlExperiencia.Name = "xrlExperiencia";
            this.xrlExperiencia.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlExperiencia.SizeF = new System.Drawing.SizeF(68.74994F, 23F);
            this.xrlExperiencia.StylePriority.UseBorders = false;
            this.xrlExperiencia.StylePriority.UseBorderWidth = false;
            this.xrlExperiencia.StylePriority.UseTextAlignment = false;
            this.xrlExperiencia.Text = "[Exp Meses]";
            this.xrlExperiencia.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlResponsable
            // 
            this.xrlResponsable.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlResponsable.BorderWidth = 1F;
            this.xrlResponsable.LocationFloat = new DevExpress.Utils.PointFloat(506.3334F, 0F);
            this.xrlResponsable.Name = "xrlResponsable";
            this.xrlResponsable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlResponsable.SizeF = new System.Drawing.SizeF(52.08328F, 23F);
            this.xrlResponsable.StylePriority.UseBorders = false;
            this.xrlResponsable.StylePriority.UseBorderWidth = false;
            this.xrlResponsable.StylePriority.UseTextAlignment = false;
            this.xrlResponsable.Text = "[Resp]";
            this.xrlResponsable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlTelefono
            // 
            this.xrlTelefono.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlTelefono.BorderWidth = 1F;
            this.xrlTelefono.LocationFloat = new DevExpress.Utils.PointFloat(702F, 0F);
            this.xrlTelefono.Name = "xrlTelefono";
            this.xrlTelefono.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlTelefono.SizeF = new System.Drawing.SizeF(84.37506F, 23F);
            this.xrlTelefono.StylePriority.UseBorders = false;
            this.xrlTelefono.StylePriority.UseBorderWidth = false;
            this.xrlTelefono.StylePriority.UseTextAlignment = false;
            this.xrlTelefono.Text = "[telefono]";
            this.xrlTelefono.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DGAPersonalReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Margins = new System.Drawing.Printing.Margins(30, 30, 23, 0);
            this.Version = "14.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
