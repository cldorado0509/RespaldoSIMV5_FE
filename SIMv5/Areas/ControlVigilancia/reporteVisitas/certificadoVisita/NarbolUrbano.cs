using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita
{
    public partial class NarbolUrbano : DevExpress.XtraReports.UI.XtraReport
    {
        public NarbolUrbano()
        {
            InitializeComponent();
        }

        private void NarbolUrbano_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrpEncabezado.Image = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/imagenes/cabezoteVisitas.png"));
        }
    }
}
