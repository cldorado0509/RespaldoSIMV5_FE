using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Areas.GestionDocumental.Models;
using SIM.Data;
using System.IO;
using System.Net.Http.Headers;
using System.Data.Entity;
using System.Transactions;
using System.Xml.Linq;
using System.Drawing.Imaging;
using System.Data.Entity.SqlServer;
using System.Reflection;
using SIM.Utilidades;
using System.Text.RegularExpressions;
using System.Globalization;
using SIM.Areas.EnCicla.Models;
using System.Web.Http.Cors;

namespace SIM.Areas.ServicioExterno.Controllers
{
    /// <summary>
    /// Controlador RadicadorApi: Operaciones para Generar Radicados e imprimir etiquetas. También suministra los datos de serie, subserie, unidad documental y el documento asociado al radicado.
    /// </summary>
    public partial class EnCiclaApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesEnCiclaSQL dbSIMEnCicla = new EntitiesEnCiclaSQL();

        /// <summary>
        /// Estructura que almacena el resultado de una consulta, entregando los datos y número de registros en total.
        /// </summary>
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }


        public class ParametrosConsulta
        {
            public string ListaDocumentos { get; set; }
            public string Estaciones { get; set; }
            public string FechaInicial { get; set; }
            public string FechaFinal { get; set; }
            public string HoraInicial { get; set; }
            public string HoraFinal { get; set; }
        }

        private IEnumerable<int> StringToIntList(string str)
        {
            if (String.IsNullOrEmpty(str))
                yield break;

            var items = str.Split(',');

            foreach (var s in items)
            {
                int num;
                if (int.TryParse(s, out num))
                    yield return num;
            }
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost, ActionName("ConsultaPrestamos")]
        public datosConsulta PostConsultaPrestamos(ParametrosConsulta parametros)
        {
            DateTime fechaInicialSel;
            DateTime fechaFinalSel;
            TimeSpan horaInicialSel;
            TimeSpan horaFinalSel;
            datosConsulta resultado;

            parametros.ListaDocumentos = "CC" + Regex.Replace(parametros.ListaDocumentos, @"\s+", "").Replace(",", ",CC");

            if (parametros.Estaciones != null)
                parametros.Estaciones = (parametros.Estaciones.Trim() == "" ? null : parametros.Estaciones.Replace(" ", ""));

            var documentos = parametros.ListaDocumentos.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var estacionesSel = StringToIntList(parametros.Estaciones);

            fechaInicialSel = DateTime.ParseExact(parametros.FechaInicial, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            fechaFinalSel = DateTime.ParseExact(parametros.FechaFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1);
            horaInicialSel = new TimeSpan(Convert.ToInt32(parametros.HoraInicial.Split(new char[] { ':' })[0]), Convert.ToInt32(parametros.HoraInicial.Split(new char[] { ':' })[1]), 0);
            horaFinalSel = new TimeSpan(Convert.ToInt32(parametros.HoraFinal.Split(new char[] { ':' })[0]), Convert.ToInt32(parametros.HoraFinal.Split(new char[] { ':' })[1]), 0);

            var model = (from prestamo in dbSIMEnCicla.I_HISTORICO_PRESTAMO
                         join usuario in dbSIMEnCicla.P_USUARIOS on prestamo.Id_Usuario equals usuario.Id_Usuario
                         join estacionO in dbSIMEnCicla.B_APARCAMIENTOS on prestamo.Id_Aparcamiento_Origen equals estacionO.Id_Aparcamiento
                         join estacionD in dbSIMEnCicla.B_APARCAMIENTOS on prestamo.Id_Aparcamiento_Destino equals estacionD.Id_Aparcamiento
                         where estacionesSel.Contains((int)prestamo.Id_Aparcamiento_Destino) &&
                            documentos.Contains(usuario.DNI) &&
                            prestamo.Fecha_Devolucion >= fechaInicialSel &&
                            prestamo.Fecha_Devolucion < fechaFinalSel &&
                            DbFunctions.CreateTime(((DateTime)prestamo.Fecha_Devolucion).Hour, ((DateTime)prestamo.Fecha_Devolucion).Minute, ((DateTime)prestamo.Fecha_Devolucion).Second) >= horaInicialSel &&
                            DbFunctions.CreateTime(((DateTime)prestamo.Fecha_Devolucion).Hour, ((DateTime)prestamo.Fecha_Devolucion).Minute, ((DateTime)prestamo.Fecha_Devolucion).Second) <= horaFinalSel
                         orderby prestamo.Fecha_Devolucion
                         select new
                         {
                             FECHA_PRESTAMO = prestamo.Fecha_Prestamo,
                             FECHA_DEVOLUCION = prestamo.Fecha_Devolucion,
                             ESTACION_ORIGEN = estacionO.Descripcion,
                             ESTACION_DESTINO = estacionD.Descripcion,
                             DOCUMENTO = usuario.DNI
                         });


            resultado.numRegistros = model.Count();
            resultado.datos = model.ToList();

            return resultado;
        }
    }
}