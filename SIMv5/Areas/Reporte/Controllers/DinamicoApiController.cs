using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using System.Dynamic;

namespace SIM.Areas.General.Controllers
{
    public class DinamicoApiController : ApiController
    {
        public class FILTROCONFIG
        {
            public string S_AGRUPAR { get; set; }
            public string S_FILTROS { get; set; }
        }

        public class FILTRO
        {
            public int id { get; set; } // Id del Filtro (Consecutivo único en el reporte)
            public string tipoId { get; set; } // int, string
            public int tipo { get; set; } // 1: Texto, 2: Combo Box, 3: Combo Box Popup, 4 treeview, 5: Fecha
            public string titulo { get; set; } // Título del filtro en el formulario
            public string campo { get; set; } // Nombre de la Columna asociada en el reporte, para finalmente filtrar por esa columna en la consulta
            public string sqlLista { get; set; } // SQL para alimentar el combo box o popup (solamente aplica para tipos 2 o 3). Debe Tener obligatoriamente un campo ID y uno NOMBRE
            public string sqlCampoFiltro { get; set; } // Campo de la lista que se utiliza para filtrar los resultados cuando la lista es muy larga
            public string mostrarSinFiltro { get; set; } // S: muestra todos los registros cuando no hay filtro, N: no muestra registros cuando no hay filtro
            public string idFiltrosDependientes { get; set; } // Lista de Ids separados por comas de los id que se deben limpiar cuando el filtro cambie, ya que dependen de él
            public string idFiltrosRestriccion { get; set; } // Lista de Ids separados por comas de los filtros necesarios para listar este filtro, de lo contrario muestra la lista en blanco. El formato es id1|Campo1,id2|Campo2,...
            public string filtrosRestriccionRequeridos { get; set; } // S: filtra y muestra los datos de la lista siempre y cuando todos los filtros de Resticción tengan datos de lo contrario no lista nada, N: Si algún filtro de restricción falta no lo toma en cuenta pero tiene en cuenta los demás.
            public string valor { get; set; }
        }

        public class DATOSFILTROINT
        {
            public int ID { get; set; }
            public string NOMBRE { get; set; }
            public int? ID_PADRE { get; set; }
        }

        public class DATOSFILTROSTRING
        {
            public string ID { get; set; }
            public string NOMBRE { get; set; }
            public string ID_PADRE { get; set; }
        }

        public class SELECCIONFILTRO
        {
            public int id { get; set; }
            public int tipo { get; set; }
            public string valor { get; set; }
        }

        public class SORT
        {
            public string selector { get; set; }
            public bool desc { get; set; }
        }

        public class DATOSCONSULTA
        {
            public string S01 { get; set; }
            public string S02 { get; set; }
            public string S03 { get; set; }
            public string S04 { get; set; }
            public string S05 { get; set; }
            public string S06 { get; set; }
            public string S07 { get; set; }
            public string S08 { get; set; }
            public string S09 { get; set; }
            public string S10 { get; set; }
            public string S11 { get; set; }
            public string S12 { get; set; }
            public string S13 { get; set; }
            public string S14 { get; set; }
            public string S15 { get; set; }
            public string S16 { get; set; }
            public string S17 { get; set; }
            public string S18 { get; set; }
            public string S19 { get; set; }
            public string S20 { get; set; }
            public string S21 { get; set; }
            public string S22 { get; set; }
            public string S23 { get; set; }
            public string S24 { get; set; }
            public string S25 { get; set; }
            public string S26 { get; set; }
            public string S27 { get; set; }
            public string S28 { get; set; }
            public string S29 { get; set; }
            public string S30 { get; set; }
            public int? I01 { get; set; }
            public int? I02 { get; set; }
            public int? I03 { get; set; }
            public int? I04 { get; set; }
            public int? I05 { get; set; }
            public int? I06 { get; set; }
            public int? I07 { get; set; }
            public int? I08 { get; set; }
            public int? I09 { get; set; }
            public int? I10 { get; set; }
            public int? I11 { get; set; }
            public int? I12 { get; set; }
            public int? I13 { get; set; }
            public int? I14 { get; set; }
            public int? I15 { get; set; }
            public int? I16 { get; set; }
            public int? I17 { get; set; }
            public int? I18 { get; set; }
            public int? I19 { get; set; }
            public int? I20 { get; set; }
            public decimal? N01 { get; set; }
            public decimal? N02 { get; set; }
            public decimal? N03 { get; set; }
            public decimal? N04 { get; set; }
            public decimal? N05 { get; set; }
            public decimal? N06 { get; set; }
            public decimal? N07 { get; set; }
            public decimal? N08 { get; set; }
            public decimal? N09 { get; set; }
            public decimal? N10 { get; set; }
            public decimal? N11 { get; set; }
            public decimal? N12 { get; set; }
            public decimal? N13 { get; set; }
            public decimal? N14 { get; set; }
            public decimal? N15 { get; set; }
            public decimal? N16 { get; set; }
            public decimal? N17 { get; set; }
            public decimal? N18 { get; set; }
            public decimal? N19 { get; set; }
            public decimal? N20 { get; set; }
            public DateTime? D01 { get; set; }
            public DateTime? D02 { get; set; }
            public DateTime? D03 { get; set; }
            public DateTime? D04 { get; set; }
            public DateTime? D05 { get; set; }
            public DateTime? D06 { get; set; }
            public DateTime? D07 { get; set; }
            public DateTime? D08 { get; set; }
            public DateTime? D09 { get; set; }
            public DateTime? D10 { get; set; }
            public string L01 { get; set; }
            public string L02 { get; set; }
            public string L03 { get; set; }
            public string L04 { get; set; }
        }

