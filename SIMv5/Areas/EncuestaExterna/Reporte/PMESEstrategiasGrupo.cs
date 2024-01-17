using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using SIM.Data;
using System.Collections.Generic;

namespace SIM.Areas.EncuestaExterna.Reporte
{
    public partial class PMESEstrategiasGrupo : DevExpress.XtraReports.UI.XtraReport
    {
        public class META
        {
            public int? ID { get; set; }
            public int ID_ESTRATEGIA_TERCERO { get; set; }
            public int ID_META { get; set; }
            public String S_META { get; set; }
            public String S_MEDICION { get; set; }
            public decimal? N_VALOR { get; set; }
        }

        public class ESTRATEGIA_P
        {
            public int ID { get; set; }
            public int ID_GRUPO { get; set; }
            public int ID_ESTRATEGIA { get; set; }
            public String S_ESTRATEGIA { get; set; }
            public int? ID_ESTRATEGIA_ACTIVIDAD { get; set; }
            public String S_ACTIVIDAD { get; set; }
            public int? ID_PERIODICIDAD { get; set; }
            public String S_PERIODICIDAD { get; set; }
            public String S_PUBLICO_OBJETIVO { get; set; }
            public String S_TIPOEVIDENCIA { get; set; }
            public String S_EVIDENCIAS { get; set; }
            public String S_TITULO { get; set; }
        }

        public class ESTRATEGIA_F
        {
            public int ID { get; set; }
            public int ID_GRUPO { get; set; }
            public int ID_ESTRATEGIA { get; set; }
            public String S_ESTRATEGIA { get; set; }
            public String S_OBJETIVO { get; set; }
            public String S_PUBLICO_OBJETIVO { get; set; }
            public String S_COMUNICACION_INTERNA { get; set; }
            public String S_COMUNICACION_EXTERNA { get; set; }
            public String S_TITULO { get; set; }
        }

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        int idEstrategiasTercero;

        public PMESEstrategiasGrupo()
        {
            InitializeComponent();
        }

