using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIM.Data;
using System.Security.Claims;
using SIM.Areas.Encuestas.Models;
using SIM.Areas.ControlVigilancia.Models;

namespace SIM.Areas.Encuestas.Controllers
{
    public class ENCUESTAsController : Controller
    {
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: ENCUESTAs
        public ActionResult Index(int CodigoVisita=0)
        {
            //db.ENCUESTAs.ToList()
            return View();
        }

        public ActionResult ObtenerEncuestas()
        {
            //System.Collections.Generic.List<SIM.Areas.Encuestas.Models.InformacionBasicaEncuestaViewModel> Respuesta = new List<InformacionBasicaEncuestaViewModel>();
            //var Resultado = from obj in db.VW_ENCUESTA
            //                select new SIM.Areas.Encuestas.Models.InformacionBasicaEncuestaViewModel
            //                {
            //                    Codigo = obj.ID_ENCUESTA,
            //                    NombreEncuesta = obj.ENCUESTA,
            //                    FechaGrid = obj.FECHA
            //                };
            //return Json(Respuesta.OrderBy(E => E.Codigo), JsonRequestBehavior.AllowGet);

            return Json((from obj in db.VW_ENCUESTA select new { ID_ENCUESTA = obj.ID_ENCUESTA, S_NOMBRE = obj.ENCUESTA, S_RESPONSABLE = obj.RESPONSABLE, Formulario=obj.FORMULARIO, Fecha = obj.FECHA }), System.Web.Mvc.JsonRequestBehavior.AllowGet);
            //return Json((from obj in db.ENCUESTAs select new { ID_ENCUESTA = obj.ID_ENCUESTA, S_NOMBRE = obj.S_NOMBRE,Fecha=obj.D_CREACION }), System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }

        // GET: ENCUESTAs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENCUESTA eNCUESTA = db.ENCUESTAs.Find(id);
            if (eNCUESTA == null)
            {
                return HttpNotFound();
            }
            return View(eNCUESTA);
        }

        // GET: ENCUESTAs/Create
        public ActionResult Create()
        {
            System.Collections.Generic.List<FORMULARIO> Formularios = db.FORMULARIO.OrderBy(F => F.S_NOMBRE).ToList();

            FORMULARIO FormularioTmp = new FORMULARIO();
            FormularioTmp.ID_FORMULARIO = -1;
            FormularioTmp.S_NOMBRE = "";
            Formularios.Add(FormularioTmp);


            ViewBag.IdFormulario = new SelectList(Formularios.OrderBy(F => F.S_NOMBRE), "id_formulario", "s_nombre");
            return View();
        }

        // POST: ENCUESTAs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_ENCUESTA,S_NOMBRE,S_DESCRIPCION,D_CREACION,ID_USUARIO,S_TIPO")] ENCUESTA eNCUESTA)
        {
            if (ModelState.IsValid)
            {
                db.ENCUESTAs.Add(eNCUESTA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eNCUESTA);
        }

        [HttpPost]
        public ActionResult CrearEncuesta(string Nombre, string Descripcion, int CodigoFormulario, string TipoEncuesta)
        {
            int idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)this.HttpContext.User).FindFirst(ClaimTypes.NameIdentifier).Value);

            bool Error = false;

            //if (Nombre == string.Empty)
            //{
            //    return Json("Nombre", JsonRequestBehavior.AllowGet);
            //}

            //if (CodigoFormulario == -1)
            //{
            //    return Json("Formulario", JsonRequestBehavior.AllowGet);
            //}

            //if (TipoEncuesta == string.Empty)
            //{
            //    return Json("Tipo", JsonRequestBehavior.AllowGet);
            //}
          
            SIM.Areas.Encuestas.Models.ENCUESTA EncuestaTmp = new ENCUESTA();
            EncuestaTmp.S_NOMBRE = Nombre;
            EncuestaTmp.S_DESCRIPCION = Descripcion;
            EncuestaTmp.ID_USUARIO = idUsuario;
            EncuestaTmp.D_CREACION = System.DateTime.Today;
            EncuestaTmp.S_TIPO = TipoEncuesta;