        public class CONFIGCOLUMNA
        {
            public string dataField { get; set; }
		    public string width { get; set; }
		    public string caption { get; set; }
		    public string dataType { get; set; }
            public string format { get; set; }
            public string cellTemplate { get; set; }
        }

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        public struct datosRespuesta
        {
            public string tipoRespuesta; // OK, Error
            public string detalleRespuesta;
        }

        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema General y Seguridad
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        // GET api/<controller>
        [HttpGet]
        [ActionName("Reportes")]
        public datosConsulta GetReportes()
        {
            List<DATOSFILTROINT> reportes;
            string idRol = null;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero;
            string rolesUsuario = "";

            idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                rolesUsuario = string.Join(", ", ((System.Security.Claims.ClaimsPrincipal)context.User).FindAll(CustomClaimTypes.IdRol).Select(r => r.Value));//.ToList();

                //foreach (var rol in ((System.Security.Claims.ClaimsPrincipal)context.User).FindAll(CustomClaimTypes.IdRol))
                //{

                //}
                //idRol = ((System.Security.Claims.ClaimsPrincipal)context.User).FindAll(CustomClaimTypes.IdRol).Value;

               var sql = "SELECT DISTINCT ID_REPORTE AS ID, S_NOMBRE AS NOMBRE " +
                        "FROM( " +
                        "    SELECT ID_REPORTE, S_NOMBRE, regexp_substr(S_ROLES, '[^,]+', 1, level) AS ROL " +
                        "    FROM CONTROL.REPORTE " +
                        "    CONNECT BY regexp_substr(S_ROLES, '[^,]+', 1, level) IS NOT NULL " +
                        ") WHERE ROL IN(" + rolesUsuario + ") ORDER BY S_NOMBRE";

                /*var sql = "SELECT ID_REPORTE ID, S_NOMBRE NOMBRE " +
                "FROM CONTROL.REPORTE WHERE S_ROLES IS NOT NULL ORDER BY S_NOMBRE";*/

                var model = dbSIM.Database.SqlQuery<DATOSFILTROINT>(sql);

                datosConsulta resultado = new datosConsulta();

                reportes = model.ToList<DATOSFILTROINT>();

                resultado.numRegistros = 0;
                resultado.datos = reportes;
                return resultado;
            }
            else
            {
                datosConsulta resultado = new datosConsulta();

                resultado.numRegistros = 0;
                resultado.datos = null;
                return resultado;
            }
        }

        [HttpGet]
        [ActionName("FiltrosReporte")]
        public dynamic GetFiltrosReporte(int idReporte)
        {
            var sql = "SELECT S_AGRUPAR, S_FILTROS " +
                        "FROM CONTROL.REPORTE " +
                        "WHERE ID_REPORTE = " + idReporte.ToString() + " " +
                        "ORDER BY S_NOMBRE";

            var filtroconfig = dbSIM.Database.SqlQuery<FILTROCONFIG>(sql).FirstOrDefault();

            string titulo = (filtroconfig.S_AGRUPAR != null && filtroconfig.S_AGRUPAR.Trim() != "" ? filtroconfig.S_AGRUPAR.Split('|')[0] : null);
            string agrupar = (filtroconfig.S_AGRUPAR != null && filtroconfig.S_AGRUPAR.Trim() != "" ? filtroconfig.S_AGRUPAR.Split('|')[1] : null);
            string filtros = filtroconfig.S_FILTROS;

            return new { titulo = titulo, agrupar = (agrupar == null ? null : JsonConvert.DeserializeObject<DATOSFILTROINT[]>(agrupar)), filtros = (filtros == null ? null : JsonConvert.DeserializeObject<FILTRO[]>(filtros)) };
        }

