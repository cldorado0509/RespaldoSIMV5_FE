using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SIM.Areas.Reporte.Reportes
{
    public partial class rptEncuestaRespuestas : DevExpress.XtraReports.UI.XtraReport
    {
        public rptEncuestaRespuestas()
        {
            InitializeComponent();
        }

        public void CargarDatos()
        {
            this.AddBoundLabel("ENCUESTA");
            this.AddBoundLabel("EMPRESA");
            this.AddBoundLabel("INSTALACION");
            this.AddBoundLabel("ENCUESTA_NO");
            this.AddBoundLabel("GRUPO");
            this.AddBoundLabel("NO_PREGUNTA");
            this.AddBoundLabel("PREGUNTA");
            this.AddBoundLabel("VALOR_NUMERO");
            this.AddBoundLabel("S_VALOR");
            this.AddBoundLabel("X_VALOR");
            this.AddBoundLabel("Y_VALOR");
            this.AddBoundLabel("OPCION");
            this.AddBoundLabel("OPCIONES");
        }

        public void AddLabelValue(string rfstrCampo, string rfstrValor)
        {
            this.FindControl("xrl" + rfstrCampo, true).Text = rfstrValor;
        }

        public void AddBoundLabel(string rfstrBindingMember)
        {
            this.FindControl("xrl" + rfstrBindingMember, true).DataBindings.Add("Text", DataSource, rfstrBindingMember);
        }

        public void AddBoundLabel(string rfstrBindingMember, string rfstrFormato)
        {
            AddBoundLabel(rfstrBindingMember, rfstrFormato, false);
        }

        public void AddBoundLabel(string rfstrBindingMember, string rfstrFormato, bool vlbolSumatoria)
        {
            this.FindControl("xrl" + rfstrBindingMember, true).DataBindings.Add("Text", DataSource, rfstrBindingMember, "{0:" + rfstrFormato + "}");
            if (vlbolSumatoria)
            {
                this.FindControl("xrl" + rfstrBindingMember + "Sum", true).DataBindings.Add("Text", DataSource, rfstrBindingMember, "{0:" + rfstrFormato + "}");
                ((XRLabel)this.FindControl("xrl" + rfstrBindingMember + "Sum", true)).Summary.Running = SummaryRunning.Report;
                ((XRLabel)this.FindControl("xrl" + rfstrBindingMember + "Sum", true)).Summary.FormatString = "{0:" + rfstrFormato + "}";
            }
        }
    }
}
