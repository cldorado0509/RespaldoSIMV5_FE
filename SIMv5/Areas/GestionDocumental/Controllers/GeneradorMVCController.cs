using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SIM.Areas.GestionDocumental.Models;
using DevExpress.Web.Mvc;
using System.Xml.Linq;
using SIM.Data;

namespace SIM.Areas.GestionDocumental.Controllers
{
    public class GeneradorMVCController : Controller
    {
        /*
        struct DatosCampos
        {
            public string ValorOriginal;
            public string Formato;
            public string ValorFinal;
        }

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        [Authorize(Roles = "VGENERADOR")]
        public ActionResult Index()
        {
            var qryTiposGenerador = from prestamotipo in dbSIM.PRESTAMO_TIPO
                                    where prestamotipo.S_CONFIGURACION != null
                                    orderby prestamotipo.ID_TIPOPRESTAMO
                                    select prestamotipo;

            return View(qryTiposGenerador.ToList());
        }

        public ActionResult ObtenerConfiguracionTipo(int idTipo)
        {
            CONFIGURACION lcobjConfiguracionTipo;
            string lcstrConfiguracion;

            //lcobjTiposGenerador = db.Database.SqlQuery<PRESTAMO_TIPO>("SELECT intIdTipo AS Tipo, strNombre AS Nombre, strConfiguracion AS Configuracion FROM TIPOS WHERE intIdTipo = " + idTipo.ToString(), new object[] { }).FirstOrDefault();
            var lcobjTiposGenerador = (from prestamoTipo in dbSIM.PRESTAMO_TIPO
                                      where prestamoTipo.ID_TIPOPRESTAMO == idTipo
                                       select prestamoTipo).FirstOrDefault();

            if (lcobjTiposGenerador != null)
            {
                lcstrConfiguracion = lcobjTiposGenerador.S_CONFIGURACION;
                lcobjConfiguracionTipo = ObtenerConfiguracion(lcstrConfiguracion, idTipo);

                ViewBag.Configuracion = lcobjConfiguracionTipo;
            }

            return PartialView("_ConfiguracionTipoPartial");
        }

        public ActionResult ObtenerConfiguracionCampo(string id, int ancho, string sqlDatos, int param)
        {
            IEnumerable<DATOS> lcobjDatos;

            ViewBag.Nombre = id;
            ViewBag.Ancho = ancho;

            //lcobjDatos = dbGestionDocumental.Database.SqlQuery<DATOS>(sqlDatos, new object[] { param });
            lcobjDatos = dbSIM.Database.SqlQuery<DATOS>(sqlDatos.Replace("{0}",param.ToString()), new object[] { });

            return PartialView("_ConfiguracionCampoPartial", lcobjDatos.ToList());
        }

        [ValidateInput(false)]
        public ActionResult DropDownPartial(string name, string columnasCombo, string columnasVisualizar, string columnasValor, string SQL, string nameValor)
        {
            bool lcbolFiltro = false;
            IEnumerable<DATOSEXT> lcobjDatos = null;
            ArrayList lcobjRestricciones = new ArrayList();
            string[] lccolColumnasCombo = columnasCombo.Split(',');

            for (int lcintCont = 0; lcintCont < 10; lcintCont++)
            {
                if (Request.Params["DD_" + name + "$DXFREditorcol" + lcintCont.ToString()] != null && Request.Params["DD_" + name + "$DXFREditorcol" + lcintCont.ToString()].Trim() != "")
                {
                    if (lccolColumnasCombo[lcintCont][0] == 'S')
                        lcobjRestricciones.Add(lccolColumnasCombo[lcintCont].Split('%')[0] + " LIKE '%" + Request.Params["DD_" + name + "$DXFREditorcol" + lcintCont.ToString()].Split('%')[0] + "%'");
                    else
                        lcobjRestricciones.Add(lccolColumnasCombo[lcintCont].Split('%')[0] + " LIKE '%" + Request.Params["DD_" + name + "$DXFREditorcol" + lcintCont.ToString()].Split('%')[0] + "%'");

                    lcbolFiltro = true;
                    break;
                }
            }

            if (lcbolFiltro)
            {
                lcobjDatos = dbSIM.Database.SqlQuery<DATOSEXT>("SELECT * FROM (" + SQL + ") TR WHERE " + string.Join(" AND ", lcobjRestricciones.ToArray()), new object[] { });
            }

            ViewBag.Name = name;
            ViewBag.ColumnasCombo = columnasCombo;
            ViewBag.ColumnasVisualizar = columnasVisualizar;
            ViewBag.ColumnasValor = columnasValor;
            ViewBag.NameValor = nameValor;
            ViewBag.SQL = SQL;

            if (lcbolFiltro)
                return PartialView("_DropDownPartial", lcobjDatos.ToList());
            else
                return PartialView("_DropDownPartial", null);
        }

        public string GenerarID()
        {
            string lcstrSufijo;
            int lcintPosicion;
            Dictionary<int, DatosCampos> lccolCampos = new Dictionary<int, DatosCampos>();
            DatosCampos lcobjCampo;
            string lcstrID = "";
            string lcstrCamposLimpiar = "";
            string lcstrCamposTexto = "";

            try
            {
                // Recorre los controles del formulario para obtener los campos y los formatos que generan el ID
                foreach (string lcstrKey in Request.Params.Keys)
                {
                    // Los nombres de los controles siempre tienen mas de 3 caracteres
                    if (lcstrKey.Length > 3)
                    {
                        // El sufijo del control son los últimos 3 caracteres que determinan si es el valor o el formato del campo
                        lcstrSufijo = lcstrKey.Substring(lcstrKey.Length - 3, 3);

                        // Los dos últimos caracters del sufijo son numéricos
                        if (int.TryParse(lcstrSufijo.Substring(1, 2), out lcintPosicion))
                        {
                            // El primer caracter del sufijo es F, C o L
                            if (lcstrSufijo[0] == 'C' || lcstrSufijo[0] == 'F' || lcstrSufijo[0] == 'L' || lcstrSufijo[0] == 'T')
                            {
                                if (!lccolCampos.ContainsKey(lcintPosicion))
                                {
                                    lccolCampos.Add(lcintPosicion, new DatosCampos());
                                }

                                lcobjCampo = lccolCampos[lcintPosicion];

                                if (lcstrSufijo[0] == 'C')
                                {
                                    lcobjCampo.ValorOriginal = Request.Params[lcstrKey];
                                }
                                else if (lcstrSufijo[0] == 'F')
                                {
                                    lcobjCampo.Formato = Request.Params[lcstrKey];
                                }
                                else if (lcstrSufijo[0] == 'L')
                                {
                                    if (Request.Params[lcstrKey].Trim().ToUpper() == "S")
                                    {
                                        if (Request.Params[lcstrKey.Substring(0, lcstrKey.Length - 3)] == null) // Si el elemento sin sufijo no existe, se toma con el sufijo C_XX
                                        {
                                            if (lcstrCamposLimpiar == "")
                                                lcstrCamposLimpiar = lcstrKey.Replace('L', 'C');
                                            else
                                                lcstrCamposLimpiar += "," + lcstrKey.Replace('L', 'C');
                                        }
                                        else
                                        {
                                            if (lcstrCamposLimpiar == "")
                                                lcstrCamposLimpiar = lcstrKey.Substring(0, lcstrKey.Length - 3);
                                            else
                                                lcstrCamposLimpiar += "," + lcstrKey.Substring(0, lcstrKey.Length - 3);
                                        }
                                    }
                                }
                                else if (lcstrSufijo[0] == 'T')
                                {
                                    if (Request.Params[lcstrKey].Trim().ToUpper() == "S")
                                    {
                                        if (Request.Params[lcstrKey.Substring(0, lcstrKey.Length - 3)] == null) // Si el elemento sin sufijo no existe, se toma con el sufijo C_XX
                                        {
                                            if (lcstrCamposTexto == "")
                                                lcstrCamposTexto = lcstrKey.Replace('T', 'C');
                                            else
                                                lcstrCamposTexto += "," + lcstrKey.Replace('T', 'C');
                                        }
                                        else
                                        {
                                            if (lcstrCamposTexto == "")
                                                lcstrCamposTexto = lcstrKey.Substring(0, lcstrKey.Length - 3);
                                            else
                                                lcstrCamposTexto += "," + lcstrKey.Substring(0, lcstrKey.Length - 3);
                                        }
                                    }
                                }

                                lccolCampos[lcintPosicion] = lcobjCampo;
                            }
                        }
                    }
                }

                foreach (KeyValuePair<int, DatosCampos> lcobjCampoObtenido in lccolCampos)
                {
                    if (lcobjCampoObtenido.Value.Formato[0] == 'L')
                    {
                        lcstrID += lcobjCampoObtenido.Value.ValorOriginal.PadRight(int.Parse(lcobjCampoObtenido.Value.Formato.Substring(1)));
                    }
                    else
                    {
                        lcstrID += long.Parse("0" + lcobjCampoObtenido.Value.ValorOriginal).ToString(lcobjCampoObtenido.Value.Formato);
                    }
                }

                var qryIdentificador = from identificador in dbSIM.IDENTIFICADORES
                                       where identificador.S_IDENTIFICADOR == lcstrID
                                       select identificador;

                if (qryIdentificador.ToList().Count > 0) // El Identificador ya existe
                {
                    return "FAIL%El Identificador ya Existe%" + lcstrID + "|" + lcstrCamposTexto + "|" + lcstrCamposLimpiar;
                }
                else
                {
                    string lcstrTexto = "";

                    foreach (string lcstrCampo in lcstrCamposTexto.Split(','))
                    {
                        if (Request.Params[lcstrCampo].Trim() != "")
                        {
                            if (lcstrTexto == "")
                            {
                                lcstrTexto = Request.Params[lcstrCampo].Trim();
                            }
                            else
                            {
                                lcstrTexto += " " + Request.Params[lcstrCampo].Trim();
                            }
                        }
                    }

                    var modelID = new IDENTIFICADORES();
                    //modelID.ID_IDENTIFICADOR = -1;
                    modelID.ID_TIPO = Convert.ToInt32(Request.Params["TipoC01"]);
                    modelID.S_IDENTIFICADOR = lcstrID;
                    modelID.D_CREACION = DateTime.Now;
                    modelID.S_DESCRIPCION = lcstrTexto;

                    dbSIM.Entry(modelID).State = EntityState.Added;
                    dbSIM.SaveChanges();

                    return lcstrID + "|" + lcstrCamposTexto + "|" + lcstrCamposLimpiar;
                }
            }
            catch (Exception err)
            {
                return "ERR_FAIL%Error Generando el Identificador\r\n" + err.Message + "||";
            }
        }

        private CONFIGURACION ObtenerConfiguracion(string rfstrConfiguracion, int vlintIdTipo)
        {
            CONFIGURACION lcobjConfiguracion = new CONFIGURACION();
            CAMPO lcobjCampo;

            // Loading from a file, you can also load from a stream
            var xml = XDocument.Parse(rfstrConfiguracion);

            // Query the data and write out a subset of contacts
            var query = from c in xml.Root.Descendants("CAMPO")
                        select new CAMPO
                        {
                            Nombre = c.Attribute("Nombre").Value,
                            Titulo = c.Attribute("Titulo").Value,
                            Requerido = (c.Attribute("Requerido") == null ? false : (c.Attribute("Requerido").Value.ToUpper() == "S" ? true : false)),
                            Longitud = Convert.ToInt32(c.Attribute("Long").Value),
                            Tipo = c.Attribute("Tipo").Value,
                            Formato = (c.Attribute("Formato") == null ? "" : c.Attribute("Formato").Value),
                            Alineacion = (c.Attribute("Alineacion") == null ? "I" : c.Attribute("Alineacion").Value),
                            Control = (c.Attribute("Control") == null ? "T" : c.Attribute("Control").Value),
                            AnchoControl = Convert.ToInt32(c.Attribute("Ancho").Value),
                            ControlesDependencia = (c.Attribute("Dependencia").Value == null ? "".Split(',') : c.Attribute("Dependencia").Value.Split(',')),
                            SQL = (c.Attribute("SQL") == null ? "" : c.Attribute("SQL").Value),
                            ColumnasCombo = (c.Attribute("ColCombo") == null ? "" : c.Attribute("ColCombo").Value),
                            ColumnasVisualizar = (c.Attribute("ColVisualizar") == null ? "" : c.Attribute("ColVisualizar").Value),
                            ColumnasValor = (c.Attribute("ColValor") == null ? "" : c.Attribute("ColValor").Value),
                            Limpiar = (c.Attribute("Limpiar") == null ? "N" : c.Attribute("Limpiar").Value),
                            ControlTexto = (c.Attribute("ControlTexto") == null ? "N" : c.Attribute("ControlTexto").Value)
                        };

            lcobjConfiguracion.Campos = new Dictionary<string, CAMPO>();

            foreach (CAMPO lcobjCampoConfig in query)
            {
                lcobjCampo = new CAMPO();

                lcobjCampo.ControlesDependientes = new Dictionary<string, CAMPO>();
                lcobjCampo.Nombre = lcobjCampoConfig.Nombre;
                lcobjCampo.Titulo = lcobjCampoConfig.Titulo;
                lcobjCampo.Requerido = lcobjCampoConfig.Requerido;
                lcobjCampo.Longitud = lcobjCampoConfig.Longitud;
                lcobjCampo.Tipo = lcobjCampoConfig.Tipo; // N Numero, S String
                lcobjCampo.Formato = lcobjCampoConfig.Formato; // N Numero, S String
                lcobjCampo.Alineacion = lcobjCampoConfig.Alineacion; // I Izquierda, D Derecha - Solo Aplica para String
                lcobjCampo.Control = lcobjCampoConfig.Control; // T TextBox, C ComboBox, CE ComboBox Extendido, CT Constante, TIPO Tipo de Identificador
                lcobjCampo.AnchoControl = lcobjCampoConfig.AnchoControl;
                lcobjCampo.ControlesDependencia = lcobjCampoConfig.ControlesDependencia; // Solo Aplica para ComboBox
                if (lcobjCampoConfig.Control == "TIPO")
                    lcobjCampo.SQL = vlintIdTipo.ToString();
                else
                    lcobjCampo.SQL = lcobjCampoConfig.SQL; // Solo Aplica para ComboBox
                lcobjCampo.ColumnasCombo = lcobjCampoConfig.ColumnasCombo; // Solo Aplica para ComboBox Extendido
                lcobjCampo.ColumnasVisualizar = lcobjCampoConfig.ColumnasVisualizar; // Solo Aplica para ComboBox Extendido
                lcobjCampo.ColumnasValor = lcobjCampoConfig.ColumnasValor; // Solo Aplica para ComboBox Extendido
                lcobjCampo.Limpiar = lcobjCampoConfig.Limpiar;
                lcobjCampo.ControlTexto = lcobjCampoConfig.ControlTexto;

                if (lcobjCampoConfig.ControlesDependencia[0] != "")
                    lcobjConfiguracion.Campos[lcobjCampoConfig.ControlesDependencia[0]].ControlesDependientes.Add(lcobjCampoConfig.Nombre, lcobjCampo);

                lcobjCampo.ControlesDependientes = new Dictionary<string, CAMPO>();
                if (lcobjCampoConfig.Control != "CT" && lcobjCampoConfig.Control != "T" && lcobjCampoConfig.Control != "TIPO")
                {
                    if (lcobjCampo.ControlesDependencia[0].Trim() == "")
                        lcobjCampo.Datos = dbSIM.Database.SqlQuery<DATOS>(lcobjCampo.SQL, new object[] { }).ToList();
                }

                lcobjConfiguracion.Campos.Add(lcobjCampoConfig.Nombre, lcobjCampo);
            }

            return lcobjConfiguracion;
        }

        protected override void Dispose(bool disposing)
        {
            dbSIM.Dispose();
            base.Dispose(disposing);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            //var model = dbGestionDocumental.TIPOS;
            //return PartialView("_GridViewPartial", model.ToList());
            return PartialView("_GridViewPartial");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialAddNew(TIPOS item)
        {
            
            //var model = dbGestionDocumental.TIPOS;
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        model.Add(item);
            //        dbGestionDocumental.SaveChanges();
            //    }
            //    catch (Exception e)
            //    {
            //        ViewData["EditError"] = e.Message;
            //    }
            //}
            //else
            //    ViewData["EditError"] = "Please, correct all errors.";
            //return PartialView("_GridViewPartial", model.ToList());
            
            return PartialView("_GridViewPartial");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate(TIPOS item)
        {
            //var model = dbGestionDocumental.TIPOS;
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        var modelItem = model.FirstOrDefault(it => it.intIdTipo == item.intIdTipo);
            //        if (modelItem != null)
            //        {
            //            this.UpdateModel(modelItem);
            //            dbGestionDocumental.SaveChanges();
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        ViewData["EditError"] = e.Message;
            //    }
            //}
            //else
            //    ViewData["EditError"] = "Please, correct all errors.";
            //return PartialView("_GridViewPartial", model.ToList());

            return PartialView("_GridViewPartial");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete(System.Int32 intIdTipo)
        {
            //var model = dbGestionDocumental.TIPOS;
            //if (intIdTipo != null)
            //{
            //    try
            //    {
            //        var item = model.FirstOrDefault(it => it.intIdTipo == intIdTipo);
            //        if (item != null)
            //            model.Remove(item);
            //        dbGestionDocumental.SaveChanges();
            //    }
            //    catch (Exception e)
            //    {
            //        ViewData["EditError"] = e.Message;
            //    }
            //}
            //return PartialView("_GridViewPartial", model.ToList());
            return PartialView("_GridViewPartial");
        }*/
    }
}