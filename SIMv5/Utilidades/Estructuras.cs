using Newtonsoft.Json;
using SIM.Areas.ControlVigilancia.Models;
using SIM.Data;
using SIM.Areas.Tramites.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SIM.Utilidades
{
    public class FormularioDatos
    {
        public class DatosJson
        {
            public int resultado { set; get; } // 0 OK, -1 Error
            public string mensajeError { set; get; }
            public string json { set; get; }
        }

        private EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public DatosJson ObtenerJsonEstadoActuacion(int idEstado, int idFormulario, int idGrupoFormulario, bool copia)
        {
            DatosJson respuesta = new DatosJson();
            SIM.Data.Control.FORMULARIO formulario = null;
            string sql;

            try
            {
                formulario = dbSIM.FORMULARIO.Where(f => f.ID_FORMULARIO == idFormulario).FirstOrDefault();
            }
            catch (Exception error)
            {
                respuesta.resultado = -1;
                respuesta.mensajeError = "Error Consultando Formulario. " + error.Message;
                return respuesta;
            }

            if (formulario != null)
            {
                try
                {
                    // Tipo de Actuacion
                    sql = "SELECT v.ID_TIPOVISITA FROM " + formulario.TBL_ESTADOS + " ve INNER JOIN CONTROL.VISITA v ON ve.ID_VISITA = v.ID_VISITA WHERE ve." + formulario.S_CAMPO_ID_ITEM + "_ESTADO = " + idEstado.ToString();
                    var idTipoActuacion = dbSIM.Database.SqlQuery<int>(sql).ToList<int>()[0];

                    // Características asociadas al formulario de la actuación
                    //sql = "SELECT DISTINCT c.ID_CARACTERISTICA, c.S_DESCRIPCION, c.S_CARDINALIDAD, c.N_ORDEN, c.DESPLIEGUE_FORM, c.ID_CARACTETERISTICA_PADRE FROM control.EST_CARACTERISTICA c INNER JOIN CONTROL.EST_CARACTERISTICA_VARIABLE ecv ON c.ID_CARACTERISTICA = ecv.ID_CARACTERISTICA INNER JOIN control.est_variable v ON v.id_variable = ecv.id_variable INNER JOIN control.variable_tipoactuacion vta ON v.id_variable = vta.id_variable WHERE GRUPO_FORMULARIO = " + idGrupoFormulario.ToString() + " AND c.D_FIN is null AND vta.id_tipoactuacion = " + idTipoActuacion.ToString() + " AND ID_FORMULARIO = " + idFormulario.ToString() + " AND ecv.d_fin is null order by 6, 4";
                    sql = "SELECT DISTINCT c.ID_CARACTERISTICA, c.S_DESCRIPCION, c.S_CARDINALIDAD, c.N_ORDEN, c.DESPLIEGUE_FORM, c.ID_CARACTETERISTICA_PADRE FROM control.EST_CARACTERISTICA c WHERE GRUPO_FORMULARIO = " + idGrupoFormulario.ToString() + " AND c.D_FIN is null AND ID_FORMULARIO = " + idFormulario.ToString() + " order by 6, 4";
                    var caracteristicas = dbSIM.Database.SqlQuery<CARACTERISTICA>(sql).ToList<CARACTERISTICA>();

                    string listaCaracteristicas = string.Join(",", caracteristicas.Select<CARACTERISTICA, string>(f => f.ID_CARACTERISTICA.ToString()));

                    // Variables asociadas al formulario de la actuación
                    sql = "SELECT v.id_variable, ecv.id_caracteristica, 0 ID_CARACTERISTICA_ESTADO, v.s_nombre, v.id_tipo_dato, v.b_requerido, v.s_formula, v.s_ayuda, null ID_VALOR, null N_VALOR, null S_VALOR, null D_VALOR FROM control.EST_CARACTERISTICA c INNER JOIN control.est_caracteristica_variable ecv ON c.ID_CARACTERISTICA = ecv.ID_CARACTERISTICA INNER JOIN control.est_variable v ON v.id_variable = ecv.id_variable INNER JOIN control.variable_tipoactuacion vta ON v.id_variable = vta.id_variable WHERE GRUPO_FORMULARIO = " + idGrupoFormulario.ToString() + " AND c.D_FIN is null AND vta.id_tipoactuacion = " + idTipoActuacion.ToString() + " AND ID_FORMULARIO = " + idFormulario.ToString() + " AND ecv.d_fin is null ORDER BY ecv.n_orden ASC";
                    var variablesPlantilla = dbSIM.Database.SqlQuery<VARIABLE>(sql).ToList<VARIABLE>();

                    // Características_Estado asociadas al formulario de la actuación
                    sql = "SELECT ID_CARACTERISTICA_ESTADO, NOMBRE, ID_ESTADO, ID_CARACTERISTICA FROM CONTROL.EST_CARACTERISTICA_ESTADO WHERE activo = 1 and ID_CARACTERISTICA IN (" + listaCaracteristicas + ") and ID_ESTADO = " + idEstado.ToString() + " and TBL_ESTADO = '" + formulario.TBL_ESTADOS + "' order by 4, 1";
                    var caracteristicasEstado = dbSIM.Database.SqlQuery<ITEM>(sql).ToList<ITEM>();

                    string listaCaracteristicasEstado = string.Join(",", caracteristicasEstado.Select<ITEM, string>(f => f.ID_CARACTERISTICA_ESTADO.ToString()));

                    // Variables asociadas a cada estado de caracteristica
                    List<VARIABLE> variablesEstado;
                    if (listaCaracteristicasEstado != "")
                    {
                        sql = "SELECT v.id_variable, c.id_caracteristica, vv.ID_ESTRUCTURA_ESTADO ID_CARACTERISTICA_ESTADO, v.s_nombre, v.id_tipo_dato, v.b_requerido, v.s_formula, v.s_ayuda, DECODE(v.ID_TIPO_DATO,7,CONTROL.ROWCONCAT('SELECT E.ID_OPCION FROM CONTROL.EST_VALOR_ESTRUCTURA_OPCION E WHERE E.ID_VALOR_EST_ESTADO = ' || vv.ID_VALOR_EST_ESTADO),10,CONTROL.ROWCONCAT('SELECT E.ID_OPCION FROM CONTROL.EST_VALOR_ESTRUCTURA_OPCION E WHERE E.ID_VALOR_EST_ESTADO = ' || vv.ID_VALOR_EST_ESTADO),vv.id_valor) id_valor, vv.n_valor, vv.s_valor, to_char(vv.d_valor,'YYYY-MM-DD') d_valor FROM CONTROL.EST_CARACTERISTICA_VARIABLE c INNER JOIN CONTROL.EST_VARIABLE v ON c.id_variable = v.id_variable INNER JOIN CONTROL.EST_VALOR_ESTRUCTURA_ESTADO vv ON c.id_caracteristica_variable = vv.id_caracteristica_variable WHERE c.id_caracteristica IN (" + listaCaracteristicas + ") AND vv.ID_ESTRUCTURA_ESTADO IN (" + listaCaracteristicasEstado + ") AND c.d_fin is null ORDER BY c.n_orden ASC";
                        variablesEstado = dbSIM.Database.SqlQuery<VARIABLE>(sql).ToList<VARIABLE>();
                    }
                    else
                    {
                        variablesEstado = new List<VARIABLE>();
                    }

                    // Opciones asociadas a la variable
                    List<OPCION> opcionesVariables;
                    string listaVariables = string.Join(",", variablesPlantilla.Select<VARIABLE, string>(f => f.ID_VARIABLE.ToString()));
                    if (listaVariables != "")
                    {
                        sql = "SELECT ID_VARIABLE, ID_OPCION, S_NOMBRE DESCRIPCION FROM CONTROL.EST_OPCIONES_VARIABLE WHERE ID_VARIABLE IN (" + listaVariables + ") order by ID_OPCION";
                        opcionesVariables = dbSIM.Database.SqlQuery<OPCION>(sql).ToList<OPCION>();
                    }
                    else
                    {
                        opcionesVariables = new List<OPCION>();
                    }

                    // Asignar las opciones a las variables que las tengan
                    this.AsignarOpcionesVariables(variablesPlantilla, variablesEstado, opcionesVariables);

                    // Plantilla
                    this.AsignarPlantilla(caracteristicas, variablesPlantilla, idEstado);

                    // Datos Almacenados Actualmente
                    this.AsignarItemsEstadoVariables(caracteristicasEstado, variablesEstado, idEstado);
                    this.AsignarItemsEstado(caracteristicas, caracteristicasEstado, idEstado);

                    // Organización Jerárquica de las características
                    this.ReorganizarCaracteristicas(caracteristicas, idEstado);

                    if (copia)
                        this.InicializarItemsCopia(caracteristicas);

                    respuesta.json = JsonConvert.SerializeObject(caracteristicas);
                }
                catch (Exception error)
                {
                    respuesta.resultado = -1;
                    respuesta.mensajeError = "Error Obteniendo las Características. " + error.Message;
                    return respuesta;
                }
            }
            else
            {
                respuesta.resultado = -1;
                respuesta.mensajeError = "Formulario no Existe";
                return respuesta;
            }

            return respuesta;



            /*var sql = "SELECT ie." + formulario.S_CAMPO_ID_ITEM + "_ESTADO ID_ESTADO, v.ID_VISITA, v.S_ASUNTO ASUNTO, tv.ID_TIPOVISITA, tv.S_NOMBRE TIPO_VISITA, ev.ID_ESTADOVISITA, ev.S_NOMBRE ESTADO " +
                     "FROM CONTROL.VISITAESTADO ve INNER JOIN " +
                     "CONTROL.ESTADOVISITA ev ON ve.ID_ESTADOVISITA = ev.ID_ESTADOVISITA INNER JOIN " +
                     "CONTROL.VISITA v ON ve.ID_VISITA = v.ID_VISITA INNER JOIN " +
                     "CONTROL.TIPO_VISITA tv ON v.ID_TIPOVISITA = tv.ID_TIPOVISITA INNER JOIN " +
                     formulario.TBL_ESTADOS + " ie ON v.ID_VISITA = ie." + formulario.S_CAMPO_ID_VISITA + " " +
                     "WHERE ie." + formulario.S_CAMPO_ID_ITEM + " = " + idItem.ToString() + " AND ve.D_FIN IS NULL " +
                     " ORDER BY v.ID_VISITA DESC";
                var datos = dbSIM.Database.SqlQuery<ActuacionesItem>(sql);*/
        }

        private void ReorganizarCaracteristicas(List<CARACTERISTICA> caracteristicas, int idEstado)
        {
            CARACTERISTICA caracteristica;
            CARACTERISTICA caracteristicaPadre;

            for (int cont = caracteristicas.Count - 1; cont >= 0; cont--)
            {
                caracteristica = caracteristicas[cont];
                if (caracteristica.ITEMCARACTERISTICA == null)
                    caracteristica.ITEMCARACTERISTICA = new List<ITEM>();

                if (caracteristica.PLANTILLA == null)
                {
                    caracteristica.PLANTILLA = new ITEM();

                    caracteristica.PLANTILLA.ID_CARACTERISTICA_ESTADO = 0;
                    caracteristica.PLANTILLA.NOMBRE = "Plantilla";
                    caracteristica.PLANTILLA.ID_ESTADO = idEstado;
                }

                if (caracteristica.PLANTILLA.VARIABLES == null)
                    caracteristica.PLANTILLA.VARIABLES = new List<VARIABLE>();

                if (caracteristica.CARACTERISTICAS == null)
                    caracteristica.CARACTERISTICAS = new List<CARACTERISTICA>();

                if (caracteristica.ID_CARACTETERISTICA_PADRE != 0)
                {
                    caracteristicaPadre = caracteristicas.Find(c => c.ID_CARACTERISTICA == caracteristica.ID_CARACTETERISTICA_PADRE);

                    if (caracteristicaPadre != null)
                    {
                        if (caracteristicaPadre.CARACTERISTICAS == null)
                            caracteristicaPadre.CARACTERISTICAS = new List<CARACTERISTICA>();
                        caracteristicaPadre.CARACTERISTICAS.Insert(0, caracteristica);
                        caracteristicas.RemoveAt(cont);
                    }
                }
            }
        }

        private void InicializarItemsCopia(List<CARACTERISTICA> caracteristicas)
        {
            CARACTERISTICA caracteristica;

            for (int cont = caracteristicas.Count - 1; cont >= 0; cont--)
            {
                caracteristica = caracteristicas[cont];

                for (int contItem = 0; contItem < caracteristica.ITEMCARACTERISTICA.Count; contItem++)
                {
                    caracteristica.ITEMCARACTERISTICA[contItem].ID_CARACTERISTICA_ESTADO = 0;
                }

                if (caracteristica.CARACTERISTICAS.Count > 0)
                {
                    this.InicializarItemsCopia(caracteristica.CARACTERISTICAS);
                }
            }
        }

        private void AsignarPlantilla(List<CARACTERISTICA> caracteristicas, List<VARIABLE> variablesPlantilla, int idEstado)
        {
            CARACTERISTICA caracteristica;

            foreach (VARIABLE variable in variablesPlantilla)
            {
                if (variable.OPCIONES == null)
                    variable.OPCIONES = new List<OPCION>();

                caracteristica = caracteristicas.Find(c => c.ID_CARACTERISTICA == variable.ID_CARACTERISTICA);

                if (caracteristica != null)
                {
                    if (caracteristica.PLANTILLA == null)
                    {
                        caracteristica.PLANTILLA = new ITEM();

                        caracteristica.PLANTILLA.ID_CARACTERISTICA_ESTADO = 0;
                        caracteristica.PLANTILLA.NOMBRE = "Plantilla";
                        caracteristica.PLANTILLA.ID_ESTADO = idEstado;
                    }

                    if (caracteristica.PLANTILLA.VARIABLES == null)
                        caracteristica.PLANTILLA.VARIABLES = new List<VARIABLE>();

                    if (caracteristica.CARACTERISTICAS == null)
                        caracteristica.CARACTERISTICAS = new List<CARACTERISTICA>();

                    caracteristica.PLANTILLA.VARIABLES.Add(variable);
                }
            }
        }

        private void AsignarItemsEstadoVariables(List<ITEM> caracteristicasEstado, List<VARIABLE> variablesEstado, int idEstado)
        {
            ITEM caracteristicaEstado;

            foreach (VARIABLE variable in variablesEstado)
            {
                if (variable.OPCIONES == null)
                    variable.OPCIONES = new List<OPCION>();

                caracteristicaEstado = caracteristicasEstado.Find(c => c.ID_CARACTERISTICA_ESTADO == variable.ID_CARACTERISTICA_ESTADO && c.ID_CARACTERISTICA == variable.ID_CARACTERISTICA);

                if (caracteristicaEstado != null)
                {
                    if (caracteristicaEstado.VARIABLES == null)
                        caracteristicaEstado.VARIABLES = new List<VARIABLE>();

                    caracteristicaEstado.VARIABLES.Add(variable);
                }
            }
        }

        private void AsignarItemsEstado(List<CARACTERISTICA> caracteristicas, List<ITEM> itemsEstado, int idEstado)
        {
            CARACTERISTICA caracteristica;

            foreach (ITEM itemEstado in itemsEstado)
            {
                caracteristica = caracteristicas.Find(c => c.ID_CARACTERISTICA == itemEstado.ID_CARACTERISTICA);

                if (caracteristica != null)
                {
                    if (caracteristica.ITEMCARACTERISTICA == null)
                    {
                        caracteristica.ITEMCARACTERISTICA = new List<ITEM>();
                    }

                    if (itemEstado.VARIABLES == null)
                    {
                        itemEstado.VARIABLES = new List<VARIABLE>();
                    }

                    caracteristica.ITEMCARACTERISTICA.Add(itemEstado);
                }
            }
        }

        private void AsignarOpcionesVariables(List<VARIABLE> variablesPlanilla, List<VARIABLE> variablesEstado, List<OPCION> opcionesVariable)
        {
            var opciones = opcionesVariable.GroupBy(v => v.ID_VARIABLE);

            foreach (IGrouping<int, OPCION> grupoOpciones in opciones)
            {
                var opcionesGrupo = grupoOpciones.ToList<OPCION>();

                foreach (VARIABLE variable in variablesPlanilla.FindAll(v => v.ID_VARIABLE == grupoOpciones.Key))
                {
                    variable.OPCIONES = opcionesGrupo;
                }

                foreach (VARIABLE variable in variablesEstado.FindAll(v => v.ID_VARIABLE == grupoOpciones.Key))
                {
                    variable.OPCIONES = opcionesGrupo;
                }
            }
        }

        public DatosJson ObtenerJsonFormularios(int idInstalacion, int idTercero, int idVisita)
        {
            DatosJson respuesta = new DatosJson();
            List<ESTADO_FORMULARIOS> estados;
            string sql;

            try
            {
                // Recursos
                sql = "SELECT R.ID_RECURSO, R.S_RECURSO NOMBRE FROM CONTROL.RECURSO R ORDER BY 1";
                var recursos = dbSIM.Database.SqlQuery<RECURSO_FORMULARIOS>(sql).ToList<RECURSO_FORMULARIOS>();

                foreach (RECURSO_FORMULARIOS recurso in recursos)
                {
                    recurso.FORMULARIOS = new List<FORMULARIO_FORMULARIOS>();
                }

                string listaRecursos = string.Join(",", recursos.Select<RECURSO_FORMULARIOS, string>(r => r.ID_RECURSO.ToString()));

                // Formularios
                sql = "SELECT F.ID_FORMULARIO ID, F.ID_RECURSO, F.S_NOMBRE NOMBRE, F.S_URL URL, F.TBL_ITEM, F.TBL_ESTADOS, F.S_CAMPO_NOMBRE, F.S_CAMPO_ID_VISITA, F.S_CAMPO_ID_ITEM, F.TBL_FOTOS, F.TBL_GEO, S_CARDINALIDAD , S_URL_MAPA FROM CONTROL.FORMULARIO F WHERE F.ID_RECURSO IN (" + listaRecursos + ") ORDER BY F.ID_FORMULARIO";
                var formularios = dbSIM.Database.SqlQuery<FORMULARIO_FORMULARIOS>(sql).ToList<FORMULARIO_FORMULARIOS>();

                string listaFormularios = string.Join(",", formularios.Select<FORMULARIO_FORMULARIOS, string>(f => f.ID.ToString()));

                sql = "";

                // Items
                foreach (FORMULARIO_FORMULARIOS formulario in formularios)
                {
                    if (formulario.S_CARDINALIDAD.ToUpper().Trim() == "N")
                    {
                        if (sql != "")
                        {
                            sql += " UNION ALL ";
                        }

                        sql += "SELECT F.ID_ITEM, " +
                            "F.ID_FORMULARIO, " +
                            "NVL( FI." + formulario.S_CAMPO_NOMBRE + ", ' ') AS NOMBRE, " +
                            //(formulario.S_CAMPO_ID_ITEM == "ID" ? "ID_ITEM" : formulario.S_CAMPO_ID_ITEM) + "_ESTADO AS ESTADO " +
                            "MAX(CASE WHEN " + formulario.S_CAMPO_ID_VISITA + " = " + idVisita.ToString() + " THEN " + (formulario.S_CAMPO_ID_ITEM == "ID" ? "ID_ITEM" : formulario.S_CAMPO_ID_ITEM) + "_ESTADO ELSE 0 END) AS ESTADO " +
                            "FROM CONTROL.FORMULARIO_ITEM F " +
                            "   INNER JOIN " + formulario.TBL_ITEM + " FI ON F.ID_ITEM = FI." + (formulario.S_CAMPO_ID_ITEM == "ID" ? "ID_ITEM" : formulario.S_CAMPO_ID_ITEM) + " " +
                            //" INNER JOIN " + formulario.TBL_ESTADOS + " FEI ON F.ID_ITEM = FEI." + (formulario.S_CAMPO_ID_ITEM == "ID" ? "ID_ITEM" : formulario.S_CAMPO_ID_ITEM) + " AND " + formulario.S_CAMPO_ID_VISITA + " = " + idVisita.ToString() + " " +
                            "   INNER JOIN " + formulario.TBL_ESTADOS + " FEI ON F.ID_ITEM = FEI." + (formulario.S_CAMPO_ID_ITEM == "ID" ? "ID_ITEM" : formulario.S_CAMPO_ID_ITEM) + " " +
                            "WHERE F.ID_FORMULARIO = " + formulario.ID + " and F.ID_INSTALACION = " + idInstalacion.ToString() + " and F.ID_TERCERO = " + idTercero.ToString() + " " +
                            "GROUP BY F.ID_ITEM, F.ID_FORMULARIO, NVL( FI." + formulario.S_CAMPO_NOMBRE + ", ' ')";
                    }
                }

                var items = dbSIM.Database.SqlQuery<ITEM_FORMULARIOS>(sql).ToList<ITEM_FORMULARIOS>();

                string listaItems = string.Join(",", items.Select<ITEM_FORMULARIOS, string>(i => i.ID_ITEM.ToString()));

                if (listaItems != "")
                {
                    sql = "";

                    // Estados
                    foreach (FORMULARIO_FORMULARIOS formulario in formularios)
                    {
                        if (formulario.S_CARDINALIDAD.ToUpper().Trim() == "N")
                        {
                            var listaItemsFormulario = string.Join(",", items.Where(i => i.ID_FORMULARIO == formulario.ID).Select<ITEM_FORMULARIOS, string>(i => i.ID_ITEM.ToString()));

                            if (listaItemsFormulario.Trim() != "")
                            {
                                if (sql != "")
                                {
                                    sql += " UNION ALL ";
                                }

                                /*sql += "SELECT " + formulario.ID.ToString() + "AS ID_FORMULARIO, " + (formulario.S_CAMPO_ID_ITEM == "ID" ? "ID_ITEM" : formulario.S_CAMPO_ID_ITEM) + " AS ID_ITEM, e.ID_ESTADO, NVL(e.S_NOMBRE, ' ') AS S_NOMBRE, e.S_DESCRIPCION, i." + formulario.S_CAMPO_ID_VISITA + " AS ID_VISITA, i.D_INICIO " +
                                    "FROM CONTROL.ESTADO_RECURSO e INNER JOIN " +
                                    formulario.TBL_ESTADOS + " i ON e.id_estado = i.id_estado " +
                                    "WHERE i.d_fin IS NULL AND i." + (formulario.S_CAMPO_ID_ITEM == "ID" ? "ID_ITEM" : formulario.S_CAMPO_ID_ITEM) + " IN (" + listaItems + ") AND " + formulario.S_CAMPO_ID_VISITA + " = " + idVisita.ToString();*/

                                sql += "SELECT " + formulario.ID.ToString() + " AS ID_FORMULARIO, " + (formulario.S_CAMPO_ID_ITEM == "ID" ? "ID_ITEM" : formulario.S_CAMPO_ID_ITEM) + " AS ID_ITEM, i." + (formulario.S_CAMPO_ID_ITEM == "ID" ? "ID_ITEM" : formulario.S_CAMPO_ID_ITEM) + "_ESTADO AS ID_ESTADO, NVL(v.S_ASUNTO, ' ') AS S_NOMBRE, NVL(v.S_ASUNTO, ' ') AS S_DESCRIPCION, i." + formulario.S_CAMPO_ID_VISITA + " AS ID_VISITA, i.D_INICIO " +
                                    "FROM " + formulario.TBL_ESTADOS + " i INNER JOIN " +
                                    "   CONTROL.VISITA v ON i.ID_VISITA = v.ID_VISITA " +
                                    "WHERE i." + (formulario.S_CAMPO_ID_ITEM == "ID" ? "ID_ITEM" : formulario.S_CAMPO_ID_ITEM) + " IN (" + listaItemsFormulario + ")";
                            }
                        }
                    }

                    if (sql != "")
                        estados = dbSIM.Database.SqlQuery<ESTADO_FORMULARIOS>(sql).ToList<ESTADO_FORMULARIOS>();
                    else
                        estados = new List<ESTADO_FORMULARIOS>();
                }
                else
                {
                    estados = new List<ESTADO_FORMULARIOS>();
                }

                AsignarEstadosItem(items, estados);
                AsignarItemsFormulario(formularios, items);
                AsignarFormulariosRecurso(recursos, formularios);

                respuesta.json = JsonConvert.SerializeObject(recursos);
            }
            catch (Exception error)
            {
                respuesta.resultado = -1;
                respuesta.mensajeError = "Error Obteniendo Formularios. " + error.Message;
                return respuesta;
            }

            return respuesta;
        }

        private void AsignarEstadosItem(List<ITEM_FORMULARIOS> items, List<ESTADO_FORMULARIOS> estados)
        {
            ITEM_FORMULARIOS item;

            foreach (ESTADO_FORMULARIOS estado in estados)
            {
                item = items.Find(i => i.ID_FORMULARIO == estado.ID_FORMULARIO && i.ID_ITEM == estado.ID_ITEM);

                if (item != null)
                {
                    if (item.ESTADOS == null)
                    {
                        item.ESTADOS = new List<ESTADO_FORMULARIOS>();
                    }

                    item.ESTADOS.Add(estado);
                }
            }
        }

        private void AsignarItemsFormulario(List<FORMULARIO_FORMULARIOS> formularios, List<ITEM_FORMULARIOS> items)
        {
            FORMULARIO_FORMULARIOS formulario;

            foreach (ITEM_FORMULARIOS item in items)
            {
                formulario = formularios.Find(f => f.ID == item.ID_FORMULARIO);

                if (formulario != null)
                {
                    if (formulario.ITEMS == null)
                    {
                        formulario.ITEMS = new List<ITEM_FORMULARIOS>();
                    }

                    formulario.ITEMS.Add(item);
                }

                if (item.ESTADOS == null)
                    item.ESTADOS = new List<ESTADO_FORMULARIOS>();
            }
        }

        private void AsignarFormulariosRecurso(List<RECURSO_FORMULARIOS> recursos, List<FORMULARIO_FORMULARIOS> formularios)
        {
            RECURSO_FORMULARIOS recurso;

            foreach (FORMULARIO_FORMULARIOS formulario in formularios)
            {
                if (formulario.NOMBRE == null)
                    formulario.NOMBRE = "";

                recurso = recursos.Find(r => r.ID_RECURSO == formulario.ID_RECURSO);

                if (recurso != null)
                {
                    if (recurso.FORMULARIOS == null)
                    {
                        recurso.FORMULARIOS = new List<FORMULARIO_FORMULARIOS>();
                    }

                    recurso.FORMULARIOS.Add(formulario);
                }

                if (formulario.ITEMS == null)
                    formulario.ITEMS = new List<ITEM_FORMULARIOS>();
            }
        }
    }

    public class EncuestaDatos
    {
        public class DatosJson
        {
            public int resultado { set; get; } // 0 OK, -1 Error
            public string mensajeError { set; get; }
            public string json { set; get; }
        }

        private EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public DatosJson ObtenerJsonEncuesta(int idEstado, int idFormulario, int idVigencia)
        {
            return ObtenerJsonEncuesta(idEstado, idFormulario, idVigencia, null, null);
        }

        public DatosJson ObtenerJsonEncuesta(int idEstado, int idFormulario, int idVigencia, int? idTercero, int? idInstalacion)
        {
            DatosJson respuesta = new DatosJson();
            SIM.Data.Control.FORMULARIO formulario = null;
            string sql;

            try
            {
                formulario = dbSIM.FORMULARIO.Where(f => f.ID_FORMULARIO == idFormulario).FirstOrDefault();
            }
            catch (Exception error)
            {
                respuesta.resultado = -1;
                respuesta.mensajeError = "Error Consultando Formulario. " + error.Message;
                return respuesta;
            }

            var valorVigencia = dbSIM.VIGENCIA_SOLUCION.Where(vs => vs.ID_ESTADO == idEstado).FirstOrDefault();

            if (formulario != null)
            {
                try
                {
                    // Encuestas
                    sql =
                        "SELECT s.ID_SOLUCION, e.ID_ENCUESTA, e.S_NOMBRE, e.S_DESCRIPCION, e.S_TIPO, s.N_VALOR, DECODE(NVL(s.ID_ESTADO, 0), 0, 0, 1) ESTADO, s.D_SOLUCION " +
                        "FROM CONTROL.FORMULARIO_ENCUESTA fe INNER JOIN " +
                        "    CONTROL.ENC_ENCUESTA e ON fe.ID_ENCUESTA = e.ID_ENCUESTA INNER JOIN " +
                        //"    CONTROL.ENCUESTA_VIGENCIA ev ON e.ID_ENCUESTA = ev.ID_ENCUESTA AND ID_VIGENCIA = " + idVigencia.ToString() + " LEFT OUTER JOIN " + // Hubo que cambiarlo porque CONTROL.ENCUESTA_VIGENCIA tiene registros repetidos como por ejemplo para ID_VIGENCIA = 1081 se repite el 1622 y el 1623
                        "    (SELECT DISTINCT ID_ENCUESTA FROM CONTROL.ENCUESTA_VIGENCIA WHERE ID_VIGENCIA = " + idVigencia.ToString() + ") ev ON e.ID_ENCUESTA = ev.ID_ENCUESTA LEFT OUTER JOIN " +
                        "    CONTROL.ENC_SOLUCION s ON s.ID_ENCUESTA = e.ID_ENCUESTA AND S.ID_ESTADO = " + idEstado.ToString() + " " +
                        "WHERE fe.ID_FORMULARIO = " + idFormulario.ToString() + " AND fe.D_FIN IS NULL " +
                        "ORDER BY fe.N_ORDEN";

                    var encuestas = dbSIM.Database.SqlQuery<ENCUESTA>(sql).ToList<ENCUESTA>();

                    string listaEncuestas = string.Join(",", encuestas.Select<ENCUESTA, string>(e => e.ID_ENCUESTA.ToString()));
                    string listaSolucionesEncuestas = string.Join(",", encuestas.Where(e => e.ID_SOLUCION != null).Select<ENCUESTA, string>(e => e.ID_SOLUCION.ToString()));

                    // Preguntas
                    sql =
                        "SELECT ep.ID_ENCUESTA, sp.ID_SOLUCION, sp.X_VALOR, sp.Y_VALOR, p.ID_PREGUNTA, p.S_NOMBRE, p.ID_TIPOPREGUNTA, p.S_AYUDA, ep.S_REQUERIDA, ep.N_FACTOR, NVL(ep.N_NIVEL, 1) AS N_NIVEL, NVL(ep.N_TIPO_TITULO, 1) AS N_TIPO_TITULO, sp.ID_VALOR, sp.N_VALOR, sp.S_VALOR, TO_CHAR(sp.D_VALOR,'yyyy-mm-dd') AS D_VALOR, sp.S_OBSERVACION, NVL(sp.ID_SOLUCION_PREGUNTAS,-1) ID_SOLUCION_PREGUNTAS, SQL_OPCION_EXTERNO, ID_OPCION_RESPUESTA_DOM " +
                        "FROM CONTROL.ENC_ENCUESTA_PREGUNTA ep INNER JOIN " +
                        "    CONTROL.ENC_PREGUNTA p ON ep.ID_PREGUNTA = p.ID_PREGUNTA LEFT OUTER JOIN " +
                        "    CONTROL.ENC_SOLUCION_PREGUNTAS sp ON p.ID_PREGUNTA = sp.ID_PREGUNTA AND sp.ID_SOLUCION IN (" + (listaSolucionesEncuestas.Trim() != "" ? listaSolucionesEncuestas : "-1") + ") " +
                        "WHERE ep.ID_ENCUESTA IN (" + listaEncuestas + ") " +
                        "ORDER BY ep.N_ORDEN";

                    var preguntas = dbSIM.Database.SqlQuery<PREGUNTA>(sql).ToList<PREGUNTA>();

                    string listaPreguntas = string.Join(",", preguntas.Select<PREGUNTA, string>(e => e.ID_PREGUNTA.ToString()));

                    // Dependencias
                    sql =
                        "SELECT pd.ID_PREGUNTA_PADRE, pd.ID_PREGUNTA_HIJA, 'encuesta_' || eph.ID_ENCUESTA || '_pregunta_' || pd.ID_PREGUNTA_HIJA AS ID_HIJO, pd.TIPO, pd.OPCIONES " +
                        "FROM CONTROL.ENC_PREGUNTA_DEPENDENCIA pd INNER JOIN " +
                        "CONTROL.ENC_ENCUESTA_PREGUNTA ep ON pd.ID_PREGUNTA_PADRE = ep.ID_PREGUNTA INNER JOIN " +
                        "CONTROL.ENC_ENCUESTA_PREGUNTA eph ON pd.ID_PREGUNTA_HIJA = eph.ID_PREGUNTA " +
                        "WHERE pd.ID_PREGUNTA_PADRE IN (" + listaPreguntas + ")";

                    var dependencias = dbSIM.Database.SqlQuery<DEPENDENCIA>(sql).ToList<DEPENDENCIA>();

                    // Asigna las opciones para las preguntas tipo 2 o 3
                    string listaOpciones = string.Join(",", preguntas.Where(p => p.ID_TIPOPREGUNTA == 2 || p.ID_TIPOPREGUNTA == 3).Select<PREGUNTA, string>(p => p.ID_PREGUNTA.ToString()));
                    string listaSoluciones = string.Join(",", preguntas.Where(p => p.ID_TIPOPREGUNTA == 2 || p.ID_TIPOPREGUNTA == 3).Select<PREGUNTA, string>(p => p.ID_SOLUCION_PREGUNTAS.ToString()));

                    if (listaOpciones.Trim() != "")
                        this.AsignarListasPreguntaOpciones(preguntas.Where(p => (p.ID_TIPOPREGUNTA == 2 || p.ID_TIPOPREGUNTA == 3)).ToList(), listaOpciones, listaSoluciones);

                    // Asigna las opciones para las preguntas tipo 12 y 13 con ID_OPCION_RESPUESTA_DOM NOT NULL
                    string listaOpcionesDom = string.Join(",", preguntas.Where(p => (p.ID_TIPOPREGUNTA == 12 || p.ID_TIPOPREGUNTA == 13) && p.ID_OPCION_RESPUESTA_DOM != null).Select<PREGUNTA, string>(e => e.ID_OPCION_RESPUESTA_DOM.ToString()));
                    string listaSolucionesDom = string.Join(",", preguntas.Where(p => (p.ID_TIPOPREGUNTA == 12 || p.ID_TIPOPREGUNTA == 13) && p.ID_OPCION_RESPUESTA_DOM != null).Select<PREGUNTA, string>(p => p.ID_SOLUCION_PREGUNTAS.ToString()));
                    if (listaOpcionesDom.Trim() != "")
                        this.AsignarListasPregunta(preguntas.Where(p => (p.ID_TIPOPREGUNTA == 12 || p.ID_TIPOPREGUNTA == 13) && p.ID_OPCION_RESPUESTA_DOM != null).ToList(), listaOpcionesDom, listaSolucionesDom, idTercero, idInstalacion, valorVigencia.VALOR);

                    // Asigna las opciones para las preguntas tipo 12 y 13 con ID_OPCION_RESPUESTA_DOM NULL
                    string listaSolucionesDomSQL = string.Join(",", preguntas.Where(p => (p.ID_TIPOPREGUNTA == 12 || p.ID_TIPOPREGUNTA == 13) && p.ID_OPCION_RESPUESTA_DOM == null && p.SQL_OPCION_EXTERNO != null && p.SQL_OPCION_EXTERNO.Trim() != "").Select<PREGUNTA, string>(p => p.ID_SOLUCION_PREGUNTAS.ToString()));
                    this.AsignarListasSQLPregunta(preguntas.Where(p => (p.ID_TIPOPREGUNTA == 12 || p.ID_TIPOPREGUNTA == 13) && p.ID_OPCION_RESPUESTA_DOM == null && p.SQL_OPCION_EXTERNO != null && p.SQL_OPCION_EXTERNO.Trim() != "").ToList(), listaSolucionesDomSQL, idTercero, idInstalacion, valorVigencia.VALOR);

                    this.AsignarOpcionesNULLPregunta(preguntas);

                    this.AsignarDependenciasPregunta(preguntas, dependencias);

                    this.AsignarPreguntasEncuesta(encuestas, preguntas);

                    respuesta.json = JsonConvert.SerializeObject(encuestas);
                }
                catch (Exception error)
                {
                    respuesta.resultado = -1;
                    respuesta.mensajeError = "Error Obteniendo las Encuestas. " + error.Message;
                    return respuesta;
                }
            }
            else
            {
                respuesta.resultado = -1;
                respuesta.mensajeError = "Formulario no Existe";
                return respuesta;
            }

            return respuesta;
        }

        private void AsignarListasPreguntaOpciones(List<PREGUNTA> preguntas, string preguntasOpciones, string listaSoluciones)
        {
            var sqlListaOpciones =
                        "SELECT eor.ID_RESPUESTA, eor.ID_PREGUNTA, eor.S_VALOR, eor.S_CODIGO, CASE WHEN sp.ID_RESPUESTA IS NULL THEN 0 ELSE 1 END SELECTED  " +
                        "FROM CONTROL.ENC_OPCION_RESPUESTA eor LEFT OUTER JOIN " +
                        "    CONTROL.ENC_SOLUCION_PREGUNTAS_OPC sp ON eor.ID_RESPUESTA = sp.ID_RESPUESTA AND sp.ID_SOLUCION_PREGUNTAS IN (" + listaSoluciones + ") " +
                        "WHERE eor.ID_PREGUNTA IN (" + preguntasOpciones + ") " +
                        "ORDER BY N_ORDEN";

            var listaOpciones = dbSIM.Database.SqlQuery<OPCIONES>(sqlListaOpciones).ToList<OPCIONES>();
            List<int> idPreguntas = preguntasOpciones.Split(',').Select(int.Parse).ToList();

            foreach (PREGUNTA pregunta in preguntas.Where(p => idPreguntas.Contains(p.ID_PREGUNTA)))
            {
                var opciones = listaOpciones.Where(lo => lo.ID_PREGUNTA == pregunta.ID_PREGUNTA).ToList();

                pregunta.OPCIONES = opciones;
            }
        }

        private void AsignarListasPregunta(List<PREGUNTA> preguntas, string opcionesDom, string listaSoluciones, int? idTercero, int? idInstalacion, string valorVigencia)
        {
            var sqlListaOpciones =
                        "SELECT ID_OPCION_RESPUESTA_DOM, S_SQL " +
                        "FROM CONTROL.ENC_OPCION_RESPUESTA_DOM " +
                        "WHERE ID_OPCION_RESPUESTA_DOM in (" + opcionesDom + ")";

            var listaOpciones = dbSIM.Database.SqlQuery<dynamic>(sqlListaOpciones).ToList<dynamic>();

            foreach (var lista in listaOpciones)
            {
                string sql;

                sql = lista.S_SQL;

                if (idTercero != null)
                    sql = sql.Replace("@IdTercero", idTercero.ToString());

                if (idInstalacion != null)
                    sql = sql.Replace("@IdInstalacion", idInstalacion.ToString());

                if (valorVigencia != null && valorVigencia.Trim() != "")
                    sql = sql.Replace("@ValorVigencia", "'" + valorVigencia + "'");

                List<OPCIONES> opciones = dbSIM.Database.SqlQuery<OPCIONES>(sql).ToList<OPCIONES>();

                foreach (PREGUNTA pregunta in preguntas.Where(p => p.ID_OPCION_RESPUESTA_DOM == lista.ID_OPCION_RESPUESTA_DOM))
                {
                    var sqlOpcionesSeleccionadas = "SELECT ID_PREGUNTA, ID_VALOR FROM CONTROL.ENC_SOLUCION_PREGUNTAS WHERE ID_PREGUNTA = " + pregunta.ID_PREGUNTA.ToString() + " AND sp.ID_SOLUCION_PREGUNTAS IN (" + listaSoluciones + ") ";
                    var opcionesSeleccionadas = dbSIM.Database.SqlQuery<dynamic>(sqlOpcionesSeleccionadas).ToList<dynamic>();

                    var opcionesPregunta = (from o in opciones
                                            select new OPCIONES
                                            {
                                                ID_RESPUESTA = o.ID_RESPUESTA,
                                                ID_PREGUNTA = o.ID_PREGUNTA,
                                                S_VALOR = o.S_VALOR,
                                                S_CODIGO = o.S_CODIGO,
                                                SELECTED = (opcionesSeleccionadas.Contains(o.ID_RESPUESTA) ? 1 : 0)
                                            }).ToList();

                    if (opcionesPregunta == null)
                        pregunta.OPCIONES = new List<OPCIONES>();
                    else
                        pregunta.OPCIONES = opcionesPregunta;
                }
            }
        }

        private void AsignarListasSQLPregunta(List<PREGUNTA> preguntas, string listaSoluciones, int? idTercero, int? idInstalacion, string valorVigencia)
        {
            Dictionary<string, List<OPCIONES>> opcionesSQL = new Dictionary<string, List<OPCIONES>>();

            foreach (var pregunta in preguntas)
            {
                if (!opcionesSQL.ContainsKey(pregunta.SQL_OPCION_EXTERNO.Trim()))
                {
                    string sql;

                    sql = pregunta.SQL_OPCION_EXTERNO.Trim();

                    if (idTercero != null)
                        sql = sql.Replace("@IdTercero", idTercero.ToString());

                    if (idInstalacion != null)
                        sql = sql.Replace("@IdInstalacion", idInstalacion.ToString());

                    if (valorVigencia != null && valorVigencia.Trim() != "")
                        sql = sql.Replace("@ValorVigencia", "'" + valorVigencia + "'");

                    List<OPCIONES> opciones = dbSIM.Database.SqlQuery<OPCIONES>(sql).ToList<OPCIONES>();

                    opcionesSQL.Add(pregunta.SQL_OPCION_EXTERNO.Trim(), opciones);
                }

                string sqlOpcionesSeleccionadas = "";
                if (pregunta.ID_TIPOPREGUNTA == 12)
                    sqlOpcionesSeleccionadas = "SELECT ID_VALOR FROM CONTROL.ENC_SOLUCION_PREGUNTAS sp WHERE ID_PREGUNTA = " + pregunta.ID_PREGUNTA.ToString() + " AND sp.ID_SOLUCION_PREGUNTAS IN (" + listaSoluciones + ") ";
                else
                    sqlOpcionesSeleccionadas = "SELECT ID_RESPUESTA AS ID_VALOR FROM CONTROL.ENC_SOLUCION_PREGUNTAS sp INNER JOIN CONTROL.ENC_SOLUCION_PREGUNTAS_OPC spo ON sp.ID_SOLUCION_PREGUNTAS = spo.ID_SOLUCION_PREGUNTAS WHERE ID_PREGUNTA = " + pregunta.ID_PREGUNTA.ToString() + " AND sp.ID_SOLUCION_PREGUNTAS IN (" + listaSoluciones + ") ";

                var opcionesSeleccionadas = dbSIM.Database.SqlQuery<int>(sqlOpcionesSeleccionadas).ToList<int>();

                var opcionesPregunta = (from o in opcionesSQL[pregunta.SQL_OPCION_EXTERNO.Trim()]
                                        select new OPCIONES
                                        {
                                            ID_RESPUESTA = o.ID_RESPUESTA,
                                            ID_PREGUNTA = o.ID_PREGUNTA,
                                            S_VALOR = o.S_VALOR,
                                            S_CODIGO = o.S_CODIGO,
                                            SELECTED = (opcionesSeleccionadas.Contains(o.ID_RESPUESTA) ? 1 : 0)
                                        }).ToList();

                if (opcionesPregunta == null)
                    pregunta.OPCIONES = new List<OPCIONES>();
                else
                    pregunta.OPCIONES = opcionesPregunta;
            }
        }

        private void AsignarOpcionesNULLPregunta(List<PREGUNTA> preguntas)
        {
            foreach (PREGUNTA pregunta in preguntas)
            {
                if (pregunta.OPCIONES == null)
                {
                    pregunta.OPCIONES = new List<OPCIONES>();
                }

                if (pregunta.DEPENDENCIA == null)
                {
                    pregunta.DEPENDENCIA = new List<DEPENDENCIA>();
                }

                if (pregunta.ID_TIPOPREGUNTA == 4)
                {
                    pregunta.RANGO = pregunta.SQL_OPCION_EXTERNO;
                }

                if (pregunta.ID_TIPOPREGUNTA == 10) // Establece condicional de diferencia entre preguntas. Por ejemplo que la diferencia entre 2 horas no sea mayor a 3
                {
                    pregunta.RANGO = pregunta.SQL_OPCION_EXTERNO;
                }
            }
        }

        private void AsignarDependenciasPregunta(List<PREGUNTA> preguntas, List<DEPENDENCIA> dependencias)
        {
            PREGUNTA pregunta;

            foreach (DEPENDENCIA dependencia in dependencias)
            {
                pregunta = preguntas.Find(e => e.ID_PREGUNTA == dependencia.ID_PREGUNTA_PADRE);

                if (pregunta != null)
                {
                    if (pregunta.DEPENDENCIA == null)
                    {
                        pregunta.DEPENDENCIA = new List<DEPENDENCIA>();
                    }

                    pregunta.DEPENDENCIA.Add(dependencia);
                }
            }
        }

        private void AsignarPreguntasEncuesta(List<ENCUESTA> encuestas, List<PREGUNTA> preguntas)
        {
            ENCUESTA encuesta;

            foreach (ENCUESTA encuestasPreguntas in encuestas)
            {
                if (encuestasPreguntas.PREGUNTAS == null)
                {
                    encuestasPreguntas.PREGUNTAS = new List<PREGUNTA>();
                }
            }

            foreach (PREGUNTA pregunta in preguntas)
            {
                encuesta = encuestas.Find(e => e.ID_ENCUESTA == pregunta.ID_ENCUESTA);

                if (encuesta != null)
                {
                    if (encuesta.PREGUNTAS == null)
                    {
                        encuesta.PREGUNTAS = new List<PREGUNTA>();
                    }

                    encuesta.PREGUNTAS.Add(pregunta);
                }
            }
        }
    }
}