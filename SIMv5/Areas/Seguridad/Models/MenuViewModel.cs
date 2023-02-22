using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Security.Claims;
using DevExpress.Web;
using SIM.Data;
using SIM.Data.Seguridad;

namespace SIM.Areas.Seguridad.Models
{
    /// <summary>
    /// Modelo del Menu de la aplicacion
    /// </summary>
    public class MenuViewModel
    {
        private EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// Metodo que devuelve el datasource del SiteMap con el menu para el usuario logeado
        /// </summary>
        public SiteMapDataSource ObtieneMenu()
        {
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
                            select m).Distinct().ToList();

            //variable agrMenu con todos los items del menu que son agrupadores
            var agrMenu = dbSIM.MENU.Where(td => string.IsNullOrEmpty(td.S_RUTA) && td.S_VISIBLE_MENU == "1").ToList();

            List<int> rmv = new List<int>();
            for (int j = agrMenu.Count-1; j >= 0; j--)
            {
                //agrMenu[j].S_RUTA = "javascript:void('" + agrMenu[j].ID_FORMA.ToString() + "')";
                if (!TieneSubItems(agrMenu[j].ID_FORMA, itemMenu, agrMenu))
                    rmv.Add(j);
            }






            foreach (int i in rmv)
                agrMenu.RemoveAt(i);

            MENU _Root = new MENU();
            _Root.ID_FORMA = 0;
            _Root.ID_PADRE = 1;
            _Root.S_NOMBRE = "Root";
            _Root.S_RUTA = "javascrip:void('1')";
            agrMenu.Add(_Root);

            agrMenu.AddRange(itemMenu);

            return GenerateSiteMapHierarchy(agrMenu);
        }
        /// <summary>
        /// Determina si la opcion del menu de tipo root tiene hijos dentro de los datos 
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_paginas"></param>
        /// <param name="_agrupadores"></param>
        /// <returns></returns>
        private bool TieneSubItems(int _item, List<MENU> _paginas, List<MENU> _agrupadores)
        {
            if (_paginas.Where(td => td.ID_PADRE == _item).Count() > 0)
                return true;
            else
            {
                var _hijos = _agrupadores.Where(td => td.ID_PADRE == _item);
                foreach (var _hijo in _hijos)
                {
                    if (TieneSubItems(_hijo.ID_FORMA, _paginas, _agrupadores))
                        return true;
                }
                return false;
            }
        }

        protected SiteMapDataSource GenerateSiteMapHierarchy(List<MENU> dataSource)
        {
            SiteMapDataSource ret = new SiteMapDataSource();

            MENU rowRootNode = dataSource.Where(td => td.ID_PADRE == 1).First();
            UnboundSiteMapProvider provider = new UnboundSiteMapProvider(rowRootNode.S_RUTA, rowRootNode.S_NOMBRE);
            AddNodeToProviderRecursive(provider.RootNode, rowRootNode.ID_FORMA, dataSource, provider);
            ret.Provider = provider;
            ret.ShowStartingNode = false;
            return ret;
        }

        private void AddNodeToProviderRecursive(SiteMapNode parentNode, int parentID, List<MENU> table, UnboundSiteMapProvider provider)
        {
            var childRows = table.Where(td => td.ID_PADRE == parentID).OrderBy(td => td.ORDEN).Distinct();
            foreach (MENU row in childRows)
            {
                SiteMapNode childNode = CreateSiteMapNode(row.S_RUTA, row.S_NOMBRE, provider);
                provider.AddSiteMapNode(childNode, parentNode);
                AddNodeToProviderRecursive(childNode, row.ID_FORMA, table, provider);
            }
        }

        private SiteMapNode CreateSiteMapNode(string url, string text, UnboundSiteMapProvider provider)
        {
            return provider.CreateNode(url, text, "", null, new NameValueCollection());
        }
    }
}