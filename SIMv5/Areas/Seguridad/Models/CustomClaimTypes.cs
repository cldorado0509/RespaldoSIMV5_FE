using System;
using System.Runtime.InteropServices;

namespace SIM.Areas.Seguridad.Models
{
    /// <summary>
    /// Tipos de claims customizados
    /// </summary>
    [ComVisible(false)]
    public static class CustomClaimTypes
    {
        public const string IdRol = "http://metropol.gov.co/identity/customclaims/idrol";
        public const string IdTercero = "http://metropol.gov.co/identity/customclaims/idtercero";
    }
}