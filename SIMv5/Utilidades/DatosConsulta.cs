using System.Collections.Generic;

namespace SIM.Utilidades
{
    /// <summary>
    /// 
    /// </summary>
    public struct datosConsulta
    {
        /// <summary>
        /// Número de Registros
        /// </summary>
        public long numRegistros;

        /// <summary>
        /// Datos devueltos
        /// </summary>
        public IEnumerable<dynamic> datos;
    }
}