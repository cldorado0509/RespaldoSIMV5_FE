using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SIM.Areas.EncuestaExterna.Reporte
{
    public partial class PMESEstrategiasDetalleTP : DevExpress.XtraReports.UI.XtraReport
    {
        public PMESEstrategiasDetalleTP()
        {
            InitializeComponent();
        }

        public void CargarDatos()
        {
            AddBoundLabel("xrlEstrategia", "S_ESTRATEGIA");
            AddBoundLabel("xrlActividad", "S_ACTIVIDAD");
            AddBoundLabel("xrlPeriodicidad", "S_PERIODICIDAD");
            AddBoundLabel("xrlPublicoObjetivo", "S_PUBLICO_OBJETIVO");
            AddBoundLabel("xrlEvidencias", "S_EVIDENCIAS");
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
