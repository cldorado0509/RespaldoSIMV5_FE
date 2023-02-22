using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using SIM.Areas.General.Models;
using SIM.Data.General;

/// <summary>
/// Summary description for DGAReport
/// </summary>
public class DGAReport : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRPictureBox xrpLogo;
    private XRPanel xrPanel1;
    private XRLabel xrlCodigo;
    private XRLabel xrlAno;
    private XRLabel xrLabel3;
    private XRLabel xrLabel2;
    private XRLabel xrLabel1;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private XRSubreport xrsPersonal;
    private XRLabel xrLabel4;
    private XRPageInfo xrPageInfo1;
    private XRLabel xrLabel10;
    private XRPanel xrPanel2;
    private XRLabel xrLabel6;
    private XRLabel xrlPlantasFiliales;
    private XRLabel xrlActivo;
    private XRLabel xrlTituloActivo;
    private XRLabel xrLabel5;
    private XRLabel xrlNit;
    private XRLabel xrlEmpresa;
    private XRLabel xrlEmpresaTit;
    private XRLabel xrLabel24;
    private XRPanel xrPanel5;
    private XRLabel xrlProduccionLimpiaSN;
    private XRLabel xrLabel34;
    private XRLabel xrlSGA;
    private XRLabel xrlEcoSN;
    private XRLabel xrlSGCSN;
    private XRLabel xrLabel27;
    private XRLabel xrlEco;
    private XRLabel xrLabel29;
    private XRLabel xrlSGC;
    private XRLabel xrlSGASN;
    private XRLabel xrLabel32;
    private XRLabel xrLabel23;
    private XRPanel xrPanel4;
    private XRPictureBox xrpOrganigrama;
    private XRLabel xrLabel13;
    private XRLabel xrLabel11;
    private XRLabel xrlPermisosAmbientales;
    private XRPanel xrPanel3;
    private XRLabel xrlAsesoriaSN;
    private XRLabel xrLabel20;
    private XRLabel xrLabel14;
    private XRLabel xrlFunciones;
    private XRLabel xrLabel16;
    private XRLabel xrlAsesoria;
    private XRLabel xrlComparteSN;
    private XRLabel xrlComparte;
    private XRLabel xrlFechaReporte;
    private XRLabel xrLabel7;
    private XRLabel xrlNombre;
    private XRLabel xrlEmail;
    private XRLabel xrlDocumento;
    private XRLabel xrlProfesion;
    private XRLabel xrlDedicacion;
    private XRLabel xrlExperiencia;
    private XRLabel xrlResponsable;
    private XRLabel xrlTelefono;
    private XRLabel xrlNumEmpleados;
    private XRLabel xrLabel12;
    private XRLabel xrLabel8;
    private XRPanel xrPanel6;
    private XRLabel xrlSeguimiento;
    private XRLabel xrLabel19;

    public DGAReport()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

    public void CargarDatos(DGA datosDGA, IList personalDGA)
    {
        if (datosDGA.N_VERSION == null || datosDGA.N_VERSION == 1)
            xrlTituloActivo.Text = "Activos (pesos $):";
        else
            xrlTituloActivo.Text = "Valor Ingresos por Actividades Ordinarias Anuales:";

        SetControlText("xrlAno", datosDGA.D_ANO.ToString("yyyy"));
        SetControlText("xrlCodigo", datosDGA.ID_DGA.ToString());
        SetControlText("xrlFechaReporte", ((DateTime)datosDGA.D_FREPORTE).ToString("yyyy/MM/dd HH:mm"));

        SetControlText("xrlEmpresa", datosDGA.TERCERO.S_RSOCIAL);
        SetControlText("xrlNit", datosDGA.TERCERO.N_DOCUMENTON.ToString());

        if (datosDGA.N_VERSION == null || datosDGA.N_VERSION == 1)
            SetControlText("xrlActivo", (datosDGA.N_ACTIVO == null ? "$ 0" : ((long)datosDGA.N_ACTIVO).ToString("$ #,##0.00")));
        else
            SetControlText("xrlActivo", (datosDGA.N_INGRESOS == null ? "$ 0" : ((long)datosDGA.N_INGRESOS).ToString("$ #,##0.00")));

        SetControlText("xrlNumEmpleados", (datosDGA.N_EMPLEADOS == null ? "0" : ((long)datosDGA.N_EMPLEADOS).ToString("#,##0")));
        SetControlText("xrlPlantasFiliales", datosDGA.S_FILIAL);
        SetControlText("xrlPermisosAmbientales", ObtenerLista(datosDGA.PERMISO_AMBIENTAL));
        SetControlText("xrlComparteSN", (datosDGA.S_COMPARTEDGA == "S" ? "Si" : "No"));
        SetControlText("xrlComparte", datosDGA.S_COMPARTEEMPRESA);
        SetControlText("xrlAsesoriaSN", (datosDGA.S_AGREMIACION == "S" ? "Si" : "No"));
        SetControlText("xrlAsesoria", datosDGA.S_AGREMIACIONASESORIA);
        SetControlText("xrlFunciones", datosDGA.S_FUNCION);

        string path = System.Configuration.ConfigurationManager.AppSettings["DocumentsDGAPath"] + "\\" + datosDGA.TERCERO.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA";
        if (datosDGA.S_ORGANIGRAMA != null && System.IO.File.Exists(path + "\\" + datosDGA.S_ORGANIGRAMA.ToString()))
        {
            xrpOrganigrama.Image = Image.FromFile(path + "\\" + datosDGA.S_ORGANIGRAMA.ToString());

            xrpOrganigrama.HeightF = xrpOrganigrama.Image.Height * (xrpOrganigrama.WidthF / xrpOrganigrama.Image.Width);
        }

        SetControlText("xrlSGASN", (datosDGA.S_ESSGA == "S" ? "Si" : "No"));
        SetControlText("xrlSGA", datosDGA.S_SGA);
        SetControlText("xrlSGCSN", (datosDGA.S_ESSGC == "S" ? "Si" : "No"));
        SetControlText("xrlSGC", datosDGA.S_SGC);
        SetControlText("xrlProduccionLimpiaSN", (datosDGA.S_PRODUCCIONMASLIMPIA == "S" ? "Si" : "No"));
        SetControlText("xrlEcoSN", (datosDGA.S_ESECOETIQUETADO == "S" ? "Si" : "No"));
        SetControlText("xrlEco", datosDGA.S_ECOETIQUETADO);

        SetControlText("xrlSeguimiento", datosDGA.S_SEGUIMIENTO);

        DGAPersonalReport personalReport = new DGAPersonalReport();
        personalReport.DataSource = personalDGA;
        personalReport.CargarDatos();

        xrsPersonal.ReportSource = personalReport;
    }

    private string ObtenerLista(ICollection<PERMISO_AMBIENTAL> permisosAmbientales)
    {
        string listaPermisos = "";
        foreach (PERMISO_AMBIENTAL permisoAmbiental in permisosAmbientales)
        {
            if (listaPermisos.Length > 0)
                listaPermisos += "," + permisoAmbiental.S_PERMISOAMBIENTAL;
            else
                listaPermisos = permisoAmbiental.S_PERMISOAMBIENTAL;
        }

        return listaPermisos;
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
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel6 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrlSeguimiento = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel5 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrlProduccionLimpiaSN = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlSGA = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlEcoSN = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlSGCSN = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlEco = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlSGC = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlSGASN = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel4 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrpOrganigrama = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrlNumEmpleados = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlPermisosAmbientales = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlPlantasFiliales = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlActivo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlTituloActivo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlNit = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlEmpresa = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlEmpresaTit = new DevExpress.XtraReports.UI.XRLabel();
            this.xrsPersonal = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrPanel3 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrlComparte = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlAsesoriaSN = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlFunciones = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlAsesoria = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlComparteSN = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlDocumento = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlEmail = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlNombre = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlProfesion = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlDedicacion = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlExperiencia = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlResponsable = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlTelefono = new DevExpress.XtraReports.UI.XRLabel();
            this.xrpLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrlFechaReporte = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlCodigo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlAno = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel8,
            this.xrPanel6,
            this.xrLabel24,
            this.xrPanel5,
            this.xrLabel23,
            this.xrPanel4,
            this.xrLabel13,
            this.xrLabel10,
            this.xrPanel2,
            this.xrPanel1,
            this.xrsPersonal,
            this.xrPanel3,
            this.xrLabel20,
            this.xrlDocumento,
            this.xrlEmail,
            this.xrlNombre,
            this.xrlProfesion,
            this.xrlDedicacion,
            this.xrlExperiencia,
            this.xrlResponsable,
            this.xrlTelefono});
            this.Detail.HeightF = 780.7396F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel8
            // 
            this.xrLabel8.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel8.BorderWidth = 1F;
            this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(6.357829E-05F, 711.9063F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(789.9999F, 23F);
            this.xrLabel8.StylePriority.UseBackColor = false;
            this.xrLabel8.StylePriority.UseBorders = false;
            this.xrLabel8.StylePriority.UseBorderWidth = false;
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "Seguimiento";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPanel6
            // 
            this.xrPanel6.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlSeguimiento});
            this.xrPanel6.LocationFloat = new DevExpress.Utils.PointFloat(6.357829E-05F, 734.9063F);
            this.xrPanel6.Name = "xrPanel6";
            this.xrPanel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 10, 10, 100F);
            this.xrPanel6.SizeF = new System.Drawing.SizeF(789.9999F, 45.83331F);
            this.xrPanel6.StylePriority.UseBorders = false;
            this.xrPanel6.StylePriority.UsePadding = false;
            // 
            // xrlSeguimiento
            // 
            this.xrlSeguimiento.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlSeguimiento.BorderWidth = 1F;
            this.xrlSeguimiento.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 12.00006F);
            this.xrlSeguimiento.Multiline = true;
            this.xrlSeguimiento.Name = "xrlSeguimiento";
            this.xrlSeguimiento.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlSeguimiento.SizeF = new System.Drawing.SizeF(769.9996F, 23F);
            this.xrlSeguimiento.StylePriority.UseBorders = false;
            this.xrlSeguimiento.StylePriority.UseBorderWidth = false;
            this.xrlSeguimiento.StylePriority.UseTextAlignment = false;
            this.xrlSeguimiento.Text = "[Seguimiento]";
            this.xrlSeguimiento.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel24
            // 
            this.xrLabel24.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrLabel24.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel24.BorderWidth = 1F;
            this.xrLabel24.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(0F, 479.3126F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(790F, 22.99994F);
            this.xrLabel24.StylePriority.UseBackColor = false;
            this.xrLabel24.StylePriority.UseBorders = false;
            this.xrLabel24.StylePriority.UseBorderWidth = false;
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.StylePriority.UseTextAlignment = false;
            this.xrLabel24.Text = "Información Adicional";
            this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPanel5
            // 
            this.xrPanel5.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel5.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlProduccionLimpiaSN,
            this.xrLabel34,
            this.xrlSGA,
            this.xrlEcoSN,
            this.xrlSGCSN,
            this.xrLabel27,
            this.xrlEco,
            this.xrLabel29,
            this.xrlSGC,
            this.xrlSGASN,
            this.xrLabel32});
            this.xrPanel5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 502.3126F);
            this.xrPanel5.Name = "xrPanel5";
            this.xrPanel5.SizeF = new System.Drawing.SizeF(788.9999F, 197.7084F);
            this.xrPanel5.StylePriority.UseBorders = false;
            // 
            // xrlProduccionLimpiaSN
            // 
            this.xrlProduccionLimpiaSN.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlProduccionLimpiaSN.BorderWidth = 1F;
            this.xrlProduccionLimpiaSN.LocationFloat = new DevExpress.Utils.PointFloat(295.4167F, 112.0833F);
            this.xrlProduccionLimpiaSN.Name = "xrlProduccionLimpiaSN";
            this.xrlProduccionLimpiaSN.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlProduccionLimpiaSN.SizeF = new System.Drawing.SizeF(41.74252F, 23F);
            this.xrlProduccionLimpiaSN.StylePriority.UseBorders = false;
            this.xrlProduccionLimpiaSN.StylePriority.UseBorderWidth = false;
            this.xrlProduccionLimpiaSN.StylePriority.UseTextAlignment = false;
            this.xrlProduccionLimpiaSN.Text = "[S/N]";
            this.xrlProduccionLimpiaSN.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel34
            // 
            this.xrLabel34.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel34.BorderWidth = 1F;
            this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(10.00005F, 161.7083F);
            this.xrLabel34.Name = "xrLabel34";
            this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel34.SizeF = new System.Drawing.SizeF(276.4166F, 23F);
            this.xrLabel34.StylePriority.UseBorders = false;
            this.xrLabel34.StylePriority.UseBorderWidth = false;
            this.xrLabel34.StylePriority.UseTextAlignment = false;
            this.xrLabel34.Text = "Hace uso del Eco-Etiquetado:";
            this.xrLabel34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlSGA
            // 
            this.xrlSGA.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlSGA.BorderWidth = 1F;
            this.xrlSGA.LocationFloat = new DevExpress.Utils.PointFloat(347.5F, 10.08331F);
            this.xrlSGA.Multiline = true;
            this.xrlSGA.Name = "xrlSGA";
            this.xrlSGA.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlSGA.SizeF = new System.Drawing.SizeF(432.4998F, 23F);
            this.xrlSGA.StylePriority.UseBorders = false;
            this.xrlSGA.StylePriority.UseBorderWidth = false;
            this.xrlSGA.StylePriority.UseTextAlignment = false;
            this.xrlSGA.Text = "[SGA]";
            this.xrlSGA.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlEcoSN
            // 
            this.xrlEcoSN.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlEcoSN.BorderWidth = 1F;
            this.xrlEcoSN.LocationFloat = new DevExpress.Utils.PointFloat(295.4167F, 161.7082F);
            this.xrlEcoSN.Name = "xrlEcoSN";
            this.xrlEcoSN.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlEcoSN.SizeF = new System.Drawing.SizeF(41.74252F, 23F);
            this.xrlEcoSN.StylePriority.UseBorders = false;
            this.xrlEcoSN.StylePriority.UseBorderWidth = false;
            this.xrlEcoSN.StylePriority.UseTextAlignment = false;
            this.xrlEcoSN.Text = "[S/N]";
            this.xrlEcoSN.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlSGCSN
            // 
            this.xrlSGCSN.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlSGCSN.BorderWidth = 1F;
            this.xrlSGCSN.LocationFloat = new DevExpress.Utils.PointFloat(295.4167F, 61.08331F);
            this.xrlSGCSN.Name = "xrlSGCSN";
            this.xrlSGCSN.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlSGCSN.SizeF = new System.Drawing.SizeF(41.74252F, 23F);
            this.xrlSGCSN.StylePriority.UseBorders = false;
            this.xrlSGCSN.StylePriority.UseBorderWidth = false;
            this.xrlSGCSN.StylePriority.UseTextAlignment = false;
            this.xrlSGCSN.Text = "[S/N]";
            this.xrlSGCSN.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel27
            // 
            this.xrLabel27.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel27.BorderWidth = 1F;
            this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 112.0833F);
            this.xrLabel27.Name = "xrLabel27";
            this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel27.SizeF = new System.Drawing.SizeF(276.4166F, 38.00006F);
            this.xrLabel27.StylePriority.UseBorders = false;
            this.xrLabel27.StylePriority.UseBorderWidth = false;
            this.xrLabel27.StylePriority.UseTextAlignment = false;
            this.xrLabel27.Text = "Es signatario del convenio de Producción Más Limpia:";
            this.xrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlEco
            // 
            this.xrlEco.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlEco.BorderWidth = 1F;
            this.xrlEco.LocationFloat = new DevExpress.Utils.PointFloat(347.5F, 161.7082F);
            this.xrlEco.Multiline = true;
            this.xrlEco.Name = "xrlEco";
            this.xrlEco.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlEco.SizeF = new System.Drawing.SizeF(432.4999F, 23F);
            this.xrlEco.StylePriority.UseBorders = false;
            this.xrlEco.StylePriority.UseBorderWidth = false;
            this.xrlEco.StylePriority.UseTextAlignment = false;
            this.xrlEco.Text = "[Eco Etiquetado]";
            this.xrlEco.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel29
            // 
            this.xrLabel29.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel29.BorderWidth = 1F;
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 61.08331F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(276.4166F, 39.66669F);
            this.xrLabel29.StylePriority.UseBorders = false;
            this.xrLabel29.StylePriority.UseBorderWidth = false;
            this.xrLabel29.StylePriority.UseTextAlignment = false;
            this.xrLabel29.Text = "Posee o está en proceso de implementar sistemas de gestión calidad:";
            this.xrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlSGC
            // 
            this.xrlSGC.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlSGC.BorderWidth = 1F;
            this.xrlSGC.LocationFloat = new DevExpress.Utils.PointFloat(347.5F, 61.08331F);
            this.xrlSGC.Multiline = true;
            this.xrlSGC.Name = "xrlSGC";
            this.xrlSGC.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlSGC.SizeF = new System.Drawing.SizeF(432.4999F, 23F);
            this.xrlSGC.StylePriority.UseBorders = false;
            this.xrlSGC.StylePriority.UseBorderWidth = false;
            this.xrlSGC.StylePriority.UseTextAlignment = false;
            this.xrlSGC.Text = "[SGC]";
            this.xrlSGC.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlSGASN
            // 
            this.xrlSGASN.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlSGASN.BorderWidth = 1F;
            this.xrlSGASN.LocationFloat = new DevExpress.Utils.PointFloat(295.4167F, 10.08336F);
            this.xrlSGASN.Name = "xrlSGASN";
            this.xrlSGASN.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlSGASN.SizeF = new System.Drawing.SizeF(41.74252F, 23F);
            this.xrlSGASN.StylePriority.UseBorders = false;
            this.xrlSGASN.StylePriority.UseBorderWidth = false;
            this.xrlSGASN.StylePriority.UseTextAlignment = false;
            this.xrlSGASN.Text = "[S/N]";
            this.xrlSGASN.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel32
            // 
            this.xrLabel32.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel32.BorderWidth = 1F;
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 10.08331F);
            this.xrLabel32.Multiline = true;
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(276.4167F, 39.66669F);
            this.xrLabel32.StylePriority.UseBorders = false;
            this.xrLabel32.StylePriority.UseBorderWidth = false;
            this.xrLabel32.StylePriority.UseTextAlignment = false;
            this.xrLabel32.Text = "Posee o está en proceso de implementar sistemas de gestión ambiental:";
            this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel23
            // 
            this.xrLabel23.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrLabel23.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel23.BorderWidth = 1F;
            this.xrLabel23.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(0F, 407.9792F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(790F, 23F);
            this.xrLabel23.StylePriority.UseBackColor = false;
            this.xrLabel23.StylePriority.UseBorders = false;
            this.xrLabel23.StylePriority.UseBorderWidth = false;
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.Text = "Organigrama";
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPanel4
            // 
            this.xrPanel4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrpOrganigrama});
            this.xrPanel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 430.9792F);
            this.xrPanel4.Name = "xrPanel4";
            this.xrPanel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 10, 10, 100F);
            this.xrPanel4.SizeF = new System.Drawing.SizeF(789.9999F, 33.33334F);
            this.xrPanel4.StylePriority.UseBorders = false;
            this.xrPanel4.StylePriority.UsePadding = false;
            // 
            // xrpOrganigrama
            // 
            this.xrpOrganigrama.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrpOrganigrama.LocationFloat = new DevExpress.Utils.PointFloat(4F, 5.000046F);
            this.xrpOrganigrama.Name = "xrpOrganigrama";
            this.xrpOrganigrama.SizeF = new System.Drawing.SizeF(780.9998F, 24.33331F);
            this.xrpOrganigrama.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            this.xrpOrganigrama.StylePriority.UseBorders = false;
            // 
            // xrLabel13
            // 
            this.xrLabel13.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrLabel13.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel13.BorderWidth = 1F;
            this.xrLabel13.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(3.178914E-05F, 299.9166F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(789.375F, 23F);
            this.xrLabel13.StylePriority.UseBackColor = false;
            this.xrLabel13.StylePriority.UseBorders = false;
            this.xrLabel13.StylePriority.UseBorderWidth = false;
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = "Información relacionada al DGA";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
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
            this.xrLabel10.Text = "Empresa";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPanel2
            // 
            this.xrPanel2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlNumEmpleados,
            this.xrLabel12,
            this.xrLabel11,
            this.xrlPermisosAmbientales,
            this.xrLabel6,
            this.xrlPlantasFiliales,
            this.xrlActivo,
            this.xrlTituloActivo});
            this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 69.79166F);
            this.xrPanel2.Name = "xrPanel2";
            this.xrPanel2.SizeF = new System.Drawing.SizeF(790F, 131.25F);
            this.xrPanel2.StylePriority.UseBorders = false;
            // 
            // xrlNumEmpleados
            // 
            this.xrlNumEmpleados.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlNumEmpleados.BorderWidth = 1F;
            this.xrlNumEmpleados.LocationFloat = new DevExpress.Utils.PointFloat(347.5F, 37.58333F);
            this.xrlNumEmpleados.Name = "xrlNumEmpleados";
            this.xrlNumEmpleados.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlNumEmpleados.SizeF = new System.Drawing.SizeF(133.4092F, 23F);
            this.xrlNumEmpleados.StylePriority.UseBorders = false;
            this.xrlNumEmpleados.StylePriority.UseBorderWidth = false;
            this.xrlNumEmpleados.StylePriority.UseTextAlignment = false;
            this.xrlNumEmpleados.Text = "[Num Empleados]";
            this.xrlNumEmpleados.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel12
            // 
            this.xrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel12.BorderWidth = 1F;
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(10.00005F, 37.58335F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(144.7916F, 23F);
            this.xrLabel12.StylePriority.UseBorders = false;
            this.xrLabel12.StylePriority.UseBorderWidth = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = "Número Empleados:";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel11.BorderWidth = 1F;
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 99.41668F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(327.1592F, 23F);
            this.xrLabel11.StylePriority.UseBorders = false;
            this.xrLabel11.StylePriority.UseBorderWidth = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "Permisos ambientales con que cuenta la Empresa:";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlPermisosAmbientales
            // 
            this.xrlPermisosAmbientales.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlPermisosAmbientales.BorderWidth = 1F;
            this.xrlPermisosAmbientales.LocationFloat = new DevExpress.Utils.PointFloat(347.5F, 99.41668F);
            this.xrlPermisosAmbientales.Multiline = true;
            this.xrlPermisosAmbientales.Name = "xrlPermisosAmbientales";
            this.xrlPermisosAmbientales.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlPermisosAmbientales.SizeF = new System.Drawing.SizeF(432.5001F, 23F);
            this.xrlPermisosAmbientales.StylePriority.UseBorders = false;
            this.xrlPermisosAmbientales.StylePriority.UseBorderWidth = false;
            this.xrlPermisosAmbientales.StylePriority.UseTextAlignment = false;
            this.xrlPermisosAmbientales.Text = "[Permisos Ambientales]";
            this.xrlPermisosAmbientales.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel6.BorderWidth = 1F;
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 68.41669F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(327.1592F, 23F);
            this.xrLabel6.StylePriority.UseBorders = false;
            this.xrLabel6.StylePriority.UseBorderWidth = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Posee otras plantas, filiales y/o sucursales en Colombia:";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlPlantasFiliales
            // 
            this.xrlPlantasFiliales.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlPlantasFiliales.BorderWidth = 1F;
            this.xrlPlantasFiliales.LocationFloat = new DevExpress.Utils.PointFloat(347.5F, 68.41667F);
            this.xrlPlantasFiliales.Multiline = true;
            this.xrlPlantasFiliales.Name = "xrlPlantasFiliales";
            this.xrlPlantasFiliales.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlPlantasFiliales.SizeF = new System.Drawing.SizeF(432.5001F, 22.99999F);
            this.xrlPlantasFiliales.StylePriority.UseBorders = false;
            this.xrlPlantasFiliales.StylePriority.UseBorderWidth = false;
            this.xrlPlantasFiliales.StylePriority.UseTextAlignment = false;
            this.xrlPlantasFiliales.Text = "[Plantas y Filiales]";
            this.xrlPlantasFiliales.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlActivo
            // 
            this.xrlActivo.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlActivo.BorderWidth = 1F;
            this.xrlActivo.LocationFloat = new DevExpress.Utils.PointFloat(347.5F, 6.583309F);
            this.xrlActivo.Name = "xrlActivo";
            this.xrlActivo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlActivo.SizeF = new System.Drawing.SizeF(133.4092F, 23F);
            this.xrlActivo.StylePriority.UseBorders = false;
            this.xrlActivo.StylePriority.UseBorderWidth = false;
            this.xrlActivo.StylePriority.UseTextAlignment = false;
            this.xrlActivo.Text = "[Activo]";
            this.xrlActivo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlTituloActivo
            // 
            this.xrlTituloActivo.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlTituloActivo.BorderWidth = 1F;
            this.xrlTituloActivo.LocationFloat = new DevExpress.Utils.PointFloat(9.99999F, 6.583351F);
            this.xrlTituloActivo.Name = "xrlTituloActivo";
            this.xrlTituloActivo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlTituloActivo.SizeF = new System.Drawing.SizeF(327.1592F, 23F);
            this.xrlTituloActivo.StylePriority.UseBorders = false;
            this.xrlTituloActivo.StylePriority.UseBorderWidth = false;
            this.xrlTituloActivo.StylePriority.UseTextAlignment = false;
            this.xrlTituloActivo.Text = "Activos (pesos $):";
            this.xrlTituloActivo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
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
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(484.375F, 6.583341F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(35.41669F, 23F);
            this.xrLabel5.StylePriority.UseBorders = false;
            this.xrLabel5.StylePriority.UseBorderWidth = false;
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
            this.xrlEmpresaTit.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 6.583341F);
            this.xrlEmpresaTit.Name = "xrlEmpresaTit";
            this.xrlEmpresaTit.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlEmpresaTit.SizeF = new System.Drawing.SizeF(65.62498F, 23F);
            this.xrlEmpresaTit.StylePriority.UseBorders = false;
            this.xrlEmpresaTit.StylePriority.UseBorderWidth = false;
            this.xrlEmpresaTit.StylePriority.UseTextAlignment = false;
            this.xrlEmpresaTit.Text = "Empresa:";
            this.xrlEmpresaTit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrsPersonal
            // 
            this.xrsPersonal.CanShrink = true;
            this.xrsPersonal.LocationFloat = new DevExpress.Utils.PointFloat(0F, 262.3333F);
            this.xrsPersonal.Name = "xrsPersonal";
            this.xrsPersonal.SizeF = new System.Drawing.SizeF(789.9999F, 26.12494F);
            // 
            // xrPanel3
            // 
            this.xrPanel3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlComparte,
            this.xrlAsesoriaSN,
            this.xrLabel14,
            this.xrlFunciones,
            this.xrLabel16,
            this.xrlAsesoria,
            this.xrlComparteSN,
            this.xrLabel19});
            this.xrPanel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 322.9166F);
            this.xrPanel3.Name = "xrPanel3";
            this.xrPanel3.SizeF = new System.Drawing.SizeF(789.9999F, 70.68771F);
            this.xrPanel3.StylePriority.UseBorders = false;
            // 
            // xrlComparte
            // 
            this.xrlComparte.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlComparte.BorderWidth = 1F;
            this.xrlComparte.LocationFloat = new DevExpress.Utils.PointFloat(294.3747F, 0F);
            this.xrlComparte.Multiline = true;
            this.xrlComparte.Name = "xrlComparte";
            this.xrlComparte.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlComparte.SizeF = new System.Drawing.SizeF(485.6251F, 23F);
            this.xrlComparte.StylePriority.UseBorders = false;
            this.xrlComparte.StylePriority.UseBorderWidth = false;
            this.xrlComparte.StylePriority.UseTextAlignment = false;
            this.xrlComparte.Text = "[Empresas que Comparte]";
            this.xrlComparte.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrlComparte.Visible = false;
            // 
            // xrlAsesoriaSN
            // 
            this.xrlAsesoriaSN.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlAsesoriaSN.BorderWidth = 1F;
            this.xrlAsesoriaSN.LocationFloat = new DevExpress.Utils.PointFloat(369.2915F, 7.999992F);
            this.xrlAsesoriaSN.Name = "xrlAsesoriaSN";
            this.xrlAsesoriaSN.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlAsesoriaSN.SizeF = new System.Drawing.SizeF(41.74252F, 23F);
            this.xrlAsesoriaSN.StylePriority.UseBorders = false;
            this.xrlAsesoriaSN.StylePriority.UseBorderWidth = false;
            this.xrlAsesoriaSN.StylePriority.UseTextAlignment = false;
            this.xrlAsesoriaSN.Text = "[S/N]";
            this.xrlAsesoriaSN.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel14
            // 
            this.xrLabel14.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel14.BorderWidth = 1F;
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(10.00005F, 39F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(211.5342F, 23F);
            this.xrLabel14.StylePriority.UseBorders = false;
            this.xrLabel14.StylePriority.UseBorderWidth = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = "Funciones del DGA:";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlFunciones
            // 
            this.xrlFunciones.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlFunciones.BorderWidth = 1F;
            this.xrlFunciones.LocationFloat = new DevExpress.Utils.PointFloat(294.375F, 39F);
            this.xrlFunciones.Multiline = true;
            this.xrlFunciones.Name = "xrlFunciones";
            this.xrlFunciones.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlFunciones.SizeF = new System.Drawing.SizeF(485.6248F, 23F);
            this.xrlFunciones.StylePriority.UseBorders = false;
            this.xrlFunciones.StylePriority.UseBorderWidth = false;
            this.xrlFunciones.StylePriority.UseTextAlignment = false;
            this.xrlFunciones.Text = "[Funciones DGA]";
            this.xrlFunciones.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel16
            // 
            this.xrLabel16.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel16.BorderWidth = 1F;
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(10.00006F, 8F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(357.2914F, 23F);
            this.xrLabel16.StylePriority.UseBorders = false;
            this.xrLabel16.StylePriority.UseBorderWidth = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "Recibe algún tipo de asesoría de Agremiaciones o un Consultor?";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlAsesoria
            // 
            this.xrlAsesoria.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlAsesoria.BorderWidth = 1F;
            this.xrlAsesoria.LocationFloat = new DevExpress.Utils.PointFloat(422.5759F, 8F);
            this.xrlAsesoria.Multiline = true;
            this.xrlAsesoria.Name = "xrlAsesoria";
            this.xrlAsesoria.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlAsesoria.SizeF = new System.Drawing.SizeF(357.4242F, 23F);
            this.xrlAsesoria.StylePriority.UseBorders = false;
            this.xrlAsesoria.StylePriority.UseBorderWidth = false;
            this.xrlAsesoria.StylePriority.UseTextAlignment = false;
            this.xrlAsesoria.Text = "[Asesoria de Agremiaciones]";
            this.xrlAsesoria.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlComparteSN
            // 
            this.xrlComparteSN.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlComparteSN.BorderWidth = 1F;
            this.xrlComparteSN.LocationFloat = new DevExpress.Utils.PointFloat(222.5001F, 0F);
            this.xrlComparteSN.Name = "xrlComparteSN";
            this.xrlComparteSN.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlComparteSN.SizeF = new System.Drawing.SizeF(41.74252F, 23F);
            this.xrlComparteSN.StylePriority.UseBorders = false;
            this.xrlComparteSN.StylePriority.UseBorderWidth = false;
            this.xrlComparteSN.StylePriority.UseTextAlignment = false;
            this.xrlComparteSN.Text = "[S/N]";
            this.xrlComparteSN.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrlComparteSN.Visible = false;
            // 
            // xrLabel19
            // 
            this.xrLabel19.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel19.BorderWidth = 1F;
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(10.00005F, 0F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(120.8333F, 23F);
            this.xrLabel19.StylePriority.UseBorders = false;
            this.xrLabel19.StylePriority.UseBorderWidth = false;
            this.xrLabel19.StylePriority.UseTextAlignment = false;
            this.xrLabel19.Text = "DGA se comparte :";
            this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel19.Visible = false;
            // 
            // xrLabel20
            // 
            this.xrLabel20.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrLabel20.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel20.BorderWidth = 1F;
            this.xrLabel20.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(6.357829E-05F, 213.0417F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(789.9999F, 23F);
            this.xrLabel20.StylePriority.UseBackColor = false;
            this.xrLabel20.StylePriority.UseBorders = false;
            this.xrLabel20.StylePriority.UseBorderWidth = false;
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.Text = "Personal DGA";
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlDocumento
            // 
            this.xrlDocumento.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlDocumento.BorderWidth = 1F;
            this.xrlDocumento.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrlDocumento.LocationFloat = new DevExpress.Utils.PointFloat(2.999996F, 236.0417F);
            this.xrlDocumento.Name = "xrlDocumento";
            this.xrlDocumento.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlDocumento.SizeF = new System.Drawing.SizeF(88.54166F, 23F);
            this.xrlDocumento.StylePriority.UseBorders = false;
            this.xrlDocumento.StylePriority.UseBorderWidth = false;
            this.xrlDocumento.StylePriority.UseFont = false;
            this.xrlDocumento.StylePriority.UseTextAlignment = false;
            this.xrlDocumento.Text = "Documento";
            this.xrlDocumento.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlEmail
            // 
            this.xrlEmail.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlEmail.BorderWidth = 1F;
            this.xrlEmail.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrlEmail.LocationFloat = new DevExpress.Utils.PointFloat(561.4167F, 236.0417F);
            this.xrlEmail.Name = "xrlEmail";
            this.xrlEmail.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlEmail.SizeF = new System.Drawing.SizeF(143.5833F, 23F);
            this.xrlEmail.StylePriority.UseBorders = false;
            this.xrlEmail.StylePriority.UseBorderWidth = false;
            this.xrlEmail.StylePriority.UseFont = false;
            this.xrlEmail.StylePriority.UseTextAlignment = false;
            this.xrlEmail.Text = "e-mail";
            this.xrlEmail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlNombre
            // 
            this.xrlNombre.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlNombre.BorderWidth = 1F;
            this.xrlNombre.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrlNombre.LocationFloat = new DevExpress.Utils.PointFloat(91.54165F, 236.0417F);
            this.xrlNombre.Name = "xrlNombre";
            this.xrlNombre.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlNombre.SizeF = new System.Drawing.SizeF(166.6666F, 23F);
            this.xrlNombre.StylePriority.UseBorders = false;
            this.xrlNombre.StylePriority.UseBorderWidth = false;
            this.xrlNombre.StylePriority.UseFont = false;
            this.xrlNombre.StylePriority.UseTextAlignment = false;
            this.xrlNombre.Text = "Nombre";
            this.xrlNombre.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlProfesion
            // 
            this.xrlProfesion.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlProfesion.BorderWidth = 1F;
            this.xrlProfesion.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrlProfesion.LocationFloat = new DevExpress.Utils.PointFloat(258.2083F, 236.0417F);
            this.xrlProfesion.Name = "xrlProfesion";
            this.xrlProfesion.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlProfesion.SizeF = new System.Drawing.SizeF(109.0832F, 23F);
            this.xrlProfesion.StylePriority.UseBorders = false;
            this.xrlProfesion.StylePriority.UseBorderWidth = false;
            this.xrlProfesion.StylePriority.UseFont = false;
            this.xrlProfesion.StylePriority.UseTextAlignment = false;
            this.xrlProfesion.Text = "Profesión";
            this.xrlProfesion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlDedicacion
            // 
            this.xrlDedicacion.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlDedicacion.BorderWidth = 1F;
            this.xrlDedicacion.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrlDedicacion.LocationFloat = new DevExpress.Utils.PointFloat(369.2915F, 236.0417F);
            this.xrlDedicacion.Name = "xrlDedicacion";
            this.xrlDedicacion.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlDedicacion.SizeF = new System.Drawing.SizeF(64.74994F, 23F);
            this.xrlDedicacion.StylePriority.UseBorders = false;
            this.xrlDedicacion.StylePriority.UseBorderWidth = false;
            this.xrlDedicacion.StylePriority.UseFont = false;
            this.xrlDedicacion.StylePriority.UseTextAlignment = false;
            this.xrlDedicacion.Text = "% Dedic";
            this.xrlDedicacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlExperiencia
            // 
            this.xrlExperiencia.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlExperiencia.BorderWidth = 1F;
            this.xrlExperiencia.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrlExperiencia.LocationFloat = new DevExpress.Utils.PointFloat(435.0415F, 236.0417F);
            this.xrlExperiencia.Name = "xrlExperiencia";
            this.xrlExperiencia.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlExperiencia.SizeF = new System.Drawing.SizeF(71.74994F, 23F);
            this.xrlExperiencia.StylePriority.UseBorders = false;
            this.xrlExperiencia.StylePriority.UseBorderWidth = false;
            this.xrlExperiencia.StylePriority.UseFont = false;
            this.xrlExperiencia.StylePriority.UseTextAlignment = false;
            this.xrlExperiencia.Text = "Exp(meses)";
            this.xrlExperiencia.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlResponsable
            // 
            this.xrlResponsable.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlResponsable.BorderWidth = 1F;
            this.xrlResponsable.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrlResponsable.LocationFloat = new DevExpress.Utils.PointFloat(509.3333F, 236.0417F);
            this.xrlResponsable.Name = "xrlResponsable";
            this.xrlResponsable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlResponsable.SizeF = new System.Drawing.SizeF(52.08328F, 23F);
            this.xrlResponsable.StylePriority.UseBorders = false;
            this.xrlResponsable.StylePriority.UseBorderWidth = false;
            this.xrlResponsable.StylePriority.UseFont = false;
            this.xrlResponsable.StylePriority.UseTextAlignment = false;
            this.xrlResponsable.Text = "Resp.";
            this.xrlResponsable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlTelefono
            // 
            this.xrlTelefono.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlTelefono.BorderWidth = 1F;
            this.xrlTelefono.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrlTelefono.LocationFloat = new DevExpress.Utils.PointFloat(705F, 236.0417F);
            this.xrlTelefono.Name = "xrlTelefono";
            this.xrlTelefono.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlTelefono.SizeF = new System.Drawing.SizeF(84.37506F, 23F);
            this.xrlTelefono.StylePriority.UseBorders = false;
            this.xrlTelefono.StylePriority.UseBorderWidth = false;
            this.xrlTelefono.StylePriority.UseFont = false;
            this.xrlTelefono.StylePriority.UseTextAlignment = false;
            this.xrlTelefono.Text = "Teléfono";
            this.xrlTelefono.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
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
            this.xrlFechaReporte,
            this.xrLabel7,
            this.xrlCodigo,
            this.xrlAno,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel1,
            this.xrpLogo});
            this.TopMargin.HeightF = 126F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrlFechaReporte
            // 
            this.xrlFechaReporte.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlFechaReporte.LocationFloat = new DevExpress.Utils.PointFloat(628.076F, 79.42589F);
            this.xrlFechaReporte.Name = "xrlFechaReporte";
            this.xrlFechaReporte.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlFechaReporte.SizeF = new System.Drawing.SizeF(151.9241F, 16.61581F);
            this.xrlFechaReporte.StylePriority.UseFont = false;
            this.xrlFechaReporte.StylePriority.UseTextAlignment = false;
            this.xrlFechaReporte.Text = "[Fecha Reporte]";
            this.xrlFechaReporte.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(484.375F, 79.42589F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(143.7009F, 16.61581F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "Fecha Reporte:";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlCodigo
            // 
            this.xrlCodigo.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlCodigo.LocationFloat = new DevExpress.Utils.PointFloat(350.3256F, 79.42589F);
            this.xrlCodigo.Name = "xrlCodigo";
            this.xrlCodigo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlCodigo.SizeF = new System.Drawing.SizeF(101.7425F, 16.61581F);
            this.xrlCodigo.StylePriority.UseFont = false;
            this.xrlCodigo.StylePriority.UseTextAlignment = false;
            this.xrlCodigo.Text = "[Código]";
            this.xrlCodigo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlAno
            // 
            this.xrlAno.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlAno.LocationFloat = new DevExpress.Utils.PointFloat(168.4091F, 79.42589F);
            this.xrlAno.Name = "xrlAno";
            this.xrlAno.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlAno.SizeF = new System.Drawing.SizeF(85.07582F, 16.61581F);
            this.xrlAno.StylePriority.UseFont = false;
            this.xrlAno.StylePriority.UseTextAlignment = false;
            this.xrlAno.Text = "[Año]";
            this.xrlAno.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(275.3747F, 79.42589F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(74.95087F, 16.61581F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Código:";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(111.4583F, 79.42589F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(56.95081F, 16.61581F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Año:";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(111.4583F, 36.04167F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(678.5417F, 16.61581F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "DEPARTAMENTO DE GESTIÓN AMBIENTAL";
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
            // DGAReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Margins = new System.Drawing.Printing.Margins(30, 30, 126, 41);
            this.Version = "18.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
