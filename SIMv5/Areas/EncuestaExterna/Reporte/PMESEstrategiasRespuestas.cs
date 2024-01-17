using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SIM.Areas.EncuestaExterna.Reporte
{
    public partial class PMESEstrategiasRespuestas : DevExpress.XtraReports.UI.XtraReport
    {
        public PMESEstrategiasRespuestas()
        {
            InitializeComponent();
        }

        public void CargarDatos()
        {
            AddBoundLabel("xrlPregunta", "S_PREGUNTA");
            AddBoundLabel("xrlRespuesta", "S_RESPUESTA");
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
    }
}
