namespace SIM.Areas.Seguridad.Models
{
    using Newtonsoft.Json;
    using SIM.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;

    public class OPCIONMENU
    {
        [JsonIgnore]
        public int ID;
        [JsonIgnore]
        public int? ID_PADRE;
        public string NOMBRE;
        public string URL;
        public int ORDEN;
        public List<OPCIONMENU> MENU;
    }

    public class MENULISTA
    {
        public int ID_FORMA { get; set; }
        public string S_NOMBRE { get; set; }
        public string S_VISIBLE_MENU { get; set; }
        public string S_PADRE { get; set; }
    }

    public class ROLESUSUARIO
    {
        public bool SEL { get; set; }
        public int ID_ROL { get; set; }
        public string S_NOMBRE { get; set; }
    }

    /// <summary>
    /// Extension de las entidades del modelo seguridad, para retornar inforamcion customizada
    /// </summary>
    public partial class ModelsToListSeguridad
    {
        public static IEnumerable GetGrupo()
        {
            EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();
            return dbSeguridad.GRUPO.OrderBy(td => td.S_NOMBRE).ToList();
        }

        public static IEnumerable GetUsuario()
        {
            EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();
            return dbSeguridad.USUARIO.OrderBy(td => (td.S_NOMBRES + td.S_APELLIDOS)).ToList();
        }

        public static IEnumerable GetRol()
        {
            EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();
            return dbSeguridad.ROL.OrderBy(td => td.S_NOMBRE).ToList();
        }

        public static IEnumerable GetDependencia()
        {
            EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();
            return dbSeguridad.DEPENDENCIA.OrderBy(td => td.S_NOMBRE).ToList();
        }

        public static IEnumerable GetCargo()
        {
            EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();
            return dbSeguridad.CARGO.OrderBy(td => td.S_NOMBRE).ToList();
        }

        public static IEnumerable GetTipoObjeto()
        {
            EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();
            return dbSeguridad.TIPO_OBJETO.OrderBy(td => td.S_TIPOOBJETO).ToList();
        }

        public static IEnumerable GetForma()
        {
            List<MENULISTA> listaMenu;
            EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();
            /* return from m in dbSeguridad.MENU.AsEnumerable()
                    orderby m.S_NOMBRE
                    select new
                    {
                        m.ID_FORMA,
                        S_NOMBRE = m.S_NOMBRE + " (" + m.ID_FORMA.ToString() + ")"
                    };
             */
            var model = from m1 in dbSeguridad.MENU.Where(m => m.S_CONTROLADOR != null).AsEnumerable()
                        join m2 in dbSeguridad.MENU.Where(m => m.S_CONTROLADOR != null).AsEnumerable() on m1.ID_PADRE equals m2.ID_FORMA into menu
                        from m2 in menu.DefaultIfEmpty()
                        orderby m1.S_NOMBRE
                        select new MENULISTA()
                        {
                            ID_FORMA = m1.ID_FORMA,
                            S_NOMBRE = m1.S_NOMBRE + " (" + m1.ID_FORMA.ToString() + ")",
                            S_VISIBLE_MENU = m1.S_VISIBLE_MENU,
                            S_PADRE = m2 == null ? "(Raiz)" : m2.S_NOMBRE + " (" + m2.ID_FORMA.ToString() + ")"
                        };

            listaMenu = model.ToList();
            listaMenu.Insert(0, new MENULISTA() { ID_FORMA = 0, S_NOMBRE = "(Raiz)", S_VISIBLE_MENU = "0", S_PADRE = "" });
            return listaMenu;
        }

        public static IEnumerable GetRolForma()
        {
            EntitiesSIMOracle dbSeguridad = new EntitiesSIMOracle();
            var qyrf = from m in dbSeguridad.MENU.AsEnumerable()
                       join rf in dbSeguridad.ROL_FORMA on m.ID_FORMA equals rf.ID_FORMA into tb
                       from mrf in tb.DefaultIfEmpty()
                       select new
                       {
                           ID_FORMA = m.ID_FORMA,
                           ID_PADRE = m.ID_PADRE,
                           S_NOMBRE = m.S_NOMBRE + " (" + m.ID_FORMA.ToString() + ")",
                           m.ORDEN,
                           m.S_VISIBLE_MENU,
                           ID_ROL = (mrf == null ? -1 : mrf.ID_ROL),
                           S_ROL = (mrf == null ? "" : mrf.ROL.S_NOMBRE),
                           S_BUSCAR = (mrf == null ? "0" : mrf.S_BUSCAR),
                           S_NUEVO = (mrf == null ? "0" : mrf.S_NUEVO),
                           S_EDITAR = (mrf == null ? "0" : mrf.S_EDITAR),
                           S_ELIMINAR = (mrf == null ? "0" : mrf.S_ELIMINAR),
                           S_ADMINISTRADOR = (mrf == null ? "0" : mrf.S_ADMINISTRADOR)
                       };

            return qyrf.ToList();
        }

