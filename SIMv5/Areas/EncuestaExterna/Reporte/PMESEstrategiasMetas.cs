using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SIM.Areas.EncuestaExterna.Reporte
{
    public partial class PMESEstrategiasMetas : DevExpress.XtraReports.UI.XtraReport
    {
        public PMESEstrategiasMetas()
        {
            InitializeComponent();
        }

        public void CargarDatos()
        {
            AddBoundLabel("xrlMeta", "S_META");
            AddBoundLabel("xrlMedicion", "S_MEDICION");
            AddBoundLabel("xrlValor", "N_VALOR");
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
