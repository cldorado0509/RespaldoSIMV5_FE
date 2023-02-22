using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardDesignerClass
{
    public class Parametro
    {
        public string Nombre { set; get; }
        public object Valor { set; get; }
    }

    public class Consultas
    {
        public static DataTable ObtenerDatosFuente(string nombre, string connectionString, List<Parametro> parametros)
        {
            var consultaDataTable = new DataTable();
            var consultaCommand = new OracleCommand();
            DataTable resultado = null;
            var oracleConnection = new OracleConnection(connectionString);

            try
            {
                switch (nombre)
                {
                    case "SPMES": //
                        var psDataTable = new DataTable();
                        var psCommand = new OracleCommand();
                        psCommand.Connection = oracleConnection;
                        psCommand.CommandType = CommandType.Text;

                        //psCommand.CommandText = "SELECT CASE WHEN VIGENCIA = '" + parametros[parametros.Count-2].Valor + "' THEN 'A' ELSE 'B' END AS TIPO_VIGENCIA, vp.* FROM CONTROL.VWM_PMES vp WHERE (ID_TERCERO = :idTercero) AND (VIGENCIA = :vigenciaEncuesta OR VIGENCIA = :vigenciaEncuestaBase)";
                        psCommand.CommandText = "SELECT CASE WHEN VIGENCIA = '" + parametros[parametros.Count - 2].Valor + "' THEN 'A' ELSE 'B' END AS TIPO_VIGENCIA, vp.*, mep.S_VALOR AS MODOPH, mel.S_VALOR AS MODOLH FROM CONTROL.VWM_PMES vp LEFT OUTER JOIN CONTROL.PMES_MODOS_EQUIV mep ON vp.ID_ENCUESTA = mep.ID_ENCUESTA AND vp.ID_MODOP = mep.ID_MODO LEFT OUTER JOIN CONTROL.PMES_MODOS_EQUIV mel ON vp.ID_ENCUESTA = mel.ID_ENCUESTA AND vp.ID_MODOL = mel.ID_MODO WHERE(ID_TERCERO = :idTercero) AND (VIGENCIA = :vigenciaEncuesta OR VIGENCIA = :vigenciaEncuestaBase)";

                        consultaCommand = psCommand;
                        break;
                    case "SPMES_DIAS_TELETRABAJO":
                        var sdttDataTable = new DataTable();
                        var sdttCommand = new OracleCommand();
                        sdttCommand.Connection = oracleConnection;
                        sdttCommand.CommandType = CommandType.Text;
                        sdttCommand.CommandText = "SELECT ID_TERCERO, ID_INSTALACION, VIGENCIA, ID_ESTADO, CASE DIA WHEN 'TT_LUNES' THEN '1. LUNES' WHEN 'TT_MARTES' THEN '2. MARTES' WHEN 'TT_MIERCOLES' THEN '3. MIÉRCOLES' WHEN 'TT_JUEVES' THEN '4. JUEVES' WHEN 'TT_VIERNES' THEN '5. VIERNES' WHEN 'TT_SABADO' THEN '6. SÁBADO' WHEN 'TT_DOMINGO' THEN '7. DOMINGO' END AS DIA, DIAS_TELETRABAJO, N_FACTORMUESTRA, ID_ENCUESTA " +
                                                        "FROM CONTROL.VWM_PMES " +
                                                        "UNPIVOT " +
                                                        "( " +
                                                        "  DIAS_TELETRABAJO " +
                                                        "    FOR DIA IN (TT_LUNES, TT_MARTES, TT_MIERCOLES, TT_JUEVES, TT_VIERNES, TT_SABADO, TT_DOMINGO) " +
                                                        ") " +
                                                        "WHERE (ID_TERCERO = :idTercero) AND (VIGENCIA = :vigenciaEncuesta1 OR VIGENCIA = :vigenciaEncuesta2)";

                        consultaCommand = sdttCommand;
                        break;
                    case "PMES": //
                        var pmesDataTable = new DataTable();
                        var pmesCommand = new OracleCommand();
                        pmesCommand.Connection = oracleConnection;
                        pmesCommand.CommandType = CommandType.Text;
                        pmesCommand.CommandText = "SELECT * FROM CONTROL.VWM_PMES WHERE (ID_TERCERO = :idTercero) AND (VIGENCIA = :vigenciaEncuesta) AND (NVL(ID_ENCUESTA, 1) = :idEncuesta)";

                        consultaCommand = pmesCommand;
                        break;
                    case "PMES_RAZONES_TELETRABAJO":
                        var rttDataTable = new DataTable();
                        var rttCommand = new OracleCommand();
                        rttCommand.Connection = oracleConnection;
                        rttCommand.CommandType = CommandType.Text;
                        rttCommand.CommandText = "SELECT ID_TERCERO, TERCERO, ID_INSTALACION, INSTALACION, VIGENCIA, ID_ESTADO, CASE RAZON WHEN 'N_IND01' THEN 'COSTO' WHEN 'N_IND02' THEN 'CONVENIENCIA' WHEN 'N_IND03' THEN 'SEGURIDAD PERSONAL' WHEN 'N_IND04' THEN 'MENOR RIESGO DE ACCIDENTE' WHEN 'N_IND05' THEN 'TIEMPO' WHEN 'N_IND06' THEN 'ECONOMÍA' WHEN 'N_IND07' THEN 'AMBIENTALES' WHEN 'N_IND08' THEN 'NO HAY ACCESO AL TRANSPORTE PÚBLICO' WHEN 'N_IND09' THEN 'PROBLEMAS DE SALUD(POR EJEMPLO, DISCAPACIDAD)' WHEN 'N_IND10' THEN 'COMPROMISOS TALES COMO DEJAR / RECOGER' WHEN 'N_IND11' THEN 'OTRO' END AS RAZON, RAZONES_TELETRABAJO, ID_ENCUESTA " +
                                                        "FROM CONTROL.VWM_PMES " +
                                                        "UNPIVOT " +
                                                        "( " +
                                                        "  RAZONES_TELETRABAJO " +
                                                        "    FOR RAZON IN (N_IND01, N_IND02, N_IND03, N_IND04, N_IND05, N_IND06, N_IND07, N_IND08, N_IND09, N_IND10, N_IND11) " +
                                                        ") " +
                                                        "WHERE (ID_TERCERO = :idTercero) AND (VIGENCIA = :vigenciaEncuesta) AND (NVL(ID_ENCUESTA, 1) = :idEncuesta)";

                        consultaCommand = rttCommand;
                        break;
                    case "PMES_DIAS_TELETRABAJO":
                        var dttDataTable = new DataTable();
                        var dttCommand = new OracleCommand();
                        dttCommand.Connection = oracleConnection;
                        dttCommand.CommandType = CommandType.Text;
                        dttCommand.CommandText = "SELECT ID_TERCERO, TERCERO, ID_INSTALACION, INSTALACION, VIGENCIA, ID_ESTADO, CASE DIA WHEN 'TT_LUNES' THEN '1. LUNES' WHEN 'TT_MARTES' THEN '2. MARTES' WHEN 'TT_MIERCOLES' THEN '3. MIÉRCOLES' WHEN 'TT_JUEVES' THEN '4. JUEVES' WHEN 'TT_VIERNES' THEN '5. VIERNES' WHEN 'TT_SABADO' THEN '6. SÁBADO' WHEN 'TT_DOMINGO' THEN '7. DOMINGO' END AS DIA, DIAS_TELETRABAJO, N_FACTORMUESTRA, ID_ENCUESTA " +
                                                        "FROM CONTROL.VWM_PMES " +
                                                        "UNPIVOT " +
                                                        "( " +
                                                        "  DIAS_TELETRABAJO " +
                                                        "    FOR DIA IN (TT_LUNES, TT_MARTES, TT_MIERCOLES, TT_JUEVES, TT_VIERNES, TT_SABADO, TT_DOMINGO) " +
                                                        ") " +
                                                        "WHERE (ID_TERCERO = :idTercero) AND (VIGENCIA = :vigenciaEncuesta) AND (NVL(ID_ENCUESTA, 1) = :idEncuesta)";

                        consultaCommand = dttCommand;
                        break;
                    case "PMES_RAZONES_ELECCION_MODO":
                        var remDataTable = new DataTable();
                        var remCommand = new OracleCommand();
                        remCommand.Connection = oracleConnection;
                        remCommand.CommandType = CommandType.Text;
                        remCommand.CommandText = "SELECT ID_TERCERO, TERCERO, ID_INSTALACION, INSTALACION, VIGENCIA, ID_ESTADO, ID_MODOP, MODOP, CASE RAZON WHEN 'N_IND15' THEN 'COMODIDAD' WHEN 'N_IND16' THEN 'RAPIDEZ' WHEN 'N_IND17' THEN 'COSTO' WHEN 'N_IND18' THEN 'CONVENIENCIA' WHEN 'N_IND19' THEN 'SEGURIDAD PERSONAL' WHEN 'N_IND20' THEN 'MENOR RIESGO DE ACCIDENTE' WHEN 'N_IND21' THEN 'NO HAY ACCESO AL TRANSPORTE PÚBLICO' WHEN 'N_IND22' THEN 'SALUD' WHEN 'N_IND23' THEN 'PROBLEMAS DE SALUD (POR EJEMPLO, DISCAPACIDAD)' WHEN 'N_IND24' THEN 'COMPROMISOS TALES COMO DEJAR / RECOGER' WHEN 'N_IND25' THEN 'RAZONES AMBIENTALES' WHEN 'N_IND26' THEN 'OTRO' END AS RAZON, RAZONES_ELECCION_MODO, ID_ENCUESTA " +
                                                        "FROM CONTROL.VWM_PMES " +
                                                        "UNPIVOT " +
                                                        "( " +
                                                        "  RAZONES_ELECCION_MODO " +
                                                        "    FOR RAZON IN (N_IND15, N_IND16, N_IND17, N_IND18, N_IND19, N_IND20, N_IND21, N_IND22, N_IND23, N_IND24, N_IND25, N_IND26) " +
                                                        ") " +
                                                        "WHERE (ID_TERCERO = :idTercero) AND (VIGENCIA = :vigenciaEncuesta) AND (NVL(ID_ENCUESTA, 1) = :idEncuesta)";

                        consultaCommand = remCommand;
                        break;
                }

                foreach (Parametro parametro in parametros)
                {
                    consultaCommand.Parameters.Add(new OracleParameter(parametro.Nombre, parametro.Valor));
                }

                oracleConnection.Open();

                (new OracleCommand("ALTER SESSION SET NLS_NUMERIC_CHARACTERS = '.,'", oracleConnection)).ExecuteNonQuery();

                var consultaAdapter = new OracleDataAdapter(consultaCommand);
                consultaAdapter.Fill(consultaDataTable);
                resultado = consultaDataTable;
            }
            catch (Exception error)
            {
                resultado = null;
            }
            finally
            {
                oracleConnection.Close();
            }

            return resultado;
        }
    }
}
