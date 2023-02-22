namespace SIM.Areas.GestionDocumental.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using SIM.Data;

    /*public struct TIPO_ANEXO
    {
        public int ID_TIPO;
        public string S_NOMBRE;
    }*/

    public class ModelsToListGestionDocumental
    {
        public static IEnumerable GetTiposAnexo()
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            var tiposAnexo = (from tipoAnexo in dbSIM.TIPO_ANEXO
                             select new
                             {
                                 ID_TIPO = tipoAnexo.ID_TIPOANEXO,
                                 tipoAnexo.S_NOMBRE
                             }).ToList();

            return tiposAnexo;
        }
    }
}