            System.Transactions.TransactionScope Transaccion = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew);
            using (Transaccion)
            {
                try
                {
                    db.ENCUESTAs.Add(EncuestaTmp);
                    db.SaveChanges();
                    
                }
                catch
                {
                    Error = true;
                }

                SIM.Areas.Encuestas.Models.FORMULARIO_ENCUESTA FormularioTmp = new FORMULARIO_ENCUESTA();
                FormularioTmp.ID_ENCUESTA = EncuestaTmp.ID_ENCUESTA;
                FormularioTmp.ID_FORMULARIO = CodigoFormulario;
                FormularioTmp.D_INICIO = System.DateTime.Now;

                try
                {
                    db.FORMULARIO_ENCUESTA.Add(FormularioTmp);
                    db.SaveChanges();
                }
                catch
                {
                    Error = true;
                }

                if (!Error)
                {
                    Transaccion.Complete();
                }
            }

            if (Error)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Ok|" + EncuestaTmp.ID_ENCUESTA.ToString(), JsonRequestBehavior.AllowGet);               
            }            
        }

        // GET: ENCUESTAs/Edit/5
        public ActionResult Edit(int? id)
        {            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENCUESTA eNCUESTA = db.ENCUESTAs.Find(id);
            if (eNCUESTA == null)
            {
                return HttpNotFound();
            }

            System.Collections.Generic.List<FORMULARIO> Formularios = db.FORMULARIO.OrderBy(F => F.S_NOMBRE).ToList();
            FORMULARIO FormularioTmp = new FORMULARIO();
            FormularioTmp.ID_FORMULARIO = -1;
            FormularioTmp.S_NOMBRE = "";
            Formularios.Add(FormularioTmp);

            SIM.Areas.Encuestas.Models.FORMULARIO_ENCUESTA VinculoTmp = db.FORMULARIO_ENCUESTA.Where(F => F.ID_ENCUESTA == eNCUESTA.ID_ENCUESTA).FirstOrDefault();

            System.Collections.Generic.List<FORMULARIO_ENCUESTA> FormulariosTmp = db.FORMULARIO_ENCUESTA.Where(F => F.ID_ENCUESTA == id).OrderBy(E => E.ID_FORMULARIOENCUESTA).ToList();
            FORMULARIO_ENCUESTA FormularioEncuestaTmp = FormulariosTmp.LastOrDefault();
            ViewBag.IdFormulario = new SelectList(Formularios.OrderBy(F => F.S_NOMBRE), "id_formulario", "s_nombre", (FormularioEncuestaTmp != null ? FormularioEncuestaTmp.ID_FORMULARIO : -1));
            ViewBag.CodigoEncuesta = id;
            ViewBag.Desactivar = (VinculoTmp != null ? (VinculoTmp.D_FIN != null ? "on" : "") : "");
            return View(eNCUESTA);
        }

        public ActionResult EditarEncuesta(int id)
        {
            ViewBag.codEnc = id;
            return View();
        }
        public ActionResult replicarEncuesta(int id)
        {
            ViewBag.codEnc = id;
            return View();
        }

        
        public ActionResult Clonar(int? id)
        {
            System.Collections.Generic.List<FORMULARIO> Formularios = db.FORMULARIO.OrderBy(F => F.S_NOMBRE).ToList();
            FORMULARIO FormularioTmp = new FORMULARIO();
            FormularioTmp.ID_FORMULARIO = -1;
            FormularioTmp.S_NOMBRE = "";
            Formularios.Add(FormularioTmp);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENCUESTA eNCUESTA = db.ENCUESTAs.Find(id);
            if (eNCUESTA == null)
            {
                return HttpNotFound();
            }

            System.Collections.Generic.List<FORMULARIO_ENCUESTA> FormulariosTmp = db.FORMULARIO_ENCUESTA.Where(F => F.ID_ENCUESTA == id).OrderBy(E => E.ID_FORMULARIOENCUESTA).ToList();
            FORMULARIO_ENCUESTA FormularioEncuestaTmp = FormulariosTmp.LastOrDefault();
            ViewBag.IdFormulario = new SelectList(Formularios.OrderBy(F => F.S_NOMBRE), "id_formulario", "s_nombre", (FormularioEncuestaTmp != null ? FormularioEncuestaTmp.ID_FORMULARIO : -1));
            ViewBag.CodigoEncuesta = id;
            return View(eNCUESTA);
        }
        public ActionResult GuardarClonar(String Nombre, int CodigoEncuesta,String tipo,String descripcion,int id_form)
        {

            int idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)this.HttpContext.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            db.SP_SET_GUARDAR_REPLICA(CodigoEncuesta, Nombre, idUsuario, tipo, descripcion, id_form);
            return Content("ok");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Clonar()
        {
            bool Error = false;
            bool ErrorBd = false;
            int CodigoEncuesta = int.Parse(Request["CodigoEncuesta"].ToString());
            string Nombre = Request["S_NOMBRE"].ToString();

            if (Nombre == string.Empty)
            {
                ModelState.AddModelError("S_NOMBRE", "El nombre es requerido.");
                Error = true;
            }

            int idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)this.HttpContext.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            ENCUESTA Original = db.ENCUESTAs.Where(E => E.ID_ENCUESTA == CodigoEncuesta).FirstOrDefault();

            ENCUESTA Clon = new ENCUESTA();
            Clon = Original;
            Clon.D_CREACION = System.DateTime.Today;
            Clon.S_NOMBRE = Nombre;
            Clon.ID_USUARIO = idUsuario;

            if(!Error)
            {
                if (ModelState.IsValid)
                {
                    System.Transactions.TransactionScope Transaccion = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew);
                    using (Transaccion)
                    {
                            try
                            {
                                //Se crea el clon.
                                db.ENCUESTAs.Add(Clon);
                                db.SaveChanges();
                            }
                            catch{
                                ErrorBd = true;
                            }

                            //Se vincula el clon a los formularios correspondientes.
                            Original = db.ENCUESTAs.Where(E => E.ID_ENCUESTA == CodigoEncuesta).FirstOrDefault();
                            foreach (FORMULARIO_ENCUESTA VinculoFormulario in Original.FORMULARIO_ENCUESTA)
                            {
                                FORMULARIO_ENCUESTA VinculoFormularioTmp = new FORMULARIO_ENCUESTA();
                                VinculoFormularioTmp.ID_ENCUESTA = Clon.ID_ENCUESTA;
                                VinculoFormularioTmp.ID_FORMULARIO = VinculoFormulario.ID_FORMULARIO;
                                db.FORMULARIO_ENCUESTA.Add(VinculoFormularioTmp);
                            }

                            try
                            {
                                db.SaveChanges();
                            }
                            catch{
                                ErrorBd = true;
                            }                        

                            //Se vinculan las preguntas al clon.
                            Original = db.ENCUESTAs.Where(E => E.ID_ENCUESTA == CodigoEncuesta).FirstOrDefault();
                            foreach (ENCUESTA_PREGUNTA VinculoPregunta in Original.ENC_ENCUESTA_PREGUNTA)
                            {
                                ENCUESTA_PREGUNTA VinculoPreguntaTmp = new ENCUESTA_PREGUNTA();
                                VinculoPreguntaTmp.ID_ENCUESTA = Clon.ID_ENCUESTA;
                                VinculoPreguntaTmp.ID_PREGUNTA = VinculoPregunta.ID_PREGUNTA;
                                VinculoPreguntaTmp.N_FACTOR = VinculoPregunta.N_FACTOR;
                                VinculoPreguntaTmp.N_ORDEN = VinculoPregunta.N_ORDEN;
                                VinculoPreguntaTmp.S_REQUERIDA = VinculoPregunta.S_REQUERIDA;
                                db.ENCUESTA_PREGUNTA.Add(VinculoPreguntaTmp);
                            }
                    
                            try
                            {
                                db.SaveChanges();
                            }
                            catch{
                                ErrorBd = true;
                            }

                            if (!ErrorBd)
                            {
                                Transaccion.Complete();
                            }
                    }                        
                }
            }

            System.Collections.Generic.List<FORMULARIO> Formularios = db.FORMULARIO.OrderBy(F => F.S_NOMBRE).ToList();
            FORMULARIO FormularioTmp = new FORMULARIO();
            FormularioTmp.ID_FORMULARIO = -1;
            FormularioTmp.S_NOMBRE = "";
            Formularios.Add(FormularioTmp);

            ViewBag.Error = Error;
            ViewBag.CodigoEncuesta = CodigoEncuesta;
            System.Collections.Generic.List<FORMULARIO_ENCUESTA> FormulariosTmp = db.FORMULARIO_ENCUESTA.Where(F => F.ID_ENCUESTA == CodigoEncuesta).OrderBy(E => E.ID_FORMULARIOENCUESTA).ToList();
            FORMULARIO_ENCUESTA FormularioEncuestaTmp = FormulariosTmp.LastOrDefault();
            ViewBag.IdFormulario = new SelectList(Formularios.OrderBy(F => F.S_NOMBRE), "id_formulario", "s_nombre", (FormularioEncuestaTmp != null ? FormularioEncuestaTmp.ID_FORMULARIO : -1));
            return View(Clon);
        }

        // POST: ENCUESTAs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_ENCUESTA,S_NOMBRE,S_DESCRIPCION,S_TIPO")] ENCUESTA eNCUESTA)
        {
            bool Error = false;
            string Desactivar = "";
            string Tipo = "";

            try
            {
                Desactivar = Request["Desactivar"].ToString();
            }
            catch { }

            try
            {
                Tipo = Request["TipoEncuesta"].ToString();
            }
            catch { }


            SIM.Areas.Encuestas.Models.FORMULARIO_ENCUESTA FormularioTmp = db.FORMULARIO_ENCUESTA.Where(F => F.ID_ENCUESTA == eNCUESTA.ID_ENCUESTA).FirstOrDefault();

            if(eNCUESTA.S_NOMBRE == null)
            {
                ModelState.AddModelError("S_NOMBRE", "El nombre es requerido.");
                Error = true;
            }

            if (Tipo == null)
            {
                ModelState.AddModelError("S_TIPO", "El tipo encuesta es requerido.");
                Error = true;
            }

            if (FormularioTmp == null)
            {
                ModelState.AddModelError("", "El formulario es requerido.");
                Error = true;
            }

            if(!Error)
            {
                if (ModelState.IsValid)
                {
                    if (Desactivar == string.Empty)
                    {
                        FormularioTmp.D_FIN = null;
                    }
                    else
                    {
                        FormularioTmp.D_FIN = System.DateTime.Now;
                    }

                    db.SaveChanges();

                    ENCUESTA EncuestaTmp = db.ENCUESTAs.Where(E => E.ID_ENCUESTA == eNCUESTA.ID_ENCUESTA).FirstOrDefault();

                    EncuestaTmp.S_NOMBRE = eNCUESTA.S_NOMBRE;
                    EncuestaTmp.S_DESCRIPCION = eNCUESTA.S_DESCRIPCION;
                    db.SaveChanges();

                    ViewBag.Error = false;
                }
            }

            System.Collections.Generic.List<FORMULARIO> Formularios = db.FORMULARIO.OrderBy(F => F.S_NOMBRE).ToList();
            FORMULARIO FormularioTmp1 = new FORMULARIO();
            FormularioTmp1.ID_FORMULARIO = -1;
            FormularioTmp1.S_NOMBRE = "";
            Formularios.Add(FormularioTmp1);

            SIM.Areas.Encuestas.Models.FORMULARIO_ENCUESTA VinculoTmp = db.FORMULARIO_ENCUESTA.Where(F => F.ID_ENCUESTA == eNCUESTA.ID_ENCUESTA).FirstOrDefault();

            System.Collections.Generic.List<FORMULARIO_ENCUESTA> FormulariosTmp = db.FORMULARIO_ENCUESTA.Where(F => F.ID_ENCUESTA == eNCUESTA.ID_ENCUESTA).OrderBy(E => E.ID_FORMULARIOENCUESTA).ToList();
            FORMULARIO_ENCUESTA FormularioEncuestaTmp = FormulariosTmp.LastOrDefault();
            ViewBag.IdFormulario = new SelectList(Formularios.OrderBy(F => F.S_NOMBRE), "id_formulario", "s_nombre", (FormularioEncuestaTmp != null ? FormularioEncuestaTmp.ID_FORMULARIO : -1));
            ViewBag.CodigoEncuesta = eNCUESTA.ID_ENCUESTA;
            ViewBag.Desactivar = (VinculoTmp != null ? (VinculoTmp.D_FIN != null ? "on" : "") : "");
            ViewBag.Tipo = Tipo;

            return View(eNCUESTA);
        }

        // GET: ENCUESTAs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENCUESTA eNCUESTA = db.ENCUESTAs.Find(id);
            if (eNCUESTA == null)
            {
                return HttpNotFound();
            }
            return View(eNCUESTA);
        }

        // POST: ENCUESTAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ENCUESTA eNCUESTA = db.ENCUESTAs.Find(id);
            db.ENCUESTAs.Remove(eNCUESTA);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult BuscarEncuestas(string Nombre = "", int Formulario = 0, string Responsable = "", string FechaCreacion = "", int take=0, int skip=0)
        {
            //System.Collections.Generic.List<SIM.Areas.Encuestas.Models.InformacionBasicaEncuestaViewModel> Respuesta = new List<InformacionBasicaEncuestaViewModel>();
            String datos;
            if (Nombre == string.Empty && Formulario == 0 && Responsable == string.Empty && FechaCreacion == null)
            {
            
                var Resultado = from obj in db.VW_ENCUESTA
                                select new SIM.Areas.Encuestas.Models.InformacionBasicaEncuestaViewModel
                                {
                                    Codigo = obj.ID_ENCUESTA,
                                    NombreEncuesta = obj.ENCUESTA,
                                    Responsable=obj.RESPONSABLE,
                                    Formulario=obj.FORMULARIO,
                                    FechaGrid = obj.FECHA,
                                    IdForm=obj.ID_FORMULARIO.ToString()
                                };
                datosConsulta resultado = new datosConsulta() { numRegistros = Resultado.Count() };

                Resultado = Resultado.Skip(skip).Take(take);
                resultado.datos = Resultado.ToList();
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

            var encuesta = from obj in db.VW_ENCUESTA
                           orderby obj.ID_ENCUESTA descending
                            select new SIM.Areas.Encuestas.Models.InformacionBasicaEncuestaViewModel
                            {
                                Codigo = obj.ID_ENCUESTA,
                                NombreEncuesta = obj.ENCUESTA,
                                Responsable = obj.RESPONSABLE,
                                Formulario = obj.FORMULARIO,
                                FechaGrid = obj.FECHA,
                                IdForm=obj.ID_FORMULARIO.ToString()
                            };
            if (Formulario != 0)
            {
                String form = Formulario.ToString();
                encuesta = encuesta.Where(R => R.IdForm == Formulario.ToString());
            }


            if (Nombre != string.Empty)
            {
                encuesta = encuesta.Where(R => R.NombreEncuesta != null);
                encuesta = encuesta.Where(R => R.NombreEncuesta.ToLower().Contains(Nombre.ToLower()));
            }

            if (Responsable != string.Empty)
            {
                encuesta = encuesta.Where(R => R.Responsable != null);
                encuesta = encuesta.Where(R => R.Responsable.ToLower().Contains(Responsable.ToLower()));
            }

            if (FechaCreacion != string.Empty)
            {
                encuesta = encuesta.Where(R => R.FechaGrid == FechaCreacion);
            }

            datosConsulta resultado2 = new datosConsulta() { numRegistros = encuesta.Count() };

            encuesta = encuesta.Skip(skip).Take(take);
            resultado2.datos = encuesta.ToList();
            return Json(resultado2, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult validarNombreEncuesta(String nombre)
        {

            var encuesta = from e in db.ENCUESTAs
                           where e.S_NOMBRE == nombre
                           select new { e.ID_ENCUESTA };
            return Json(encuesta);
        }
        public ActionResult ConsultarFormularioCombo()
        {

            var formulario = from f in db.FORMULARIO
                          
                           select new { f.ID_FORMULARIO,f.S_NOMBRE };
            return Json(formulario); 
        }

        public ActionResult ConsultarEncuestaEditar(int CodigoEncuesta)
        {

            var encuesta = from e in db.VW_ENCUESTAFORM
                           where e.ID_ENCUESTA == CodigoEncuesta
                             select new { e.ID_ENCUESTA,e.ID_FORMULARIO,e.S_DESCRIPCION,e.S_NOMBRE,e.TIPO};
            return Json(encuesta);
        }

        public ActionResult ValidarPreuntaAsociadaEncuesta(int id)
        {

            var solucion = from s in db.ENC_SOLUCION
                            where s.ID_ENCUESTA==id
                             select new { s.ID_ENCUESTA};
            return Json(solucion);
        }
        public ActionResult ValidarEncuestaDiligenciada(int id)
        {

            var encuestaPregunta = from ep in db.ENCUESTA_PREGUNTA
                                   where ep.ID_ENCUESTA == id
                                   select new { ep.ID_PREGUNTA };
            return Json(encuestaPregunta);
        }
        public ActionResult EliminarEncuesta(int id)
        {
            db.SP_ELIMINAR_ENCUESTA(id);
            return Content("1");
        }
        public ActionResult EliminarPreguntaVinculada(int idEnc,int idPregunta)
        {
            db.SP_ELIMINAR_PREGUNTA_VINCULADA(idEnc, idPregunta);
            return Content("1");
        }
        public ActionResult ModificarEncuesta(int id,String nombre,String descripcion)
        {
            var encuesta= (from e in db.ENCUESTAs
                          where e.ID_ENCUESTA==id
                          select e).FirstOrDefault();
            encuesta.S_NOMBRE = nombre;
            encuesta.S_DESCRIPCION = descripcion;
            db.SaveChanges();
            return Content("1");
        }
    }
}
