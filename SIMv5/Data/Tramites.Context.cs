namespace SIM.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using SIM.Data.Tramites;

    public partial class EntitiesSIMOracle : DbContext
    {
        public virtual DbSet<ACTUACION_DOCUMENTO> ACTUACION_DOCUMENTO { get; set; }
        public virtual DbSet<CORRESPONDENCIAENV_DET> CORRESPONDENCIAENV_DET { get; set; }
        public virtual DbSet<CORRESPONDENCIAENVIADA> CORRESPONDENCIAENVIADA { get; set; }
        public virtual DbSet<CORRESPONDENCIASELECCION> CORRESPONDENCIASELECCION { get; set; }
        public virtual DbSet<DEFRADICADOS> DEFRADICADOS { get; set; }
        public virtual DbSet<DETALLE_PRESTAMO> DETALLE_PRESTAMO { get; set; }
        public virtual DbSet<DETALLE_REGLA> DETALLE_REGLA { get; set; }
        public virtual DbSet<DEVOLUCION_TAREA> DEVOLUCION_TAREA { get; set; }
        public virtual DbSet<DEVOLUCION_TAREAMOTIVO> DEVOLUCION_TAREAMOTIVO { get; set; }
        public virtual DbSet<DOCUMENTO_TEMPORAL> DOCUMENTO_TEMPORAL { get; set; }
        public virtual DbSet<FORMULARIO_TIPOACTO> FORMULARIO_TIPOACTO { get; set; }
        public virtual DbSet<MUNICIPIO> MUNICIPIO { get; set; }
        public virtual DbSet<PRESTAMO_DETALLE_TRAMITES> PRESTAMO_DETALLE_TRAMITES { get; set; }
        public virtual DbSet<PRESTAMO_EXPEDIENTE> PRESTAMO_EXPEDIENTE { get; set; }
        public virtual DbSet<PRESTAMO_TIPO> PRESTAMO_TIPO { get; set; }
        public virtual DbSet<PRESTAMOS_TRAMITES> PRESTAMOS_TRAMITES { get; set; }
        public virtual DbSet<PROGRAMACION_ACTUACION> PROGRAMACION_ACTUACION { get; set; }
        public virtual DbSet<PROGRAMACION_ASIGNACION> PROGRAMACION_ASIGNACION { get; set; }
        public virtual DbSet<PROGRAMACION_EJECUCION> PROGRAMACION_EJECUCION { get; set; }
        public virtual DbSet<PROGRAMACION_NUEVOS_ASUNTOS> PROGRAMACION_NUEVOS_ASUNTOS { get; set; }
        public virtual DbSet<PROGRAMACION_TAREAS_ASUNTOS> PROGRAMACION_TAREAS_ASUNTOS { get; set; }
        public virtual DbSet<PROGRAMACION_TRAMITES> PROGRAMACION_TRAMITES { get; set; }
        public virtual DbSet<PROYECCION_DOC> PROYECCION_DOC { get; set; }
        public virtual DbSet<PROYECCION_DOC_ARCHIVOS> PROYECCION_DOC_ARCHIVOS { get; set; }
        public virtual DbSet<PROYECCION_DOC_COM> PROYECCION_DOC_COM { get; set; }
        public virtual DbSet<PROYECCION_DOC_DET_ARCH> PROYECCION_DOC_DET_ARCH { get; set; }
        public virtual DbSet<PROYECCION_DOC_FIRMAS> PROYECCION_DOC_FIRMAS { get; set; }
        public virtual DbSet<PROYECCION_DOC_INDICES> PROYECCION_DOC_INDICES { get; set; }
        public virtual DbSet<RADICADO_DOCUMENTO> RADICADO_DOCUMENTO { get; set; }
        public virtual DbSet<RADICADO_UNIDADDOC> RADICADO_UNIDADDOC { get; set; }
        public virtual DbSet<RADICADOS> RADICADOS { get; set; }
        public virtual DbSet<REQUISITOS_TRAMITE> REQUISITOS_TRAMITE { get; set; }
        public virtual DbSet<TBFUNCIONARIO> TBFUNCIONARIO { get; set; }
        public virtual DbSet<TBINDICEDOCUMENTO> TBINDICEDOCUMENTO { get; set; }
        public virtual DbSet<TBINDICESERIE> TBINDICESERIE { get; set; }
        public virtual DbSet<TBMUNICIPIO> TBMUNICIPIO { get; set; }
        public virtual DbSet<TBPROCESO> TBPROCESO { get; set; }
        public virtual DbSet<TBPROYECTO> TBPROYECTO { get; set; }
        public virtual DbSet<TBQUEJA> TBQUEJA { get; set; }
        public virtual DbSet<TBRUTAPROCESO> TBRUTAPROCESO { get; set; }
        public virtual DbSet<TBSERIE> TBSERIE { get; set; }
        public virtual DbSet<TBSERIE_DOCUMENTAL> TBSERIE_DOCUMENTAL { get; set; }
        public virtual DbSet<TBSOLICITUD> TBSOLICITUD { get; set; }
        public virtual DbSet<TBSUBSERIE> TBSUBSERIE { get; set; }
        public virtual DbSet<TBSUBSERIE_DOCUMENTAL> TBSUBSERIE_DOCUMENTAL { get; set; }
        public virtual DbSet<TBTAREA> TBTAREA { get; set; }
        public virtual DbSet<TBTARIFAS_CALCULO> TBTARIFAS_CALCULO { get; set; }
        public virtual DbSet<TBTARIFAS_PARAMETRO> TBTARIFAS_PARAMETRO { get; set; }
        public virtual DbSet<TBTARIFAS_TOPES> TBTARIFAS_TOPES { get; set; }
        public virtual DbSet<TBTARIFAS_TRAMITE> TBTARIFAS_TRAMITE { get; set; }
        public virtual DbSet<TBTIPO_SOLICITUD> TBTIPO_SOLICITUD { get; set; }
        public virtual DbSet<TBTRAMITE> TBTRAMITE { get; set; }
        public virtual DbSet<TBTRAMITEDOCUMENTO> TBTRAMITEDOCUMENTO { get; set; }
        public virtual DbSet<TBTRAMITES_BLOQUEADOS> TBTRAMITES_BLOQUEADOS { get; set; }
        public virtual DbSet<TBTRAMITETAREA> TBTRAMITETAREA { get; set; }
        public virtual DbSet<TERMINOSCONDICIONES_TRAMITE> TERMINOSCONDICIONES_TRAMITE { get; set; }
        public virtual DbSet<TIPO_ACTO> TIPO_ACTO { get; set; }
        public virtual DbSet<TIPO_ACTUACION> TIPO_ACTUACION { get; set; }
        public virtual DbSet<TRAMITE_EXPEDIENTE_AMBIENTAL> TRAMITE_EXPEDIENTE_AMBIENTAL { get; set; }
        public virtual DbSet<TRAMITE_EXPEDIENTE_QUEJA> TRAMITE_EXPEDIENTE_QUEJA { get; set; }
        public virtual DbSet<TRAMITES_PROYECCION> TRAMITES_PROYECCION { get; set; }
        public virtual DbSet<ZONA_PROGRAMACION> ZONA_PROGRAMACION { get; set; }
        public virtual DbSet<QRY_FUNCIONARIO_ALL> QRY_FUNCIONARIO_ALL { get; set; }
        public virtual DbSet<QRY_INDICESDESPACHADOS> QRY_INDICESDESPACHADOS { get; set; }
        public virtual DbSet<QRY_LISTADOTRAMITES> QRY_LISTADOTRAMITES { get; set; }
        public virtual DbSet<QRY_REQUISITOS_TRAMITE> QRY_REQUISITOS_TRAMITE { get; set; }
        public virtual DbSet<VW_PROGRAMACIONES_PENDIENTES> VW_PROGRAMACIONES_PENDIENTES { get; set; }
        public virtual DbSet<VW_TAREAS_PROYECCION> VW_TAREAS_PROYECCION { get; set; }
        public DbSet<QRY_INDICE_EXPEDIENTETRAMITE> QRY_INDICE_EXPEDIENTETRAMITE { get; set; }
        public DbSet<QRY_TRAMITEPRIORITARIO> QRY_TRAMITEPRIORITARIO { get; set; }
        public virtual DbSet<ANULACION_DOC> ANULACION_DOC { get; set; }
        public virtual DbSet<TBTAREACOMENTARIO> TBTAREACOMENTARIO { get; set; }
        public virtual DbSet<MOTIVO_ANULACION> MOTIVO_ANULACION { get; set; }
        public virtual DbSet<TBTAREARESPONSABLE> TBTAREARESPONSABLE { get; set; }
        public virtual DbSet<TBINDICEPROCESO> TBINDICEPROCESO { get; set; }
        public virtual DbSet<TBINDICETRAMITE> TBINDICETRAMITE { get; set; }
        public virtual DbSet<EXP_EXPEDIENTES> EXP_EXPEDIENTES { get; set; }
        public virtual DbSet<EXP_BUSQUEDA> EXP_BUSQUEDA { get; set; }
        //public virtual DbSet<EXP_ANEXOS> EXP_ANEXOS { get; set; }
        public virtual DbSet<EXP_INDICES> EXP_INDICES { get; set; }
        public virtual DbSet<EXP_DOCUMENTOSEXPEDIENTE> EXP_DOCUMENTOSEXPEDIENTE { get; set; }
        public virtual DbSet<QRY_INFORMESFACTURA> QRY_INFORMEFACTURA { get; set; }
        public virtual DbSet<QRY_RESOLUCIONFACTURA> QRY_RESOLUCIONFACTURA { get; set; }
        public virtual DbSet<EXP_ESTADOSEXPEDIENTE> EXP_ESTADOSEXPEDIENTE { get; set; }
        public virtual DbSet<EXP_TOMOS> EXP_TOMOS { get; set; }
        //public virtual DbSet<EXP_RESPOSABLESEXPEDIENTE> EXP_RESPOSABLESEXPEDIENTE { get; set; }
        //public virtual DbSet<EXP_TIPOANEXO> EXP_TIPOANEXO { get; set; }
        public virtual DbSet<EXP_TIPOESTADO> EXP_TIPOESTADO { get; set; }
        public virtual DbSet<BUSQUEDA_DOCUMENTO> BUSQUEDA_DOCUMENTO { get; set; }
        public virtual DbSet<TBUNIDADESDOC_VIGENCIATRD> TBUNIDADESDOC_VIGENCIATRD { get; set; }
        public virtual DbSet<TBVIGENCIA_TRD> TBVIGENCIA_TRD { get; set; }
        public virtual DbSet<TBCARGO> TBCARGO { get; set; }
        public virtual DbSet<QRY_MEMORANDOS> QRY_MEMORANDOS { get; set; }
        public virtual DbSet<TBACTIVIDADES_VITAL> TBACTIVIDADES_VITAL { get; set; }
        public virtual DbSet<TBVISITADOCUMENTO> TBVISITADOCUMENTO { get; set; }
        public virtual DbSet<QRY_INFORMESTECNICOS> QRY_INFORMESTECNICOS { get; set; }
        public virtual DbSet<TBSOLICITUDES_VITAL> TBSOLICITUDES_VITAL { get; set; }
        public virtual DbSet<QRY_RESOLUCIONES> QRY_RESOLUCIONES { get; set; }
        public virtual DbSet<TBPERMISOS_SERIE> PERMISOSSERIE { get; set; }
        /*
        public virtual int SP_SET_ITEMVISITA(Nullable<decimal> iDVISITA, Nullable<decimal> iDITEM, Nullable<decimal> iDTRAMITE, ObjectParameter rTAI)
        {
            var iDVISITAParameter = iDVISITA.HasValue ?
                new ObjectParameter("IDVISITA", iDVISITA) :
                new ObjectParameter("IDVISITA", typeof(decimal));

            var iDITEMParameter = iDITEM.HasValue ?
                new ObjectParameter("IDITEM", iDITEM) :
                new ObjectParameter("IDITEM", typeof(decimal));

            var iDTRAMITEParameter = iDTRAMITE.HasValue ?
                new ObjectParameter("IDTRAMITE", iDTRAMITE) :
                new ObjectParameter("IDTRAMITE", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesTramitesOracle.SP_SET_ITEMVISITA", iDVISITAParameter, iDITEMParameter, iDTRAMITEParameter, rTAI);
        }

        public virtual int SP_PROGRAMACION_PROXEJECUCION(Nullable<decimal> iDPROGRAMACION, ObjectParameter rTAI)
        {
            var iDPROGRAMACIONParameter = iDPROGRAMACION.HasValue ?
                new ObjectParameter("IDPROGRAMACION", iDPROGRAMACION) :
                new ObjectParameter("IDPROGRAMACION", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesTramitesOracle.SP_PROGRAMACION_PROXEJECUCION", iDPROGRAMACIONParameter, rTAI);
        }

        public virtual int SP_NUEVO_TRAMITE_REGISTRO(Nullable<decimal> cODPROCESO, Nullable<decimal> cODFUNCIONARIO, string mENSAJE, ObjectParameter rESPCODTRAMITE, ObjectParameter rESPCODTAREA, ObjectParameter rTARESULTADO)
        {
            var cODPROCESOParameter = cODPROCESO.HasValue ?
                new ObjectParameter("CODPROCESO", cODPROCESO) :
                new ObjectParameter("CODPROCESO", typeof(decimal));

            var cODFUNCIONARIOParameter = cODFUNCIONARIO.HasValue ?
                new ObjectParameter("CODFUNCIONARIO", cODFUNCIONARIO) :
                new ObjectParameter("CODFUNCIONARIO", typeof(decimal));

            var mENSAJEParameter = mENSAJE != null ?
                new ObjectParameter("MENSAJE", mENSAJE) :
                new ObjectParameter("MENSAJE", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesTramitesOracle.SP_NUEVO_TRAMITE_REGISTRO", cODPROCESOParameter, cODFUNCIONARIOParameter, mENSAJEParameter, rESPCODTRAMITE, rESPCODTAREA, rTARESULTADO);
        }

        public virtual int SP_NUEVO_TRAMITE(Nullable<decimal> cODPROCESO, Nullable<decimal> cODTAREA, Nullable<decimal> cODFUNCIONARIO, string mENSAJE, ObjectParameter rESPCODTRAMITE, ObjectParameter rESPCODTAREA, ObjectParameter rTARESULTADO)
        {
            var cODPROCESOParameter = cODPROCESO.HasValue ?
                new ObjectParameter("CODPROCESO", cODPROCESO) :
                new ObjectParameter("CODPROCESO", typeof(decimal));

            var cODTAREAParameter = cODTAREA.HasValue ?
                new ObjectParameter("CODTAREA", cODTAREA) :
                new ObjectParameter("CODTAREA", typeof(decimal));

            var cODFUNCIONARIOParameter = cODFUNCIONARIO.HasValue ?
                new ObjectParameter("CODFUNCIONARIO", cODFUNCIONARIO) :
                new ObjectParameter("CODFUNCIONARIO", typeof(decimal));

            var mENSAJEParameter = mENSAJE != null ?
                new ObjectParameter("MENSAJE", mENSAJE) :
                new ObjectParameter("MENSAJE", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesTramitesOracle.SP_NUEVO_TRAMITE", cODPROCESOParameter, cODTAREAParameter, cODFUNCIONARIOParameter, mENSAJEParameter, rESPCODTRAMITE, rESPCODTAREA, rTARESULTADO);
        }

        public virtual int SP_AVANZA_TAREA(Nullable<decimal> tIPO, Nullable<decimal> cODTRAMITE, Nullable<decimal> cODTAREA, Nullable<decimal> cODTAREA_SIGUIENTE, Nullable<decimal> cODFUNCIONARIO, string cOPIAS, string cOMENTARIO, ObjectParameter rTARESULTADO)
        {
            var tIPOParameter = tIPO.HasValue ?
                new ObjectParameter("TIPO", tIPO) :
                new ObjectParameter("TIPO", typeof(decimal));

            var cODTRAMITEParameter = cODTRAMITE.HasValue ?
                new ObjectParameter("CODTRAMITE", cODTRAMITE) :
                new ObjectParameter("CODTRAMITE", typeof(decimal));

            var cODTAREAParameter = cODTAREA.HasValue ?
                new ObjectParameter("CODTAREA", cODTAREA) :
                new ObjectParameter("CODTAREA", typeof(decimal));

            var cODTAREA_SIGUIENTEParameter = cODTAREA_SIGUIENTE.HasValue ?
                new ObjectParameter("CODTAREA_SIGUIENTE", cODTAREA_SIGUIENTE) :
                new ObjectParameter("CODTAREA_SIGUIENTE", typeof(decimal));

            var cODFUNCIONARIOParameter = cODFUNCIONARIO.HasValue ?
                new ObjectParameter("CODFUNCIONARIO", cODFUNCIONARIO) :
                new ObjectParameter("CODFUNCIONARIO", typeof(decimal));

            var cOPIASParameter = cOPIAS != null ?
                new ObjectParameter("COPIAS", cOPIAS) :
                new ObjectParameter("COPIAS", typeof(string));

            var cOMENTARIOParameter = cOMENTARIO != null ?
                new ObjectParameter("COMENTARIO", cOMENTARIO) :
                new ObjectParameter("COMENTARIO", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesTramitesOracle.SP_AVANZA_TAREA", tIPOParameter, cODTRAMITEParameter, cODTAREAParameter, cODTAREA_SIGUIENTEParameter, cODFUNCIONARIOParameter, cOPIASParameter, cOMENTARIOParameter, rTARESULTADO);
        }

        public virtual int SP_ASIGNAR_TEMPORAL_TRAMITE(Nullable<decimal> cODTRAMITE, Nullable<decimal> cODTAREA, Nullable<decimal> cODFUNCIONARIO, Nullable<decimal> oRDEN, Nullable<decimal> vERSIONDOC, string dESCRIPCION, string rUTA, ObjectParameter rTARESULTADO)
        {
            var cODTRAMITEParameter = cODTRAMITE.HasValue ?
                new ObjectParameter("CODTRAMITE", cODTRAMITE) :
                new ObjectParameter("CODTRAMITE", typeof(decimal));

            var cODTAREAParameter = cODTAREA.HasValue ?
                new ObjectParameter("CODTAREA", cODTAREA) :
                new ObjectParameter("CODTAREA", typeof(decimal));

            var cODFUNCIONARIOParameter = cODFUNCIONARIO.HasValue ?
                new ObjectParameter("CODFUNCIONARIO", cODFUNCIONARIO) :
                new ObjectParameter("CODFUNCIONARIO", typeof(decimal));

            var oRDENParameter = oRDEN.HasValue ?
                new ObjectParameter("ORDEN", oRDEN) :
                new ObjectParameter("ORDEN", typeof(decimal));

            var vERSIONDOCParameter = oRDEN.HasValue ?
                new ObjectParameter("VERSIONDOC", vERSIONDOC) :
                new ObjectParameter("VERSIONDOC", typeof(decimal));

            var dESCRIPCIONParameter = dESCRIPCION != null ?
                new ObjectParameter("DESCRIPCION", dESCRIPCION) :
                new ObjectParameter("DESCRIPCION", typeof(string));

            var rUTAParameter = rUTA != null ?
                new ObjectParameter("RUTA", rUTA) :
                new ObjectParameter("RUTA", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesTramitesOracle.SP_ASIGNAR_TEMPORAL_TRAMITE", cODTRAMITEParameter, cODTAREAParameter, cODFUNCIONARIOParameter, oRDENParameter, vERSIONDOCParameter, dESCRIPCIONParameter, rUTAParameter, rTARESULTADO);
        }

        public virtual int SP_OBTENER_PROCESO_TAREA(Nullable<decimal> cODTAREA, ObjectParameter rESPCODPROCESO, ObjectParameter rTARESULTADO)
        {
            var cODTAREAParameter = cODTAREA.HasValue ?
                new ObjectParameter("CODTAREA", cODTAREA) :
                new ObjectParameter("CODTAREA", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesTramitesOracle.SP_OBTENER_PROCESO_TAREA", cODTAREAParameter, rESPCODPROCESO, rTARESULTADO);
        }

        public virtual int SP_AVANZA_TAREA_FORMULARIO(Nullable<decimal> cODTRAMITE, Nullable<decimal> cODTAREA, Nullable<decimal> cODTAREA_SIGUIENTE, Nullable<decimal> cODFUNCIONARIO, string fORMULARIO_SIGUIENTE, string cOPIAS, string cOMENTARIO, ObjectParameter rTARESULTADO)
        {
            var cODTRAMITEParameter = cODTRAMITE.HasValue ?
                new ObjectParameter("CODTRAMITE", cODTRAMITE) :
                new ObjectParameter("CODTRAMITE", typeof(decimal));

            var cODTAREAParameter = cODTAREA.HasValue ?
                new ObjectParameter("CODTAREA", cODTAREA) :
                new ObjectParameter("CODTAREA", typeof(decimal));

            var cODTAREA_SIGUIENTEParameter = cODTAREA_SIGUIENTE.HasValue ?
                new ObjectParameter("CODTAREA_SIGUIENTE", cODTAREA_SIGUIENTE) :
                new ObjectParameter("CODTAREA_SIGUIENTE", typeof(decimal));

            var cODFUNCIONARIOParameter = cODFUNCIONARIO.HasValue ?
                new ObjectParameter("CODFUNCIONARIO", cODFUNCIONARIO) :
                new ObjectParameter("CODFUNCIONARIO", typeof(decimal));

            var fORMULARIO_SIGUIENTEParameter = fORMULARIO_SIGUIENTE != null ?
                new ObjectParameter("FORMULARIO_SIGUIENTE", fORMULARIO_SIGUIENTE) :
                new ObjectParameter("FORMULARIO_SIGUIENTE", typeof(string));

            var cOPIASParameter = cOPIAS != null ?
                new ObjectParameter("COPIAS", cOPIAS) :
                new ObjectParameter("COPIAS", typeof(string));

            var cOMENTARIOParameter = cOMENTARIO != null ?
                new ObjectParameter("COMENTARIO", cOMENTARIO) :
                new ObjectParameter("COMENTARIO", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesProyeccionDocumentoOracle.SP_AVANZA_TAREA_FORMULARIO", cODTRAMITEParameter, cODTAREAParameter, cODTAREA_SIGUIENTEParameter, cODFUNCIONARIOParameter, fORMULARIO_SIGUIENTEParameter, cOPIASParameter, cOMENTARIOParameter, rTARESULTADO);
        }*/
    }
}
