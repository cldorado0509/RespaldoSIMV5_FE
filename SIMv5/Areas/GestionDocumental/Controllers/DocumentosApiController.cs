using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace SIM.Areas.GestionDocumental.Controllers
{
    public class DocumentosApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        /// <summary>
        /// Retorna los documentos de un tramite
        /// </summary>
        /// <param name="CodTramite">Codigo del trámite</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ObtieneDocumento")]
        public JArray GetObtieneDocumento(int IdDocumento)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int idUsuario = 0;
                decimal funcionario = 0;
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

                    funcionario = SIM.Utilidades.Security.Obtener_Codigo_Funcionario(idUsuario);
                }


                var model = (from Doc in dbSIM.TBTRAMITEDOCUMENTO
                             join Ser in dbSIM.TBSERIE on Doc.CODSERIE equals Ser.CODSERIE
                             where Doc.ID_DOCUMENTO == IdDocumento
                             orderby Doc.CODDOCUMENTO descending
                             select new Documento
                             {
                                 ID_DOCUMENTO = Doc.ID_DOCUMENTO,
                                 CODDOC = Doc.CODDOCUMENTO,
                                 SERIE = Ser.NOMBRE,
                                 FECHA = Doc.FECHACREACION.Value,
                                 ESTADO = Doc.S_ESTADO == "N" ? "Anulado" : "",
                                 ADJUNTO = Doc.S_ADJUNTO != "1" ? "No" : "Si",
                                 CODTRAMITE = Doc.CODTRAMITE,
                                 EDIT_INDICES = (from PER in dbSIM.PERMISOSSERIE where PER.CODFUNCIONARIO == funcionario &&
                                                 PER.CODSERIE == Doc.CODSERIE select PER.PM).FirstOrDefault() == "1" ? true : false
                             }).ToList();
                if (model != null)
                {
                    foreach (var doc in model)
                    {
                        if (doc.ESTADO != "Anulado")
                        {
                            var estDoc = (from A in dbSIM.ANULACION_DOC
                                          join D in dbSIM.TRAMITES_PROYECCION on A.ID_PROYECCION_DOC equals D.ID_PROYECCION_DOC
                                          where D.CODTRAMITE == doc.CODTRAMITE && D.CODDOCUMENTO == doc.CODDOC && A.S_ESTADO == "P"
                                          select A.ID_ANULACION_DOC).FirstOrDefault();
                            if (estDoc > 0) doc.ESTADO = "En proceso";
                        }
                    }
                }
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
    }

    public class Documento
    {
        public decimal ID_DOCUMENTO { get; set; }
        public decimal CODDOC { get; set; }
        public string SERIE { get; set; }
        public DateTime FECHA { get; set; }
        public string ESTADO { get; set; }
        public string ADJUNTO { get; set; }
        public decimal CODTRAMITE { get; set; }
        public bool EDIT_INDICES { get; set; }   
    }
}
