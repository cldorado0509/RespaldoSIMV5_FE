using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DashboardDesignerClass;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using DevExpress.DashboardCommon;
using DevExpress.DataAccess.ConnectionParameters;

namespace DashBoardDesigner
{
    public partial class FRMDashboardDesigner : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FRMDashboardDesigner()
        {
            InitializeComponent();
        }

        private void FRMDashboardDesigner_Load(object sender, EventArgs e)
        {
            /*
            var visitas = new DashboardDesignerClass.VisitasDataSource.VW_VISITASDataTable();

            //var visitasAdapter = new DashboardDesignerClass.VisitasDataSourceTableAdapters.VW_VISITASTableAdapter();
            //visitasAdapter.Fill(visitas);

            var terceros = new DashboardDesignerClass.TercerosDataSource.QRY_INSTALACION_EMPRESA_MRQDataTable();

            //var tercerosAdapter = new DashboardDesignerClass.TercerosDataSourceTableAdapters.QRY_INSTALACION_EMPRESA_MRQTableAdapter();
            //tercerosAdapter.Fill(terceros);

            //dbdDesigner.Dashboard.AddDataSource("Visitas", (new DataSet2()).VWR_VISITA);
            //dbdDesigner.Dashboard.AddDataSource("Visitas", visitas);
            dbdDesigner.Dashboard.AddDataSource("Visitas", visitas);
            //dbdDesigner.Dashboard.AddDataSource("Terceros", terceros);
            dbdDesigner.Dashboard.AddDataSource("Terceros", terceros);
            */
        }

        /*private void dbdDesigner_DataLoading(object sender, DevExpress.DataAccess.DataLoadingEventArgs e)
        {
            e.Data = Consultas.ObtenerDatosFuente(e.DataSourceName, ConfigurationManager.ConnectionStrings["SIMOracle"].ConnectionString, new List<Parametro>() { new Parametro() { Nombre = ":idTercero", Valor = 414 }, new Parametro() { Nombre = ":vigenciaEncuesta", Valor = 2018 } });
        }*/

        private void dbdDesigner_DataLoading(object sender, DevExpress.DashboardCommon.DataLoadingEventArgs e)
        {
            if (e.DataSourceName.Substring(0, 4) == "PMES")
                e.Data = Consultas.ObtenerDatosFuente(e.DataSourceName, ConfigurationManager.ConnectionStrings["SIMOracle"].ConnectionString, new List<Parametro>() { new Parametro() { Nombre = ":idTercero", Valor = 148285 }, new Parametro() { Nombre = ":vigenciaEncuesta", Valor = 2020 }, new Parametro() { Nombre = ":idEncuesta", Valor = 2 } });
            else if (e.DataSourceName.Substring(0, 5) == "SPMES")
                //e.Data = Consultas.ObtenerDatosFuente(e.DataSourceName, ConfigurationManager.ConnectionStrings["SIMOracle"].ConnectionString, new List<Parametro>() { new Parametro() { Nombre = ":idTercero", Valor = 148285 }, new Parametro() { Nombre = ":vigenciaEncuesta1", Valor = 2019 }, new Parametro() { Nombre = ":vigenciaEncuesta2", Valor = 2020 } });
                //e.Data = Consultas.ObtenerDatosFuente(e.DataSourceName, ConfigurationManager.ConnectionStrings["SIMOracle"].ConnectionString, new List<Parametro>() { new Parametro() { Nombre = ":idTercero", Valor = 148285 }, new Parametro() { Nombre = ":vigenciaEncuesta", Valor = 2020 }, new Parametro() { Nombre = ":vigenciaEncuestaBase", Valor = 2018 } });
                e.Data = Consultas.ObtenerDatosFuente(e.DataSourceName, ConfigurationManager.ConnectionStrings["SIMOracle"].ConnectionString, new List<Parametro>() { new Parametro() { Nombre = ":idTercero", Valor = 71254 }, new Parametro() { Nombre = ":vigenciaEncuesta", Valor = 2021 }, new Parametro() { Nombre = ":vigenciaEncuestaBase", Valor = 2018 } });
            else
                e.Data = Consultas.ObtenerDatosFuente(e.DataSourceName, ConfigurationManager.ConnectionStrings["SIMOracle"].ConnectionString, new List<Parametro>() { new Parametro() { Nombre = ":idTercero", Valor = 148285 }, new Parametro() { Nombre = ":vigenciaEncuesta", Valor = 2020 }, new Parametro() { Nombre = ":idEncuesta", Valor = 2 } });
        }

        private void dbdDesigner_DashboardCreating(object sender, DevExpress.DashboardWin.DashboardCreatingEventArgs e)
        {
            e.Handled = true;

            dbdDesigner.HandleDashboardClosing();
            dbdDesigner.Dashboard = new DevExpress.DashboardCommon.Dashboard();

            var fuentesDatos = new FRMFuenteDatos();
            fuentesDatos.ShowDialog();
            
            if (fuentesDatos.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (fuentesDatos.FuenteDatosSeleccionadas != null)
                {
                    foreach (FuenteDatos fuenteDatos in fuentesDatos.FuenteDatosSeleccionadas)
                    {
                        DashboardObjectDataSource objectDataSource;

                        if (fuenteDatos.Nombre.Substring(0, 4) == "PMES")
                            objectDataSource = new DashboardObjectDataSource(fuenteDatos.Nombre, Consultas.ObtenerDatosFuente(fuenteDatos.Nombre, ConfigurationManager.ConnectionStrings["SIMOracle"].ConnectionString, new List<Parametro>() { new Parametro() { Nombre = ":idTercero", Valor = 148285 }, new Parametro() { Nombre = ":vigenciaEncuesta", Valor = 2020 }, new Parametro() { Nombre = ":idEncuesta", Valor = 2 } }));
                        else if (fuenteDatos.Nombre.Substring(0, 5) == "SPMES")
                            objectDataSource = new DashboardObjectDataSource(fuenteDatos.Nombre, Consultas.ObtenerDatosFuente(fuenteDatos.Nombre, ConfigurationManager.ConnectionStrings["SIMOracle"].ConnectionString, new List<Parametro>() { new Parametro() { Nombre = ":idTercero", Valor = 148285 }, new Parametro() { Nombre = ":vigenciaEncuesta", Valor = 2020 }, new Parametro() { Nombre = ":vigenciaEncuestaBase", Valor = 2018 } }));
                        else
                            objectDataSource = new DashboardObjectDataSource(fuenteDatos.Nombre, Consultas.ObtenerDatosFuente(fuenteDatos.Nombre, ConfigurationManager.ConnectionStrings["SIMOracle"].ConnectionString, new List<Parametro>() { new Parametro() { Nombre = ":idTercero", Valor = 148285 }, new Parametro() { Nombre = ":vigenciaEncuesta", Valor = 2020 }, new Parametro() { Nombre = ":idEncuesta", Valor = 2 } }));

                        //dbdDesigner.Dashboard.AddDataSource(fuenteDatos.Nombre, Consultas.ObtenerDatosFuente(fuenteDatos.Nombre, ConfigurationManager.ConnectionStrings["SIMOracle"].ConnectionString, new List<Parametro>() { new Parametro() { Nombre = ":idTercero", Valor = 414 }, new Parametro() { Nombre = ":vigenciaEncuesta", Valor = 2018 } }));
                        dbdDesigner.Dashboard.DataSources.Add(objectDataSource);
                    }
                }
            }
        }
    }
}
