using DevExpress.Utils.Extensions;
using SIM.Areas.Poeca.Reportes;
using SIM.Data.General;
using SIM.Data.Poeca;
using SIM.Data.Tramites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SIM.Areas.Poeca.Utilidades
{
    public class Reportes
    {
        public static MemoryStream crearCartaCreacion(TPOEAIR_PLAN plan, TERCERO tercero, TBTRAMITE tramite)
        {
            var pdfStream = new MemoryStream();

            var reporteCreacion = new R_CreacionPlan();
            var parametros = reporteCreacion.Parameters;

            parametros["anioPlan"].Value = plan.N_ANIO;
            parametros["email"].Value = tercero.S_CORREO;
            parametros["identificacion"].Value = tercero.N_DOCUMENTO;
            parametros["idTercero"].Value = tercero.ID_TERCERO;
            parametros["razonSocial"].Value = tercero.S_RSOCIAL;
            parametros["telefono"].Value = tercero.N_TELEFONO;
            parametros["tramite"].Value = tramite.CODTRAMITE;

            reporteCreacion.ExportToPdf(pdfStream);
            reporteCreacion.Dispose();

            return pdfStream;
        }

        public static MemoryStream crearCartaFinEpisodio(DPOEAIR_EPISODIO episodio, TERCERO tercero, TBTRAMITE tramite)
        {
            var pdfStream = new MemoryStream();

            var reporteCreacion = new R_CulminacionEpisodio();
            var parametros = reporteCreacion.Parameters;

            parametros["periodoEpisodio"].Value = episodio.DPOEAIR_PERIODO_IMPLEMENTACION.NombrePeriodo;
            parametros["anioEpisodio"].Value = episodio.N_ANIO;
            parametros["email"].Value = tercero.S_CORREO;
            parametros["identificacion"].Value = tercero.N_DOCUMENTO;
            parametros["idTercero"].Value = tercero.ID_TERCERO;
            parametros["razonSocial"].Value = tercero.S_RSOCIAL;
            parametros["telefono"].Value = tercero.N_TELEFONO;
            parametros["tramite"].Value = tramite.CODTRAMITE;

            reporteCreacion.ExportToPdf(pdfStream);
            reporteCreacion.Dispose();

            return pdfStream;
        }
    }
}