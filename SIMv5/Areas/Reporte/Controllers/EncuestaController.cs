using DevExpress.XtraGrid;
using SIM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using System.IO;
using System.Data;
using SIM.Areas.Reporte.Reportes;
using System.Data.SqlClient;

namespace SIM.Areas.Reporte.Controllers
{
    public class EncuestaController : Controller
    {
        public class REPORTE
        {
            public string ENCUESTA { get; set; }
            public string EMPRESA { get; set; }
            public string INSTALACION { get; set; }
            public string ENCUESTA_NO { get; set; }
            public string GRUPO { get; set; }
            public int NO_PREGUNTA { get; set; }
            public string PREGUNTA { get; set; }
            public decimal? VALOR_NUMERO { get; set; }
            public string S_VALOR { get; set; }
            public string X_VALOR { get; set; }
            public string Y_VALOR { get; set; }
            public string OPCION { get; set; }
            public string OPCIONES { get; set; }
        }

        /// <summary>
        /// Variable de Contexto de Base de Datos
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public ActionResult DetalleRespuestas(int idTercero, int idInstalacion, int idVigencia, string valorVigencia)
        {
            string sql = "SELECT DISTINCT v.NOMBRE||'-'||vs.VALOR  ENCUESTA, t.S_RSOCIAL EMPRESA,i.S_NOMBRE INSTALACION, ge.NOMBRE AS ENCUESTA_NO, e.S_NOMBRE AS GRUPO, ep.N_ORDEN AS No_PREGUNTA, p.S_NOMBRE AS PREGUNTA, sp.N_VALOR VALOR_NUMERO, sp.S_VALOR, sp.X_VALOR, sp.Y_VALOR, eor.S_VALOR AS OPCION, CASE WHEN p.ID_TIPOPREGUNTA IN (3, 13) THEN CONTROL.ROWCONCAT('SELECT EOR.S_VALOR FROM ENC_SOLUCION_PREGUNTAS_OPC ESP INNER JOIN ENC_OPCION_RESPUESTA EOR ON ESP.ID_RESPUESTA = EOR.ID_RESPUESTA WHERE ESP.ID_SOLUCION_PREGUNTAS = ' || sp.ID_SOLUCION_PREGUNTAS) ELSE '' END AS OPCIONES FROM control.ENC_ENCUESTA e INNER JOIN control.FORMULARIO_ENCUESTA fe ON fe.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN control.ENCUESTA_VIGENCIA ev ON ev.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN control.VIGENCIA v ON v.ID_VIGENCIA=ev.ID_VIGENCIA INNER JOIN control.VIGENCIA_SOLUCION vs ON vs.ID_VIGENCIA=v.ID_VIGENCIA INNER JOIN control.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO=ge.ID_ESTADO inner join GENERAL.TERCERO t on t.ID_TERCERO=ge.ID_TERCERO inner join GENERAL.INSTALACION i on i.ID_INSTALACION=ge.ID_INSTALACION INNER JOIN CONTROL.ENC_SOLUCION s on ge.id_estado = s.id_estado and e.ID_ENCUESTA = s.ID_ENCUESTA inner join CONTROL.ENC_ENCUESTA_PREGUNTA ep on s.ID_ENCUESTA = ep.ID_ENCUESTA inner join control.ENC_SOLUCION_PREGUNTAS sp on s.id_solucion = sp.id_solucion and ep.ID_PREGUNTA = sp.ID_PREGUNTA inner join control.enc_pregunta p on sp.ID_PREGUNTA = p.ID_PREGUNTA LEFT OUTER JOIN control.ENC_OPCION_RESPUESTA eor on p.ID_PREGUNTA = eor.ID_PREGUNTA AND sp.ID_VALOR = eor.ID_RESPUESTA WHERE ge.ID_TERCERO = " + idTercero.ToString() + " AND ge.ID_INSTALACION = " + idInstalacion.ToString() + " AND ge.ACTIVO = 0 AND v.ID_VIGENCIA = " + idVigencia.ToString() + " AND vs.VALOR = '" + valorVigencia + "' ORDER BY ge.NOMBRE, e.S_NOMBRE, ep.N_ORDEN";

            var reportes = dbSIM.Database.SqlQuery<REPORTE>(sql).ToList<REPORTE>();

            rptEncuestaRespuestas reporte = new rptEncuestaRespuestas();
            reporte.DataSource = reportes;
            reporte.CargarDatos();

            MemoryStream ms = new MemoryStream();

            reporte.ExportToXls(ms);
            return File(ms.GetBuffer(), "application/xls", "Reporte.xls");
        }
	}
}