        [HttpGet]
        [ActionName("ConsultaConfig")]
        public datosConsulta GetConsultaConfig(int idReporte, int agrupar)
        {
            agrupar = (agrupar == -1 ? 0 : agrupar);

            string sql = "SELECT S_CONFIGURACION_GRID " +
                        "FROM CONTROL.REPORTE " +
                        "WHERE ID_REPORTE = " + idReporte.ToString();

            var a = dbSIM.Database.SqlQuery<string>(sql).FirstOrDefault();

            string sqlConsulta = dbSIM.Database.SqlQuery<string>(sql).FirstOrDefault().Split(new string[] { "####" }, StringSplitOptions.None)[agrupar];

            datosConsulta resultado = new datosConsulta();
            resultado.datos = JsonConvert.DeserializeObject<List<CONFIGCOLUMNA>>(sqlConsulta);
            resultado.numRegistros = 0;

            return resultado;
        }

        [HttpGet]
        [ActionName("Consulta")]
        public datosConsulta GetConsulta(int idReporte, string sort, int skip, int take, string valoresFiltros, int agrupar)
        {
            string idFiltro = "";
            string tipoFiltro = "";
            string[] campoFiltro;
            string valorFiltro = "";
            string where = "";
            string whereCond = "";

            string sql = "SELECT S_CONSULTA " +
                        "FROM CONTROL.REPORTE " +
                        "WHERE ID_REPORTE = " + idReporte.ToString();

            agrupar = (agrupar == -1 ? 0 : agrupar);

            string sqlConsulta = dbSIM.Database.SqlQuery<string>(sql).FirstOrDefault().Split(new string[] {"####"}, StringSplitOptions.None)[agrupar];

            string[] ordenDefault = dbSIM.Database.SqlQuery<string>("SELECT S_ORDEN_DEFAULT FROM CONTROL.REPORTE WHERE ID_REPORTE = " + idReporte.ToString()).FirstOrDefault().Split(new string[] { "####" }, StringSplitOptions.None)[agrupar].Split('|');

            string paginar = dbSIM.Database.SqlQuery<string>("SELECT S_PAGINAR FROM CONTROL.REPORTE WHERE ID_REPORTE = " + idReporte.ToString()).FirstOrDefault();

            if (valoresFiltros != null && valoresFiltros.Trim() != "")
            {
                foreach (string filtro in valoresFiltros.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    idFiltro = filtro.Split('|')[0];
                    tipoFiltro = filtro.Split('|')[1];
                    campoFiltro = filtro.Split('|')[2].Split(',');
                    valorFiltro = filtro.Split('|')[3];

                    if (valorFiltro.Trim() != "")
                    {
                        where += (where == "" ? "WHERE " : " AND ");
                        whereCond += " AND ";

                        if (campoFiltro[1].Trim() == "=")
                        {
                            where += campoFiltro[2] + " = " + (campoFiltro[0].Trim().ToUpper() == "INT" ? "" : "'") + valorFiltro.Trim() + (campoFiltro[0].Trim().ToUpper() == "INT" ? "" : "'");
                            whereCond += campoFiltro[2] + " = " + (campoFiltro[0].Trim().ToUpper() == "INT" ? "" : "'") + valorFiltro.Trim() + (campoFiltro[0].Trim().ToUpper() == "INT" ? "" : "'");
                        }
                        if (campoFiltro[1].Trim().ToUpper() == "LIKE")
                        {
                            where += "(LOWER(\"" + campoFiltro[2] + "\") LIKE '%" + valorFiltro.Trim().ToLower() + "%')";
                            whereCond += "(LOWER(\"" + campoFiltro[2] + "\") LIKE '%" + valorFiltro.Trim().ToLower() + "%')";
                        }
                        if (campoFiltro[1].Trim().ToUpper() == "IN")
                        {
                            where += "(" + campoFiltro[2] + " IN (" + valorFiltro.Trim() + "))";
                            whereCond += "(" + campoFiltro[2] + " IN (" + valorFiltro.Trim() + "))";
                        }
                    }
                }

                sqlConsulta = sqlConsulta.Replace("#WHERE#", where);
                sqlConsulta = sqlConsulta.Replace("#WHERECOND#", whereCond);
            }
            else
            {
                sqlConsulta = sqlConsulta.Replace("#WHERE#", "");
                sqlConsulta = sqlConsulta.Replace("#WHERECOND#", "");
            }

            int numRegistros = 0;

            if (paginar.Trim().ToUpper() == "S")
            {
                numRegistros = dbSIM.Database.SqlQuery<int>("SELECT COUNT(0) FROM (" + sqlConsulta.Replace("#ORDER BY#", "").Replace("#ORDER COL#", "0 AS ROW_NUMBER") + ") NumReg").FirstOrDefault();
            }

            sqlConsulta = sqlConsulta.Replace("#ORDER COL#", "row_number() OVER (ORDER BY " + ordenDefault[0] + ") AS ROW_NUMBER");

            /*if (sort.Trim() == "")
                sqlConsulta = sqlConsulta.Replace("#ORDER COL#", "row_number() OVER (ORDER BY \"" + ordenDefault + "\") AS \"row_number\"");
            else
            {
                SORT orden = JsonConvert.DeserializeObject<SORT>(sort);

                sqlConsulta = sqlConsulta.Replace("#ORDER COL#", "row_number() OVER (ORDER BY \"" + orden.selector + "\") AS \"row_number\"");
            }*/

            /*if (sort != null && sort.Trim() != "")
            {
                sqlConsulta = sqlConsulta.Replace("#ORDER BY#", "");
            }
            else
            {
                sqlConsulta = sqlConsulta.Replace("#ORDER BY#", "");
            }*/

            //sqlConsulta = sqlConsulta.Replace("#ORDER BY#", "ORDER BY " + ordenDefault[1]);

            string sqlDatosConsulta = "";

            if (paginar.Trim().ToUpper() == "S" && take > 0)
            {
                sqlDatosConsulta = "SELECT * FROM (SELECT * FROM (" + sqlConsulta + ") ConsultaResultado WHERE (ROW_NUMBER > " + skip.ToString() + ") ORDER BY " + ordenDefault[1] + ") WHERE (ROWNUM <= (" + take.ToString() + "))";
            }
            else
            {
                sqlDatosConsulta = "SELECT * FROM (" + sqlConsulta + ") ConsultaResultado ORDER BY " + ordenDefault[1];
            }

            var datosConsulta = dbSIM.Database.SqlQuery<DATOSCONSULTA>(sqlDatosConsulta);

            datosConsulta resultado = new datosConsulta();
            resultado.numRegistros = numRegistros;
            resultado.datos = datosConsulta.ToList();

            return resultado;
        }

