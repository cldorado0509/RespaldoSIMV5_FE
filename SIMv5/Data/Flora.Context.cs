namespace SIM.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using SIM.Data.Flora;
    
    public partial class EntitiesSIMOracle : DbContext
    {
        public virtual DbSet<ACTOR> ACTOR { get; set; }
        public virtual DbSet<FLR_ESPECIE> FLR_ESPECIE { get; set; }
        public virtual DbSet<FLR_INTERVENCION> FLR_INTERVENCION { get; set; }
        public virtual DbSet<FLR_RIESGO> FLR_RIESGO { get; set; }
        public virtual DbSet<FLR_RIESGO_EXTINCION> FLR_RIESGO_EXTINCION { get; set; }
        public virtual DbSet<FLR_SINTOMA_DM> FLR_SINTOMA_DM { get; set; }
        public virtual DbSet<FLR_SINTOMA_EF> FLR_SINTOMA_EF { get; set; }
        public virtual DbSet<INT_INTERVENCION> INT_INTERVENCION { get; set; }
        public virtual DbSet<INT_REGISTRO_FOTOGRAFICO> INT_REGISTRO_FOTOGRAFICO { get; set; }
        public virtual DbSet<INV_ESTADO_INDIVIDUO> INV_ESTADO_INDIVIDUO { get; set; }
        public virtual DbSet<INV_INDIVIDUO_DISPERSO_2> INV_INDIVIDUO_DISPERSO_2 { get; set; }
        public virtual DbSet<TEMP_DOC> TEMP_DOC { get; set; }
        public virtual DbSet<INV_INDIVIDUO_DISPERSO1> INV_INDIVIDUO_DISPERSO1 { get; set; }
        public virtual DbSet<INFO_INDIVIDUO_DISPERSO> INFO_INDIVIDUO_DISPERSO { get; set; }

        /*
        public virtual int SP_SET_ARBOLES(Nullable<decimal> iDVISITA, string jSONOUT, ObjectParameter rTA)
        {
            var iDVISITAParameter = iDVISITA.HasValue ?
                new ObjectParameter("IDVISITA", iDVISITA) :
                new ObjectParameter("IDVISITA", typeof(decimal));
    
            var jSONOUTParameter = jSONOUT != null ?
                new ObjectParameter("JSONOUT", jSONOUT) :
                new ObjectParameter("JSONOUT", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_SET_ARBOLES", iDVISITAParameter, jSONOUTParameter, rTA);
        }

        public virtual int SP_GET_DATOSF(string sTR_SQL, ObjectParameter jSONOUT)
        {
            var sTR_SQLParameter = sTR_SQL != null ?
                new ObjectParameter("STR_SQL", sTR_SQL) :
                new ObjectParameter("STR_SQL", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_DATOSF", sTR_SQLParameter, jSONOUT);
        }

        public virtual int SP_INSERTFOTO(byte[] fOTO)
        {
            var fOTOParameter = fOTO != null ?
                new ObjectParameter("FOTO", fOTO) :
                new ObjectParameter("FOTO", typeof(byte[]));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_INSERTFOTO", fOTOParameter);
        }

        public virtual int SP_SET_FOTO_TALA(Nullable<decimal> iDTEMP, byte[] fOTO)
        {
            var iDTEMPParameter = iDTEMP.HasValue ?
                new ObjectParameter("IDTEMP", iDTEMP) :
                new ObjectParameter("IDTEMP", typeof(decimal));

            var fOTOParameter = fOTO != null ?
                new ObjectParameter("FOTO", fOTO) :
                new ObjectParameter("FOTO", typeof(byte[]));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_SET_FOTO_TALA", iDTEMPParameter, fOTOParameter);
        }

        public virtual int SP_SET_DOC_TALA(Nullable<decimal> iDARBOL, byte[] dOC, Nullable<decimal> tIPO)
        {
            var iDARBOLParameter = iDARBOL.HasValue ?
                new ObjectParameter("IDARBOL", iDARBOL) :
                new ObjectParameter("IDARBOL", typeof(decimal));

            var dOCParameter = dOC != null ?
                new ObjectParameter("DOC", dOC) :
                new ObjectParameter("DOC", typeof(byte[]));

            var tIPOParameter = tIPO.HasValue ?
                new ObjectParameter("TIPO", tIPO) :
                new ObjectParameter("TIPO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_SET_DOC_TALA", iDARBOLParameter, dOCParameter, tIPOParameter);
        }

        public virtual int SP_SET_TERMINOSCONDICIONES(Nullable<decimal> iDTRAMITE, string mENSAJE, ObjectParameter rTA)
        {
            var iDTRAMITEParameter = iDTRAMITE.HasValue ?
                new ObjectParameter("IDTRAMITE", iDTRAMITE) :
                new ObjectParameter("IDTRAMITE", typeof(decimal));

            var mENSAJEParameter = mENSAJE != null ?
                new ObjectParameter("MENSAJE", mENSAJE) :
                new ObjectParameter("MENSAJE", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_SET_TERMINOSCONDICIONES", iDTRAMITEParameter, mENSAJEParameter, rTA);
        }

        public virtual int SP_ELIMINAR_FOTO_TALA(Nullable<decimal> iD_FOTO)
        {
            var iD_FOTOParameter = iD_FOTO.HasValue ?
                new ObjectParameter("ID_FOTO", iD_FOTO) :
                new ObjectParameter("ID_FOTO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_ELIMINAR_FOTO_TALA", iD_FOTOParameter);
        }

        public virtual int SP_ELIMINAR_DOC_TALA(Nullable<decimal> aRRIDARBOL)
        {
            var aRRIDARBOLParameter = aRRIDARBOL.HasValue ?
                new ObjectParameter("ARRIDARBOL", aRRIDARBOL) :
                new ObjectParameter("ARRIDARBOL", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_ELIMINAR_DOC_TALA", aRRIDARBOLParameter);
        }

        public virtual int SP_SET_ENCUESTA_ROL(Nullable<decimal> vIGENCIA, string nOMBRE)
        {
            var vIGENCIAParameter = vIGENCIA.HasValue ?
                new ObjectParameter("VIGENCIA", vIGENCIA) :
                new ObjectParameter("VIGENCIA", typeof(decimal));

            var nOMBREParameter = nOMBRE != null ?
                new ObjectParameter("NOMBRE", nOMBRE) :
                new ObjectParameter("NOMBRE", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_SET_ENCUESTA_ROL", vIGENCIAParameter, nOMBREParameter);
        }

        public virtual int SP_SET_CLONAR_ENCUESTA_EXTERNO(Nullable<decimal> iDVIG, string vALOR, Nullable<decimal> cOD_FUN, Nullable<decimal> iDES)
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

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_SET_CLONAR_ENCUESTA_EXTERNO", iDVIGParameter, vALORParameter, cOD_FUNParameter, iDESParameter);
        }

        public virtual int SP_SET_ARBOLES_CARGA_CONTRATO(string jSONOUT, Nullable<decimal> fIN, ObjectParameter rTA)
        {
            var jSONOUTParameter = jSONOUT != null ?
                new ObjectParameter("JSONOUT", jSONOUT) :
                new ObjectParameter("JSONOUT", typeof(string));

            var fINParameter = fIN.HasValue ?
                new ObjectParameter("FIN", fIN) :
                new ObjectParameter("FIN", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_SET_ARBOLES_CARGA_CONTRATO", jSONOUTParameter, fINParameter, rTA);
        }

        public virtual int SP_GET_INTV_VISITA_TEMP(Nullable<decimal> iDVISITA, ObjectParameter jSONOUT)
        {
            var iDVISITAParameter = iDVISITA.HasValue ?
                new ObjectParameter("IDVISITA", iDVISITA) :
                new ObjectParameter("IDVISITA", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_INTV_VISITA_TEMP", iDVISITAParameter, jSONOUT);
        }

        public virtual int SP_SET_INTV_VISITA_TEMP(string jSONOUT, ObjectParameter rTA)
        {
            var jSONOUTParameter = jSONOUT != null ?
                new ObjectParameter("JSONOUT", jSONOUT) :
                new ObjectParameter("JSONOUT", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_SET_INTV_VISITA_TEMP", jSONOUTParameter, rTA);
        }

        public virtual int SP_ASOCIAR_DESASOCIAR_ARBOL(Nullable<decimal> iDTEMP, Nullable<decimal> iDARBOL)
        {
            var iDTEMPParameter = iDTEMP.HasValue ?
                new ObjectParameter("IDTEMP", iDTEMP) :
                new ObjectParameter("IDTEMP", typeof(decimal));

            var iDARBOLParameter = iDARBOL.HasValue ?
                new ObjectParameter("IDARBOL", iDARBOL) :
                new ObjectParameter("IDARBOL", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_ASOCIAR_DESASOCIAR_ARBOL", iDTEMPParameter, iDARBOLParameter);
        }

        public virtual int SP_SET_USO_URBANO(Nullable<decimal> iD_USOS, string cX, string cY, string dIR, Nullable<decimal> mUNICIPIO, string uBIC)
        {
            var iD_USOSParameter = iD_USOS.HasValue ?
                new ObjectParameter("ID_USOS", iD_USOS) :
                new ObjectParameter("ID_USOS", typeof(decimal));

            var cXParameter = cX != null ?
                new ObjectParameter("CX", cX) :
                new ObjectParameter("CX", typeof(string));

            var cYParameter = cY != null ?
                new ObjectParameter("CY", cY) :
                new ObjectParameter("CY", typeof(string));

            var dIRParameter = dIR != null ?
                new ObjectParameter("DIR", dIR) :
                new ObjectParameter("DIR", typeof(string));

            var mUNICIPIOParameter = mUNICIPIO.HasValue ?
                new ObjectParameter("MUNICIPIO", mUNICIPIO) :
                new ObjectParameter("MUNICIPIO", typeof(decimal));

            var uBICParameter = uBIC != null ?
                new ObjectParameter("UBIC", uBIC) :
                new ObjectParameter("UBIC", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_SET_USO_URBANO", iD_USOSParameter, cXParameter, cYParameter, dIRParameter, mUNICIPIOParameter, uBICParameter);
        }

        public virtual int SP_GET_ENCUESTAS_REPORT(Nullable<decimal> iDESTADO, ObjectParameter jSONOUT)
        {
            var iDESTADOParameter = iDESTADO.HasValue ?
                new ObjectParameter("IDESTADO", iDESTADO) :
                new ObjectParameter("IDESTADO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_ENCUESTAS_REPORT", iDESTADOParameter, jSONOUT);
        }

        public virtual int SP_SET_FOTO_INTV(Nullable<decimal> iDTERCERO, string oBS, string jSONOUT)
        {
            var iDTERCEROParameter = iDTERCERO.HasValue ?
                new ObjectParameter("IDTERCERO", iDTERCERO) :
                new ObjectParameter("IDTERCERO", typeof(decimal));

            var oBSParameter = oBS != null ?
                new ObjectParameter("OBS", oBS) :
                new ObjectParameter("OBS", typeof(string));

            var jSONOUTParameter = jSONOUT != null ?
                new ObjectParameter("JSONOUT", jSONOUT) :
                new ObjectParameter("JSONOUT", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_SET_FOTO_INTV", iDTERCEROParameter, oBSParameter, jSONOUTParameter);
        }

        public virtual int SP_GET_COMBOHIJO(Nullable<decimal> iDPRE, Nullable<decimal> fIL, ObjectParameter jSONOUT)
        {
            var iDPREParameter = iDPRE.HasValue ?
                new ObjectParameter("IDPRE", iDPRE) :
                new ObjectParameter("IDPRE", typeof(decimal));

            var fILParameter = fIL.HasValue ?
                new ObjectParameter("FIL", fIL) :
                new ObjectParameter("FIL", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_COMBOHIJO", iDPREParameter, fILParameter, jSONOUT);
        }

        public virtual int SP_ELIMINAR_DOC_TALA_TODO()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_ELIMINAR_DOC_TALA_TODO");
        }

        public virtual int SP_GET_AGENTE_DM(ObjectParameter jSONOUT)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_AGENTE_DM", jSONOUT);
        }

        public virtual int SP_GET_ARBOLES(Nullable<decimal> iDVISITA, string lSTIDIND, ObjectParameter jSONOUT)
        {
            var iDVISITAParameter = iDVISITA.HasValue ?
                new ObjectParameter("IDVISITA", iDVISITA) :
                new ObjectParameter("IDVISITA", typeof(decimal));

            var lSTIDINDParameter = lSTIDIND != null ?
                new ObjectParameter("LSTIDIND", lSTIDIND) :
                new ObjectParameter("LSTIDIND", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_ARBOLES", iDVISITAParameter, lSTIDINDParameter, jSONOUT);
        }

        public virtual int SP_GET_ARBOLES_CARGA(Nullable<decimal> iD_TRAMITE, Nullable<decimal> iD_CONTRATO, ObjectParameter jSONOUT)
        {
            var iD_TRAMITEParameter = iD_TRAMITE.HasValue ?
                new ObjectParameter("ID_TRAMITE", iD_TRAMITE) :
                new ObjectParameter("ID_TRAMITE", typeof(decimal));

            var iD_CONTRATOParameter = iD_CONTRATO.HasValue ?
                new ObjectParameter("ID_CONTRATO", iD_CONTRATO) :
                new ObjectParameter("ID_CONTRATO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_ARBOLES_CARGA", iD_TRAMITEParameter, iD_CONTRATOParameter, jSONOUT);
        }

        public virtual int SP_GET_ENTORNO(ObjectParameter jSONOUT)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_ENTORNO", jSONOUT);
        }

        public virtual int SP_GET_ESPECIES(ObjectParameter jSONOUT)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_ESPECIES", jSONOUT);
        }

        public virtual int SP_GET_ESTADOS(ObjectParameter jSONOUT)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_ESTADOS", jSONOUT);
        }

        public virtual int SP_GET_RIESGO(ObjectParameter jSONOUT)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_RIESGO", jSONOUT);
        }

        public virtual int SP_GET_SINTOMA_DM(ObjectParameter jSONOUT)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_SINTOMA_DM", jSONOUT);
        }

        public virtual int SP_SET_ARBOLES_CARGA(string jSONOUT, Nullable<decimal> fIN, ObjectParameter rTA)
        {
            var jSONOUTParameter = jSONOUT != null ?
                new ObjectParameter("JSONOUT", jSONOUT) :
                new ObjectParameter("JSONOUT", typeof(string));

            var fINParameter = fIN.HasValue ?
                new ObjectParameter("FIN", fIN) :
                new ObjectParameter("FIN", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_SET_ARBOLES_CARGA", jSONOUTParameter, fINParameter, rTA);
        }

        public virtual int SP_SET_TRAMITE_TALA_PODA(string mENSAJ, ObjectParameter rTA, ObjectParameter rTA2, ObjectParameter rTA3, ObjectParameter rTA4)
        {
            var mENSAJParameter = mENSAJ != null ?
                new ObjectParameter("MENSAJ", mENSAJ) :
                new ObjectParameter("MENSAJ", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_SET_TRAMITE_TALA_PODA", mENSAJParameter, rTA, rTA2, rTA3, rTA4);
        }

        public virtual int SP_SET_TALA_PODA(string jSONOUT, Nullable<decimal> iDTRAMITE, Nullable<decimal> cODFUNCIONARIO, ObjectParameter rTA)
        {
            var jSONOUTParameter = jSONOUT != null ?
                new ObjectParameter("JSONOUT", jSONOUT) :
                new ObjectParameter("JSONOUT", typeof(string));

            var iDTRAMITEParameter = iDTRAMITE.HasValue ?
                new ObjectParameter("IDTRAMITE", iDTRAMITE) :
                new ObjectParameter("IDTRAMITE", typeof(decimal));

            var cODFUNCIONARIOParameter = cODFUNCIONARIO.HasValue ?
                new ObjectParameter("CODFUNCIONARIO", cODFUNCIONARIO) :
                new ObjectParameter("CODFUNCIONARIO", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_SET_TALA_PODA", jSONOUTParameter, iDTRAMITEParameter, cODFUNCIONARIOParameter, rTA);
        }

        public virtual int SP_SET_INDICE_TALA(Nullable<decimal> iDDOC, string mEDIORES, string mAIL, string tIPOUSER, string tIPOSOLI, string rEMITENTE, Nullable<decimal> iDTRAMITE, string mUNICIPIOARBOL, string dIRECCIONMUNICIPIO, string fECHARADICADO, string rADICADO, ObjectParameter rTA)
        {
            var iDDOCParameter = iDDOC.HasValue ?
                new ObjectParameter("IDDOC", iDDOC) :
                new ObjectParameter("IDDOC", typeof(decimal));

            var mEDIORESParameter = mEDIORES != null ?
                new ObjectParameter("MEDIORES", mEDIORES) :
                new ObjectParameter("MEDIORES", typeof(string));

            var mAILParameter = mAIL != null ?
                new ObjectParameter("MAIL", mAIL) :
                new ObjectParameter("MAIL", typeof(string));

            var tIPOUSERParameter = tIPOUSER != null ?
                new ObjectParameter("TIPOUSER", tIPOUSER) :
                new ObjectParameter("TIPOUSER", typeof(string));

            var tIPOSOLIParameter = tIPOSOLI != null ?
                new ObjectParameter("TIPOSOLI", tIPOSOLI) :
                new ObjectParameter("TIPOSOLI", typeof(string));

            var rEMITENTEParameter = rEMITENTE != null ?
                new ObjectParameter("REMITENTE", rEMITENTE) :
                new ObjectParameter("REMITENTE", typeof(string));

            var iDTRAMITEParameter = iDTRAMITE.HasValue ?
                new ObjectParameter("IDTRAMITE", iDTRAMITE) :
                new ObjectParameter("IDTRAMITE", typeof(decimal));

            var mUNICIPIOARBOLParameter = mUNICIPIOARBOL != null ?
                new ObjectParameter("MUNICIPIOARBOL", mUNICIPIOARBOL) :
                new ObjectParameter("MUNICIPIOARBOL", typeof(string));

            var dIRECCIONMUNICIPIOParameter = dIRECCIONMUNICIPIO != null ?
                new ObjectParameter("DIRECCIONMUNICIPIO", dIRECCIONMUNICIPIO) :
                new ObjectParameter("DIRECCIONMUNICIPIO", typeof(string));

            var fECHARADICADOParameter = fECHARADICADO != null ?
                new ObjectParameter("FECHARADICADO", fECHARADICADO) :
                new ObjectParameter("FECHARADICADO", typeof(string));

            var rADICADOParameter = rADICADO != null ?
                new ObjectParameter("RADICADO", rADICADO) :
                new ObjectParameter("RADICADO", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_SET_INDICE_TALA", iDDOCParameter, mEDIORESParameter, mAILParameter, tIPOUSERParameter, tIPOSOLIParameter, rEMITENTEParameter, iDTRAMITEParameter, mUNICIPIOARBOLParameter, dIRECCIONMUNICIPIOParameter, fECHARADICADOParameter, rADICADOParameter, rTA);
        }

        public virtual int SP_SET_INDICE_TALACOD(Nullable<decimal> iDDOC, string fECHARADICADO, string rADICADO, string nOMBRE, Nullable<decimal> iDTRAMITE, string mUNICIPIOARBOL, string dIRECCIONMUNICIPIO, ObjectParameter rTA)
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

            var nOMBREParameter = nOMBRE != null ?
                new ObjectParameter("NOMBRE", nOMBRE) :
                new ObjectParameter("NOMBRE", typeof(string));

            var iDTRAMITEParameter = iDTRAMITE.HasValue ?
                new ObjectParameter("IDTRAMITE", iDTRAMITE) :
                new ObjectParameter("IDTRAMITE", typeof(decimal));

            var mUNICIPIOARBOLParameter = mUNICIPIOARBOL != null ?
                new ObjectParameter("MUNICIPIOARBOL", mUNICIPIOARBOL) :
                new ObjectParameter("MUNICIPIOARBOL", typeof(string));

            var dIRECCIONMUNICIPIOParameter = dIRECCIONMUNICIPIO != null ?
                new ObjectParameter("DIRECCIONMUNICIPIO", dIRECCIONMUNICIPIO) :
                new ObjectParameter("DIRECCIONMUNICIPIO", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_SET_INDICE_TALACOD", iDDOCParameter, fECHARADICADOParameter, rADICADOParameter, nOMBREParameter, iDTRAMITEParameter, mUNICIPIOARBOLParameter, dIRECCIONMUNICIPIOParameter, rTA);
        }

        public virtual int SP_GET_CARAC_ENCUESTAS(Nullable<decimal> iDESTADO, Nullable<decimal> iD_TIPO_PADRE, Nullable<decimal> iD_FORM, ObjectParameter jSONOUT, ObjectParameter iD_S, ObjectParameter iD_P)
        {
            var iDESTADOParameter = iDESTADO.HasValue ?
                new ObjectParameter("IDESTADO", iDESTADO) :
                new ObjectParameter("IDESTADO", typeof(decimal));

            var iD_TIPO_PADREParameter = iD_TIPO_PADRE.HasValue ?
                new ObjectParameter("ID_TIPO_PADRE", iD_TIPO_PADRE) :
                new ObjectParameter("ID_TIPO_PADRE", typeof(decimal));

            var iD_FORMParameter = iD_FORM.HasValue ?
                new ObjectParameter("ID_FORM", iD_FORM) :
                new ObjectParameter("ID_FORM", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_CARAC_ENCUESTAS", iDESTADOParameter, iD_TIPO_PADREParameter, iD_FORMParameter, jSONOUT, iD_S, iD_P);
        }

        public virtual int SP_GET_CARAC_ENCUESTAS1(Nullable<decimal> iDESTADO, Nullable<decimal> iD_TIPO_PADRE, Nullable<decimal> iD_FORM, ObjectParameter jSONOUT, ObjectParameter iD_S, ObjectParameter iD_P)
        {
            var iDESTADOParameter = iDESTADO.HasValue ?
                new ObjectParameter("IDESTADO", iDESTADO) :
                new ObjectParameter("IDESTADO", typeof(decimal));

            var iD_TIPO_PADREParameter = iD_TIPO_PADRE.HasValue ?
                new ObjectParameter("ID_TIPO_PADRE", iD_TIPO_PADRE) :
                new ObjectParameter("ID_TIPO_PADRE", typeof(decimal));

            var iD_FORMParameter = iD_FORM.HasValue ?
                new ObjectParameter("ID_FORM", iD_FORM) :
                new ObjectParameter("ID_FORM", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_CARAC_ENCUESTAS1", iDESTADOParameter, iD_TIPO_PADREParameter, iD_FORMParameter, jSONOUT, iD_S, iD_P);
        }
        */
    }
}
