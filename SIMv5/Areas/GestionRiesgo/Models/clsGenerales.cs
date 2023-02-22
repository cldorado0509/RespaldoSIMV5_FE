namespace SIM.Areas.GestionRiesgo.Models
{
    using System;
    using System.Data.Entity.Core.Objects;
    using SIM.Data;
    using SIM.Models;

    public class clsGenerales
    {

        public static decimal Obtener_Codigo_Funcionario(EntitiesControlOracle bd, Int32 idUsuario)
        {
            ObjectParameter OutCodFuncionario = new ObjectParameter("codFuncionario", typeof(decimal));
            bd.SP_OBTENER_CODFUNCIONARIO(idUsuario, OutCodFuncionario);
            return decimal.Parse(OutCodFuncionario.Value.ToString());
        }
        public static decimal Procedure_GuardarVisita(EntitiesSIMOracle bd, String p_Asunto, decimal p_cx, decimal p_cy, String p_TipoUbicacion, decimal codFuncionario, int p_IdVisita, int idTercero, int idInstalacion, String atiende, String observacion)
        {

            ObjectParameter respuestaVisita = new ObjectParameter("respuesta", typeof(decimal));
            return decimal.Parse(respuestaVisita.Value.ToString());
        }
    }
}