using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SIM.Areas.EncuestaExterna.Reporte
{
    public partial class PMESEstrategiasDetalleTF : DevExpress.XtraReports.UI.XtraReport
    {
        public PMESEstrategiasDetalleTF()
        {
            InitializeComponent();
        }

        public void CargarDatos()
        {
            AddBoundLabel("xrlEstrategia", "S_ESTRATEGIA");
            AddBoundLabel("xrlObjetivo", "S_ACTIVIDAD");
            AddBoundLabel("xrlPublicoObjetivo", "S_PUBLICO_OBJETIVO");
            AddBoundLabel("xrlComunicacionInterna", "S_COMUNICACION_INTERNA");
            AddBoundLabel("xrlComunicacionExterna", "S_COMUNICACION_EXTERNA");
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
