namespace SIM.Data
{
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web;
    using SIM.Data.Control;

    public partial class EntitiesSIMOracle : DbContext
    {
        public virtual DbSet<AYUDAS> AYUDAS_CONTROL { get; set; }
        public virtual DbSet<DOCUMENTO> DOCUMENTO { get; set; }
        public virtual DbSet<DOCUMENTO_ADJUNTO> DOCUMENTO_ADJUNTO { get; set; }
        public virtual DbSet<ENC_ENCUESTA> ENC_ENCUESTA { get; set; }
        public virtual DbSet<ENC_OPCION_RESPUESTA> ENC_OPCION_RESPUESTA { get; set; }
        public virtual DbSet<ENC_OPCION_RESPUESTA_DOM> ENC_OPCION_RESPUESTA_DOM { get; set; }
        public virtual DbSet<ENC_PREGUNTA> ENC_PREGUNTA { get; set; }
        public virtual DbSet<ENC_SOLUCION> ENC_SOLUCION { get; set; }
        public virtual DbSet<ENC_SOLUCION_PREGUNTAS> ENC_SOLUCION_PREGUNTAS { get; set; }
        public virtual DbSet<ENC_SOLUCION_PREGUNTAS_OPC> ENC_SOLUCION_PREGUNTAS_OPC { get; set; }
        public virtual DbSet<ENC_TIPO_PREGUNTA> ENC_TIPO_PREGUNTA { get; set; }
        public virtual DbSet<ENCUESTA_VIGENCIA> ENCUESTA_VIGENCIA { get; set; }
        public virtual DbSet<EST_TIPO_DATO> EST_TIPO_DATO { get; set; }
        public virtual DbSet<ESTADOVISITA> ESTADOVISITA { get; set; }
        public virtual DbSet<EVALUACION_ENCUESTA> EVALUACION_ENCUESTA { get; set; }
        public virtual DbSet<EVALUACION_ENCUESTA_TERCERO> EVALUACION_ENCUESTA_TERCERO { get; set; }
        public virtual DbSet<EVALUACION_PREGUNTA> EVALUACION_PREGUNTA { get; set; }
        public virtual DbSet<EVALUACION_PREGUNTA_COMP> EVALUACION_PREGUNTA_COMP { get; set; }
        public virtual DbSet<EVALUACION_PREGUNTA_GRUPO> EVALUACION_PREGUNTA_GRUPO { get; set; }
        public virtual DbSet<EVALUACION_RESPUESTA> EVALUACION_RESPUESTA { get; set; }
        public virtual DbSet<EVALUACION_RESPUESTA_DETALLE> EVALUACION_RESPUESTA_DETALLE { get; set; }
        public virtual DbSet<EVALUACION_TIPO> EVALUACION_TIPO { get; set; }
        public virtual DbSet<EVALUACION_ESTRATEGIAS> EVALUACION_ESTRATEGIAS { get; set; }
        public virtual DbSet<EVALUACION_ESTRATEGIAS_GRUPO> EVALUACION_ESTRATEGIAS_GRUPO { get; set; }
        public virtual DbSet<EVALUACION_ESTRATEGIAS_T> EVALUACION_ESTRATEGIAS_T { get; set; }
        public virtual DbSet<FORMULARIO> FORMULARIO { get; set; }
        public virtual DbSet<FORMULARIO_ENCUESTA> FORMULARIO_ENCUESTA { get; set; }
        public virtual DbSet<FOTOGRAFIA> FOTOGRAFIA { get; set; }
        public virtual DbSet<FOTOGRAFIA_VISITA> FOTOGRAFIA_VISITA { get; set; }
        public virtual DbSet<FRM_GENERICO_ESTADO> FRM_GENERICO_ESTADO { get; set; }
        public virtual DbSet<FRM_RESIDUOS_FOTOGRAFIA> FRM_RESIDUOS_FOTOGRAFIA { get; set; }
        public virtual DbSet<INFESTADO> INFESTADO { get; set; }
        public virtual DbSet<INFORME_TECNICO> INFORME_TECNICO { get; set; }
        public virtual DbSet<INFORMATIVO_DOC> INFORMATIVO_DOC { get; set; }
        public virtual DbSet<INFORMATIVO_TIPOCOM> INFORMATIVO_TIPOCOM { get; set; }
        public virtual DbSet<INFORMATIVO_DOC_ARCHIVOS> INFORMATIVO_DOC_ARCHIVOS { get; set; }
        public virtual DbSet<INFORMATIVO_DOC_DET_ARCH> INFORMATIVO_DOC_DET_ARCH { get; set; }
        public virtual DbSet<INSTALACION_TIPO> INSTALACION_TIPO { get; set; }
        public virtual DbSet<INSTALACION_VISITA> INSTALACION_VISITA { get; set; }
        public virtual DbSet<MAESTRO_TIPO> MAESTRO_TIPO { get; set; }
        public virtual DbSet<PREGUNTA> PREGUNTA { get; set; }
        public virtual DbSet<RESPUESTA> RESPUESTA { get; set; }
        public virtual DbSet<ROL_VISITA> ROL_VISITA { get; set; }
        public virtual DbSet<TBREPOSICION> TBREPOSICION { get; set; }
        public virtual DbSet<TERCERO_VISITA> TERCERO_VISITA { get; set; }
        public virtual DbSet<TIPO_PREGUNTA> TIPO_PREGUNTA { get; set; }
        public virtual DbSet<TIPO_VISITA> TIPO_VISITA { get; set; }
        public virtual DbSet<TRAMITE_VISITA> TRAMITE_VISITA { get; set; }
        public virtual DbSet<VIGENCIA> VIGENCIA { get; set; }
        public virtual DbSet<VIGENCIA_SOLUCION> VIGENCIA_SOLUCION { get; set; }
        public virtual DbSet<VISITA> VISITA { get; set; }
        public virtual DbSet<VISITA_SOLICITUD> VISITA_SOLICITUD { get; set; }
        public virtual DbSet<VISITA_INFORME> VISITA_INFORME { get; set; }
        public virtual DbSet<VISITAESTADO> VISITAESTADO { get; set; }
        public virtual DbSet<ENC_PREGUNTA_DEPENDENCIA> ENC_PREGUNTA_DEPENDENCIA { get; set; }
        public virtual DbSet<VW_AYUDAS> VW_AYUDAS { get; set; }
        public virtual DbSet<VW_DETALLE_TRAMITE_V> VW_DETALLE_TRAMITE_V { get; set; }
        public virtual DbSet<VW_FOTOGRAFIA_VISITA> VW_FOTOGRAFIA_VISITA { get; set; }
        public virtual DbSet<VW_FUNCIONARIO> VW_FUNCIONARIO { get; set; }
        public virtual DbSet<VW_FUNCIONARIOTAREA> VW_FUNCIONARIOTAREA { get; set; }
        public virtual DbSet<VW_INSTALACION> VW_INSTALACION { get; set; }
        public virtual DbSet<VW_PAG_REPARTO_GEO> VW_PAG_REPARTO_GEO { get; set; }
        public virtual DbSet<VW_PAG_REPARTO_NOGEO> VW_PAG_REPARTO_NOGEO { get; set; }
        public virtual DbSet<VW_PMES_EVALUACION_EMPRESAS> VW_PMES_EVALUACION_EMPRESAS { get; set; }
        public virtual DbSet<VW_PMES_EVALUACION_EMPRESAS_H> VW_PMES_EVALUACION_EMPRESAS_H { get; set; }
        public virtual DbSet<VW_RADICADO_VISITA> VW_RADICADO_VISITA { get; set; }
        public virtual DbSet<VW_REALIZAR_VISITA> VW_REALIZAR_VISITA { get; set; }
        public virtual DbSet<VW_REPARTO_GEO> VW_REPARTO_GEO { get; set; }
        public virtual DbSet<VW_REPARTO_NOGEO> VW_REPARTO_NOGEO { get; set; }
        public virtual DbSet<VW_REPRESENTANTE_LEGAL> VW_REPRESENTANTE_LEGAL { get; set; }
        public virtual DbSet<VW_TERCERO> VW_TERCERO { get; set; }
        public virtual DbSet<VW_TRAMITE_A_VISITAR> VW_TRAMITE_A_VISITAR { get; set; }
        public virtual DbSet<VW_TRAMITE_INFORME> VW_TRAMITE_INFORME { get; set; }
        public virtual DbSet<VW_TRAMITE_TODOS> VW_TRAMITE_TODOS { get; set; }
        public virtual DbSet<VW_TRAMITE_VISITA> VW_TRAMITE_VISITA { get; set; }
        public virtual DbSet<VW_TRAMITES_VISITAS> VW_TRAMITES_VISITAS { get; set; }
        public virtual DbSet<VW_VISITAS> VW_VISITAS { get; set; }
        public virtual DbSet<VWM_PMES> VWM_PMES { get; set; }
        public virtual DbSet<TBDETALLE_REPOSICION> TBDETALLE_REPOSICION { get; set; }
        public virtual DbSet<TBDERECHOS_PETICION> TBDERECHOS_PETICION { get; set; }
        public virtual DbSet<TBTRAMITES_SEGUIMIENTO> TBTRAMITES_SEGUIMIENTO { get; set; }
        public virtual DbSet<TIPO_MEDIDAADICIONAL> TIPO_MEDIDAADICIONAL { get; set; }
        public virtual DbSet<TBSEGUIMIENTO_TRAMITE_NUEVO> TBSEGUIMIENTO_TRAMITE_NUEVO { get; set; }
        public virtual DbSet<INFORMACION_INDUSTRIA> INFORMACION_INDUSTRIA { get; set; }
        public virtual DbSet<V_REPOCISIONES> V_REPOCISIONES { get; set; }
        public virtual DbSet<V_SEGUIMIENTO_TRAMITES_NUEVOS> V_SEGUIMIENTO_TRAMITES_NUEVOS { get; set; }
        public virtual DbSet<PMES_ESTRATEGIAS> PMES_ESTRATEGIAS { get; set; }
        public virtual DbSet<PMES_ESTRATEGIAS_ACTIVIDADES> PMES_ESTRATEGIAS_ACTIVIDADES { get; set; }
        public virtual DbSet<PMES_ESTRATEGIAS_ENCABEZADO> PMES_ESTRATEGIAS_ENCABEZADO { get; set; }
        public virtual DbSet<PMES_ESTRATEGIAS_GRUPO> PMES_ESTRATEGIAS_GRUPO { get; set; }
        public virtual DbSet<PMES_ESTRATEGIAS_METAS> PMES_ESTRATEGIAS_METAS { get; set; }
        public virtual DbSet<PMES_ESTRATEGIAS_METAS_T> PMES_ESTRATEGIAS_METAS_T { get; set; }
        public virtual DbSet<PMES_ESTRATEGIAS_PERIODICIDAD> PMES_ESTRATEGIAS_PERIODICIDAD { get; set; }
        public virtual DbSet<PMES_ESTRATEGIAS_PREGUNTA> PMES_ESTRATEGIAS_PREGUNTA { get; set; }
        public virtual DbSet<PMES_ESTRATEGIAS_PREGUNTA_GRUPO> PMES_ESTRATEGIAS_PREGUNTA_GRUPO { get; set; }
        public virtual DbSet<PMES_ESTRATEGIAS_RESPUESTA> PMES_ESTRATEGIAS_RESPUESTA { get; set; }
        public virtual DbSet<PMES_ESTRATEGIAS_TERCERO> PMES_ESTRATEGIAS_TERCERO { get; set; }
        public virtual DbSet<PMES_ESTRATEGIAS_TF> PMES_ESTRATEGIAS_TF { get; set; }
        public virtual DbSet<PMES_ESTRATEGIAS_TIPOEVIDENCIA> PMES_ESTRATEGIAS_TIPOEVIDENCIA { get; set; }
        public virtual DbSet<PMES_ESTRATEGIAS_TP> PMES_ESTRATEGIAS_TP { get; set; }

