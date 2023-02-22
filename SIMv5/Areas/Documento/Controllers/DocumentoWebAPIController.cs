using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using System.Web;
using DevExpress.Web.Mvc;
using SIM.Areas.ControlVigilancia.Models;
using System.Security.Claims;
using System.Data.Linq.SqlClient;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using SIM.Data.Control;
using SIM.Models;

namespace SIM.Areas.Documento.Controllers
{
    public class DocumentoWebAPIController : ApiController
    {
        EntitiesSIMOracle db = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();

        System.Web.HttpContext context = System.Web.HttpContext.Current;
        Int32 idUsuario;
        decimal codFuncionario;
        string Responsable;

        public IHttpActionResult GetGuardarDocumentoAdjunto(String idDocumentoAdjunto, int idFormulario, int idEstado)
        {
            string[] arrDocumentoAdjunto = idDocumentoAdjunto.Split(',');

            DOCUMENTO_ADJUNTO DocumentoAdjunto = new DOCUMENTO_ADJUNTO();
            for (int i = 0; i < arrDocumentoAdjunto.Length; i++)
            {

                DocumentoAdjunto.ID_DOCUMENTO = Convert.ToInt32(arrDocumentoAdjunto[i]);
                DocumentoAdjunto.ID_FORMULARIO = idFormulario;
                DocumentoAdjunto.ID_ESTADO = idEstado;
                db.DOCUMENTO_ADJUNTO.Add(DocumentoAdjunto);
                db.SaveChanges();
            }
            return Ok();
        }

        [HttpGet]
        public decimal EliminarDocumentoAdjunto([FromUri] int id_Doc, int idForm)
        {

            Decimal res;
            try
            {
                dbControl.SP_ELIMINAR_DOCUMENTO_ADJUNTO(id_Doc, idForm);
                res = 1;
            }
            catch
            {
                res = 0;
            }
            return res;
        }
        public IHttpActionResult GetConsultarDocumentoAdjunto( Int32 idFormulario, int idEstado)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
                Responsable = codFuncionario.ToString();
            }

            //var documentoadjunto = from f in db.VW_DOCUMENTO_ADJUNTO
            //                       where f.ID_FORMULARIO == idFormulario && f.ID_ESTADO == idEstado
            //                       select new { f.S_ETIQUETA, f.URL, f.ID_DOCUMENTO, f.S_ARCHIVO, f.D_CREACION, f.S_USUARIO };



            //if (filtro != null)
            //{

            //    string[] arrFiltros = filtro.Split(',');

            //    for (int contFiltro = 0; contFiltro < arrFiltros.Length; contFiltro += 4)
            //    {
            //        long datos = 0;
            //        string nombreFiltro = "";
            //        switch (arrFiltros[contFiltro + 1])
            //        {

            //            case "contains":
            //                nombreFiltro = arrFiltros[contFiltro + 2];
            //                documentoadjunto = documentoadjunto.Where(t => t.S_ARCHIVO.ToLower().Contains(nombreFiltro.ToLower()));
            //                break;
            //            case "notcontains":
            //                nombreFiltro = arrFiltros[contFiltro + 2];
            //                documentoadjunto = documentoadjunto.Where(t => !t.S_ARCHIVO.ToLower().Contains(nombreFiltro.ToLower()));
            //                break;
            //            case "startswith":
            //                nombreFiltro = arrFiltros[contFiltro + 2];
            //                documentoadjunto = documentoadjunto.Where(t => t.S_ARCHIVO.ToLower().StartsWith(nombreFiltro.ToLower()));

            //                break;
            //            case "endswith":
            //                nombreFiltro = arrFiltros[contFiltro + 2];
            //                documentoadjunto = documentoadjunto.Where(t => t.S_ARCHIVO.ToLower().EndsWith(nombreFiltro.ToLower()));

            //                break;
            //            case "=":
            //                nombreFiltro = arrFiltros[contFiltro + 2];
            //                documentoadjunto = documentoadjunto.Where(t => t.S_ARCHIVO.ToLower().Contains(nombreFiltro.ToLower()));

            //                break;
            //        }
            //    }
            //}


            //if (documentoadjunto == null)
            //{
            //    return NotFound();
            //}
            //return Ok(documentoadjunto);
            String sql = "select f.S_ETIQUETA, f.URL, f.ID_DOCUMENTO, f.S_ARCHIVO, f.D_CREACION, f.S_USUARIO from control.VW_DOCUMENTO_ADJUNTO f  where f.ID_FORMULARIO = '"+idFormulario+"' and  f.ID_ESTADO = '"+idEstado+"'";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public IHttpActionResult GetconsultarTipoArchivo()
        {

            AppSettingsReader webConfigReader = new AppSettingsReader();
            string tipoArchivo = (string)webConfigReader.GetValue("tipo_archivo", typeof(string));
            return Ok(tipoArchivo);
        }
    }
}