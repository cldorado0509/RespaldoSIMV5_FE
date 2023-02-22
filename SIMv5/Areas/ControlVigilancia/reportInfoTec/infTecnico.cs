using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SIM.Areas.ControlVigilancia.reportInfoTec
{
    public partial class infTecnico : DevExpress.XtraReports.UI.XtraReport
    {
        public infTecnico()
        {
            InitializeComponent();
        }

        private void xrLabel2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void xrPageInfo2_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            //if(e.PageCount==1)
            //{
            //    //((XRPageInfo)sender).Visible = true;
            //    xrPageInfo2.Visible = false;
            //}
        }

    }
}