        public static List<OPCIONMENU> GetOpcionesMenu()
        {
            OPCIONMENU menuPpal = new OPCIONMENU();
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            //Se obitnen todos los idRoles que el usuario tiene asignado
            List<int> idRoles = new List<int>();
            ClaimsPrincipal claimsPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            foreach (Claim claim in claimsPrincipal.FindAll(CustomClaimTypes.IdRol))
                idRoles.Add(int.Parse(claim.Value));

            //variable itemMenu con todos los items del menu que son link y que el usuario tiene permiso
            var itemMenu = (from m in dbSIM.MENU
                            join rf in dbSIM.ROL_FORMA on m.ID_FORMA equals rf.ID_FORMA
                            join r in idRoles on rf.ID_ROL equals r
                            where m.S_VISIBLE_MENU == "1" && string.IsNullOrEmpty(m.S_RUTA) == false &&
                                  (rf.S_BUSCAR == "1" || rf.S_EDITAR == "1" || rf.S_ELIMINAR == "1" || rf.S_NUEVO == "1" || rf.S_ADMINISTRADOR == "1")
                            select new OPCIONMENU
                            {
                                ID = m.ID_FORMA,
                                ID_PADRE = m.ID_PADRE,
                                NOMBRE = m.S_NOMBRE,
                                URL = m.S_RUTA,
                                ORDEN = m.ORDEN
                            }).Distinct().OrderBy(o => o.ORDEN).ToList();

            //variable agrMenu con todos los items del menu que son agrupadores
            var agrMenu = (from m in dbSIM.MENU
                           where !string.IsNullOrEmpty(m.S_CONTROLADOR) && string.IsNullOrEmpty(m.S_RUTA) && m.S_VISIBLE_MENU == "1"
                           select new OPCIONMENU
                           {
                               ID = m.ID_FORMA,
                               ID_PADRE = m.ID_PADRE,
                               NOMBRE = m.S_NOMBRE,
                               URL = (m.S_RUTA == null ? "" : m.S_RUTA),
                               ORDEN = m.ORDEN
                           }).Distinct().OrderBy(o => o.ORDEN).ToList();

            List<int> rmv = new List<int>();
            for (int j = agrMenu.Count - 1; j >= 0; j--)
            {
                if (!TieneSubItems(agrMenu[j].ID, itemMenu, agrMenu))
                    rmv.Add(j);
            }

            foreach (int i in rmv)
                agrMenu.RemoveAt(i);


            OPCIONMENU root = new OPCIONMENU();
            root.ID = 0;
            root.ID_PADRE = null;
            root.NOMBRE = "Root";
            root.URL = null;
            root.ORDEN = 0;
            agrMenu.Add(root);

            agrMenu.AddRange(itemMenu);

            return BuildTreeAndReturnRootNodes(agrMenu);
        }

        public static string ObtenerNombreUsuario(string uId)
        {
            int userId = Convert.ToInt32(uId);
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            var usuario = from usuarios in dbSIM.USUARIO
                          where usuarios.ID_USUARIO == userId
                          select usuarios.S_NOMBRES;

            if (usuario.Count() > 0)
                return usuario.First().Split(' ')[0];
            else
                return "";
        }

        public static string ObtenerNombresUsuario(string uId)
        {
            int userId = Convert.ToInt32(uId);
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            var usuario = from usuarios in dbSIM.USUARIO
                          where usuarios.ID_USUARIO == userId
                          select usuarios.S_NOMBRES;

            if (usuario.Count() > 0)
                return usuario.First();
            else
                return "";
        }

        public static string ObtenerApellidosUsuario(string uId)
        {
            int userId = Convert.ToInt32(uId);
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            var usuario = from usuarios in dbSIM.USUARIO
                          where usuarios.ID_USUARIO == userId
                          select usuarios.S_APELLIDOS;

            if (usuario.Count() > 0)
                return usuario.First();
            else
                return "";
        }

        /// <summary>
        /// Determina si la opcion del menu de tipo root tiene hijos dentro de los datos 
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_paginas"></param>
        /// <param name="_agrupadores"></param>
        /// <returns></returns>
        private static bool TieneSubItems(int _item, List<OPCIONMENU> _paginas, List<OPCIONMENU> _agrupadores)
        {
            if (_paginas.Where(td => td.ID_PADRE == _item).Count() > 0)
                return true;
            else
            {
                var _hijos = _agrupadores.Where(td => td.ID_PADRE == _item);
                foreach (var _hijo in _hijos)
                {
                    if (TieneSubItems(_hijo.ID, _paginas, _agrupadores))
                        return true;
                }
                return false;
            }
        }

        private static List<OPCIONMENU> BuildTreeAndReturnRootNodes(List<OPCIONMENU> flatItems)
        {
            var request = HttpContext.Current.Request;
            string baseURL;

            List<OPCIONMENU> menuRaiz;
            List<OPCIONMENU> resultado;

            baseURL = Utilidades.Data.GetBaseUrl();

            var byIdLookup = flatItems.ToLookup(i => i.ID);
            foreach (var item in flatItems)
            {
                if (item.MENU == null)
                {
                    item.MENU = new List<OPCIONMENU>();
                    item.URL = (item.URL == null ? "" : item.URL.Replace("~", baseURL));
                }

                if (item.ID_PADRE != null)
                {
                    try
                    {
                        var parent = byIdLookup[(int)item.ID_PADRE].First();

                        if (parent.MENU == null)
                            parent.MENU = new List<OPCIONMENU>();
                        parent.MENU.Add(item);
                    }
                    catch (Exception e)
                    {
                        string a = e.Message;
                    }
                }
            }

            menuRaiz = (List<OPCIONMENU>)flatItems.Where(i => i.ID_PADRE == null).ToList();

            resultado = new List<OPCIONMENU>();
            resultado.AddRange(menuRaiz[0].MENU);

            return resultado;
        }
    }
}