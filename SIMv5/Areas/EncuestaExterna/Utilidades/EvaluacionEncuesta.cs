using Oracle.ManagedDataAccess.Client;
using SIM.Data;
using SIM.Data.Control;
using SIM.Data.Tramites;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SIM.Areas.EncuestaExterna
{
    public class datosEncuestaDireccion
    {
        public string S_DIRECCION { get; set; }
        public string X_VALOR { get; set; }
        public string Y_VALOR { get; set; }
    }

    public class datosRadicado
    {
        public string S_RADICADO { get; set; }
        public DateTime? D_FECHA { get; set; }
    }

    public class encuestasInstalaciones
    {
        public int? ID_EVALUACION_ENCUESTA { get; set; }
        public int? ID_EVALUACION_TERCERO { get; set; }
        public int ID_TERCERO { get; set; }
        public int ID_INSTALACION { get; set; }
        public string INSTALACION { get; set; }
        public string ENC_SITIO { get; set; }
        public string ENC_TRABAJADORES { get; set; }
        public string RESULTADO { get; set; }
        public string S_EXCLUIR { get; set; }
    }

    public class datosInstalacion
    {
        public string S_DIRECCION { get; set; }
        public string S_TELEFONO { get; set; }
        public string S_CORREO { get; set; }
        public string S_MUNICIPIO { get; set; }
    }

    public class datosSeguimiento
    {
        public string BASE { get; set; }
        public string ACTUAL { get; set; }
        public float CO2B { get; set; }
        public float CO2BT { get; set; }
        public float CO2A { get; set; }
        public float CO2AT { get; set; }
        public float CO2VAR { get; set; }
        public float PM25B { get; set; }
        public float PM25BT { get; set; }
        public float PM25A { get; set; }
        public float PM25AT { get; set; }
        public float PM25VAR { get; set; }
    }

    public class EvaluacionEncuestaUtilidad
    {
        public static EVALUACION_ENCUESTA GenerarEvaluacionEncuesta(int ideet, int i, bool excluir, bool principal, EntitiesSIMOracle dbSIM)
        {
            datosRadicado radicado = (from ee in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                      join ge in dbSIM.FRM_GENERICO_ESTADO on ee.ID_ESTADO equals ge.ID_ESTADO
                                      join rd in dbSIM.RADICADO_DOCUMENTO on ge.CODRADICADO equals rd.ID_RADICADODOC
                                      where ee.ID == ideet
                                      select new datosRadicado { S_RADICADO = rd.S_RADICADO, D_FECHA = rd.D_RADICADO }).FirstOrDefault();

            if (radicado == null)
            {
                radicado = (from ee in dbSIM.VW_PMES_EVALUACION_EMPRESAS
                            join eet in dbSIM.EVALUACION_ENCUESTA_TERCERO on ee.CODTRAMITE equals eet.CODTRAMITE
                            where eet.ID == ideet
                            select new datosRadicado { S_RADICADO = ee.S_RADICADO, D_FECHA = ee.D_RADICADO }).FirstOrDefault();
            }

            //dbSIM.EVALUACION_ENCUESTA_TERCERO

            var poblacion = (from ee in dbSIM.EVALUACION_ENCUESTA_TERCERO
                             join vpmes in dbSIM.VWM_PMES on ee.ID_TERCERO equals vpmes.ID_TERCERO
                             where ee.ID == ideet && vpmes.ID_INSTALACION == i && vpmes.VIGENCIA == ee.S_VALOR_VIGENCIA
                             select vpmes.N_POBLACION).FirstOrDefault();
            /*if (poblacion == null)
            {
            }*/

            var co2 = (from ee in dbSIM.EVALUACION_ENCUESTA_TERCERO
                       join vpmes in dbSIM.VWM_PMES on ee.ID_TERCERO equals vpmes.ID_TERCERO
                       where ee.ID == ideet && vpmes.ID_INSTALACION == i && vpmes.VIGENCIA == ee.S_VALOR_VIGENCIA
                       select vpmes.N_CO2P).Sum();

            var pm25 = (from ee in dbSIM.EVALUACION_ENCUESTA_TERCERO
                        join vpmes in dbSIM.VWM_PMES on ee.ID_TERCERO equals vpmes.ID_TERCERO
                        where ee.ID == ideet && vpmes.ID_INSTALACION == i && vpmes.VIGENCIA == ee.S_VALOR_VIGENCIA
                        select vpmes.N_PM25P).Sum();

            var valorVigencia = (from ee in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                 where ee.ID == ideet
                                 select ee.S_VALOR_VIGENCIA).FirstOrDefault();

            OracleParameter idInstalacionParameter = new OracleParameter("idInstalacion", i);
            OracleParameter valorVigenciaParameter = new OracleParameter("valorVigencia", valorVigencia);

            datosEncuestaDireccion direccion = dbSIM.Database.SqlQuery<datosEncuestaDireccion>("SELECT sp.S_VALOR AS S_DIRECCION, sp.X_VALOR, sp.Y_VALOR " +
                                                                "FROM CONTROL.ENC_SOLUCION_PREGUNTAS sp INNER JOIN " +
                                                                "  CONTROL.ENC_SOLUCION s ON sp.ID_SOLUCION = s.ID_SOLUCION INNER JOIN " +
                                                                "  CONTROL.VIGENCIA_SOLUCION vs ON s.ID_ESTADO = vs.ID_ESTADO INNER JOIN " +
                                                                "  CONTROL.FRM_GENERICO_ESTADO ge ON s.ID_ESTADO = ge.ID_ESTADO " +
                                                                "WHERE sp.ID_PREGUNTA = 552 AND ge.ID_INSTALACION = :idInstalacion AND vs.VALOR = :valorVigencia", new OracleParameter[] { idInstalacionParameter, valorVigenciaParameter }).FirstOrDefault();

            var evaluacionEncuesta = (from ee in dbSIM.EVALUACION_ENCUESTA
                                      where ee.ID_EVALUACION_TERCERO == ideet && ee.ID_INSTALACION == i
                                      select ee).FirstOrDefault();

            if (evaluacionEncuesta == null) // No existe la evaluación de la encuesta, por lo tanto se crea
            {
                evaluacionEncuesta = new EVALUACION_ENCUESTA();
                evaluacionEncuesta.ID_EVALUACION_TERCERO = (int)ideet;
                evaluacionEncuesta.ID_INSTALACION = (int)i;
                evaluacionEncuesta.S_ESTADO = "R"; // R: Registrado, G: Generado
                evaluacionEncuesta.S_RESULTADO = "P"; // P-Pendiente, C - Cumplió, N - No Cumplió
                evaluacionEncuesta.S_EXCLUIR = excluir ? "S" : "N";
                evaluacionEncuesta.S_PRINCIPAL = principal ? "S" : "N";

                /*if (radicado != null)
                {
                    evaluacionEncuesta.S_MEDIO_ENTREGA = "S";
                    evaluacionEncuesta.S_RADICADO = radicado.S_RADICADO;
                    evaluacionEncuesta.D_FECHA_ENTREGA = radicado.D_FECHA;
                }*/

                if (direccion != null)
                {
                    if (direccion.X_VALOR != null && direccion.Y_VALOR != null)
                        evaluacionEncuesta.S_COORDENADA = direccion.X_VALOR.ToString().Replace(",", ".") + " | " + direccion.Y_VALOR.ToString().Replace(",", ".");

                    evaluacionEncuesta.S_DIRECCION = direccion.S_DIRECCION;
                }

                if (co2 != null)
                {
                    evaluacionEncuesta.N_CO2P = co2;

                    if (poblacion != null && poblacion > 0)
                        evaluacionEncuesta.N_CO2I = (co2 / poblacion) * 1000;
                }

                if (pm25 != null)
                {
                    evaluacionEncuesta.N_PM25P = pm25;

                    if (poblacion != null && poblacion > 0)
                        evaluacionEncuesta.N_PM25I = (pm25 / poblacion);
                }

                dbSIM.Entry(evaluacionEncuesta).State = EntityState.Added;

                dbSIM.SaveChanges();
            }

            return evaluacionEncuesta;
        }

        public static EVALUACION_ENCUESTA ActualizarDatosGeneralesEvaluacionEncuesta(int idee, EntitiesSIMOracle dbSIM)
        {
            EVALUACION_ENCUESTA evaluacionEncuesta = (from ee in dbSIM.EVALUACION_ENCUESTA
                                                      where ee.ID == idee
                                                      select ee).FirstOrDefault();

            var poblacion = (from ee in dbSIM.EVALUACION_ENCUESTA
                             join vpmes in dbSIM.VWM_PMES on new { ID_TERCERO = ee.EVALUACION_ENCUESTA_TERCERO.ID_TERCERO, ID_INSTALACION = ee.ID_INSTALACION, S_VIGENCIA = ee.EVALUACION_ENCUESTA_TERCERO.S_VALOR_VIGENCIA } equals new { ID_TERCERO = vpmes.ID_TERCERO, ID_INSTALACION = vpmes.ID_INSTALACION, S_VIGENCIA = vpmes.VIGENCIA }
                             where ee.ID == idee
                             select vpmes.N_POBLACION).FirstOrDefault();

            var co2 = (from ee in dbSIM.EVALUACION_ENCUESTA
                       join vpmes in dbSIM.VWM_PMES on new { ID_TERCERO = ee.EVALUACION_ENCUESTA_TERCERO.ID_TERCERO, ID_INSTALACION = ee.ID_INSTALACION, VIGENCIA = ee.EVALUACION_ENCUESTA_TERCERO.S_VALOR_VIGENCIA } equals new { ID_TERCERO = vpmes.ID_TERCERO, ID_INSTALACION = vpmes.ID_INSTALACION, VIGENCIA = vpmes.VIGENCIA }
                       where ee.ID == idee
                       select vpmes.N_CO2P).Sum();

            var pm25 = (from ee in dbSIM.EVALUACION_ENCUESTA
                        join vpmes in dbSIM.VWM_PMES on new { ee.EVALUACION_ENCUESTA_TERCERO.ID_TERCERO, ee.ID_INSTALACION, VIGENCIA = ee.EVALUACION_ENCUESTA_TERCERO.S_VALOR_VIGENCIA } equals new { vpmes.ID_TERCERO, vpmes.ID_INSTALACION, vpmes.VIGENCIA }
                        where ee.ID == idee
                        select vpmes.N_PM25P).Sum();

            var valorVigencia = evaluacionEncuesta.EVALUACION_ENCUESTA_TERCERO.S_VALOR_VIGENCIA;

            OracleParameter idInstalacionParameter = new OracleParameter("idInstalacion", evaluacionEncuesta.ID_INSTALACION);
            OracleParameter valorVigenciaParameter = new OracleParameter("valorVigencia", evaluacionEncuesta.EVALUACION_ENCUESTA_TERCERO.S_VALOR_VIGENCIA);

            datosEncuestaDireccion direccion = dbSIM.Database.SqlQuery<datosEncuestaDireccion>("SELECT sp.S_VALOR AS S_DIRECCION, sp.X_VALOR, sp.Y_VALOR " +
                                                                "FROM CONTROL.ENC_SOLUCION_PREGUNTAS sp INNER JOIN " +
                                                                "  CONTROL.ENC_SOLUCION s ON sp.ID_SOLUCION = s.ID_SOLUCION INNER JOIN " +
                                                                "  CONTROL.VIGENCIA_SOLUCION vs ON s.ID_ESTADO = vs.ID_ESTADO INNER JOIN " +
                                                                "  CONTROL.FRM_GENERICO_ESTADO ge ON s.ID_ESTADO = ge.ID_ESTADO " +
                                                                "WHERE sp.ID_PREGUNTA = 552 AND ge.ID_INSTALACION = :idInstalacion AND vs.VALOR = :valorVigencia", new OracleParameter[] { idInstalacionParameter, valorVigenciaParameter }).FirstOrDefault();

            if (direccion != null)
            {
                if (direccion.X_VALOR != null && direccion.Y_VALOR != null)
                    evaluacionEncuesta.S_COORDENADA = direccion.X_VALOR.ToString().Replace(",", ".") + " | " + direccion.Y_VALOR.ToString().Replace(",", ".");

                evaluacionEncuesta.S_DIRECCION = direccion.S_DIRECCION;
            }

            if (co2 != null)
            {
                evaluacionEncuesta.N_CO2P = co2;

                if (poblacion != null && poblacion > 0)
                    evaluacionEncuesta.N_CO2I = (co2 / poblacion) * 1000;
            }

            if (pm25 != null)
            {
                evaluacionEncuesta.N_PM25P = pm25;

                if (poblacion != null && poblacion > 0)
                    evaluacionEncuesta.N_PM25I = (pm25 / poblacion);
            }

            dbSIM.Entry(evaluacionEncuesta).State = EntityState.Modified;

            dbSIM.SaveChanges();

            return evaluacionEncuesta;
        }

        public static dynamic AlimentarRespuestasIniciales(EVALUACION_ENCUESTA evaluacionEncuesta, EntitiesSIMOracle dbSIM)
        {
            EVALUACION_RESPUESTA evaluacionRespuesta;
            decimal muestraCalculada;

            // Se alimentan las preguntas que se toman de otro lado y que aun no tienen datos
            dynamic datosVigencia = (from ee in dbSIM.EVALUACION_ENCUESTA
                                     join eet in dbSIM.EVALUACION_ENCUESTA_TERCERO on ee.ID_EVALUACION_TERCERO equals eet.ID
                                     where ee.ID == evaluacionEncuesta.ID
                                     select new
                                     {
                                         eet.ID_TERCERO,
                                         ee.ID_INSTALACION,
                                         VALOR_VIGENCIA = eet.S_VALOR_VIGENCIA,
                                         eet.S_VERSION
                                     }).FirstOrDefault();

            if (datosVigencia != null)
            {
                try
                {
                    int offset = (datosVigencia.S_VERSION == null || datosVigencia.S_VERSION == "1" ? 0 : 100);

                    int idTercero = datosVigencia.ID_TERCERO;
                    int idInstalacion = datosVigencia.ID_INSTALACION;
                    string valorVigencia = datosVigencia.VALOR_VIGENCIA;

                    var datosVistaPMES = (from vpmes in dbSIM.VWM_PMES
                                          where vpmes.ID_TERCERO == idTercero && vpmes.ID_INSTALACION == idInstalacion && vpmes.VIGENCIA == valorVigencia
                                          select vpmes).FirstOrDefault();

                    evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                           where er.ID_EVALUACION_ENCUESTA == evaluacionEncuesta.ID && er.ID_PREGUNTA == offset + 6
                                           select er).FirstOrDefault();
                    if (evaluacionRespuesta == null)
                    {
                        evaluacionRespuesta = new EVALUACION_RESPUESTA();
                        evaluacionRespuesta.ID_EVALUACION_ENCUESTA = evaluacionEncuesta.ID;
                        evaluacionRespuesta.ID_PREGUNTA = offset + 6; // Numero de trabajadores de la sede (N)
                        evaluacionRespuesta.N_RESPUESTA = datosVistaPMES.N_POBLACION;
                        evaluacionRespuesta.S_RESPUESTA = null;
                        dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }

                    evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                           where er.ID_EVALUACION_ENCUESTA == evaluacionEncuesta.ID && er.ID_PREGUNTA == offset + 8
                                           select er).FirstOrDefault();
                    if (evaluacionRespuesta == null)
                    {
                        evaluacionRespuesta = new EVALUACION_RESPUESTA();
                        evaluacionRespuesta.ID_EVALUACION_ENCUESTA = evaluacionEncuesta.ID;
                        evaluacionRespuesta.ID_PREGUNTA = offset + 8; // Muestra (n)
                        evaluacionRespuesta.N_RESPUESTA = datosVistaPMES.N_MUESTRA;
                        evaluacionRespuesta.S_RESPUESTA = null;
                        dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }

                    muestraCalculada = Math.Round(Convert.ToDecimal((1.95 * 1.95 * 0.5 * 0.5 / (0.05 * 0.05)) / (1 + (1.95 * 1.95 * 0.5 * 0.5 / (0.05 * 0.05)) / Convert.ToDouble(datosVistaPMES.N_POBLACION))));

                    evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                           where er.ID_EVALUACION_ENCUESTA == evaluacionEncuesta.ID && er.ID_PREGUNTA == offset + 10
                                           select er).FirstOrDefault();
                    if (evaluacionRespuesta == null)
                    {
                        evaluacionRespuesta = new EVALUACION_RESPUESTA();
                        evaluacionRespuesta.ID_EVALUACION_ENCUESTA = evaluacionEncuesta.ID;
                        evaluacionRespuesta.ID_PREGUNTA = offset + 10; // Muestra calculada con un 5% de error y un Z de confiabilidad 1.95
                        evaluacionRespuesta.N_RESPUESTA = muestraCalculada;
                        evaluacionRespuesta.S_RESPUESTA = null;
                        dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }

                    evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                           where er.ID_EVALUACION_ENCUESTA == evaluacionEncuesta.ID && er.ID_PREGUNTA == offset + 11
                                           select er).FirstOrDefault();
                    if (evaluacionRespuesta == null)
                    {
                        evaluacionRespuesta = new EVALUACION_RESPUESTA();
                        evaluacionRespuesta.ID_EVALUACION_ENCUESTA = evaluacionEncuesta.ID;
                        evaluacionRespuesta.ID_PREGUNTA = offset + 11; // Cumplimiento de la muestra minima según poblacion
                        evaluacionRespuesta.N_RESPUESTA = (datosVistaPMES.N_MUESTRA >= muestraCalculada ? 1 : 2); // 1 Cumple, 2 No Cumple
                        evaluacionRespuesta.S_RESPUESTA = null;
                        dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }
                }
                catch { }
            }

            return datosVigencia;
        }

        public static void AlimentarRespuestasInicialesT(EVALUACION_ENCUESTA_TERCERO evaluacionEncuestaTercero, EntitiesSIMOracle dbSIM)
        {
            EVALUACION_RESPUESTA evaluacionRespuesta;

            try
            {
                int idTercero = evaluacionEncuestaTercero.ID_TERCERO;
                string valorVigencia = evaluacionEncuestaTercero.S_VALOR_VIGENCIA;

                /*var datosVistaPMES = (from vpmes in dbSIM.VWM_PMES
                                      where vpmes.ID_TERCERO == idTercero// && vpmes.VIGENCIA == valorVigencia
                                      select new
                                      {
                                          vpmes.
                                      }).ToList();*/
                string sql = "SELECT BASE, ACTUAL, CO2B/1000 AS CO2B, CO2BT/1000000 AS CO2BT, CO2A/1000 AS CO2A, CO2AT/1000000 AS CO2AT, (CO2A/CO2B - 1)*100 AS CO2VAR, PM25B, PM25BT, PM25A, PM25AT, (PM25A/PM25B - 1)*100 AS PM25VAR " +
                            "FROM( " +
                            "   SELECT MAX(v.BASE) AS BASE, " +
                            "        MAX(v.ACTUAL) AS ACTUAL, " +
                            "        SUM(CASE WHEN d.VIGENCIA = v.BASE THEN d.N_CO2P ELSE 0 END) / SUM(CASE WHEN d.VIGENCIA = v.BASE THEN d.N_CANTIDADP ELSE 0 END) AS CO2B, " +
                            "        SUM(CASE WHEN d.VIGENCIA = v.BASE THEN d.N_CO2P ELSE 0 END) AS CO2BT, " +
                            "        SUM(CASE WHEN d.VIGENCIA = v.ACTUAL THEN d.N_CO2P ELSE 0 END) / SUM(CASE WHEN d.VIGENCIA = v.ACTUAL THEN d.N_CANTIDADP ELSE 0 END) AS CO2A, " +
                            "        SUM(CASE WHEN d.VIGENCIA = v.ACTUAL THEN d.N_CO2P ELSE 0 END) AS CO2AT, " +
                            "        SUM(CASE WHEN d.VIGENCIA = v.BASE THEN d.N_PM25P ELSE 0 END) / SUM(CASE WHEN d.VIGENCIA = v.BASE THEN d.N_CANTIDADP ELSE 0 END) AS PM25B, " +
                            "        SUM(CASE WHEN d.VIGENCIA = v.BASE THEN d.N_PM25P ELSE 0 END) AS PM25BT, " +
                            "        SUM(CASE WHEN d.VIGENCIA = v.ACTUAL THEN d.N_PM25P ELSE 0 END) / SUM(CASE WHEN d.VIGENCIA = v.ACTUAL THEN d.N_CANTIDADP ELSE 0 END) AS PM25A, " +
                            "        SUM(CASE WHEN d.VIGENCIA = v.ACTUAL THEN d.N_PM25P ELSE 0 END) AS PM25AT " +
                            "    FROM " +
                            "        (" +
                            "            SELECT MIN(VIGENCIA) AS BASE, '" + valorVigencia + "' AS ACTUAL " +
                            "            FROM CONTROL.VWM_PMES " +
                            "            WHERE ID_TERCERO = " + idTercero.ToString() + " " +
                            "        ) v INNER JOIN " +
                            "        CONTROL.VWM_PMES d ON d.ID_TERCERO = " + idTercero.ToString() + " AND(v.BASE = d.VIGENCIA OR v.ACTUAL = d.VIGENCIA) " +
                            ") T";

                datosSeguimiento seguimiento = dbSIM.Database.SqlQuery<datosSeguimiento>(sql).FirstOrDefault();

                evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                       where er.ID_EVALUACION_ENCUESTA_TERCERO == evaluacionEncuestaTercero.ID && er.ID_PREGUNTA == 1001
                                       select er).FirstOrDefault();

                if (evaluacionRespuesta == null)
                {
                    evaluacionRespuesta = new EVALUACION_RESPUESTA();
                    evaluacionRespuesta.ID_EVALUACION_ENCUESTA_TERCERO = evaluacionEncuestaTercero.ID;
                    evaluacionRespuesta.ID_PREGUNTA = 1001; // Año de la línea base
                    evaluacionRespuesta.N_RESPUESTA = Convert.ToInt32(seguimiento.BASE);
                    evaluacionRespuesta.S_RESPUESTA = null;
                    dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;
                    dbSIM.SaveChanges();
                }

                evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                       where er.ID_EVALUACION_ENCUESTA_TERCERO == evaluacionEncuestaTercero.ID && er.ID_PREGUNTA == 1002
                                       select er).FirstOrDefault();

                if (evaluacionRespuesta == null)
                {
                    evaluacionRespuesta = new EVALUACION_RESPUESTA();
                    evaluacionRespuesta.ID_EVALUACION_ENCUESTA_TERCERO = evaluacionEncuestaTercero.ID;
                    evaluacionRespuesta.ID_PREGUNTA = 1002; // Vigencia del reporte
                    evaluacionRespuesta.N_RESPUESTA = Convert.ToInt32(seguimiento.ACTUAL);
                    evaluacionRespuesta.S_RESPUESTA = null;
                    dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;
                    dbSIM.SaveChanges();
                }

                evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                       where er.ID_EVALUACION_ENCUESTA_TERCERO == evaluacionEncuestaTercero.ID && er.ID_PREGUNTA == 1003
                                       select er).FirstOrDefault();

                if (evaluacionRespuesta == null)
                {
                    evaluacionRespuesta = new EVALUACION_RESPUESTA();
                    evaluacionRespuesta.ID_EVALUACION_ENCUESTA_TERCERO = evaluacionEncuestaTercero.ID;
                    evaluacionRespuesta.ID_PREGUNTA = 1003; // Kg de CO2/ percápita emitidos en la línea base
                    evaluacionRespuesta.N_RESPUESTA = Convert.ToDecimal(seguimiento.CO2B);
                    evaluacionRespuesta.N_RESPUESTA_AUX = Convert.ToDecimal(seguimiento.CO2BT);
                    evaluacionRespuesta.S_RESPUESTA = null;
                    dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;
                    dbSIM.SaveChanges();
                }

                evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                       where er.ID_EVALUACION_ENCUESTA_TERCERO == evaluacionEncuestaTercero.ID && er.ID_PREGUNTA == 1004
                                       select er).FirstOrDefault();

                if (evaluacionRespuesta == null)
                {
                    evaluacionRespuesta = new EVALUACION_RESPUESTA();
                    evaluacionRespuesta.ID_EVALUACION_ENCUESTA_TERCERO = evaluacionEncuestaTercero.ID;
                    evaluacionRespuesta.ID_PREGUNTA = 1004; // Kg de CO2/ percápita emitidos en el periodo de reporte
                    evaluacionRespuesta.N_RESPUESTA = Convert.ToDecimal(seguimiento.CO2A);
                    evaluacionRespuesta.N_RESPUESTA_AUX = Convert.ToDecimal(seguimiento.CO2AT);
                    evaluacionRespuesta.S_RESPUESTA = null;
                    dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;
                    dbSIM.SaveChanges();
                }

                evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                       where er.ID_EVALUACION_ENCUESTA_TERCERO == evaluacionEncuestaTercero.ID && er.ID_PREGUNTA == 1005
                                       select er).FirstOrDefault();

                if (evaluacionRespuesta == null)
                {
                    evaluacionRespuesta = new EVALUACION_RESPUESTA();
                    evaluacionRespuesta.ID_EVALUACION_ENCUESTA_TERCERO = evaluacionEncuestaTercero.ID;
                    evaluacionRespuesta.ID_PREGUNTA = 1005; // % de variación de emisiones de kg de CO2 percápita
                    evaluacionRespuesta.N_RESPUESTA = Convert.ToDecimal(seguimiento.CO2VAR);
                    evaluacionRespuesta.S_RESPUESTA = null;
                    dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;
                    dbSIM.SaveChanges();
                }

                evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                       where er.ID_EVALUACION_ENCUESTA_TERCERO == evaluacionEncuestaTercero.ID && er.ID_PREGUNTA == 1006
                                       select er).FirstOrDefault();

                if (evaluacionRespuesta == null)
                {
                    evaluacionRespuesta = new EVALUACION_RESPUESTA();
                    evaluacionRespuesta.ID_EVALUACION_ENCUESTA_TERCERO = evaluacionEncuestaTercero.ID;
                    evaluacionRespuesta.ID_PREGUNTA = 1006; // Cumplimiento de la meta de reducción del 10% de emisiones de CO2 percápita
                    evaluacionRespuesta.N_RESPUESTA = (seguimiento.CO2VAR <= -10 ? 1 : 2);
                    evaluacionRespuesta.S_RESPUESTA = null;
                    dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;
                    dbSIM.SaveChanges();
                }

                evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                       where er.ID_EVALUACION_ENCUESTA_TERCERO == evaluacionEncuestaTercero.ID && er.ID_PREGUNTA == 1007
                                       select er).FirstOrDefault();

                if (evaluacionRespuesta == null)
                {
                    evaluacionRespuesta = new EVALUACION_RESPUESTA();
                    evaluacionRespuesta.ID_EVALUACION_ENCUESTA_TERCERO = evaluacionEncuestaTercero.ID;
                    evaluacionRespuesta.ID_PREGUNTA = 1007; // Kg de PM2.5/ percápita emitidos en la línea base
                    evaluacionRespuesta.N_RESPUESTA = Convert.ToDecimal(seguimiento.PM25B);
                    evaluacionRespuesta.N_RESPUESTA_AUX = Convert.ToDecimal(seguimiento.PM25BT);
                    evaluacionRespuesta.S_RESPUESTA = null;
                    dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;
                    dbSIM.SaveChanges();
                }

                evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                       where er.ID_EVALUACION_ENCUESTA_TERCERO == evaluacionEncuestaTercero.ID && er.ID_PREGUNTA == 1008
                                       select er).FirstOrDefault();

                if (evaluacionRespuesta == null)
                {
                    evaluacionRespuesta = new EVALUACION_RESPUESTA();
                    evaluacionRespuesta.ID_EVALUACION_ENCUESTA_TERCERO = evaluacionEncuestaTercero.ID;
                    evaluacionRespuesta.ID_PREGUNTA = 1008; // Kg de PM2.5/ percápita emitidos en el periodo de reporte
                    evaluacionRespuesta.N_RESPUESTA = Convert.ToDecimal(seguimiento.PM25A);
                    evaluacionRespuesta.N_RESPUESTA_AUX = Convert.ToDecimal(seguimiento.PM25AT);
                    evaluacionRespuesta.S_RESPUESTA = null;
                    dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;
                    dbSIM.SaveChanges();
                }

                evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                       where er.ID_EVALUACION_ENCUESTA_TERCERO == evaluacionEncuestaTercero.ID && er.ID_PREGUNTA == 1009
                                       select er).FirstOrDefault();

                if (evaluacionRespuesta == null)
                {
                    evaluacionRespuesta = new EVALUACION_RESPUESTA();
                    evaluacionRespuesta.ID_EVALUACION_ENCUESTA_TERCERO = evaluacionEncuestaTercero.ID;
                    evaluacionRespuesta.ID_PREGUNTA = 1009; // % de variación de emisiones de kg de PM2.5 percápita
                    evaluacionRespuesta.N_RESPUESTA = Convert.ToDecimal(seguimiento.PM25VAR);
                    evaluacionRespuesta.S_RESPUESTA = null;
                    dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;
                    dbSIM.SaveChanges();
                }

                evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                       where er.ID_EVALUACION_ENCUESTA_TERCERO == evaluacionEncuestaTercero.ID && er.ID_PREGUNTA == 1010
                                       select er).FirstOrDefault();

                if (evaluacionRespuesta == null)
                {
                    evaluacionRespuesta = new EVALUACION_RESPUESTA();
                    evaluacionRespuesta.ID_EVALUACION_ENCUESTA_TERCERO = evaluacionEncuestaTercero.ID;
                    evaluacionRespuesta.ID_PREGUNTA = 1010; // Cumplimiento de la meta de reducción del 10% de emisiones de PM2.5 percápita
                    evaluacionRespuesta.N_RESPUESTA = (seguimiento.PM25VAR <= 10 ? 1 : 2);
                    evaluacionRespuesta.S_RESPUESTA = null;
                    dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e) {
                string error = e.Message;
            }
        }
    }
}