        public void CargarDatos(int idEstrategiasTercero)
        {
            this.idEstrategiasTercero = idEstrategiasTercero;
            AddBoundLabel("xrlGrupo", "S_TITULO");
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

        private void xrsEstrategiasGrupo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int idEncabezado = Convert.ToInt32(GetCurrentColumnValue("ID_ENCABEZADO"));
            int idGrupo = Convert.ToInt32(GetCurrentColumnValue("ID"));
            dynamic estrategiasGrupo;

            var tipoDatos = (from eg in dbSIM.PMES_ESTRATEGIAS_GRUPO
                             where eg.ID == idGrupo
                             select eg).FirstOrDefault();

            if (idEncabezado == 1)
            {
                estrategiasGrupo = (from etp in dbSIM.PMES_ESTRATEGIAS_TP.Where(etp => etp.ID_ESTRATEGIA_TERCERO == idEstrategiasTercero)
                                    join a in dbSIM.PMES_ESTRATEGIAS_ACTIVIDADES on etp.ID equals a.ID_ESTRATEGIA_TP into ealj
                                    from eava in ealj.DefaultIfEmpty()
                                    join p in dbSIM.PMES_ESTRATEGIAS_PERIODICIDAD on eava.ID_PERIODICIDAD equals p.ID into eplj
                                    from epva in eplj.DefaultIfEmpty()
                                    join es in dbSIM.PMES_ESTRATEGIAS on etp.ID_ESTRATEGIA equals es.ID
                                    join g in dbSIM.PMES_ESTRATEGIAS_GRUPO on es.ID_GRUPO equals g.ID
                                    join et in dbSIM.PMES_ESTRATEGIAS_TERCERO.Where(eti => eti.ID == idEstrategiasTercero) on etp.ID_ESTRATEGIA_TERCERO equals et.ID into etlj
                                    where es.ID_GRUPO == idGrupo
                                    orderby etp.ID_ESTRATEGIA
                                    select new ESTRATEGIA_P
                                    {
                                        ID = etp.ID,
                                        ID_GRUPO = es.ID_GRUPO,
                                        ID_ESTRATEGIA = etp.ID_ESTRATEGIA,
                                        S_ESTRATEGIA = es.S_NOMBRE + (es.S_NOMBRE.ToUpper().Trim() == "OTROS" ? " (" + etp.S_OTRO + ")" : ""),
                                        ID_ESTRATEGIA_ACTIVIDAD = eava.ID,
                                        S_ACTIVIDAD = eava.S_ACTIVIDAD,
                                        ID_PERIODICIDAD = eava.ID_PERIODICIDAD,
                                        S_PERIODICIDAD = epva.S_PERIODICIDAD,
                                        S_PUBLICO_OBJETIVO = etp.S_PUBLICO_OBJETIVO,
                                        S_TIPOEVIDENCIA = etp.S_TIPOEVIDENCIA,
                                        S_TITULO = g.S_TITULO
                                    }).ToList();

                Dictionary<string, string> tiposEvidencia = new Dictionary<string, string>();

                foreach (ESTRATEGIA_P estrategia in estrategiasGrupo)
                {
                    if (!tiposEvidencia.ContainsKey(estrategia.S_TIPOEVIDENCIA))
                    {
                        List<int> listaEvidencias = estrategia.S_TIPOEVIDENCIA.Split(',').Select(x => Int32.Parse(x)).ToList();

                        var evidencias = (from te in dbSIM.PMES_ESTRATEGIAS_TIPOEVIDENCIA
                                          where listaEvidencias.Contains(te.ID)
                                          select te.S_TIPOEVIDENCIA).ToList();

                        tiposEvidencia.Add(estrategia.S_TIPOEVIDENCIA, string.Join(", ", evidencias));
                    }

                    estrategia.S_EVIDENCIAS = tiposEvidencia[estrategia.S_TIPOEVIDENCIA];
                }

                PMESEstrategiasDetalleTP estrategiasDetalle = new PMESEstrategiasDetalleTP();
                estrategiasDetalle.DataSource = estrategiasGrupo;
                estrategiasDetalle.CargarDatos();

                ((XRSubreport)sender).ReportSource = estrategiasDetalle;
            }
            else
            {
                estrategiasGrupo = (from etf in dbSIM.PMES_ESTRATEGIAS_TF.Where(etf => etf.ID_ESTRATEGIA_TERCERO == idEstrategiasTercero)
                                    join es in dbSIM.PMES_ESTRATEGIAS on etf.ID_ESTRATEGIA equals es.ID
                                   join g in dbSIM.PMES_ESTRATEGIAS_GRUPO on es.ID_GRUPO equals g.ID
                                   join et in dbSIM.PMES_ESTRATEGIAS_TERCERO.Where(eti => eti.ID == idEstrategiasTercero) on etf.ID_ESTRATEGIA_TERCERO equals et.ID
                                   where es.ID_GRUPO == idGrupo
                                   select new ESTRATEGIA_F
                                   {
                                       ID = etf.ID,
                                       ID_GRUPO = es.ID_GRUPO,
                                       ID_ESTRATEGIA = etf.ID_ESTRATEGIA,
                                       S_ESTRATEGIA = es.S_NOMBRE + (es.S_NOMBRE.ToUpper().Trim() == "OTROS" ? etf.S_OTRO : ""),
                                       S_OBJETIVO = etf.S_OBJETIVO,
                                       S_PUBLICO_OBJETIVO = etf.S_PUBLICO_OBJETIVO,
                                       S_COMUNICACION_INTERNA = etf.S_COMUNICACION_INTERNA,
                                       S_COMUNICACION_EXTERNA = etf.S_COMUNICACION_EXTERNA,
                                       S_TITULO = g.S_TITULO
                                   }).ToList();

                PMESEstrategiasDetalleTF estrategiasDetalle = new PMESEstrategiasDetalleTF();
                estrategiasDetalle.DataSource = estrategiasGrupo;
                estrategiasDetalle.CargarDatos();

                ((XRSubreport)sender).ReportSource = estrategiasDetalle;
            }
        }

        private void xrsMetasGrupo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int idGrupo = Convert.ToInt32(GetCurrentColumnValue("ID"));

            var metasGrupo = (from em in dbSIM.PMES_ESTRATEGIAS_METAS
                             join emv in dbSIM.PMES_ESTRATEGIAS_METAS_T.Where(emt => emt.ID_ESTRATEGIA_TERCERO == idEstrategiasTercero) on em.ID equals emv.ID_ESTRATEGIAS_METAS into emlj
                             from emva in emlj.DefaultIfEmpty()
                             join et in dbSIM.PMES_ESTRATEGIAS_TERCERO.Where(eti => eti.ID == idEstrategiasTercero) on emva.ID_ESTRATEGIA_TERCERO equals et.ID into etlj
                             from eta in etlj.DefaultIfEmpty()
                             where em.ID_ESTRATEGIAS_GRUPO == idGrupo
                             orderby em.N_ORDEN
                             select new META
                             {
                                 ID = emva.ID,
                                 ID_META = em.ID,
                                 S_META = em.S_META,
                                 S_MEDICION = em.S_MEDICION,
                                 N_VALOR = emva.N_VALOR
                             }).ToList();

            PMESEstrategiasMetas estrategiasDetalle = new PMESEstrategiasMetas();
            estrategiasDetalle.DataSource = metasGrupo;
            estrategiasDetalle.CargarDatos();

            ((XRSubreport)sender).ReportSource = estrategiasDetalle;
        }
    }
}
