using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Areas.Models;
using Newtonsoft.Json;
using System.Data.Entity.SqlServer;
using System.Security.Claims;
using System.Data.Entity;
using System.IO;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using SIM.Utilidades;
using System.Globalization;
using System.Text;
using System.Web.Hosting;
using Oracle.ManagedDataAccess.Client;
using SIM.Data;
using SIM.Data.Control;
using System.Configuration;

namespace SIM.Areas.EncuestaExterna.Controllers
{
    public class DATOSARCHIVO {
        public int ID_TERCERO { get; set; }
        public string TERCERO { get; set; }
        public int ID_INSTALACION { get; set; }
        public string INSTALACION { get; set; }
        public string VIGENCIA { get; set; }
        public int ID_PREGUNTA { get; set; }
        public string S_ARCHIVO { get; set; }
        public string S_ARCHIVO_NUEVO { get; set; }
    }

    public class EncuestaExternaApiController : ApiController
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema General y Seguridad
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        // GET api/<controller>
        [HttpGet]
        [ActionName("ArchivosEncuestas")]
        public void GetArchivosEncuestas(string vigencia, string valor, string tercerosInstalaciones)
        {
            AppSettingsReader webConfigReader = new AppSettingsReader();

            string pathBase1 = (string)webConfigReader.GetValue("ruta_base_Documentos", typeof(string)) + "\\ArchivosEncuestas\\" + vigencia + "\\" + valor;
            string pathBase2 = (string)webConfigReader.GetValue("ruta_base_Documentos", typeof(string)) + "\\Tercero\\";

            string archivoCopia;

            foreach (string terceroInstacion in tercerosInstalaciones.Split(';'))
            {
                string tercero = terceroInstacion.Split(',')[0];
                string instalacion = terceroInstacion.Split(',')[1];

                string sql = "" +
                    "SELECT ge.ID_TERCERO, t.S_RSOCIAL AS TERCERO, ge.ID_INSTALACION, i.S_NOMBRE AS INSTALACION, vs.VALOR AS VIGENCIA, sp.ID_PREGUNTA, sp.S_VALOR AS S_ARCHIVO, CAST(ge.ID_ESTADO AS varchar(20)) || '_' || CAST(p.ID_PREGUNTA AS varchar(20)) || '_' || sp.S_VALOR AS S_ARCHIVO_NUEVO " +
                    "FROM CONTROL.VIGENCIA v INNER JOIN " +
                    "    CONTROL.VIGENCIA_SOLUCION vs ON v.ID_VIGENCIA = vs.ID_VIGENCIA INNER JOIN " +
                    "    CONTROL.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO = ge.ID_ESTADO INNER JOIN " +
                    "    GENERAL.TERCERO t ON ge.ID_TERCERO = t.ID_TERCERO INNER JOIN " +
                    "    GENERAL.INSTALACION i ON ge.ID_INSTALACION = i.ID_INSTALACION INNER JOIN " +
                    "    CONTROL.ENC_SOLUCION s ON ge.ID_ESTADO = s.ID_ESTADO INNER JOIN " +
                    "    CONTROL.ENC_SOLUCION_PREGUNTAS sp ON s.ID_SOLUCION = sp.ID_SOLUCION INNER JOIN " +
                    "    CONTROL.ENC_PREGUNTA p ON sp.ID_PREGUNTA = p.ID_PREGUNTA " +
                    "WHERE v.ID_VIGENCIA = " + vigencia + " AND vs.VALOR = '" + valor + "' AND ge.ID_TERCERO = " + tercero + " AND ge.ID_INSTALACION = " + instalacion + " AND p.ID_TIPOPREGUNTA = 8 " +
                    "ORDER BY ge.ID_INSTALACION, vs.VALOR, sp.ID_PREGUNTA";

                string path = "C:\\Temp\\EncuestaAdjuntos_" + vigencia + "_" + valor;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var archivosEncuesta = dbSIM.Database.SqlQuery<DATOSARCHIVO>(sql, new object[0]).ToList();

                foreach (var archivo in archivosEncuesta)
                {
                    if (archivo.S_ARCHIVO != null && archivo.S_ARCHIVO.Trim() != "")
                    {
                        archivoCopia = "";
                        path = "C:\\Temp\\EncuestaAdjuntos_" + vigencia + "_" + valor + "\\" + archivo.TERCERO + "\\" + archivo.INSTALACION;

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        if (File.Exists(pathBase1 + "\\" + archivo.ID_TERCERO.ToString() + "\\" + archivo.S_ARCHIVO_NUEVO))
                        {
                            archivoCopia = pathBase1 + "\\" + archivo.ID_TERCERO.ToString() + "\\" + archivo.S_ARCHIVO_NUEVO;
                        }
                        else if (File.Exists(pathBase2 + "\\" + archivo.ID_TERCERO.ToString() + "\\Pregunta\\" + archivo.ID_PREGUNTA.ToString() + "\\" + archivo.S_ARCHIVO))
                        {
                            archivoCopia = pathBase2 + "\\" + archivo.ID_TERCERO.ToString() + "\\Pregunta\\" + archivo.ID_PREGUNTA.ToString() + "\\" + archivo.S_ARCHIVO;
                        }

                        if (archivoCopia != "")
                            File.Copy(archivoCopia, path + "\\" + archivo.S_ARCHIVO);
                    }
                }
            }
        }
    }
}