        /*
        public virtual int SP_CREATE_ITEM(Nullable<decimal> iDTERCERO, Nullable<decimal> iDINSTALACION, Nullable<decimal> iDFORM, string nOMBRE, ObjectParameter rESPIDITEM, ObjectParameter rESPMSG)
        {
            var iDTERCEROParameter = iDTERCERO.HasValue ?
                new ObjectParameter("IDTERCERO", iDTERCERO) :
                new ObjectParameter("IDTERCERO", typeof(decimal));

            var iDINSTALACIONParameter = iDINSTALACION.HasValue ?
                new ObjectParameter("IDINSTALACION", iDINSTALACION) :
                new ObjectParameter("IDINSTALACION", typeof(decimal));

            var iDFORMParameter = iDFORM.HasValue ?
                new ObjectParameter("IDFORM", iDFORM) :
                new ObjectParameter("IDFORM", typeof(decimal));

            var nOMBREParameter = nOMBRE != null ?
                new ObjectParameter("NOMBRE", nOMBRE) :
                new ObjectParameter("NOMBRE", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_CREATE_ITEM", iDTERCEROParameter, iDINSTALACIONParameter, iDFORMParameter, nOMBREParameter, rESPIDITEM, rESPMSG);
        }

        public virtual int SP_CREATE_ITEM_ESTADO(Nullable<decimal> iDFORM, Nullable<decimal> iDACTUACION, Nullable<decimal> iDITEM, ObjectParameter rESPIDITEMESTADO, ObjectParameter rESPMSG)
        {
            var iDFORMParameter = iDFORM.HasValue ?
                new ObjectParameter("IDFORM", iDFORM) :
                new ObjectParameter("IDFORM", typeof(decimal));

            var iDACTUACIONParameter = iDACTUACION.HasValue ?
                new ObjectParameter("IDACTUACION", iDACTUACION) :
                new ObjectParameter("IDACTUACION", typeof(decimal));

            var iDITEMParameter = iDITEM.HasValue ?
                new ObjectParameter("IDITEM", iDITEM) :
                new ObjectParameter("IDITEM", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_CREATE_ITEM_ESTADO", iDFORMParameter, iDACTUACIONParameter, iDITEMParameter, rESPIDITEMESTADO, rESPMSG);
        }

        public virtual int SP_ADD_COPIA_TAREA_TRAMITE(Nullable<decimal> iD_TRAMITE, Nullable<decimal> iD_TAREA, string cOPIAS)
        {
            var iD_TRAMITEParameter = iD_TRAMITE.HasValue ?
                new ObjectParameter("ID_TRAMITE", iD_TRAMITE) :
                new ObjectParameter("ID_TRAMITE", typeof(decimal));

            var iD_TAREAParameter = iD_TAREA.HasValue ?
                new ObjectParameter("ID_TAREA", iD_TAREA) :
                new ObjectParameter("ID_TAREA", typeof(decimal));

            var cOPIASParameter = cOPIAS != null ?
                new ObjectParameter("COPIAS", cOPIAS) :
                new ObjectParameter("COPIAS", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_ADD_COPIA_TAREA_TRAMITE", iD_TRAMITEParameter, iD_TAREAParameter, cOPIASParameter);
        }

        public virtual int SP_ADD_TRAMITE_INFORME(string tRAMITES, Nullable<decimal> uSUARIO, string cOPIAS, Nullable<decimal> rESPONSABLE, Nullable<decimal> cODTAREA, Nullable<decimal> tAREASIG)
        {
            var tRAMITESParameter = tRAMITES != null ?
                new ObjectParameter("TRAMITES", tRAMITES) :
                new ObjectParameter("TRAMITES", typeof(string));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var cOPIASParameter = cOPIAS != null ?
                new ObjectParameter("COPIAS", cOPIAS) :
                new ObjectParameter("COPIAS", typeof(string));

            var rESPONSABLEParameter = rESPONSABLE.HasValue ?
                new ObjectParameter("RESPONSABLE", rESPONSABLE) :
                new ObjectParameter("RESPONSABLE", typeof(decimal));

            var cODTAREAParameter = cODTAREA.HasValue ?
                new ObjectParameter("CODTAREA", cODTAREA) :
                new ObjectParameter("CODTAREA", typeof(decimal));

            var tAREASIGParameter = tAREASIG.HasValue ?
                new ObjectParameter("TAREASIG", tAREASIG) :
                new ObjectParameter("TAREASIG", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_ADD_TRAMITE_INFORME", tRAMITESParameter, uSUARIOParameter, cOPIASParameter, rESPONSABLEParameter, cODTAREAParameter, tAREASIGParameter);
        }

        public virtual int SP_ASOCIARTRAMITE_VISITA(string tRAMITES, Nullable<decimal> iD_VISITA)
        {
            var tRAMITESParameter = tRAMITES != null ?
                new ObjectParameter("TRAMITES", tRAMITES) :
                new ObjectParameter("TRAMITES", typeof(string));

            var iD_VISITAParameter = iD_VISITA.HasValue ?
                new ObjectParameter("ID_VISITA", iD_VISITA) :
                new ObjectParameter("ID_VISITA", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_ASOCIARTRAMITE_VISITA", tRAMITESParameter, iD_VISITAParameter);
        }

        public virtual int SP_AVANZA_TAREA_TRAMITE(Nullable<decimal> iD_TRAMITE, Nullable<decimal> uSUARIO, Nullable<decimal> iD_TAREA, string cOMENTARIO, Nullable<decimal> oRDEN, Nullable<decimal> iD_RESPONSABLE, string cOPIAS, Nullable<decimal> iD_TAREA_SIGUIENTE)
        {
            var iD_TRAMITEParameter = iD_TRAMITE.HasValue ?
                new ObjectParameter("ID_TRAMITE", iD_TRAMITE) :
                new ObjectParameter("ID_TRAMITE", typeof(decimal));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var iD_TAREAParameter = iD_TAREA.HasValue ?
                new ObjectParameter("ID_TAREA", iD_TAREA) :
                new ObjectParameter("ID_TAREA", typeof(decimal));

            var cOMENTARIOParameter = cOMENTARIO != null ?
                new ObjectParameter("COMENTARIO", cOMENTARIO) :
                new ObjectParameter("COMENTARIO", typeof(string));

            var oRDENParameter = oRDEN.HasValue ?
                new ObjectParameter("ORDEN", oRDEN) :
                new ObjectParameter("ORDEN", typeof(decimal));

            var iD_RESPONSABLEParameter = iD_RESPONSABLE.HasValue ?
                new ObjectParameter("ID_RESPONSABLE", iD_RESPONSABLE) :
                new ObjectParameter("ID_RESPONSABLE", typeof(decimal));

            var cOPIASParameter = cOPIAS != null ?
                new ObjectParameter("COPIAS", cOPIAS) :
                new ObjectParameter("COPIAS", typeof(string));

            var iD_TAREA_SIGUIENTEParameter = iD_TAREA_SIGUIENTE.HasValue ?
                new ObjectParameter("ID_TAREA_SIGUIENTE", iD_TAREA_SIGUIENTE) :
                new ObjectParameter("ID_TAREA_SIGUIENTE", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_AVANZA_TAREA_TRAMITE", iD_TRAMITEParameter, uSUARIOParameter, iD_TAREAParameter, cOMENTARIOParameter, oRDENParameter, iD_RESPONSABLEParameter, cOPIASParameter, iD_TAREA_SIGUIENTEParameter);
        }

        public virtual int SP_CERRAR_VISITA(Nullable<decimal> uSUARIO, Nullable<decimal> iD_VISITA_, string rADICADO_CONSTANCIA_VISITA)
        {
            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var iD_VISITA_Parameter = iD_VISITA_.HasValue ?
                new ObjectParameter("ID_VISITA_", iD_VISITA_) :
                new ObjectParameter("ID_VISITA_", typeof(decimal));

            var rADICADO_CONSTANCIA_VISITAParameter = rADICADO_CONSTANCIA_VISITA != null ?
                new ObjectParameter("RADICADO_CONSTANCIA_VISITA", rADICADO_CONSTANCIA_VISITA) :
                new ObjectParameter("RADICADO_CONSTANCIA_VISITA", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_CERRAR_VISITA", uSUARIOParameter, iD_VISITA_Parameter, rADICADO_CONSTANCIA_VISITAParameter);
        }

        public virtual int SP_CONSULTAR_INFORME(Nullable<decimal> iD_INFORME_ACTUAL, ObjectParameter aNTECEDENTES, ObjectParameter aNALISIS, ObjectParameter cONCLUCIONES, ObjectParameter rECOMENDACIONES, ObjectParameter cODIGO)
        {
            var iD_INFORME_ACTUALParameter = iD_INFORME_ACTUAL.HasValue ?
                new ObjectParameter("ID_INFORME_ACTUAL", iD_INFORME_ACTUAL) :
                new ObjectParameter("ID_INFORME_ACTUAL", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_CONSULTAR_INFORME", iD_INFORME_ACTUALParameter, aNTECEDENTES, aNALISIS, cONCLUCIONES, rECOMENDACIONES, cODIGO);
        }

        public virtual int SP_CREATE_ESTADO_ITEM(Nullable<decimal> iDTERCERO, Nullable<decimal> iDINSTALACION, Nullable<decimal> iDFORM, Nullable<decimal> iDITEM, Nullable<decimal> iD_TIPOESTADO, Nullable<decimal> iD_VIS, ObjectParameter rESPIDESTADO)
        {
            var iDTERCEROParameter = iDTERCERO.HasValue ?
                new ObjectParameter("IDTERCERO", iDTERCERO) :
                new ObjectParameter("IDTERCERO", typeof(decimal));

            var iDINSTALACIONParameter = iDINSTALACION.HasValue ?
                new ObjectParameter("IDINSTALACION", iDINSTALACION) :
                new ObjectParameter("IDINSTALACION", typeof(decimal));

            var iDFORMParameter = iDFORM.HasValue ?
                new ObjectParameter("IDFORM", iDFORM) :
                new ObjectParameter("IDFORM", typeof(decimal));

            var iDITEMParameter = iDITEM.HasValue ?
                new ObjectParameter("IDITEM", iDITEM) :
                new ObjectParameter("IDITEM", typeof(decimal));

            var iD_TIPOESTADOParameter = iD_TIPOESTADO.HasValue ?
                new ObjectParameter("ID_TIPOESTADO", iD_TIPOESTADO) :
                new ObjectParameter("ID_TIPOESTADO", typeof(decimal));

            var iD_VISParameter = iD_VIS.HasValue ?
                new ObjectParameter("ID_VIS", iD_VIS) :
                new ObjectParameter("ID_VIS", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_CREATE_ESTADO_ITEM", iDTERCEROParameter, iDINSTALACIONParameter, iDFORMParameter, iDITEMParameter, iD_TIPOESTADOParameter, iD_VISParameter, rESPIDESTADO);
        }

        public virtual int SP_ELIMINAR_DOCUMENTO_ADJUNTO(Nullable<decimal> iD_DOC, Nullable<decimal> iDFORM)
        {
            var iD_DOCParameter = iD_DOC.HasValue ?
                new ObjectParameter("ID_DOC", iD_DOC) :
                new ObjectParameter("ID_DOC", typeof(decimal));

            var iDFORMParameter = iDFORM.HasValue ?
                new ObjectParameter("IDFORM", iDFORM) :
                new ObjectParameter("IDFORM", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_ELIMINAR_DOCUMENTO_ADJUNTO", iD_DOCParameter, iDFORMParameter);
        }

        public virtual int SP_ELIMINAR_ENCUESTA(Nullable<decimal> iD_ENC)
        {
            var iD_ENCParameter = iD_ENC.HasValue ?
                new ObjectParameter("ID_ENC", iD_ENC) :
                new ObjectParameter("ID_ENC", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_ELIMINAR_ENCUESTA", iD_ENCParameter);
        }

        public virtual int SP_ELIMINAR_FOTO_FLORA(Nullable<decimal> iD_FOTO)
        {
            var iD_FOTOParameter = iD_FOTO.HasValue ?
                new ObjectParameter("ID_FOTO", iD_FOTO) :
                new ObjectParameter("ID_FOTO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_ELIMINAR_FOTO_FLORA", iD_FOTOParameter);
        }

        public virtual int SP_ELIMINAR_FOTO_FORM(Nullable<decimal> iD_FOTO, string tABLA)
        {
            var iD_FOTOParameter = iD_FOTO.HasValue ?
                new ObjectParameter("ID_FOTO", iD_FOTO) :
                new ObjectParameter("ID_FOTO", typeof(decimal));

            var tABLAParameter = tABLA != null ?
                new ObjectParameter("TABLA", tABLA) :
                new ObjectParameter("TABLA", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_ELIMINAR_FOTO_FORM", iD_FOTOParameter, tABLAParameter);
        }

        public virtual int SP_ELIMINAR_FOTO_RESIDUOS(Nullable<decimal> iD_FOTO)
        {
            var iD_FOTOParameter = iD_FOTO.HasValue ?
                new ObjectParameter("ID_FOTO", iD_FOTO) :
                new ObjectParameter("ID_FOTO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_ELIMINAR_FOTO_RESIDUOS", iD_FOTOParameter);
        }

        public virtual int SP_ELIMINAR_FOTO_VISITA(Nullable<decimal> iD_FOTO)
        {
            var iD_FOTOParameter = iD_FOTO.HasValue ?
                new ObjectParameter("ID_FOTO", iD_FOTO) :
                new ObjectParameter("ID_FOTO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_ELIMINAR_FOTO_VISITA", iD_FOTOParameter);
        }

        public virtual int SP_ELIMINAR_PREGUNTA(Nullable<decimal> iD_PREG)
        {
            var iD_PREGParameter = iD_PREG.HasValue ?
                new ObjectParameter("ID_PREG", iD_PREG) :
                new ObjectParameter("ID_PREG", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_ELIMINAR_PREGUNTA", iD_PREGParameter);
        }

        public virtual int SP_ELIMINAR_PREGUNTA_VINCULADA(Nullable<decimal> iD_ENC, Nullable<decimal> iD_PREG)
        {
            var iD_ENCParameter = iD_ENC.HasValue ?
                new ObjectParameter("ID_ENC", iD_ENC) :
                new ObjectParameter("ID_ENC", typeof(decimal));

            var iD_PREGParameter = iD_PREG.HasValue ?
                new ObjectParameter("ID_PREG", iD_PREG) :
                new ObjectParameter("ID_PREG", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_ELIMINAR_PREGUNTA_VINCULADA", iD_ENCParameter, iD_PREGParameter);
        }

        public virtual int SP_ELIMINAR_RESPUESTA(Nullable<decimal> iD_RESP)
        {
            var iD_RESPParameter = iD_RESP.HasValue ?
                new ObjectParameter("ID_RESP", iD_RESP) :
                new ObjectParameter("ID_RESP", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_ELIMINAR_RESPUESTA", iD_RESPParameter);
        }

        public virtual int SP_END_VISITA(Nullable<decimal> uSUARIO, string oBSERVACIONES, string cOMENTARIO, string cOPIAS, Nullable<decimal> iD_VISITA_ACTUAL)
        {
            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var oBSERVACIONESParameter = oBSERVACIONES != null ?
                new ObjectParameter("OBSERVACIONES", oBSERVACIONES) :
                new ObjectParameter("OBSERVACIONES", typeof(string));

            var cOMENTARIOParameter = cOMENTARIO != null ?
                new ObjectParameter("COMENTARIO", cOMENTARIO) :
                new ObjectParameter("COMENTARIO", typeof(string));

            var cOPIASParameter = cOPIAS != null ?
                new ObjectParameter("COPIAS", cOPIAS) :
                new ObjectParameter("COPIAS", typeof(string));

            var iD_VISITA_ACTUALParameter = iD_VISITA_ACTUAL.HasValue ?
                new ObjectParameter("ID_VISITA_ACTUAL", iD_VISITA_ACTUAL) :
                new ObjectParameter("ID_VISITA_ACTUAL", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_END_VISITA", uSUARIOParameter, oBSERVACIONESParameter, cOMENTARIOParameter, cOPIASParameter, iD_VISITA_ACTUALParameter);
        }

        public virtual int SP_FIN_VISITA(Nullable<decimal> uSUARIO, string rADICADO, string pDF, Nullable<decimal> iD_VISITA_, string cOMENTARIO, Nullable<decimal> rESPONSABLE, string cOPIAS)
        {
            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var rADICADOParameter = rADICADO != null ?
                new ObjectParameter("RADICADO", rADICADO) :
                new ObjectParameter("RADICADO", typeof(string));

            var pDFParameter = pDF != null ?
                new ObjectParameter("PDF", pDF) :
                new ObjectParameter("PDF", typeof(string));

            var iD_VISITA_Parameter = iD_VISITA_.HasValue ?
                new ObjectParameter("ID_VISITA_", iD_VISITA_) :
                new ObjectParameter("ID_VISITA_", typeof(decimal));

            var cOMENTARIOParameter = cOMENTARIO != null ?
                new ObjectParameter("COMENTARIO", cOMENTARIO) :
                new ObjectParameter("COMENTARIO", typeof(string));

            var rESPONSABLEParameter = rESPONSABLE.HasValue ?
                new ObjectParameter("RESPONSABLE", rESPONSABLE) :
                new ObjectParameter("RESPONSABLE", typeof(decimal));

            var cOPIASParameter = cOPIAS != null ?
                new ObjectParameter("COPIAS", cOPIAS) :
                new ObjectParameter("COPIAS", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_FIN_VISITA", uSUARIOParameter, rADICADOParameter, pDFParameter, iD_VISITA_Parameter, cOMENTARIOParameter, rESPONSABLEParameter, cOPIASParameter);
        }

        public virtual int SP_GET_CARACTERISTICAS(Nullable<decimal> iDESTADO, string tBLESTADO, Nullable<decimal> iD_FORMULARIO, Nullable<decimal> gRUPO_FORM, Nullable<decimal> iD_CARACT_PADRE, ObjectParameter jSONOUT)
        {
            var iDESTADOParameter = iDESTADO.HasValue ?
                new ObjectParameter("IDESTADO", iDESTADO) :
                new ObjectParameter("IDESTADO", typeof(decimal));

            var tBLESTADOParameter = tBLESTADO != null ?
                new ObjectParameter("TBLESTADO", tBLESTADO) :
                new ObjectParameter("TBLESTADO", typeof(string));

            var iD_FORMULARIOParameter = iD_FORMULARIO.HasValue ?
                new ObjectParameter("ID_FORMULARIO", iD_FORMULARIO) :
                new ObjectParameter("ID_FORMULARIO", typeof(decimal));

            var gRUPO_FORMParameter = gRUPO_FORM.HasValue ?
                new ObjectParameter("GRUPO_FORM", gRUPO_FORM) :
                new ObjectParameter("GRUPO_FORM", typeof(decimal));

            var iD_CARACT_PADREParameter = iD_CARACT_PADRE.HasValue ?
                new ObjectParameter("ID_CARACT_PADRE", iD_CARACT_PADRE) :
                new ObjectParameter("ID_CARACT_PADRE", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GET_CARACTERISTICAS", iDESTADOParameter, tBLESTADOParameter, iD_FORMULARIOParameter, gRUPO_FORMParameter, iD_CARACT_PADREParameter, jSONOUT);
        }

        public virtual int SP_GET_CONSULTARFOTOSFORM(string tABLA, ObjectParameter jSONOUT)
        {
            var tABLAParameter = tABLA != null ?
                new ObjectParameter("TABLA", tABLA) :
                new ObjectParameter("TABLA", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GET_CONSULTARFOTOSFORM", tABLAParameter, jSONOUT);
        }

        public virtual int SP_GET_DATOS(string sTR_SQL, ObjectParameter jSONOUT)
        {
            var sTR_SQLParameter = sTR_SQL != null ?
                new ObjectParameter("STR_SQL", sTR_SQL) :
                new ObjectParameter("STR_SQL", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GET_DATOS", sTR_SQLParameter, jSONOUT);
        }

        public virtual int SP_GET_ENCUESTAS(Nullable<decimal> iDESTADO, Nullable<decimal> iD_FORM, ObjectParameter jSONOUT)
        {
            var iDESTADOParameter = iDESTADO.HasValue ?
                new ObjectParameter("IDESTADO", iDESTADO) :
                new ObjectParameter("IDESTADO", typeof(decimal));

            var iD_FORMParameter = iD_FORM.HasValue ?
                new ObjectParameter("ID_FORM", iD_FORM) :
                new ObjectParameter("ID_FORM", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GET_ENCUESTAS", iDESTADOParameter, iD_FORMParameter, jSONOUT);
        }

        public virtual int SP_GET_INFO_INDUSTRIA(Nullable<decimal> iDINSTALACION, Nullable<decimal> iDTERCERO, Nullable<decimal> iDVISITA, ObjectParameter jSONOUT)
        {
            var iDINSTALACIONParameter = iDINSTALACION.HasValue ?
                new ObjectParameter("IDINSTALACION", iDINSTALACION) :
                new ObjectParameter("IDINSTALACION", typeof(decimal));

            var iDTERCEROParameter = iDTERCERO.HasValue ?
                new ObjectParameter("IDTERCERO", iDTERCERO) :
                new ObjectParameter("IDTERCERO", typeof(decimal));

            var iDVISITAParameter = iDVISITA.HasValue ?
                new ObjectParameter("IDVISITA", iDVISITA) :
                new ObjectParameter("IDVISITA", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GET_INFO_INDUSTRIA", iDINSTALACIONParameter, iDTERCEROParameter, iDVISITAParameter, jSONOUT);
        }

        public virtual int SP_GET_ITEM(Nullable<decimal> iDFORM, Nullable<decimal> iDITEM, ObjectParameter jSONOUT)
        {
            var iDFORMParameter = iDFORM.HasValue ?
                new ObjectParameter("IDFORM", iDFORM) :
                new ObjectParameter("IDFORM", typeof(decimal));

            var iDITEMParameter = iDITEM.HasValue ?
                new ObjectParameter("IDITEM", iDITEM) :
                new ObjectParameter("IDITEM", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GET_ITEM", iDFORMParameter, iDITEMParameter, jSONOUT);
        }

        public virtual int SP_GET_ITEM_L(Nullable<decimal> iDFORM, Nullable<decimal> iDITEM, ObjectParameter jSONOUT, ObjectParameter jSONOUTXY)
        {
            var iDFORMParameter = iDFORM.HasValue ?
                new ObjectParameter("IDFORM", iDFORM) :
                new ObjectParameter("IDFORM", typeof(decimal));

            var iDITEMParameter = iDITEM.HasValue ?
                new ObjectParameter("IDITEM", iDITEM) :
                new ObjectParameter("IDITEM", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GET_ITEM_L", iDFORMParameter, iDITEMParameter, jSONOUT, jSONOUTXY);
        }

        public virtual int SP_GUARDAR_VISITA(string aSUNTO, Nullable<decimal> cX, Nullable<decimal> cY, string tIPOUBICACION, Nullable<decimal> uSUARIO, Nullable<decimal> iD_VISITA1, Nullable<decimal> iD_TERCERO_EMPRESA, Nullable<decimal> iD_INSTALACION, string aTIENDES)
        {
            var aSUNTOParameter = aSUNTO != null ?
                new ObjectParameter("ASUNTO", aSUNTO) :
                new ObjectParameter("ASUNTO", typeof(string));

            var cXParameter = cX.HasValue ?
                new ObjectParameter("CX", cX) :
                new ObjectParameter("CX", typeof(decimal));

            var cYParameter = cY.HasValue ?
                new ObjectParameter("CY", cY) :
                new ObjectParameter("CY", typeof(decimal));

            var tIPOUBICACIONParameter = tIPOUBICACION != null ?
                new ObjectParameter("TIPOUBICACION", tIPOUBICACION) :
                new ObjectParameter("TIPOUBICACION", typeof(string));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var iD_VISITA1Parameter = iD_VISITA1.HasValue ?
                new ObjectParameter("ID_VISITA1", iD_VISITA1) :
                new ObjectParameter("ID_VISITA1", typeof(decimal));

            var iD_TERCERO_EMPRESAParameter = iD_TERCERO_EMPRESA.HasValue ?
                new ObjectParameter("ID_TERCERO_EMPRESA", iD_TERCERO_EMPRESA) :
                new ObjectParameter("ID_TERCERO_EMPRESA", typeof(decimal));

            var iD_INSTALACIONParameter = iD_INSTALACION.HasValue ?
                new ObjectParameter("ID_INSTALACION", iD_INSTALACION) :
                new ObjectParameter("ID_INSTALACION", typeof(decimal));

            var aTIENDESParameter = aTIENDES != null ?
                new ObjectParameter("ATIENDES", aTIENDES) :
                new ObjectParameter("ATIENDES", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GUARDAR_VISITA", aSUNTOParameter, cXParameter, cYParameter, tIPOUBICACIONParameter, uSUARIOParameter, iD_VISITA1Parameter, iD_TERCERO_EMPRESAParameter, iD_INSTALACIONParameter, aTIENDESParameter);
        }

        public virtual int SP_INIT_VISITA(Nullable<decimal> uSUARIO, Nullable<decimal> iD_VISITA_)
        {
            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var iD_VISITA_Parameter = iD_VISITA_.HasValue ?
                new ObjectParameter("ID_VISITA_", iD_VISITA_) :
                new ObjectParameter("ID_VISITA_", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_INIT_VISITA", uSUARIOParameter, iD_VISITA_Parameter);
        }

        public virtual int SP_NEW_INFORME(string aNTECEDENTES, string aNALISIS, string cONCLUCIONES, string rECOMENDACIONES, Nullable<decimal> iD_VISITA, string cODIGO, Nullable<decimal> iD_INFORME_ACTUAL, Nullable<decimal> uSUARIO)
        {
            var aNTECEDENTESParameter = aNTECEDENTES != null ?
                new ObjectParameter("ANTECEDENTES", aNTECEDENTES) :
                new ObjectParameter("ANTECEDENTES", typeof(string));

            var aNALISISParameter = aNALISIS != null ?
                new ObjectParameter("ANALISIS", aNALISIS) :
                new ObjectParameter("ANALISIS", typeof(string));

            var cONCLUCIONESParameter = cONCLUCIONES != null ?
                new ObjectParameter("CONCLUCIONES", cONCLUCIONES) :
                new ObjectParameter("CONCLUCIONES", typeof(string));

            var rECOMENDACIONESParameter = rECOMENDACIONES != null ?
                new ObjectParameter("RECOMENDACIONES", rECOMENDACIONES) :
                new ObjectParameter("RECOMENDACIONES", typeof(string));

            var iD_VISITAParameter = iD_VISITA.HasValue ?
                new ObjectParameter("ID_VISITA", iD_VISITA) :
                new ObjectParameter("ID_VISITA", typeof(decimal));

            var cODIGOParameter = cODIGO != null ?
                new ObjectParameter("CODIGO", cODIGO) :
                new ObjectParameter("CODIGO", typeof(string));

            var iD_INFORME_ACTUALParameter = iD_INFORME_ACTUAL.HasValue ?
                new ObjectParameter("ID_INFORME_ACTUAL", iD_INFORME_ACTUAL) :
                new ObjectParameter("ID_INFORME_ACTUAL", typeof(decimal));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_NEW_INFORME", aNTECEDENTESParameter, aNALISISParameter, cONCLUCIONESParameter, rECOMENDACIONESParameter, iD_VISITAParameter, cODIGOParameter, iD_INFORME_ACTUALParameter, uSUARIOParameter);
        }

        public virtual int SP_NEW_P_TRAMITE(Nullable<decimal> iD_V, Nullable<decimal> x, Nullable<decimal> y, Nullable<decimal> uSUARIO, string tIPOUBICACION)
        {
            var iD_VParameter = iD_V.HasValue ?
                new ObjectParameter("ID_V", iD_V) :
                new ObjectParameter("ID_V", typeof(decimal));

            var xParameter = x.HasValue ?
                new ObjectParameter("X", x) :
                new ObjectParameter("X", typeof(decimal));

            var yParameter = y.HasValue ?
                new ObjectParameter("Y", y) :
                new ObjectParameter("Y", typeof(decimal));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var tIPOUBICACIONParameter = tIPOUBICACION != null ?
                new ObjectParameter("TIPOUBICACION", tIPOUBICACION) :
                new ObjectParameter("TIPOUBICACION", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_NEW_P_TRAMITE", iD_VParameter, xParameter, yParameter, uSUARIOParameter, tIPOUBICACIONParameter);
        }

        public virtual int SP_NEW_P_TRAMITE1(Nullable<decimal> iD_V, Nullable<decimal> x, Nullable<decimal> y, Nullable<decimal> uSUARIO, string tIPOUBICACION)
        {
            var iD_VParameter = iD_V.HasValue ?
                new ObjectParameter("ID_V", iD_V) :
                new ObjectParameter("ID_V", typeof(decimal));

            var xParameter = x.HasValue ?
                new ObjectParameter("X", x) :
                new ObjectParameter("X", typeof(decimal));

            var yParameter = y.HasValue ?
                new ObjectParameter("Y", y) :
                new ObjectParameter("Y", typeof(decimal));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var tIPOUBICACIONParameter = tIPOUBICACION != null ?
                new ObjectParameter("TIPOUBICACION", tIPOUBICACION) :
                new ObjectParameter("TIPOUBICACION", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_NEW_P_TRAMITE1", iD_VParameter, xParameter, yParameter, uSUARIOParameter, tIPOUBICACIONParameter);
        }

        public virtual int SP_NEW_P_TRAMITE2(Nullable<decimal> iD_V, Nullable<decimal> x, Nullable<decimal> y, Nullable<decimal> uSUARIO, string tIPOUBICACION)
        {
            var iD_VParameter = iD_V.HasValue ?
                new ObjectParameter("ID_V", iD_V) :
                new ObjectParameter("ID_V", typeof(decimal));

            var xParameter = x.HasValue ?
                new ObjectParameter("X", x) :
                new ObjectParameter("X", typeof(decimal));

            var yParameter = y.HasValue ?
                new ObjectParameter("Y", y) :
                new ObjectParameter("Y", typeof(decimal));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var tIPOUBICACIONParameter = tIPOUBICACION != null ?
                new ObjectParameter("TIPOUBICACION", tIPOUBICACION) :
                new ObjectParameter("TIPOUBICACION", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_NEW_P_TRAMITE2", iD_VParameter, xParameter, yParameter, uSUARIOParameter, tIPOUBICACIONParameter);
        }

        public virtual int SP_NEW_P_VISITA(Nullable<decimal> iD_V, Nullable<decimal> x, Nullable<decimal> y, Nullable<decimal> uSUARIO, string tIPOUBICACION)
        {
            var iD_VParameter = iD_V.HasValue ?
                new ObjectParameter("ID_V", iD_V) :
                new ObjectParameter("ID_V", typeof(decimal));

            var xParameter = x.HasValue ?
                new ObjectParameter("X", x) :
                new ObjectParameter("X", typeof(decimal));

            var yParameter = y.HasValue ?
                new ObjectParameter("Y", y) :
                new ObjectParameter("Y", typeof(decimal));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var tIPOUBICACIONParameter = tIPOUBICACION != null ?
                new ObjectParameter("TIPOUBICACION", tIPOUBICACION) :
                new ObjectParameter("TIPOUBICACION", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_NEW_P_VISITA", iD_VParameter, xParameter, yParameter, uSUARIOParameter, tIPOUBICACIONParameter);
        }

        public virtual int SP_NEW_VISITA(string aSUNTO, Nullable<decimal> cX, Nullable<decimal> cY, string tIPOUBICACION, string tRAMITES, Nullable<decimal> uSUARIO, string cOMENTARIO, string cOPIAS, Nullable<decimal> tIPOVISITA, Nullable<decimal> iD_VISITA_ACTUAL)
        {
            var aSUNTOParameter = aSUNTO != null ?
                new ObjectParameter("ASUNTO", aSUNTO) :
                new ObjectParameter("ASUNTO", typeof(string));

            var cXParameter = cX.HasValue ?
                new ObjectParameter("CX", cX) :
                new ObjectParameter("CX", typeof(decimal));

            var cYParameter = cY.HasValue ?
                new ObjectParameter("CY", cY) :
                new ObjectParameter("CY", typeof(decimal));

            var tIPOUBICACIONParameter = tIPOUBICACION != null ?
                new ObjectParameter("TIPOUBICACION", tIPOUBICACION) :
                new ObjectParameter("TIPOUBICACION", typeof(string));

            var tRAMITESParameter = tRAMITES != null ?
                new ObjectParameter("TRAMITES", tRAMITES) :
                new ObjectParameter("TRAMITES", typeof(string));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var cOMENTARIOParameter = cOMENTARIO != null ?
                new ObjectParameter("COMENTARIO", cOMENTARIO) :
                new ObjectParameter("COMENTARIO", typeof(string));

            var cOPIASParameter = cOPIAS != null ?
                new ObjectParameter("COPIAS", cOPIAS) :
                new ObjectParameter("COPIAS", typeof(string));

            var tIPOVISITAParameter = tIPOVISITA.HasValue ?
                new ObjectParameter("TIPOVISITA", tIPOVISITA) :
                new ObjectParameter("TIPOVISITA", typeof(decimal));

            var iD_VISITA_ACTUALParameter = iD_VISITA_ACTUAL.HasValue ?
                new ObjectParameter("ID_VISITA_ACTUAL", iD_VISITA_ACTUAL) :
                new ObjectParameter("ID_VISITA_ACTUAL", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_NEW_VISITA", aSUNTOParameter, cXParameter, cYParameter, tIPOUBICACIONParameter, tRAMITESParameter, uSUARIOParameter, cOMENTARIOParameter, cOPIASParameter, tIPOVISITAParameter, iD_VISITA_ACTUALParameter);
        }

        public virtual int SP_OBTENER_CODFUNCIONARIO(Nullable<decimal> p_ID_USUARIO, ObjectParameter cODFUNCIONARIO)
        {
            var p_ID_USUARIOParameter = p_ID_USUARIO.HasValue ?
                new ObjectParameter("P_ID_USUARIO", p_ID_USUARIO) :
                new ObjectParameter("P_ID_USUARIO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_OBTENER_CODFUNCIONARIO", p_ID_USUARIOParameter, cODFUNCIONARIO);
        }

        public virtual int SP_SET_CARACTERISTICAS(Nullable<decimal> iDESTADO, string tBLESTADO, string jSONOUT, ObjectParameter rTA)
        {
            var iDESTADOParameter = iDESTADO.HasValue ?
                new ObjectParameter("IDESTADO", iDESTADO) :
                new ObjectParameter("IDESTADO", typeof(decimal));

            var tBLESTADOParameter = tBLESTADO != null ?
                new ObjectParameter("TBLESTADO", tBLESTADO) :
                new ObjectParameter("TBLESTADO", typeof(string));

            var jSONOUTParameter = jSONOUT != null ?
                new ObjectParameter("JSONOUT", jSONOUT) :
                new ObjectParameter("JSONOUT", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_CARACTERISTICAS", iDESTADOParameter, tBLESTADOParameter, jSONOUTParameter, rTA);
        }

        public virtual int SP_SET_CLONARENCUESTA(Nullable<decimal> iD_ENCUESTA, string nOMBRE, Nullable<decimal> uSUARIO)
        {
            var iD_ENCUESTAParameter = iD_ENCUESTA.HasValue ?
                new ObjectParameter("ID_ENCUESTA", iD_ENCUESTA) :
                new ObjectParameter("ID_ENCUESTA", typeof(decimal));

            var nOMBREParameter = nOMBRE != null ?
                new ObjectParameter("NOMBRE", nOMBRE) :
                new ObjectParameter("NOMBRE", typeof(string));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_CLONARENCUESTA", iD_ENCUESTAParameter, nOMBREParameter, uSUARIOParameter);
        }

        public virtual int SP_SET_ENCUESTAS(Nullable<decimal> iDESTADO, Nullable<decimal> iDFORM, string jSONOUT, ObjectParameter rTA)
        {
            var iDESTADOParameter = iDESTADO.HasValue ?
                new ObjectParameter("IDESTADO", iDESTADO) :
                new ObjectParameter("IDESTADO", typeof(decimal));

            var iDFORMParameter = iDFORM.HasValue ?
                new ObjectParameter("IDFORM", iDFORM) :
                new ObjectParameter("IDFORM", typeof(decimal));

            var jSONOUTParameter = jSONOUT != null ?
                new ObjectParameter("JSONOUT", jSONOUT) :
                new ObjectParameter("JSONOUT", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_ENCUESTAS", iDESTADOParameter, iDFORMParameter, jSONOUTParameter, rTA);
        }

        public virtual int SP_SET_DATOS_MODIFICADOS(Nullable<decimal> iD_ESTADO)
        {
            var iD_ESTADOParameter = iD_ESTADO.HasValue ?
                new ObjectParameter("ID_ESTADO", iD_ESTADO) :
                new ObjectParameter("ID_ESTADO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_DATOS_MODIFICADOS", iD_ESTADOParameter);
        }

        public virtual int SP_SET_FOTOS_FORM(Nullable<decimal> iD_ESTADO, string tABLA, string fOTOS, string iDFORM)
        {
            var iD_ESTADOParameter = iD_ESTADO.HasValue ?
                new ObjectParameter("ID_ESTADO", iD_ESTADO) :
                new ObjectParameter("ID_ESTADO", typeof(decimal));

            var tABLAParameter = tABLA != null ?
                new ObjectParameter("TABLA", tABLA) :
                new ObjectParameter("TABLA", typeof(string));

            var fOTOSParameter = fOTOS != null ?
                new ObjectParameter("FOTOS", fOTOS) :
                new ObjectParameter("FOTOS", typeof(string));

            var iDFORMParameter = iDFORM != null ?
                new ObjectParameter("IDFORM", iDFORM) :
                new ObjectParameter("IDFORM", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_FOTOS_FORM", iD_ESTADOParameter, tABLAParameter, fOTOSParameter, iDFORMParameter);
        }

        public virtual int SP_SET_FOTO_SIS_CONTROL(Nullable<decimal> eSTADO, string jSONOUT, ObjectParameter rTA)
        {
            var eSTADOParameter = eSTADO.HasValue ?
                new ObjectParameter("ESTADO", eSTADO) :
                new ObjectParameter("ESTADO", typeof(decimal));

            var jSONOUTParameter = jSONOUT != null ?
                new ObjectParameter("JSONOUT", jSONOUT) :
                new ObjectParameter("JSONOUT", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_FOTO_SIS_CONTROL", eSTADOParameter, jSONOUTParameter, rTA);
        }

        public virtual int SP_SET_GUARDAR_REPLICA(Nullable<decimal> iD_ENC, string nOMBRE, Nullable<decimal> uSUARIO, string tIPO, string dESCRIPCION, Nullable<decimal> iD_FORM)
        {
            var iD_ENCParameter = iD_ENC.HasValue ?
                new ObjectParameter("ID_ENC", iD_ENC) :
                new ObjectParameter("ID_ENC", typeof(decimal));

            var nOMBREParameter = nOMBRE != null ?
                new ObjectParameter("NOMBRE", nOMBRE) :
                new ObjectParameter("NOMBRE", typeof(string));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var tIPOParameter = tIPO != null ?
                new ObjectParameter("TIPO", tIPO) :
                new ObjectParameter("TIPO", typeof(string));

            var dESCRIPCIONParameter = dESCRIPCION != null ?
                new ObjectParameter("DESCRIPCION", dESCRIPCION) :
                new ObjectParameter("DESCRIPCION", typeof(string));

            var iD_FORMParameter = iD_FORM.HasValue ?
                new ObjectParameter("ID_FORM", iD_FORM) :
                new ObjectParameter("ID_FORM", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_GUARDAR_REPLICA", iD_ENCParameter, nOMBREParameter, uSUARIOParameter, tIPOParameter, dESCRIPCIONParameter, iD_FORMParameter);
        }

        public virtual int SP_SET_GUARDAR_RESPUESTA(Nullable<decimal> iDPREG, string vALOR, string cODIGO, Nullable<decimal> oRDEN, ObjectParameter iDRESPUESTA)
        {
            var iDPREGParameter = iDPREG.HasValue ?
                new ObjectParameter("IDPREG", iDPREG) :
                new ObjectParameter("IDPREG", typeof(decimal));

            var vALORParameter = vALOR != null ?
                new ObjectParameter("VALOR", vALOR) :
                new ObjectParameter("VALOR", typeof(string));

            var cODIGOParameter = cODIGO != null ?
                new ObjectParameter("CODIGO", cODIGO) :
                new ObjectParameter("CODIGO", typeof(string));

            var oRDENParameter = oRDEN.HasValue ?
                new ObjectParameter("ORDEN", oRDEN) :
                new ObjectParameter("ORDEN", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_GUARDAR_RESPUESTA", iDPREGParameter, vALORParameter, cODIGOParameter, oRDENParameter, iDRESPUESTA);
        }

        public virtual int SP_SET_INFO_INDUSTRIA(Nullable<decimal> iDINSTALACION, Nullable<decimal> iDTERCERO, Nullable<decimal> tIPO, Nullable<decimal> iDVISITA, string jSONOUT, ObjectParameter rTA)
        {
            var iDINSTALACIONParameter = iDINSTALACION.HasValue ?
                new ObjectParameter("IDINSTALACION", iDINSTALACION) :
                new ObjectParameter("IDINSTALACION", typeof(decimal));

            var iDTERCEROParameter = iDTERCERO.HasValue ?
                new ObjectParameter("IDTERCERO", iDTERCERO) :
                new ObjectParameter("IDTERCERO", typeof(decimal));

            var tIPOParameter = tIPO.HasValue ?
                new ObjectParameter("TIPO", tIPO) :
                new ObjectParameter("TIPO", typeof(decimal));

            var iDVISITAParameter = iDVISITA.HasValue ?
                new ObjectParameter("IDVISITA", iDVISITA) :
                new ObjectParameter("TIPO", typeof(decimal));

            var jSONOUTParameter = jSONOUT != null ?
                new ObjectParameter("JSONOUT", jSONOUT) :
                new ObjectParameter("JSONOUT", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_INFO_INDUSTRIA", iDINSTALACIONParameter, iDTERCEROParameter, tIPOParameter, iDVISITAParameter, jSONOUTParameter, rTA);
        }

        public virtual int SP_SET_ITEM(Nullable<decimal> iDFORM, string jSONIN, Nullable<decimal> uSUARIO, string tIPOUBICACION, ObjectParameter rTA)
        {
            var iDFORMParameter = iDFORM.HasValue ?
                new ObjectParameter("IDFORM", iDFORM) :
                new ObjectParameter("IDFORM", typeof(decimal));

            var jSONINParameter = jSONIN != null ?
                new ObjectParameter("JSONIN", jSONIN) :
                new ObjectParameter("JSONIN", typeof(string));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var tIPOUBICACIONParameter = tIPOUBICACION != null ?
                new ObjectParameter("TIPOUBICACION", tIPOUBICACION) :
                new ObjectParameter("TIPOUBICACION", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_ITEM", iDFORMParameter, jSONINParameter, uSUARIOParameter, tIPOUBICACIONParameter, rTA);
        }

        public virtual int SP_SET_ITEM_L(Nullable<decimal> iDFORM, string jSONIN, string jSONINXY, Nullable<decimal> uSUARIO, string tIPOUBICACION, ObjectParameter rTA)
        {
            var iDFORMParameter = iDFORM.HasValue ?
                new ObjectParameter("IDFORM", iDFORM) :
                new ObjectParameter("IDFORM", typeof(decimal));

            var jSONINParameter = jSONIN != null ?
                new ObjectParameter("JSONIN", jSONIN) :
                new ObjectParameter("JSONIN", typeof(string));

            var jSONINXYParameter = jSONINXY != null ?
                new ObjectParameter("JSONINXY", jSONINXY) :
                new ObjectParameter("JSONINXY", typeof(string));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var tIPOUBICACIONParameter = tIPOUBICACION != null ?
                new ObjectParameter("TIPOUBICACION", tIPOUBICACION) :
                new ObjectParameter("TIPOUBICACION", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_ITEM_L", iDFORMParameter, jSONINParameter, jSONINXYParameter, uSUARIOParameter, tIPOUBICACIONParameter, rTA);
        }

        public virtual int SP_GET_FORMULARIOS(Nullable<decimal> iDINSTALACION, Nullable<decimal> iDTERCERO, Nullable<decimal> iDVISITA, ObjectParameter jSONOUT)
        {
            var iDINSTALACIONParameter = iDINSTALACION.HasValue ?
                new ObjectParameter("IDINSTALACION", iDINSTALACION) :
                new ObjectParameter("IDINSTALACION", typeof(decimal));

            var iDTERCEROParameter = iDTERCERO.HasValue ?
                new ObjectParameter("IDTERCERO", iDTERCERO) :
                new ObjectParameter("IDTERCERO", typeof(decimal));

            var iDVISITAParameter = iDVISITA.HasValue ?
                new ObjectParameter("IDVISITA", iDVISITA) :
                new ObjectParameter("IDVISITA", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GET_FORMULARIOS", iDINSTALACIONParameter, iDTERCEROParameter, iDVISITAParameter, jSONOUT);
        }

        public virtual int SP_SET_GUARDAR_DOCUMENTO(Nullable<decimal> iDVISITA, string nOMBRE, Nullable<decimal> tIPO)
        {
            var iDVISITAParameter = iDVISITA.HasValue ?
                new ObjectParameter("IDVISITA", iDVISITA) :
                new ObjectParameter("IDVISITA", typeof(decimal));

            var nOMBREParameter = nOMBRE != null ?
                new ObjectParameter("NOMBRE", nOMBRE) :
                new ObjectParameter("NOMBRE", typeof(string));

            var tIPOParameter = tIPO.HasValue ?
                new ObjectParameter("TIPO", tIPO) :
                new ObjectParameter("TIPO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_GUARDAR_DOCUMENTO", iDVISITAParameter, nOMBREParameter, tIPOParameter);
        }

        public virtual int SP_SET_GUARDAR_PREGUNTA(string nOMBRE, Nullable<decimal> tIPOPREGUNTA, string aYUDA, ObjectParameter iDPREGUNTA)
        {
            var nOMBREParameter = nOMBRE != null ?
                new ObjectParameter("NOMBRE", nOMBRE) :
                new ObjectParameter("NOMBRE", typeof(string));

            var tIPOPREGUNTAParameter = tIPOPREGUNTA.HasValue ?
                new ObjectParameter("TIPOPREGUNTA", tIPOPREGUNTA) :
                new ObjectParameter("TIPOPREGUNTA", typeof(decimal));

            var aYUDAParameter = aYUDA != null ?
                new ObjectParameter("AYUDA", aYUDA) :
                new ObjectParameter("AYUDA", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_GUARDAR_PREGUNTA", nOMBREParameter, tIPOPREGUNTAParameter, aYUDAParameter, iDPREGUNTA);
        }

        public virtual int SP_GET_FOTOSISCONTROL(Nullable<decimal> eSTADO, ObjectParameter jSONOUT)
        {
            var eSTADOParameter = eSTADO.HasValue ?
                new ObjectParameter("ESTADO", eSTADO) :
                new ObjectParameter("ESTADO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GET_FOTOSISCONTROL", eSTADOParameter, jSONOUT);
        }

        public virtual int SP_GUADAR_INF_URL(Nullable<decimal> iDVIS, string uRLINF, string uRLINF2)
        {
            var iDVISParameter = iDVIS.HasValue ?
                new ObjectParameter("IDVIS", iDVIS) :
                new ObjectParameter("IDVIS", typeof(decimal));

            var uRLINFParameter = uRLINF != null ?
                new ObjectParameter("URLINF", uRLINF) :
                new ObjectParameter("URLINF", typeof(string));

            var uRLINF2Parameter = uRLINF2 != null ?
                new ObjectParameter("URLINF2", uRLINF2) :
                new ObjectParameter("URLINF2", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GUADAR_INF_URL", iDVISParameter, uRLINFParameter, uRLINF2Parameter);
        }

        public virtual int SP_DESASOCIARTRAMITE(string tRAMITES, Nullable<decimal> uSUARIO, string cOMENTARIO, string cOPIAS, Nullable<decimal> tIPO)
        {
            var tRAMITESParameter = tRAMITES != null ?
                new ObjectParameter("TRAMITES", tRAMITES) :
                new ObjectParameter("TRAMITES", typeof(string));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var cOMENTARIOParameter = cOMENTARIO != null ?
                new ObjectParameter("COMENTARIO", cOMENTARIO) :
                new ObjectParameter("COMENTARIO", typeof(string));

            var cOPIASParameter = cOPIAS != null ?
                new ObjectParameter("COPIAS", cOPIAS) :
                new ObjectParameter("COPIAS", typeof(string));

            var tIPOParameter = tIPO.HasValue ?
                new ObjectParameter("TIPO", tIPO) :
                new ObjectParameter("TIPO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_DESASOCIARTRAMITE", tRAMITESParameter, uSUARIOParameter, cOMENTARIOParameter, cOPIASParameter, tIPOParameter);
        }

        public virtual int SP_DEVOLVERTRAMITE(Nullable<decimal> iDTRAMITE, Nullable<decimal> uSUARIO, string cOMENTARIO, string cOPIAS, Nullable<decimal> iD_CODTAREA, Nullable<decimal> iD_TAREA_SIG)
        {
            var iDTRAMITEParameter = iDTRAMITE.HasValue ?
                new ObjectParameter("IDTRAMITE", iDTRAMITE) :
                new ObjectParameter("IDTRAMITE", typeof(decimal));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var cOMENTARIOParameter = cOMENTARIO != null ?
                new ObjectParameter("COMENTARIO", cOMENTARIO) :
                new ObjectParameter("COMENTARIO", typeof(string));

            var cOPIASParameter = cOPIAS != null ?
                new ObjectParameter("COPIAS", cOPIAS) :
                new ObjectParameter("COPIAS", typeof(string));

            var iD_CODTAREAParameter = iD_CODTAREA.HasValue ?
                new ObjectParameter("ID_CODTAREA", iD_CODTAREA) :
                new ObjectParameter("ID_CODTAREA", typeof(decimal));

            var iD_TAREA_SIGParameter = iD_TAREA_SIG.HasValue ?
                new ObjectParameter("ID_TAREA_SIG", iD_TAREA_SIG) :
                new ObjectParameter("ID_TAREA_SIG", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_DEVOLVERTRAMITE", iDTRAMITEParameter, uSUARIOParameter, cOMENTARIOParameter, cOPIASParameter, iD_CODTAREAParameter, iD_TAREA_SIGParameter);
        }

        public virtual int SP_GUADAR_INFORMETECNICO(string sTRASUNTO, string sTROBSERVACION, Nullable<decimal> eSTADO, Nullable<decimal> iDVISITA, Nullable<decimal> uSUARIO)
        {
            var sTRASUNTOParameter = sTRASUNTO != null ?
                new ObjectParameter("STRASUNTO", sTRASUNTO) :
                new ObjectParameter("STRASUNTO", typeof(string));

            var sTROBSERVACIONParameter = sTROBSERVACION != null ?
                new ObjectParameter("STROBSERVACION", sTROBSERVACION) :
                new ObjectParameter("STROBSERVACION", typeof(string));

            var eSTADOParameter = eSTADO.HasValue ?
                new ObjectParameter("ESTADO", eSTADO) :
                new ObjectParameter("ESTADO", typeof(decimal));

            var iDVISITAParameter = iDVISITA.HasValue ?
                new ObjectParameter("IDVISITA", iDVISITA) :
                new ObjectParameter("IDVISITA", typeof(decimal));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GUADAR_INFORMETECNICO", sTRASUNTOParameter, sTROBSERVACIONParameter, eSTADOParameter, iDVISITAParameter, uSUARIOParameter);
        }

        public virtual int SP_ADD_VISITA_INF(string tRAMITES, Nullable<decimal> uSUARIO, Nullable<decimal> iD_VISITA, Nullable<decimal> tAREASIGIENTE)
        {
            var tRAMITESParameter = tRAMITES != null ?
                new ObjectParameter("TRAMITES", tRAMITES) :
                new ObjectParameter("TRAMITES", typeof(string));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var iD_VISITAParameter = iD_VISITA.HasValue ?
                new ObjectParameter("ID_VISITA", iD_VISITA) :
                new ObjectParameter("ID_VISITA", typeof(decimal));

            var tAREASIGIENTEParameter = tAREASIGIENTE.HasValue ?
                new ObjectParameter("TAREASIGIENTE", tAREASIGIENTE) :
                new ObjectParameter("TAREASIGIENTE", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_ADD_VISITA_INF", tRAMITESParameter, uSUARIOParameter, iD_VISITAParameter, tAREASIGIENTEParameter);
        }

        public virtual int SP_CONSULTARTAREASIG_VISITA(Nullable<decimal> iDVISITA, ObjectParameter jSONOUT)
        {
            var iDVISITAParameter = iDVISITA.HasValue ?
                new ObjectParameter("IDVISITA", iDVISITA) :
                new ObjectParameter("IDVISITA", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_CONSULTARTAREASIG_VISITA", iDVISITAParameter, jSONOUT);
        }

        public virtual int SP_MODIFICAR_INFTECNICO(Nullable<decimal> iDVISITA, Nullable<decimal> uSUARIO, Nullable<decimal> tIPO)
        {
            var iDVISITAParameter = iDVISITA.HasValue ?
                new ObjectParameter("IDVISITA", iDVISITA) :
                new ObjectParameter("IDVISITA", typeof(decimal));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var tIPOParameter = tIPO.HasValue ?
                new ObjectParameter("TIPO", tIPO) :
                new ObjectParameter("TIPO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_MODIFICAR_INFTECNICO", iDVISITAParameter, uSUARIOParameter, tIPOParameter);
        }

        public virtual int SP_ELIMINAR_SESSION(string uSUARIO, string cLAVE)
        {
            var uSUARIOParameter = uSUARIO != null ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(string));

            var cLAVEParameter = cLAVE != null ?
                new ObjectParameter("CLAVE", cLAVE) :
                new ObjectParameter("CLAVE", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_ELIMINAR_SESSION", uSUARIOParameter, cLAVEParameter);
        }

        public virtual int SP_GUARDARSESSION(string uSUARIO, string cLAVE)
        {
            var uSUARIOParameter = uSUARIO != null ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(string));

            var cLAVEParameter = cLAVE != null ?
                new ObjectParameter("CLAVE", cLAVE) :
                new ObjectParameter("CLAVE", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GUARDARSESSION", uSUARIOParameter, cLAVEParameter);
        }

        public virtual int SP_DELETESESSIONACTIVA(string uSUARIO, string cLAVE)
        {
            var uSUARIOParameter = uSUARIO != null ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(string));

            var cLAVEParameter = cLAVE != null ?
                new ObjectParameter("CLAVE", cLAVE) :
                new ObjectParameter("CLAVE", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_DELETESESSIONACTIVA", uSUARIOParameter, cLAVEParameter);
        }

        public virtual int SP_GET_CAPTACION(Nullable<decimal> iDCAPT, Nullable<decimal> x, Nullable<decimal> y, ObjectParameter jSONOUT)
        {
            var iDCAPTParameter = iDCAPT.HasValue ?
                new ObjectParameter("IDCAPT", iDCAPT) :
                new ObjectParameter("IDCAPT", typeof(decimal));

            var xParameter = x.HasValue ?
                new ObjectParameter("X", x) :
                new ObjectParameter("X", typeof(decimal));

            var yParameter = y.HasValue ?
                new ObjectParameter("Y", y) :
                new ObjectParameter("Y", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GET_CAPTACION", iDCAPTParameter, xParameter, yParameter, jSONOUT);
        }

        public virtual int SP_CREATE_ESTADO(Nullable<decimal> iDFORM, Nullable<decimal> iDTERCERO, Nullable<decimal> iDINSTALACION, Nullable<decimal> iD_CAPT, Nullable<decimal> iD_TIPOESTADO, Nullable<decimal> iD_VIS, ObjectParameter rESPIDESTADO)
        {
            var iDFORMParameter = iDFORM.HasValue ?
                new ObjectParameter("IDFORM", iDFORM) :
                new ObjectParameter("IDFORM", typeof(decimal));

            var iDTERCEROParameter = iDTERCERO.HasValue ?
                new ObjectParameter("IDTERCERO", iDTERCERO) :
                new ObjectParameter("IDTERCERO", typeof(decimal));

            var iDINSTALACIONParameter = iDINSTALACION.HasValue ?
                new ObjectParameter("IDINSTALACION", iDINSTALACION) :
                new ObjectParameter("IDINSTALACION", typeof(decimal));

            var iD_CAPTParameter = iD_CAPT.HasValue ?
                new ObjectParameter("ID_CAPT", iD_CAPT) :
                new ObjectParameter("ID_CAPT", typeof(decimal));

            var iD_TIPOESTADOParameter = iD_TIPOESTADO.HasValue ?
                new ObjectParameter("ID_TIPOESTADO", iD_TIPOESTADO) :
                new ObjectParameter("ID_TIPOESTADO", typeof(decimal));

            var iD_VISParameter = iD_VIS.HasValue ?
                new ObjectParameter("ID_VIS", iD_VIS) :
                new ObjectParameter("ID_VIS", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_CREATE_ESTADO", iDFORMParameter, iDTERCEROParameter, iDINSTALACIONParameter, iD_CAPTParameter, iD_TIPOESTADOParameter, iD_VISParameter, rESPIDESTADO);
        }

        public virtual int SP_NEW_P_CAPTACION(Nullable<decimal> iD_C, Nullable<decimal> x, Nullable<decimal> y, Nullable<decimal> z, Nullable<decimal> uSUARIO, string tIPOUBICACION)
        {
            var iD_CParameter = iD_C.HasValue ?
                new ObjectParameter("ID_C", iD_C) :
                new ObjectParameter("ID_C", typeof(decimal));

            var xParameter = x.HasValue ?
                new ObjectParameter("X", x) :
                new ObjectParameter("X", typeof(decimal));

            var yParameter = y.HasValue ?
                new ObjectParameter("Y", y) :
                new ObjectParameter("Y", typeof(decimal));

            var zParameter = z.HasValue ?
                new ObjectParameter("Z", z) :
                new ObjectParameter("Z", typeof(decimal));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var tIPOUBICACIONParameter = tIPOUBICACION != null ?
                new ObjectParameter("TIPOUBICACION", tIPOUBICACION) :
                new ObjectParameter("TIPOUBICACION", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_NEW_P_CAPTACION", iD_CParameter, xParameter, yParameter, zParameter, uSUARIOParameter, tIPOUBICACIONParameter);
        }

        public virtual int SP_SET_CAPTACION(string jSONIN, Nullable<decimal> uSUARIO, string tIPOUBICACION, ObjectParameter rTA)
        {
            var jSONINParameter = jSONIN != null ?
                new ObjectParameter("JSONIN", jSONIN) :
                new ObjectParameter("JSONIN", typeof(string));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var tIPOUBICACIONParameter = tIPOUBICACION != null ?
                new ObjectParameter("TIPOUBICACION", tIPOUBICACION) :
                new ObjectParameter("TIPOUBICACION", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_CAPTACION", jSONINParameter, uSUARIOParameter, tIPOUBICACIONParameter, rTA);
        }

        public virtual int SP_GET_USOS(Nullable<decimal> iDCAPTACIONESTADO, ObjectParameter jSONOUT)
        {
            var iDCAPTACIONESTADOParameter = iDCAPTACIONESTADO.HasValue ?
                new ObjectParameter("IDCAPTACIONESTADO", iDCAPTACIONESTADO) :
                new ObjectParameter("IDCAPTACIONESTADO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GET_USOS", iDCAPTACIONESTADOParameter, jSONOUT);
        }

        public virtual int SP_SET_USOS(Nullable<decimal> iDCAPTACIONESTADO, string jSONOUT, ObjectParameter rTA)
        {
            var iDCAPTACIONESTADOParameter = iDCAPTACIONESTADO.HasValue ?
                new ObjectParameter("IDCAPTACIONESTADO", iDCAPTACIONESTADO) :
                new ObjectParameter("IDCAPTACIONESTADO", typeof(decimal));

            var jSONOUTParameter = jSONOUT != null ?
                new ObjectParameter("JSONOUT", jSONOUT) :
                new ObjectParameter("JSONOUT", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_USOS", iDCAPTACIONESTADOParameter, jSONOUTParameter, rTA);
        }

        public virtual int SP_GET_VERTIMIENTO(Nullable<decimal> iD_VERT, Nullable<decimal> x, Nullable<decimal> y, ObjectParameter jSONOUT)
        {
            var iD_VERTParameter = iD_VERT.HasValue ?
                new ObjectParameter("ID_VERT", iD_VERT) :
                new ObjectParameter("ID_VERT", typeof(decimal));

            var xParameter = x.HasValue ?
                new ObjectParameter("X", x) :
                new ObjectParameter("X", typeof(decimal));

            var yParameter = y.HasValue ?
                new ObjectParameter("Y", y) :
                new ObjectParameter("Y", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GET_VERTIMIENTO", iD_VERTParameter, xParameter, yParameter, jSONOUT);
        }

        public virtual int SP_NEW_P_VERTIMIENTO(Nullable<decimal> iD_C, Nullable<decimal> x, Nullable<decimal> y, Nullable<decimal> z, Nullable<decimal> uSUARIO, string tIPOUBICACION)
        {
            var iD_CParameter = iD_C.HasValue ?
                new ObjectParameter("ID_C", iD_C) :
                new ObjectParameter("ID_C", typeof(decimal));

            var xParameter = x.HasValue ?
                new ObjectParameter("X", x) :
                new ObjectParameter("X", typeof(decimal));

            var yParameter = y.HasValue ?
                new ObjectParameter("Y", y) :
                new ObjectParameter("Y", typeof(decimal));

            var zParameter = z.HasValue ?
                new ObjectParameter("Z", z) :
                new ObjectParameter("Z", typeof(decimal));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var tIPOUBICACIONParameter = tIPOUBICACION != null ?
                new ObjectParameter("TIPOUBICACION", tIPOUBICACION) :
                new ObjectParameter("TIPOUBICACION", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_NEW_P_VERTIMIENTO", iD_CParameter, xParameter, yParameter, zParameter, uSUARIOParameter, tIPOUBICACIONParameter);
        }

        public virtual int SP_SET_VERTIMIENTO(string jSONIN, Nullable<decimal> uSUARIO, string tIPOUBICACION, ObjectParameter rTA)
        {
            var jSONINParameter = jSONIN != null ?
                new ObjectParameter("JSONIN", jSONIN) :
                new ObjectParameter("JSONIN", typeof(string));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var tIPOUBICACIONParameter = tIPOUBICACION != null ?
                new ObjectParameter("TIPOUBICACION", tIPOUBICACION) :
                new ObjectParameter("TIPOUBICACION", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_VERTIMIENTO", jSONINParameter, uSUARIOParameter, tIPOUBICACIONParameter, rTA);
        }

        public virtual int SP_CREATE_ESTADO_V(Nullable<decimal> iDFORM, Nullable<decimal> iDTERCERO, Nullable<decimal> iDINSTALACION, Nullable<decimal> iD_CAPT, Nullable<decimal> iD_TIPOESTADO, Nullable<decimal> iD_VIS, ObjectParameter rESPIDESTADO)
        {
            var iDFORMParameter = iDFORM.HasValue ?
                new ObjectParameter("IDFORM", iDFORM) :
                new ObjectParameter("IDFORM", typeof(decimal));

            var iDTERCEROParameter = iDTERCERO.HasValue ?
                new ObjectParameter("IDTERCERO", iDTERCERO) :
                new ObjectParameter("IDTERCERO", typeof(decimal));

            var iDINSTALACIONParameter = iDINSTALACION.HasValue ?
                new ObjectParameter("IDINSTALACION", iDINSTALACION) :
                new ObjectParameter("IDINSTALACION", typeof(decimal));

            var iD_CAPTParameter = iD_CAPT.HasValue ?
                new ObjectParameter("ID_CAPT", iD_CAPT) :
                new ObjectParameter("ID_CAPT", typeof(decimal));

            var iD_TIPOESTADOParameter = iD_TIPOESTADO.HasValue ?
                new ObjectParameter("ID_TIPOESTADO", iD_TIPOESTADO) :
                new ObjectParameter("ID_TIPOESTADO", typeof(decimal));

            var iD_VISParameter = iD_VIS.HasValue ?
                new ObjectParameter("ID_VIS", iD_VIS) :
                new ObjectParameter("ID_VIS", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_CREATE_ESTADO_V", iDFORMParameter, iDTERCEROParameter, iDINSTALACIONParameter, iD_CAPTParameter, iD_TIPOESTADOParameter, iD_VISParameter, rESPIDESTADO);
        }

        public virtual int SP_SET_FOTO_SIS_CONTROL1(Nullable<decimal> eSTADO, string jSONOUT, ObjectParameter rTA)
        {
            var eSTADOParameter = eSTADO.HasValue ?
                new ObjectParameter("ESTADO", eSTADO) :
                new ObjectParameter("ESTADO", typeof(decimal));

            var jSONOUTParameter = jSONOUT != null ?
                new ObjectParameter("JSONOUT", jSONOUT) :
                new ObjectParameter("JSONOUT", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_FOTO_SIS_CONTROL1", eSTADOParameter, jSONOUTParameter, rTA);
        }

        public virtual int SP_GET_COMBOHIJO(Nullable<decimal> iDPRE, ObjectParameter jSONOUT)
        {
            var iDPREParameter = iDPRE.HasValue ?
                new ObjectParameter("IDPRE", iDPRE) :
                new ObjectParameter("IDPRE", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GET_COMBOHIJO", iDPREParameter, jSONOUT);
        }

        public virtual int SP_NEW_P_IND_DISPERSO(Nullable<decimal> iD_ESPECIE_, Nullable<decimal> iD_LOCALIZACION_ARBOL_, Nullable<decimal> iD_EDAD_SIEMBRA_, Nullable<decimal> iD_ESTADO_, Nullable<decimal> iD_PROCEDENCIA_, Nullable<decimal> iD_UBICACION_, string l_SETO_, Nullable<decimal> n_INDIVIDUOS_SETO_, Nullable<decimal> iD_TIPO_ARBOL_, Nullable<decimal> x, Nullable<decimal> y, Nullable<decimal> uSUARIO, ObjectParameter iD_IND)
        {
            var iD_ESPECIE_Parameter = iD_ESPECIE_.HasValue ?
                new ObjectParameter("ID_ESPECIE_", iD_ESPECIE_) :
                new ObjectParameter("ID_ESPECIE_", typeof(decimal));

            var iD_LOCALIZACION_ARBOL_Parameter = iD_LOCALIZACION_ARBOL_.HasValue ?
                new ObjectParameter("ID_LOCALIZACION_ARBOL_", iD_LOCALIZACION_ARBOL_) :
                new ObjectParameter("ID_LOCALIZACION_ARBOL_", typeof(decimal));

            var iD_EDAD_SIEMBRA_Parameter = iD_EDAD_SIEMBRA_.HasValue ?
                new ObjectParameter("ID_EDAD_SIEMBRA_", iD_EDAD_SIEMBRA_) :
                new ObjectParameter("ID_EDAD_SIEMBRA_", typeof(decimal));

            var iD_ESTADO_Parameter = iD_ESTADO_.HasValue ?
                new ObjectParameter("ID_ESTADO_", iD_ESTADO_) :
                new ObjectParameter("ID_ESTADO_", typeof(decimal));

            var iD_PROCEDENCIA_Parameter = iD_PROCEDENCIA_.HasValue ?
                new ObjectParameter("ID_PROCEDENCIA_", iD_PROCEDENCIA_) :
                new ObjectParameter("ID_PROCEDENCIA_", typeof(decimal));

            var iD_UBICACION_Parameter = iD_UBICACION_.HasValue ?
                new ObjectParameter("ID_UBICACION_", iD_UBICACION_) :
                new ObjectParameter("ID_UBICACION_", typeof(decimal));

            var l_SETO_Parameter = l_SETO_ != null ?
                new ObjectParameter("L_SETO_", l_SETO_) :
                new ObjectParameter("L_SETO_", typeof(string));

            var n_INDIVIDUOS_SETO_Parameter = n_INDIVIDUOS_SETO_.HasValue ?
                new ObjectParameter("N_INDIVIDUOS_SETO_", n_INDIVIDUOS_SETO_) :
                new ObjectParameter("N_INDIVIDUOS_SETO_", typeof(decimal));

            var iD_TIPO_ARBOL_Parameter = iD_TIPO_ARBOL_.HasValue ?
                new ObjectParameter("ID_TIPO_ARBOL_", iD_TIPO_ARBOL_) :
                new ObjectParameter("ID_TIPO_ARBOL_", typeof(decimal));

            var xParameter = x.HasValue ?
                new ObjectParameter("X", x) :
                new ObjectParameter("X", typeof(decimal));

            var yParameter = y.HasValue ?
                new ObjectParameter("Y", y) :
                new ObjectParameter("Y", typeof(decimal));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_NEW_P_IND_DISPERSO", iD_ESPECIE_Parameter, iD_LOCALIZACION_ARBOL_Parameter, iD_EDAD_SIEMBRA_Parameter, iD_ESTADO_Parameter, iD_PROCEDENCIA_Parameter, iD_UBICACION_Parameter, l_SETO_Parameter, n_INDIVIDUOS_SETO_Parameter, iD_TIPO_ARBOL_Parameter, xParameter, yParameter, uSUARIOParameter, iD_IND);
        }

        public virtual int SP_UPDATE_P_IND_DISPERSO(Nullable<decimal> iD_ESPECIE_, Nullable<decimal> iD_LOCALIZACION_ARBOL_, Nullable<decimal> iD_EDAD_SIEMBRA_, Nullable<decimal> iD_ESTADO_, Nullable<decimal> iD_PROCEDENCIA_, Nullable<decimal> iD_UBICACION_, string l_SETO_, Nullable<decimal> n_INDIVIDUOS_SETO_, Nullable<decimal> iD_TIPO_ARBOL_, Nullable<decimal> x, Nullable<decimal> y, Nullable<decimal> uSUARIO, Nullable<decimal> iD_IND)
        {
            var iD_ESPECIE_Parameter = iD_ESPECIE_.HasValue ?
                new ObjectParameter("ID_ESPECIE_", iD_ESPECIE_) :
                new ObjectParameter("ID_ESPECIE_", typeof(decimal));

            var iD_LOCALIZACION_ARBOL_Parameter = iD_LOCALIZACION_ARBOL_.HasValue ?
                new ObjectParameter("ID_LOCALIZACION_ARBOL_", iD_LOCALIZACION_ARBOL_) :
                new ObjectParameter("ID_LOCALIZACION_ARBOL_", typeof(decimal));

            var iD_EDAD_SIEMBRA_Parameter = iD_EDAD_SIEMBRA_.HasValue ?
                new ObjectParameter("ID_EDAD_SIEMBRA_", iD_EDAD_SIEMBRA_) :
                new ObjectParameter("ID_EDAD_SIEMBRA_", typeof(decimal));

            var iD_ESTADO_Parameter = iD_ESTADO_.HasValue ?
                new ObjectParameter("ID_ESTADO_", iD_ESTADO_) :
                new ObjectParameter("ID_ESTADO_", typeof(decimal));

            var iD_PROCEDENCIA_Parameter = iD_PROCEDENCIA_.HasValue ?
                new ObjectParameter("ID_PROCEDENCIA_", iD_PROCEDENCIA_) :
                new ObjectParameter("ID_PROCEDENCIA_", typeof(decimal));

            var iD_UBICACION_Parameter = iD_UBICACION_.HasValue ?
                new ObjectParameter("ID_UBICACION_", iD_UBICACION_) :
                new ObjectParameter("ID_UBICACION_", typeof(decimal));

            var l_SETO_Parameter = l_SETO_ != null ?
                new ObjectParameter("L_SETO_", l_SETO_) :
                new ObjectParameter("L_SETO_", typeof(string));

            var n_INDIVIDUOS_SETO_Parameter = n_INDIVIDUOS_SETO_.HasValue ?
                new ObjectParameter("N_INDIVIDUOS_SETO_", n_INDIVIDUOS_SETO_) :
                new ObjectParameter("N_INDIVIDUOS_SETO_", typeof(decimal));

            var iD_TIPO_ARBOL_Parameter = iD_TIPO_ARBOL_.HasValue ?
                new ObjectParameter("ID_TIPO_ARBOL_", iD_TIPO_ARBOL_) :
                new ObjectParameter("ID_TIPO_ARBOL_", typeof(decimal));

            var xParameter = x.HasValue ?
                new ObjectParameter("X", x) :
                new ObjectParameter("X", typeof(decimal));

            var yParameter = y.HasValue ?
                new ObjectParameter("Y", y) :
                new ObjectParameter("Y", typeof(decimal));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var iD_INDParameter = iD_IND.HasValue ?
                new ObjectParameter("ID_IND", iD_IND) :
                new ObjectParameter("ID_IND", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_UPDATE_P_IND_DISPERSO", iD_ESPECIE_Parameter, iD_LOCALIZACION_ARBOL_Parameter, iD_EDAD_SIEMBRA_Parameter, iD_ESTADO_Parameter, iD_PROCEDENCIA_Parameter, iD_UBICACION_Parameter, l_SETO_Parameter, n_INDIVIDUOS_SETO_Parameter, iD_TIPO_ARBOL_Parameter, xParameter, yParameter, uSUARIOParameter, iD_INDParameter);
        }

        public virtual int SP_DESASOCIAR_ARBOL(Nullable<decimal> iDTEMP, Nullable<decimal> iDARBOL)
        {
            var iDTEMPParameter = iDTEMP.HasValue ?
                new ObjectParameter("IDTEMP", iDTEMP) :
                new ObjectParameter("IDTEMP", typeof(decimal));

            var iDARBOLParameter = iDARBOL.HasValue ?
                new ObjectParameter("IDARBOL", iDARBOL) :
                new ObjectParameter("IDARBOL", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_DESASOCIAR_ARBOL", iDTEMPParameter, iDARBOLParameter);
        }

        public virtual int SP_GET_SINTOMA_EF(ObjectParameter jSONOUT)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GET_SINTOMA_EF", jSONOUT);
        }

        public virtual int SP_END_ARBOLES_CARGA(Nullable<decimal> iDTRAMITE, Nullable<decimal> uSUARIO, ObjectParameter rTA)
        {
            var iDTRAMITEParameter = iDTRAMITE.HasValue ?
                new ObjectParameter("IDTRAMITE", iDTRAMITE) :
                new ObjectParameter("IDTRAMITE", typeof(decimal));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_END_ARBOLES_CARGA", iDTRAMITEParameter, uSUARIOParameter, rTA);
        }

        public virtual int SP_AYUDAS(ObjectParameter jSONOUT)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_AYUDAS", jSONOUT);
        }

        //public virtual int SP_AYUDA_TXT1(ObjectParameter jSONOUT)
        //{
        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_AYUDA_TXT1", jSONOUT);
        //}

        public virtual int SP_GET_AYUDA(Nullable<decimal> iD_AYUDA_PADRE, ObjectParameter jSONOUT)
        {
            var iD_AYUDA_PADREParameter = iD_AYUDA_PADRE.HasValue ?
                new ObjectParameter("ID_AYUDA_PADRE", iD_AYUDA_PADRE) :
                new ObjectParameter("ID_AYUDA_PADRE", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GET_AYUDA", iD_AYUDA_PADREParameter, jSONOUT);
        }

        public virtual int SP_GET_CARACTERISTICAS_TL(Nullable<decimal> iDESTADO, string tBLESTADO, Nullable<decimal> iD_FORMULARIO, Nullable<decimal> gRUPO_FORM, Nullable<decimal> iD_CARACT_PADRE, ObjectParameter jSONOUT)
        {
            var iDESTADOParameter = iDESTADO.HasValue ?
                new ObjectParameter("IDESTADO", iDESTADO) :
                new ObjectParameter("IDESTADO", typeof(decimal));

            var tBLESTADOParameter = tBLESTADO != null ?
                new ObjectParameter("TBLESTADO", tBLESTADO) :
                new ObjectParameter("TBLESTADO", typeof(string));

            var iD_FORMULARIOParameter = iD_FORMULARIO.HasValue ?
                new ObjectParameter("ID_FORMULARIO", iD_FORMULARIO) :
                new ObjectParameter("ID_FORMULARIO", typeof(decimal));

            var gRUPO_FORMParameter = gRUPO_FORM.HasValue ?
                new ObjectParameter("GRUPO_FORM", gRUPO_FORM) :
                new ObjectParameter("GRUPO_FORM", typeof(decimal));

            var iD_CARACT_PADREParameter = iD_CARACT_PADRE.HasValue ?
                new ObjectParameter("ID_CARACT_PADRE", iD_CARACT_PADRE) :
                new ObjectParameter("ID_CARACT_PADRE", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GET_CARACTERISTICAS_TL", iDESTADOParameter, tBLESTADOParameter, iD_FORMULARIOParameter, gRUPO_FORMParameter, iD_CARACT_PADREParameter, jSONOUT);
        }

        public virtual int SP_SET_ACTUACION(string aSUNTO, Nullable<decimal> cX, Nullable<decimal> cY, string tIPOUBICACION, string tRAMITES, Nullable<decimal> uSUARIO, string cOMENTARIO, string cOPIAS, Nullable<decimal> tIPOVISITA, Nullable<decimal> iD_VISITA_ACTUAL)
        {
            var aSUNTOParameter = aSUNTO != null ?
                new ObjectParameter("ASUNTO", aSUNTO) :
                new ObjectParameter("ASUNTO", typeof(string));

            var cXParameter = cX.HasValue ?
                new ObjectParameter("CX", cX) :
                new ObjectParameter("CX", typeof(decimal));

            var cYParameter = cY.HasValue ?
                new ObjectParameter("CY", cY) :
                new ObjectParameter("CY", typeof(decimal));

            var tIPOUBICACIONParameter = tIPOUBICACION != null ?
                new ObjectParameter("TIPOUBICACION", tIPOUBICACION) :
                new ObjectParameter("TIPOUBICACION", typeof(string));

            var tRAMITESParameter = tRAMITES != null ?
                new ObjectParameter("TRAMITES", tRAMITES) :
                new ObjectParameter("TRAMITES", typeof(string));

            var uSUARIOParameter = uSUARIO.HasValue ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(decimal));

            var cOMENTARIOParameter = cOMENTARIO != null ?
                new ObjectParameter("COMENTARIO", cOMENTARIO) :
                new ObjectParameter("COMENTARIO", typeof(string));

            var cOPIASParameter = cOPIAS != null ?
                new ObjectParameter("COPIAS", cOPIAS) :
                new ObjectParameter("COPIAS", typeof(string));

            var tIPOVISITAParameter = tIPOVISITA.HasValue ?
                new ObjectParameter("TIPOVISITA", tIPOVISITA) :
                new ObjectParameter("TIPOVISITA", typeof(decimal));

            var iD_VISITA_ACTUALParameter = iD_VISITA_ACTUAL.HasValue ?
                new ObjectParameter("ID_VISITA_ACTUAL", iD_VISITA_ACTUAL) :
                new ObjectParameter("ID_VISITA_ACTUAL", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_ACTUACION", aSUNTOParameter, cXParameter, cYParameter, tIPOUBICACIONParameter, tRAMITESParameter, uSUARIOParameter, cOMENTARIOParameter, cOPIASParameter, tIPOVISITAParameter, iD_VISITA_ACTUALParameter);
        }

        public virtual int SP_SET_TRAMITE_PMES(string mENSAJ, string fUNCIONARIO, string tAREA, string pROCESO, Nullable<decimal> iDESTADO, ObjectParameter rTA, ObjectParameter rTA2, ObjectParameter rTA3, ObjectParameter rTA4)
        {
            var mENSAJParameter = mENSAJ != null ?
                new ObjectParameter("MENSAJ", mENSAJ) :
                new ObjectParameter("MENSAJ", typeof(string));

            var fUNCIONARIOParameter = fUNCIONARIO != null ?
                new ObjectParameter("FUNCIONARIO", fUNCIONARIO) :
                new ObjectParameter("FUNCIONARIO", typeof(string));

            var tAREAParameter = tAREA != null ?
                new ObjectParameter("TAREA", tAREA) :
                new ObjectParameter("TAREA", typeof(string));

            var pROCESOParameter = pROCESO != null ?
                new ObjectParameter("PROCESO", pROCESO) :
                new ObjectParameter("PROCESO", typeof(string));

            var iDESTADOParameter = iDESTADO != null ?
                new ObjectParameter("IDESTADO", iDESTADO) :
                new ObjectParameter("IDESTADO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_TRAMITE_PMES", mENSAJParameter, fUNCIONARIOParameter, tAREAParameter, pROCESOParameter, iDESTADOParameter, rTA, rTA2, rTA3, rTA4);
        }

        public virtual int SP_SET_CARACTERISTICAS_TL(Nullable<decimal> iDESTADO, string tBLESTADO, string jSONOUT, ObjectParameter rTA)
        {
            var iDESTADOParameter = iDESTADO.HasValue ?
                new ObjectParameter("IDESTADO", iDESTADO) :
                new ObjectParameter("IDESTADO", typeof(decimal));

            var tBLESTADOParameter = tBLESTADO != null ?
                new ObjectParameter("TBLESTADO", tBLESTADO) :
                new ObjectParameter("TBLESTADO", typeof(string));

            var jSONOUTParameter = jSONOUT != null ?
                new ObjectParameter("JSONOUT", jSONOUT) :
                new ObjectParameter("JSONOUT", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_CARACTERISTICAS_TL", iDESTADOParameter, tBLESTADOParameter, jSONOUTParameter, rTA);
        }

        public virtual int SP_SET_GUARDARRADICADO(Nullable<decimal> iDESTADO, string cORADICADO)
        {
            var iDESTADOParameter = iDESTADO.HasValue ?
                new ObjectParameter("IDESTADO", iDESTADO) :
                new ObjectParameter("IDESTADO", typeof(decimal));

            var cORADICADOParameter = cORADICADO != null ?
                new ObjectParameter("CORADICADO", cORADICADO) :
                new ObjectParameter("CORADICADO", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_GUARDARRADICADO", iDESTADOParameter, cORADICADOParameter);
        }

        public virtual int SP_GET_ENCUESTAS2(Nullable<decimal> iDESTADO, Nullable<decimal> iD_FORM, Nullable<decimal> iDVIGENCIA, ObjectParameter jSONOUT)
        {
            var iDESTADOParameter = iDESTADO.HasValue ?
                new ObjectParameter("IDESTADO", iDESTADO) :
                new ObjectParameter("IDESTADO", typeof(decimal));

            var iD_FORMParameter = iD_FORM.HasValue ?
                new ObjectParameter("ID_FORM", iD_FORM) :
                new ObjectParameter("ID_FORM", typeof(decimal));

            var iDVIGENCIAParameter = iDVIGENCIA.HasValue ?
                new ObjectParameter("IDVIGENCIA", iDVIGENCIA) :
                new ObjectParameter("IDVIGENCIA", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_GET_ENCUESTAS2", iDESTADOParameter, iD_FORMParameter, iDVIGENCIAParameter, jSONOUT);
        }

        public virtual int SP_SET_ENCUESTA_ROL2(Nullable<decimal> iDVIGENCIA, string nOMBREENCU, string fECHAINI, Nullable<decimal> iDENCUENTA, string fECHAFIN, string aRRENCUESTA, Nullable<decimal> rOL, string tERMINOS, Nullable<decimal> tIPOINSTALACION, Nullable<decimal> cARD, Nullable<decimal> rADICAR, string iTEM)
        {
            var iDVIGENCIAParameter = iDVIGENCIA.HasValue ?
                new ObjectParameter("IDVIGENCIA", iDVIGENCIA) :
                new ObjectParameter("IDVIGENCIA", typeof(decimal));

            var nOMBREENCUParameter = nOMBREENCU != null ?
                new ObjectParameter("NOMBREENCU", nOMBREENCU) :
                new ObjectParameter("NOMBREENCU", typeof(string));

            var fECHAINIParameter = fECHAINI != null ?
                new ObjectParameter("FECHAINI", fECHAINI) :
                new ObjectParameter("FECHAINI", typeof(string));

            var iDENCUENTAParameter = iDENCUENTA.HasValue ?
                new ObjectParameter("IDENCUENTA", iDENCUENTA) :
                new ObjectParameter("IDENCUENTA", typeof(decimal));

            var fECHAFINParameter = fECHAFIN != null ?
                new ObjectParameter("FECHAFIN", fECHAFIN) :
                new ObjectParameter("FECHAFIN", typeof(string));

            var aRRENCUESTAParameter = aRRENCUESTA != null ?
                new ObjectParameter("ARRENCUESTA", aRRENCUESTA) :
                new ObjectParameter("ARRENCUESTA", typeof(string));

            var rOLParameter = rOL.HasValue ?
                new ObjectParameter("ROL", rOL) :
                new ObjectParameter("ROL", typeof(decimal));

            var tERMINOSParameter = tERMINOS != null ?
                new ObjectParameter("TERMINOS", tERMINOS) :
                new ObjectParameter("TERMINOS", typeof(string));

            var tIPOINSTALACIONParameter = tIPOINSTALACION.HasValue ?
                new ObjectParameter("TIPOINSTALACION", tIPOINSTALACION) :
                new ObjectParameter("TIPOINSTALACION", typeof(decimal));

            var cARDParameter = cARD.HasValue ?
                new ObjectParameter("CARD", cARD) :
                new ObjectParameter("CARD", typeof(decimal));

            var rADICARParameter = rADICAR.HasValue ?
                new ObjectParameter("RADICAR", rADICAR) :
                new ObjectParameter("RADICAR", typeof(decimal));

            var iTEMParameter = iTEM != null ?
                  new ObjectParameter("NOMBREITEM", iTEM) :
                  new ObjectParameter("NOMBREITEM", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_ENCUESTA_ROL2", iDVIGENCIAParameter, nOMBREENCUParameter, fECHAINIParameter, iDENCUENTAParameter, fECHAFINParameter, aRRENCUESTAParameter, rOLParameter, tERMINOSParameter, tIPOINSTALACIONParameter, cARDParameter, rADICARParameter, iTEMParameter);
        }

        public virtual int SP_SET_MODIFICARVIGENCIA(Nullable<decimal> iDVIGENCIA, string vIGEN, Nullable<decimal> iDESTADO)
        {
            var iDVIGENCIAParameter = iDVIGENCIA.HasValue ?
                new ObjectParameter("IDVIGENCIA", iDVIGENCIA) :
                new ObjectParameter("IDVIGENCIA", typeof(decimal));

            var vIGENParameter = vIGEN != null ?
                new ObjectParameter("VIGEN", vIGEN) :
                new ObjectParameter("VIGEN", typeof(string));

            var iDESTADOParameter = iDESTADO.HasValue ?
                new ObjectParameter("IDESTADO", iDESTADO) :
                new ObjectParameter("IDESTADO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_MODIFICARVIGENCIA", iDVIGENCIAParameter, vIGENParameter, iDESTADOParameter);
        }

        public virtual int SP_SET_CREAR_ESTADO_GENERICO2(Nullable<decimal> iDENC, Nullable<decimal> cOD_FUN, ObjectParameter rTA, Nullable<decimal> iDTERCERO, Nullable<decimal> iDINSTALACION, Nullable<decimal> tIPORAD)
        {
            var iDENCParameter = iDENC.HasValue ?
                new ObjectParameter("IDENC", iDENC) :
                new ObjectParameter("IDENC", typeof(decimal));

            var cOD_FUNParameter = cOD_FUN.HasValue ?
                new ObjectParameter("COD_FUN", cOD_FUN) :
                new ObjectParameter("COD_FUN", typeof(decimal));

            var iDTERCEROParameter = iDTERCERO.HasValue ?
                new ObjectParameter("IDTERCERO", iDTERCERO) :
                new ObjectParameter("IDTERCERO", typeof(decimal));

            var iDINSTALACIONParameter = iDINSTALACION.HasValue ?
                new ObjectParameter("IDINSTALACION", iDINSTALACION) :
                new ObjectParameter("IDINSTALACION", typeof(decimal));

            var tIPORADParameter = tIPORAD.HasValue ?
                new ObjectParameter("TIPORAD", tIPORAD) :
                new ObjectParameter("TIPORAD", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_CREAR_ESTADO_GENERICO2", iDENCParameter, cOD_FUNParameter, rTA, iDTERCEROParameter, iDINSTALACIONParameter, tIPORADParameter);
        }

        public virtual int SP_SET_ESTADO_CARDINALIDAD2(Nullable<decimal> iDENC, Nullable<decimal> cOD_FUN, Nullable<decimal> iDTERCERO, Nullable<decimal> iDINSTALACION, Nullable<decimal> cARD, Nullable<decimal> iDVIGENCIA, string vIGEN)
        {
            var iDENCParameter = iDENC.HasValue ?
                new ObjectParameter("IDENC", iDENC) :
                new ObjectParameter("IDENC", typeof(decimal));

            var cOD_FUNParameter = cOD_FUN.HasValue ?
                new ObjectParameter("COD_FUN", cOD_FUN) :
                new ObjectParameter("COD_FUN", typeof(decimal));

            var iDTERCEROParameter = iDTERCERO.HasValue ?
                new ObjectParameter("IDTERCERO", iDTERCERO) :
                new ObjectParameter("IDTERCERO", typeof(decimal));

            var iDINSTALACIONParameter = iDINSTALACION.HasValue ?
                new ObjectParameter("IDINSTALACION", iDINSTALACION) :
                new ObjectParameter("IDINSTALACION", typeof(decimal));

            var cARDParameter = cARD.HasValue ?
                new ObjectParameter("CARD", cARD) :
                new ObjectParameter("CARD", typeof(decimal));

            var iDVIGENCIAParameter = iDVIGENCIA.HasValue ?
                new ObjectParameter("IDVIGENCIA", iDVIGENCIA) :
                new ObjectParameter("IDVIGENCIA", typeof(decimal));

            var vIGENParameter = vIGEN != null ?
                new ObjectParameter("VIGEN", vIGEN) :
                new ObjectParameter("VIGEN", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_ESTADO_CARDINALIDAD2", iDENCParameter, cOD_FUNParameter, iDTERCEROParameter, iDINSTALACIONParameter, cARDParameter, iDVIGENCIAParameter, vIGENParameter);
        }

        public virtual int SP_SET_MODIFICAR_ESTADO(Nullable<decimal> iDESTADO)
        {
            var iDESTADOParameter = iDESTADO.HasValue ?
                new ObjectParameter("IDESTADO", iDESTADO) :
                new ObjectParameter("IDESTADO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_MODIFICAR_ESTADO", iDESTADOParameter);
        }

        public virtual int SP_SET_ELIMINAR_ENCUESTA(Nullable<decimal> iDESTADO)
        {
            var iDESTADOParameter = iDESTADO.HasValue ?
                new ObjectParameter("IDESTADO", iDESTADO) :
                new ObjectParameter("IDESTADO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_ELIMINAR_ENCUESTA", iDESTADOParameter);
        }

        public virtual int SP_SET_ELI_ENC_CARDINALIAD(Nullable<decimal> vAL, Nullable<decimal> iNST)
        {
            var vALParameter = vAL.HasValue ?
                new ObjectParameter("VAL", vAL) :
                new ObjectParameter("VAL", typeof(decimal));

            var iNSTParameter = iNST.HasValue ?
                new ObjectParameter("INST", iNST) :
                new ObjectParameter("INST", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_ELI_ENC_CARDINALIAD", vALParameter, iNSTParameter);
        }

        public virtual int SP_SET_CLONAR_ENCUESTA_EXTERNO(Nullable<decimal> iDVIG, string vALOR, Nullable<decimal> cOD_FUN, Nullable<decimal> iDES, Nullable<decimal> iNSTALACION)
        {
            var iDVIGParameter = iDVIG.HasValue ?
                new ObjectParameter("IDVIG", iDVIG) :
                new ObjectParameter("IDVIG", typeof(decimal));

            var vALORParameter = vALOR != null ?
                new ObjectParameter("VALOR", vALOR) :
                new ObjectParameter("VALOR", typeof(string));

            var cOD_FUNParameter = cOD_FUN.HasValue ?
                new ObjectParameter("COD_FUN", cOD_FUN) :
                new ObjectParameter("COD_FUN", typeof(decimal));

            var iDESParameter = iDES.HasValue ?
                new ObjectParameter("IDES", iDES) :
                new ObjectParameter("IDES", typeof(decimal));

            var iNSTALACIONParameter = iNSTALACION.HasValue ?
                new ObjectParameter("INSTALACION", iNSTALACION) :
                new ObjectParameter("INSTALACION", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_CLONAR_ENCUESTA_EXTERNO", iDVIGParameter, vALORParameter, cOD_FUNParameter, iDESParameter, iNSTALACIONParameter);
        }

        public virtual int SP_SET_MODIFICARENCUESTACARD(string jSONOUT)
        {
            var jSONOUTParameter = jSONOUT != null ?
                new ObjectParameter("JSONOUT", jSONOUT) :
                new ObjectParameter("JSONOUT", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_MODIFICARENCUESTACARD", jSONOUTParameter);
        }

        public virtual int SP_SET_URLRADICADO(Nullable<decimal> iDESTADO, string uRLRAD)
        {
            var iDESTADOParameter = iDESTADO.HasValue ?
                new ObjectParameter("IDESTADO", iDESTADO) :
                new ObjectParameter("IDESTADO", typeof(decimal));

            var uRLRADParameter = uRLRAD != null ?
                new ObjectParameter("URLRAD", uRLRAD) :
                new ObjectParameter("URLRAD", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_URLRADICADO", iDESTADOParameter, uRLRADParameter);
        }

        public virtual int SP_SET_INDICE_COR(Nullable<decimal> iDDOC, string fECHARADICADO, string rADICADO, Nullable<decimal> iDTRAMITE, string aSUNTO, string nOMBREUSUARIO, string nOMBRETERCERO, Nullable<decimal> iDTERCERO, Nullable<decimal> iDINSTALACION, ObjectParameter rTA)
        {
            var iDDOCParameter = iDDOC.HasValue ?
                new ObjectParameter("IDDOC", iDDOC) :
                new ObjectParameter("IDDOC", typeof(decimal));

            var fECHARADICADOParameter = fECHARADICADO != null ?
                new ObjectParameter("FECHARADICADO", fECHARADICADO) :
                new ObjectParameter("FECHARADICADO", typeof(string));

            var rADICADOParameter = rADICADO != null ?
                new ObjectParameter("RADICADO", rADICADO) :
                new ObjectParameter("RADICADO", typeof(string));

            var iDTRAMITEParameter = iDTRAMITE.HasValue ?
                new ObjectParameter("IDTRAMITE", iDTRAMITE) :
                new ObjectParameter("IDTRAMITE", typeof(decimal));

            var aSUNTOParameter = aSUNTO != null ?
                new ObjectParameter("ASUNTO", aSUNTO) :
                new ObjectParameter("ASUNTO", typeof(string));

            var nOMBREUSUARIOParameter = nOMBREUSUARIO != null ?
                new ObjectParameter("NOMBREUSUARIO", nOMBREUSUARIO) :
                new ObjectParameter("NOMBREUSUARIO", typeof(string));

            var nOMBRETERCEROParameter = nOMBRETERCERO != null ?
                new ObjectParameter("NOMBRETERCERO", nOMBRETERCERO) :
                new ObjectParameter("NOMBRETERCERO", typeof(string));

            var iDTERCEROParameter = iDTERCERO.HasValue ?
                new ObjectParameter("IDTERCERO", iDTERCERO) :
                new ObjectParameter("IDTERCERO", typeof(decimal));

            var iDINSTALACIONParameter = iDINSTALACION.HasValue ?
                new ObjectParameter("IDINSTALACION", iDINSTALACION) :
                new ObjectParameter("IDINSTALACION", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EntitiesControlOracle.SP_SET_INDICE_COR", iDDOCParameter, fECHARADICADOParameter, rADICADOParameter, iDTRAMITEParameter, aSUNTOParameter, nOMBREUSUARIOParameter, nOMBRETERCEROParameter, iDTERCEROParameter, iDINSTALACIONParameter, rTA);
        }

        public virtual int SP_GET_CARACTERISTICASRIESGOS(Nullable<decimal> iDESTADO, Nullable<decimal> tIPO_CARAC, Nullable<decimal> pADRE_CARAC, ObjectParameter jSONOUT)
        {
            var iDESTADOParameter = iDESTADO.HasValue ?
                new ObjectParameter("IDESTADO", iDESTADO) :
                new ObjectParameter("IDESTADO", typeof(decimal));

            var tIPO_CARACParameter = tIPO_CARAC.HasValue ?
                new ObjectParameter("TIPO_CARAC", tIPO_CARAC) :
                new ObjectParameter("TIPO_CARAC", typeof(decimal));

            var pADRE_CARACParameter = pADRE_CARAC.HasValue ?
                new ObjectParameter("PADRE_CARAC", pADRE_CARAC) :
                new ObjectParameter("PADRE_CARAC", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_CARACTERISTICASRIESGOS", iDESTADOParameter, tIPO_CARACParameter, pADRE_CARACParameter, jSONOUT);
        }*/
    }
}
 