        [HttpGet]
        [ActionName("ConsultaFiltro")]
        public datosConsulta GetConsultaFiltro(int idReporte, int idFiltro, int skip, int take, string searchValue, string valoresFiltros)
        {
            //Dictionary<int, int> valoresFiltrosSeleccionados = new Dictionary<int, int>();

            if (idFiltro >= 0)
            {
                var condicionSQL = "";

                string sql = "SELECT S_FILTROS " +
                            "FROM CONTROL.REPORTE " +
                            "WHERE ID_REPORTE = " + idReporte.ToString();

                FILTRO filtroSeleccionado = JsonConvert.DeserializeObject<List<FILTRO>>(dbSIM.Database.SqlQuery<string>(sql).FirstOrDefault()).Where(f => f.id == idFiltro).FirstOrDefault();

                if (take > 0 || filtroSeleccionado.tipo == 2)
                {
                    datosConsulta resultado = new datosConsulta();

                    if (valoresFiltros != null && valoresFiltros.Trim() != "")
                    {
                        var valoresFiltrosArray = valoresFiltros.Split(',');

                        foreach (string valorFiltro in valoresFiltrosArray)
                        {
                            var valorFiltroArray = valorFiltro.Split('|');

                            if (valorFiltroArray[2].Trim() != "")
                            {
                                //valoresFiltrosSeleccionados.Add(Convert.ToInt32(valorFiltro.Split('|')[0]), Convert.ToInt32(valorFiltro.Split('|')[1]));

                                condicionSQL += " AND " + valorFiltroArray[1] + " = " + valorFiltroArray[2];
                            }
                            else if (filtroSeleccionado.filtrosRestriccionRequeridos.Trim().ToUpper() == "S")
                            {
                                return new datosConsulta() { numRegistros = 0, datos = null };
                            }
                        }
                    }

                    condicionSQL += (filtroSeleccionado.mostrarSinFiltro.ToUpper() == "S" ? ((searchValue ?? "").Trim() == "" ? "" : " AND (LOWER(" + filtroSeleccionado.sqlCampoFiltro + ") LIKE '%" + searchValue.ToLower() + "%')") : ((searchValue ?? "").Trim() == "" ? " AND 0=1 " : " AND (LOWER(" + filtroSeleccionado.sqlCampoFiltro + ") LIKE '%" + searchValue.ToLower() + "%')"));

                    string sqlDatosLista = "";

                    if (filtroSeleccionado.tipo == 3)
                        sqlDatosLista = "SELECT * FROM (SELECT * FROM (" + filtroSeleccionado.sqlLista.Replace("#WHERE#", condicionSQL) + ") Filter WHERE (row_number > " + skip.ToString() + ") ORDER BY NOMBRE ASC) WHERE (ROWNUM <= (" + take.ToString() + "))";
                    else
                        sqlDatosLista = filtroSeleccionado.sqlLista.Replace("#WHERE#", condicionSQL);

                    if (filtroSeleccionado.tipoId == null || filtroSeleccionado.tipoId.Trim() == "" || filtroSeleccionado.tipoId.ToUpper().Trim() == "INT")
                    {
                        var datosLista = dbSIM.Database.SqlQuery<DATOSFILTROINT>(sqlDatosLista);

                        resultado.datos = datosLista;
                    }

                    if (filtroSeleccionado.tipoId.ToUpper().Trim() == "STRING")
                    {
                        var datosLista = dbSIM.Database.SqlQuery<DATOSFILTROSTRING>(sqlDatosLista);

                        resultado.datos = datosLista;
                    }

                    //string sqlNumRegistros = "SELECT COUNT(0) FROM (" + filtrosReporte.sqlLista.Replace("#WHERE#", (filtrosReporte.mostrarSinFiltro.ToUpper() == "S" ? ((searchValue ?? "").Trim() == "" ? "" : " AND (LOWER(" + filtrosReporte.sqlCampoFiltro + ") LIKE '%" + searchValue.ToLower() + "%')") : ((searchValue ?? "").Trim() == "" ? " AND 0=1" : " AND (LOWER(" + filtrosReporte.sqlCampoFiltro + ") LIKE '%" + searchValue.ToLower() + "%')"))) + ") Filter";
                    string sqlNumRegistros = "SELECT COUNT(0) FROM (" + filtroSeleccionado.sqlLista.Replace("#WHERE#", condicionSQL) + ") Filter";

                    var numRegistros = dbSIM.Database.SqlQuery<int>(sqlNumRegistros).FirstOrDefault();

                    resultado.numRegistros = numRegistros;

                    return resultado;
                }
                else
                {
                    datosConsulta resultado = new datosConsulta();
                    resultado.numRegistros = 0;
                    resultado.datos = null;

                    return resultado;
                }
            } else {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
        }

        [HttpGet]
        [ActionName("ConsultaFiltroTodos")]
        public datosConsulta GetConsultaFiltroTodos(int idReporte, int idFiltro, string valoresFiltros)
        {

            var condicionSQL = "";

            string sql = "SELECT S_FILTROS " +
                        "FROM CONTROL.REPORTE " +
                        "WHERE ID_REPORTE = " + idReporte.ToString();

            FILTRO filtroSeleccionado = JsonConvert.DeserializeObject<List<FILTRO>>(dbSIM.Database.SqlQuery<string>(sql).FirstOrDefault()).Where(f => f.id == idFiltro).FirstOrDefault();

            datosConsulta resultado = new datosConsulta();

            if (valoresFiltros != null && valoresFiltros.Trim() != "")
            {
                var valoresFiltrosArray = valoresFiltros.Split(',');

                foreach (string valorFiltro in valoresFiltrosArray)
                {
                    var valorFiltroArray = valorFiltro.Split('|');

                    if (valorFiltroArray[2].Trim() != "")
                    {
                        condicionSQL += " AND " + valorFiltroArray[1] + " = " + valorFiltroArray[2];
                    }
                    /*else
                    {
                        return new datosConsulta() { numRegistros = 0, datos = null };
                    }*/
                }
            }

            string sqlDatosLista = filtroSeleccionado.sqlLista.Replace("#WHERE#", condicionSQL);
            if (filtroSeleccionado.tipoId == null || filtroSeleccionado.tipoId.Trim() == "" || filtroSeleccionado.tipoId.ToUpper().Trim() == "INT")
            {
                var datosLista = dbSIM.Database.SqlQuery<DATOSFILTROINT>(sqlDatosLista);

                resultado.datos = datosLista;
            }

            if (filtroSeleccionado.tipoId.ToUpper().Trim() == "STRING")
            {
                var datosLista = dbSIM.Database.SqlQuery<DATOSFILTROSTRING>(sqlDatosLista);

                resultado.datos = datosLista;
            }

            resultado.numRegistros = 0;

            return resultado;
        }
    }
}