using SIM.Areas.Seguridad.Models;
using SIM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Seguridad.Class
{
    public  class Permisos
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

       
        /// <summary>
        /// Retorna la unión de los permisos que poseen todos los roles vinculados a una opción del menú (Forma)
        /// </summary>
        /// <param name="FormaId">Identifica la Opción dle Menú</param>
        /// <param name="UsuarioId">Identifica el usuario</param>
        /// <returns></returns>
        public PermisosRolModel ObtenerPermisosRolForma( int FormaId, int UsuarioId)
        {

            PermisosRolModel permisosRolModel = new PermisosRolModel
            {
                IdRol = 0,
                CanRead = false,
                CanInsert = false,
                CanUpdate = false,
                CanDelete = false,
                CanPrint = false,
            };

            var rolesForma = dbSIM.ROL_FORMA.Where(f => f.ID_FORMA == FormaId).ToList();
            if (rolesForma != null && rolesForma.Count > 0 ) 
            {
               foreach(var role in rolesForma)
                {
                    var usuarioRol = dbSIM.USUARIO_ROL.Where(f => f.ID_USUARIO == UsuarioId && f.ID_ROL== role.ID_ROL).FirstOrDefault();
                    if (usuarioRol != null)
                    {
                        var permisosRolForma = dbSIM.ROL_FORMA.Where(f => f.ID_ROL == role.ID_ROL && f.ID_FORMA == FormaId).FirstOrDefault();
                        if (permisosRolForma != null)
                        {
                            if (permisosRolForma.S_BUSCAR == "1") permisosRolModel.CanRead = true;
                            if (permisosRolForma.S_NUEVO == "1") permisosRolModel.CanInsert = true;
                            if (permisosRolForma.S_EDITAR == "1") permisosRolModel.CanUpdate = true;
                            if (permisosRolForma.S_ELIMINAR == "1") permisosRolModel.CanDelete = true;
                            if (permisosRolForma.S_BUSCAR == "1") permisosRolModel.CanPrint = true;
                        }
                    }
                }
            }
            return permisosRolModel;
        }

    }
}