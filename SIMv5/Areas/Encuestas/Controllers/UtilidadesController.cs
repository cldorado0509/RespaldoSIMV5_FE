using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIM.Areas.Encuestas.Models;
using SIM.Data;

namespace SIM.Areas.Encuestas.Controllers
{
    public class UtilidadesController : Controller
    {
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        public ActionResult ObtenerInfoBasicaEncuestas(int CodigoFormulario)
        {
            System.Collections.Generic.List<FORMULARIO_ENCUESTA> Encuestas = db.FORMULARIO_ENCUESTA.Where(F => F.ID_FORMULARIO == CodigoFormulario && F.D_FIN == null).ToList();

            return Json(from obj in Encuestas select new InformacionBasicaEncuestaViewModel { Codigo = obj.ID_ENCUESTA, NombreEncuesta = obj.ENC_ENCUESTA.S_NOMBRE, Descripcion = obj.ENC_ENCUESTA.S_DESCRIPCION }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerPreguntasEncuesta(int CodigoEncuesta)
        {
            System.Collections.Generic.List<ENCUESTA_PREGUNTA> Preguntas = db.ENCUESTA_PREGUNTA.Where(P => P.ID_ENCUESTA == CodigoEncuesta).ToList();

            System.Collections.Generic.List<DefinicionPreguntaViewModel> Resultado = (from obj in Preguntas select new DefinicionPreguntaViewModel {                
                Codigo = obj.ENC_PREGUNTA.ID_PREGUNTA,
                Nombre = obj.ENC_PREGUNTA.S_NOMBRE,
                TipoPregunta = obj.ENC_PREGUNTA.ENC_TIPO_PREGUNTA.ID_TIPOPREGUNTA,
                //CampoRespuesta = obj.ENC_PREGUNTA.S_CAMPORESPUESTA,
                Respuestas = (from objTmp in obj.ENC_PREGUNTA.ENC_OPCION_RESPUESTA
                              select new DefinicionRespuestaViewModel
                              { 
                    Codigo = objTmp.ID_RESPUESTA,
                    CodigoPregunta = objTmp.ID_PREGUNTA,
                    Valor  = objTmp.S_CODIGO,
                    Descripcion = objTmp.S_VALOR
                }).ToList(),
                Orden = obj.N_ORDEN.Value,
                Peso = obj.N_FACTOR.Value,
                Requerida = (obj.S_REQUERIDA == "S" ? true : false),
                Ayuda = obj.ENC_PREGUNTA.S_AYUDA
            }).ToList();

            return Json(Resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //public ActionResult GuardarSolucionEncuesta(int CodigoEncuesta, int CodigoObjeto, System.Collections.Generic.List<SIM.Areas.Encuestas.Models.SolucionEncuestaViewModel> Solucion)
        //{
        //    foreach (SIM.Areas.Encuestas.Models.SolucionEncuestaViewModel SolucionTmp in Solucion)
        //    {
        //        SIM.Areas.Encuestas.Models.SOLUCION NuevaSolucion = new SOLUCION();
        //        NuevaSolucion.D_TIPOFECHA = System.DateTime.Now;
        //        NuevaSolucion.ID_ENCUESTA = CodigoEncuesta;
        //        NuevaSolucion.ID_OBJETOCONTROL = CodigoObjeto;
        //        NuevaSolucion.ID_PREGUNTA = SolucionTmp.CodigoPregunta;
        //        if(SolucionTmp.CodigoRespuesta != null)
        //        {
        //            NuevaSolucion.ID_RESPUESTA = SolucionTmp.CodigoRespuesta;
        //        }

        //        if (SolucionTmp.ValorFecha != null)
        //        {
        //            NuevaSolucion.D_TIPOFECHA = SolucionTmp.ValorFecha.Value;
        //        }

        //        if(SolucionTmp.ValorNumero != null)
        //        {
        //            NuevaSolucion.N_TIPONUMERO = SolucionTmp.ValorNumero.Value;
        //        }

        //        if (SolucionTmp.ValorTexto != string.Empty)
        //        {
        //            NuevaSolucion.S_TIPOTEXTO = SolucionTmp.ValorTexto;
        //        }

        //        db.SOLUCIONs.Add(NuevaSolucion);
        //        db.SaveChanges();                
        //    }

        //    return Json(true, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public ActionResult BuscarPreguntas(string Pregunta = "", string Encuesta = "", string Responsable = "", System.DateTime? FechaCreacion = null)
        {
            System.Collections.Generic.List<SIM.Areas.Encuestas.Models.DefinicionPreguntaViewModel> Preguntas = new List<DefinicionPreguntaViewModel>();

            var Resultado = from prg in db.ENCUESTA_PREGUNTA
                            select new SIM.Areas.Encuestas.Models.DefinicionPreguntaViewModel{
                                Codigo = prg.ID_PREGUNTA,
                                Nombre = prg.ENC_PREGUNTA.S_NOMBRE,
                                NombreEncuesta = prg.ENC_ENCUESTA.S_NOMBRE,
                                Orden = (int) prg.N_ORDEN,
                                Peso = (int) prg.N_FACTOR,
                                Requerida = (prg.S_REQUERIDA == "S" ? true : false),
                                //Estado = (prg.ENC_PREGUNTA.S_CAMPORESPUESTA == "S" ? true : false),
                                TipoPregunta = (int)prg.ENC_PREGUNTA.ID_TIPOPREGUNTA,
                                Fecha = (prg.ENC_PREGUNTA.D_INICIO != null ? prg.ENC_PREGUNTA.D_INICIO.Value : new System.DateTime())
                            };

            Preguntas = Resultado.ToList();

            if (Pregunta != string.Empty)
            {
                Preguntas = Preguntas.Where(P => P.Nombre.ToLower().Contains(Pregunta.ToLower())).ToList();
            }

            if (Encuesta != string.Empty)
            {
                Preguntas = Preguntas.Where(P => P.NombreEncuesta.ToLower().Contains(Encuesta.ToLower())).ToList();
            }

            //if (Responsable != string.Empty)
            //{
            //    Preguntas = Preguntas.Where(P => P.NombreEncuesta.Contains(Encuesta)).ToList();
            //}

            if (FechaCreacion != null)
            {
                Preguntas = Preguntas.Where(P => P.Fecha.ToString("dd/MM/yyyy") == FechaCreacion.Value.ToString("dd/MM/yyyy")).ToList();
            }

            return Json(Preguntas.OrderBy(P => P.Codigo), JsonRequestBehavior.AllowGet);
        }
    }
}