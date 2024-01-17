using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using SIM.Data;

namespace SIM.Areas.EncuestaExterna.Reporte
{
    public partial class PMESEstrategias : DevExpress.XtraReports.UI.XtraReport
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public PMESEstrategias()
        {
            InitializeComponent();
        }

        public void CargarDatos(int idEstado)
        {
            var idEstrategiasTercero = (from et in dbSIM.PMES_ESTRATEGIAS_TERCERO where et.ID_ESTADO == idEstado select et.ID).FirstOrDefault();
            var grupos1 = (from gr in dbSIM.PMES_ESTRATEGIAS_GRUPO where gr.ID_ENCABEZADO == 1 select gr).ToList();

            PMESEstrategiasGrupo gruposReport1 = new PMESEstrategiasGrupo();
            gruposReport1.DataSource = grupos1;
            gruposReport1.CargarDatos(idEstrategiasTercero);

            xrsEstrategiasGrupos1.ReportSource = gruposReport1;

            var grupos2 = (from gr in dbSIM.PMES_ESTRATEGIAS_GRUPO where gr.ID_ENCABEZADO == 2 select gr).ToList();

            PMESEstrategiasGrupo gruposReport2 = new PMESEstrategiasGrupo();
            gruposReport2.DataSource = grupos2;
            gruposReport2.CargarDatos(idEstrategiasTercero);

            xrsEstrategiasGrupos2.ReportSource = gruposReport2;

            var respuestasEstrategias = (from pe in dbSIM.PMES_ESTRATEGIAS_PREGUNTA
                                      join er in dbSIM.PMES_ESTRATEGIAS_RESPUESTA.Where(e => e.ID_ESTRATEGIA_TERCERO == idEstrategiasTercero) on pe.ID equals er.ID_PREGUNTA into perj
                                      from per in perj.DefaultIfEmpty()
                                      orderby pe.N_ORDEN
                                      select new
                                      {
                                          ID = per == null ? 0 : per.ID,
                                          ID_PREGUNTA = pe.ID,
                                          S_PREGUNTA = pe.S_DESCRIPCION,
                                          N_TIPO_RESPUESTA = pe.N_TIPO_RESPUESTA, // 1 Si/No, 2 Cumple/No Cumple, 3 Número, 4 % , 5 String
                                          S_RESPUESTA = (pe.N_TIPO_RESPUESTA == 1 && per.N_RESPUESTA != null ? (per.N_RESPUESTA == 1 ? "SI" : "NO") : (pe.N_TIPO_RESPUESTA == 2 && per.N_RESPUESTA != null ? (per.N_RESPUESTA == 1 ? "CUMPLE" : "NO CUMPLE") : (pe.N_TIPO_RESPUESTA == 3 && per.N_RESPUESTA != null ? per.N_RESPUESTA.ToString() : (pe.N_TIPO_RESPUESTA == 4 && per.N_RESPUESTA != null ? (per.N_RESPUESTA * 100).ToString() + " %" : (pe.N_TIPO_RESPUESTA == 5 && per.N_RESPUESTA != null) ? per.S_RESPUESTA : null))))
                                      }).ToList();

            PMESEstrategiasRespuestas respuestas = new PMESEstrategiasRespuestas();
            respuestas.DataSource = respuestasEstrategias;
            respuestas.CargarDatos();
            xrsEstrategiasDetalle3.ReportSource = respuestas;
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
