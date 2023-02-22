using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIM.Data;
using SIM.Areas.Encuestas.Models;
using System.Data.Entity.Core.Objects;

namespace SIM.Areas.Encuestas.Controllers
{
    public class PREGUNTAsController : Controller
    {
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }
        // GET: PREGUNTAs
        public ActionResult Index()
        {            
            //var pREGUNTAs = db.PREGUNTAs.Include(p => p.TIPO_PREGUNTA);
            //pREGUNTAs.ToList()
            return View();
        }

        public ActionResult crearRespuesta(int id)
        {
            ViewBag.codPreg = id;
            return View();
        }

        public ActionResult Edit(int id)
        {
            ViewBag.codPreg = id;
            return View();
        }
        public ActionResult NuevaPregunta()
        {
            return View();
        }
        public ActionResult VincularPreguntasEncuesta(int CodigoEncuesta, string TipoEncuesta = "")
        {
            ViewBag.CodigoEncuesta = CodigoEncuesta;
            ViewBag.TipoEncuesta = TipoEncuesta;
            return View();
        }

        [HttpPost]
        public ActionResult VincularPreguntasEncuesta(int CodigoEncuesta, System.Collections.Generic.List<VincularPreguntaViewModel> PreguntasSeleccionadas)
        {
            string Respuesta = "Ok";
            try
            {
                foreach (VincularPreguntaViewModel Vinculo in PreguntasSeleccionadas)
                {
                    int Existe = db.ENCUESTA_PREGUNTA.Where(E => E.ID_ENCUESTA == CodigoEncuesta && E.ID_PREGUNTA == Vinculo.CodigoPregunta).Count();
                    if(Existe == 0)
                    {
                        ENCUESTA_PREGUNTA VinculoNuevo = new ENCUESTA_PREGUNTA();
                        VinculoNuevo.ID_ENCUESTA = CodigoEncuesta;
                        VinculoNuevo.ID_PREGUNTA = Vinculo.CodigoPregunta;
                        VinculoNuevo.N_FACTOR = Vinculo.Peso;
                        VinculoNuevo.N_ORDEN = Vinculo.Orden;
                        VinculoNuevo.S_REQUERIDA = (Vinculo.Requerida == true ? "S" : "N");

                        db.ENCUESTA_PREGUNTA.Add(VinculoNuevo);
                        db.SaveChanges();
                    }
                }
            }
            catch (System.Exception Exc)
            {
                Respuesta = "No fue posible vincular una o todas las preguntas. [" + Exc.Message + "]";
            }

            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerPreguntasxEncuenta(int CodigoEncuesta = -1)
        {
            System.Collections.Generic.List<ENCUESTA_PREGUNTA> PreguntasTmp = new List<ENCUESTA_PREGUNTA>();
       
            if (CodigoEncuesta != -1)
            {
                PreguntasTmp = db.ENCUESTA_PREGUNTA.Where(P => P.ID_ENCUESTA == CodigoEncuesta).ToList();
            }
            else
            {
                PreguntasTmp = db.ENCUESTA_PREGUNTA.ToList(); 
            }
            PreguntasTmp = PreguntasTmp.OrderByDescending(P => P.N_ORDEN).ToList();
            //Estado = obj.ENC_PREGUNTA.S_CAMPORESPUESTA,
            return Json((from obj in PreguntasTmp select new { Codigo = obj.ENC_PREGUNTA.ID_PREGUNTA, Pregunta = obj.ENC_PREGUNTA.S_NOMBRE,  Peso = obj.N_FACTOR, Orden = obj.N_ORDEN, Requerida = (obj.S_REQUERIDA == "S" ? true : false), Vincular = false }), System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerPreguntasTodas(bool Vincular, string TipoEncuesta = "", int CodigoEncuesta = -1, string preguntab = "")
        {
            var datos = from obj in db.VW_PREGUNTAVINCULAR
                        orderby obj.ID_PREGUNTA descending
                        select new { Codigo = obj.ID_PREGUNTA, Pregunta = obj.S_NOMBRE, obj.ID_TIPOPREGUNTA, Peso = 0, Orden = 0, Requerida = false, Vincular = false };
            if (TipoEncuesta == "S")
            {
                datos = datos.Where(P => P.ID_TIPOPREGUNTA == 4);
            }
            if (preguntab != "0")
            {
                datos = datos.Where(P => P.Pregunta.ToLower().Contains(preguntab.ToLower()));
            }
            //datosConsulta resultado = new datosConsulta() { numRegistros = datos.Count() };
            //datos = datos.Skip(skip).Take(take);
            //resultado.datos = datos.ToList();
            return Json(datos, JsonRequestBehavior.AllowGet);
      
        }


        // GET: PREGUNTAs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PREGUNTA pREGUNTA = db.PREGUNTAs.Find(id);
            if (pREGUNTA == null)
            {
                return HttpNotFound();
            }
            return View(pREGUNTA);
        }

        // GET: PREGUNTAs/Create
        public ActionResult Create()
        {
            //SIM.Areas.Encuestas.Models.TIPO_PREGUNTA TPTmp = new TIPO_PREGUNTA();
            //TPTmp.ID_TIPOPREGUNTA = -1;
            //TPTmp.S_NOMBRE = "";

            //System.Collections.Generic.List<SIM.Areas.Encuestas.Models.TIPO_PREGUNTA> Tipos = db.TIPO_PREGUNTA.ToList();
            //Tipos.Add(TPTmp);

            //ViewBag.ID_TIPOPREGUNTA = new SelectList(Tipos.OrderBy(T => T.S_NOMBRE) , "ID_TIPOPREGUNTA", "S_NOMBRE");
            return View();
        }

        // POST: PREGUNTAs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      
        public ActionResult CreatePregunta(String nombre,int tipoPregunta,String ayuda)
        {
            ObjectParameter iDPREGUNTA = new ObjectParameter("iDPREGUNTA", typeof(string));
            db.SP_SET_GUARDAR_PREGUNTA(nombre, tipoPregunta, ayuda, iDPREGUNTA);
            ViewBag.Codigo = iDPREGUNTA.Value;
            return Json(iDPREGUNTA.Value);
            //return Content("1"+iDPREGUNTA.Value);
        }

        // GET: PREGUNTAs/Edit/5


        // POST: PREGUNTAs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PREGUNTA pREGUNTA = db.PREGUNTAs.Find(id);
            if (pREGUNTA == null)
            {
                return HttpNotFound();
            }
            return View(pREGUNTA);
        }

        // POST: PREGUNTAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PREGUNTA pREGUNTA = db.PREGUNTAs.Find(id);
            db.PREGUNTAs.Remove(pREGUNTA);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult consultarVincularPregunta()
        {
            var Resultado = from obj in db.VW_PREGUNTA
                            orderby obj.ID_PREGUNTA descending
                            select new SIM.Areas.Encuestas.Models.DefinicionPreguntaViewModel
                            {
                                Codigo = obj.ID_PREGUNTA,
                                Nombre = obj.PREGUNTA,
                                NombreEncuesta = obj.ENCUESTA,
                                Responsable = obj.RESPONSABLE,
                                FechaGrid = obj.FECHACREACION
                            };
            return Json(Resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarPreguntasB(string Pregunta = "", string Encuesta = "", string Responsable = "", string FechaCreacion = "", int take = 0, int skip = 0)
        {
          
 
                  var Resultado = from obj in db.VW_PREGUNTA
                                  orderby obj.ID_PREGUNTA descending
                                select new SIM.Areas.Encuestas.Models.DefinicionPreguntaViewModel
                                {
                                    Codigo = obj.ID_PREGUNTA,
                                    Nombre = obj.PREGUNTA,
                                    NombreEncuesta=obj.ENCUESTA,
                                   Responsable=obj.RESPONSABLE,
                                    FechaGrid = obj.FECHACREACION
                                };
                

  
            if (Pregunta != string.Empty)
            {
                Resultado = Resultado.Where(R => R.Nombre != null);
                Resultado = Resultado.Where(R => R.Nombre.ToLower().Contains(Pregunta.ToLower()));
            }

            if (Responsable != string.Empty)
            {
                Resultado = Resultado.Where(R => R.Responsable != null);
                Resultado = Resultado.Where(R => R.Responsable.ToLower().Contains(Responsable.ToLower()));
            }
            if (Encuesta != string.Empty)
            {
                Resultado = Resultado.Where(R => R.NombreEncuesta != null);
                Resultado = Resultado.Where(R => R.NombreEncuesta.ToLower().Contains(Encuesta.ToLower()));
            }
            if (FechaCreacion != string.Empty)
            {
                Resultado = Resultado.Where(R => R.FechaGrid == FechaCreacion);
            }
            datosConsulta resultado = new datosConsulta() { numRegistros = Resultado.Count() };
            Resultado = Resultado.Skip(skip).Take(take);
            resultado.datos = Resultado.ToList();
            return Json(resultado, JsonRequestBehavior.AllowGet);
          
        }
        public ActionResult ConsultarTipoPregunta()
        {

            var tipoPregunta = from t in db.TIPO_PREGUNTA

                             select new { t.ID_TIPOPREGUNTA, t.S_NOMBRE};
            return Json(tipoPregunta);
        }
        public ActionResult GuardarRespuesta(String valor, String codigo, int orden,int idPregunta)
        {
            ObjectParameter iDRESPUESTA = new ObjectParameter("iDRESPUESTA", typeof(string));
            db.SP_SET_GUARDAR_RESPUESTA(idPregunta, valor, codigo, orden, iDRESPUESTA);
            ViewBag.Codigo = iDRESPUESTA.Value;
            return Json(iDRESPUESTA.Value);
            //return Content("1"+iDPREGUNTA.Value);
        }
        public ActionResult consultarRespuesta(int id)
        {

            var respuesta = from r in db.ENC_OPCION_RESPUESTA
                            where r.ID_PREGUNTA==id
                            select new { r.ID_RESPUESTA, r.N_ORDEN, r.S_CODIGO, r.S_VALOR };
        
       
           

            return Json(respuesta);
        }
        public ActionResult EliminarPregunta(int id)
        {
            db.SP_ELIMINAR_PREGUNTA(id);
            return Content("1");
        }
        public ActionResult ValidarPreguntaAsignada(int id)
        {

            var encuestaPregunta = from ep in db.ENCUESTA_PREGUNTA
                                   where ep.ID_PREGUNTA== id
                                   select new { ep.ID_PREGUNTA };
            return Json(encuestaPregunta);
        }
        public ActionResult ConsultarPreguntaEditar(int id)
        {
            var datos = from p in db.VW_PREGUNTA_DATOS_EDITAR
                        where p.ID_PREGUNTA == id
                        select new { p.ID_PREGUNTA, p.FECHAFIN,p.FECHAINICIO,p.ID_TIPOPREGUNTA,p.S_AYUDA,p.S_NOMBRE };
            return Json(datos);
        }
        public ActionResult ModificarPregunta(int id, String nombre, String ayuda,int tipoPregunta,String fechaFin)
        {
            var datos = (from p in db.ENC_PREGUNTA
                            where p.ID_PREGUNTA== id
                            select p).FirstOrDefault();
            datos.S_NOMBRE = nombre;
            datos.S_AYUDA = ayuda;
            datos.ID_TIPOPREGUNTA = tipoPregunta;
            if (fechaFin!="")
            datos.D_FIN = Convert.ToDateTime(fechaFin);
            db.SaveChanges();
            return Content("1");
        }
        public ActionResult EliminarRespuesta(int id)
        {
            db.SP_ELIMINAR_RESPUESTA(id);
            return Content("1");
        }
    } 
}
