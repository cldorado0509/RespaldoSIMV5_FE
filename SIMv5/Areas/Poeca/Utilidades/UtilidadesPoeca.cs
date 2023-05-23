using SIM.Areas.Poeca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;
using SIM.Data;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Oracle.ManagedDataAccess.Types;
using System.IO;
using SIM.Data.General;
using SIM.Data.Seguridad;
using SIM.Data.Tramites;
using SIM.Models;

namespace SIM.Utilidades
{
    public class UtilidadesPoeca
    {
        public static ListItem[] crearListaDeAnios()
        {
            ListItem[] aniosDisponibles = {
                new ListItem(){ Id = (DateTime.Today.Year + 1), Text = (DateTime.Today.Year + 1).ToString() },
                new ListItem(){ Id = DateTime.Today.Year, Text = DateTime.Today.Year.ToString() }
            };

            return aniosDisponibles;
        }

        public static decimal? crearTramite(int codFuncionario, string descripcion = "Esto es una prueba")
        {
            EntitiesTramitesOracle dbTramites = new EntitiesTramitesOracle();
            var codProceso = Convert.ToDecimal(SIM.Utilidades.Data.ObtenerValorParametro("ProcesoNuevoTramite"));
            var codTarea = Convert.ToDecimal(SIM.Utilidades.Data.ObtenerValorParametro("TareaNuevoTramite"));
            //var codFuncionario = documento.ID_RESPONSABLE;
            var respCodTramite = new ObjectParameter("respCodTramite", typeof(decimal));
            var respCodTarea = new ObjectParameter("respCodTarea", typeof(decimal));
            var rtaResultado = new ObjectParameter("rtaResultado", typeof(string));

            var test = dbTramites.SP_NUEVO_TRAMITE(codProceso, codTarea, codFuncionario, descripcion, respCodTramite, respCodTarea, rtaResultado);

            var respuesta = (decimal?)respCodTramite.Value;
            return respuesta;
        }


        public static void RegistrarDocumento(int idUsuario, int codTramite, int codSerie, int? idRadicado, byte[] documentoBinario, int numPaginas)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            TBINDICEDOCUMENTO indiceDocumento;
            int idCodDocumento;
            string rutaDocumento = "";

            TBTRAMITE tramite = dbSIM.TBTRAMITE.Where(t => t.CODTRAMITE == codTramite).FirstOrDefault();
            TBRUTAPROCESO rutaProceso = dbSIM.TBRUTAPROCESO.Where(rp => rp.CODPROCESO == tramite.CODPROCESO).FirstOrDefault();
            TBTRAMITEDOCUMENTO ultimoDocumento = dbSIM.TBTRAMITEDOCUMENTO.Where(td => td.CODTRAMITE == codTramite).OrderByDescending(td => td.CODDOCUMENTO).FirstOrDefault();
            RADICADO_DOCUMENTO radicado = dbSIM.RADICADO_DOCUMENTO.Where(r => r.ID_RADICADODOC == idRadicado).FirstOrDefault();

            if (ultimoDocumento == null)
                idCodDocumento = 1;
            else
                idCodDocumento = Convert.ToInt32(ultimoDocumento.CODDOCUMENTO) + 1;

            rutaDocumento = rutaProceso.PATH + "\\" + Archivos.GetRutaDocumento(Convert.ToUInt64(tramite.CODTRAMITE), 100) + tramite.CODTRAMITE.ToString("0") + "-" + idCodDocumento.ToString() + ".pdf";

            if (!Directory.Exists(Path.GetDirectoryName(rutaDocumento)))
                Directory.CreateDirectory(Path.GetDirectoryName(rutaDocumento));

            FileStream archivoRadicado = new FileStream(rutaDocumento, FileMode.CreateNew);

            archivoRadicado.Write(documentoBinario, 0, documentoBinario.Length);
            archivoRadicado.Close();

            TBTRAMITEDOCUMENTO documento = new TBTRAMITEDOCUMENTO();
            TBTRAMITE_DOC relDocTra = new TBTRAMITE_DOC();
            documento.CODDOCUMENTO = idCodDocumento;
            documento.CODTRAMITE = tramite.CODTRAMITE;
            documento.TIPODOCUMENTO = 1;
            documento.FECHACREACION = DateTime.Now;
            documento.CODFUNCIONARIO = decimal.Parse(SIM.Utilidades.Data.ObtenerValorParametro("UnidadDocCOR")); // TODO
            documento.ID_USUARIO = idUsuario;
            documento.RUTA = rutaDocumento;
            documento.MAPAARCHIVO = "M";
            documento.MAPABD = "M";
            documento.PAGINAS = numPaginas;
            documento.CODSERIE = codSerie;
            documento.CIFRADO = "0";

            dbSIM.Entry(documento).State = System.Data.Entity.EntityState.Added;
            dbSIM.SaveChanges();

            relDocTra.CODTRAMITE = tramite.CODTRAMITE;
            relDocTra.CODDOCUMENTO = idCodDocumento;
            relDocTra.ID_DOCUMENTO = documento.ID_DOCUMENTO;
            dbSIM.Entry(relDocTra).State = System.Data.Entity.EntityState.Added;
            dbSIM.SaveChanges();
            // Consulta Datos de los Indices del Documento
            var indiceSerieRadicado = dbSIM.TBINDICESERIE.Where(isd => isd.CODSERIE == codSerie && isd.INDICE_RADICADO != null).ToList();

            foreach (var indice in indiceSerieRadicado)
            {
                string valor = "";
                if (indice.INDICE_RADICADO.Trim() == "R")
                {
                    valor = radicado.S_RADICADO;
                }
                else
                {
                    valor = radicado.D_RADICADO.ToString("dd/MM/yyyy");
                }

                indiceDocumento = new TBINDICEDOCUMENTO();
                indiceDocumento.CODTRAMITE = codTramite;
                indiceDocumento.CODDOCUMENTO = idCodDocumento;
                indiceDocumento.CODINDICE = indice.CODINDICE;
                indiceDocumento.VALOR = valor;
                dbSIM.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                dbSIM.SaveChanges();
            }

            /* PARA CADA INDICE DEL DOCUMENTO */
            //indiceDocumento = new TBINDICEDOCUMENTO();
            //indiceDocumento.CODTRAMITE = codTramite;
            //indiceDocumento.CODDOCUMENTO = idCodDocumento;
            //indiceDocumento.CODINDICE = #COD INDICE#;
            //indiceDocumento.VALOR = #VALOR#;
            //dbSIM.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
            //dbSIM.SaveChanges();
        }

        public static TERCERO AdquirirTercero(EntitiesSIMOracle dbSIM, int userId, int? id = null)
        {
            TERCERO terceroDb = null;
            PROPIETARIO propietario = null;
            
            if (id == null)
                propietario = dbSIM.PROPIETARIO.Where(x => x.ID_USUARIO == userId).FirstOrDefault();
            
            if (propietario == null && id.HasValue)
            {
                propietario = dbSIM.PROPIETARIO.Where(x => x.ID_TERCERO == id).FirstOrDefault();
            }

            if (propietario != null)
            {
                terceroDb = dbSIM.TERCERO.Where(x => x.ID_TERCERO == propietario.ID_TERCERO).FirstOrDefault();
            }

            return terceroDb;
        }

    }
}