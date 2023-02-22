using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using SIM.Data;
using SIM.Models;

namespace SIM.Areas.ControlVigilancia.Models
{
    
    public class clsGenerales
    {

        public static decimal Obtener_Codigo_Funcionario(EntitiesControlOracle bd, Int32 idUsuario)
        {
        //public static decimal Obtener_Codigo_Funcionario(SIM.Areas.ControlVigilancia.Models.EntitiesCont bd, Int32 idUsuario)
        //{
            ObjectParameter OutCodFuncionario = new ObjectParameter("codFuncionario", typeof(decimal));
            bd.SP_OBTENER_CODFUNCIONARIO(idUsuario, OutCodFuncionario);
            return decimal.Parse(OutCodFuncionario.Value.ToString());
        }
        public static decimal Procedure_GuardarVisita(EntitiesSIMOracle bd, String p_Asunto, decimal p_cx, decimal p_cy, String p_TipoUbicacion, decimal codFuncionario, int p_IdVisita, int idTercero, int idInstalacion, String atiende, String observacion)
        {

            ObjectParameter respuestaVisita = new ObjectParameter("respuesta", typeof(decimal));
           // bd.SP_GUARDAR_VISITA(p_Asunto, p_cx, p_cy, p_TipoUbicacion, codFuncionario, p_IdVisita, idTercero, idInstalacion, atiende, observacion, respuestaVisita);
           // bd.SP_GUARDAR_VISITA("alejo", 1, 2, "ffff", 4570, 34, 4570, 4661, "0", "hhh", respuestaVisita);
            return decimal.Parse(respuestaVisita.Value.ToString());
        }
    }
}