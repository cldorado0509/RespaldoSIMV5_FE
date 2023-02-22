namespace SIM.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Core.Objects.DataClasses;
    using System.Linq;
    using SIM.Data.General;

    public partial class EntitiesSIMOracle : DbContext
    {
        public virtual DbSet<ACTIVIDAD_ECONOMICA> ACTIVIDAD_ECONOMICA { get; set; }
        public virtual DbSet<AUTORIDAD_AMBIENTAL> AUTORIDAD_AMBIENTAL { get; set; }
        public virtual DbSet<AYUDAS_GENERAL> AYUDAS_GENERAL { get; set; }
        public virtual DbSet<BUSQUEDA> BUSQUEDA { get; set; }
        public virtual DbSet<CAL_LABORAL> CAL_LABORAL { get; set; }
        public virtual DbSet<CALENDARIO> CALENDARIO { get; set; }
        public virtual DbSet<CONTACTOS> CONTACTOS { get; set; }
        public virtual DbSet<CPC> CPC { get; set; }
        public virtual DbSet<DEC_INDICADOR> DEC_INDICADOR { get; set; }
        public virtual DbSet<DEC_INDICADORDETALLE> DEC_INDICADORDETALLE { get; set; }
        public virtual DbSet<DEC_INDICADORVARIABLE> DEC_INDICADORVARIABLE { get; set; }
        public virtual DbSet<DGA> DGA { get; set; }
        public virtual DbSet<DIAS_NOLABORABLES> DIAS_NOLABORABLES { get; set; }
        public virtual DbSet<DIVIPOLA> DIVIPOLA { get; set; }

        //public virtual DbSet<DMRQEMP_TAMANIO_EMPRESA> DMRQEMP_TAMANIO_EMPRESA { get; set; }
        public virtual DbSet<DOCUMENTOS_TIPOPERMISO> DOCUMENTOS_TIPOPERMISO { get; set; }
        public virtual DbSet<ESTADO> ESTADO { get; set; }
        public virtual DbSet<ESTADO_CIVIL> ESTADO_CIVIL { get; set; }
        public virtual DbSet<EVENTO> EVENTO { get; set; }
        public virtual DbSet<EVENTO_PARTICIPANTE> EVENTOPARTICIPANTES { get; set; }
        public virtual DbSet<INSTALACION> INSTALACION { get; set; }
        public virtual DbSet<JURIDICA> JURIDICA { get; set; }
        public virtual DbSet<LETRA_VIA> LETRA_VIA { get; set; }
        public virtual DbSet<LOTE_INSTALACION> LOTE_INSTALACION { get; set; }
        public virtual DbSet<MUNICIPIOS> MUNICIPIOS { get; set; }
        public virtual DbSet<NATURAL> NATURAL { get; set; }
        public virtual DbSet<PARAMETRO> PARAMETRO { get; set; }
        public virtual DbSet<PARAMETRO_INSTALACION> PARAMETRO_INSTALACION { get; set; }
        public virtual DbSet<PARAMETRO_TERCERO> PARAMETRO_TERCERO { get; set; }
        public virtual DbSet<PARAMETRO_TERCEROINSTALACION> PARAMETRO_TERCEROINSTALACION { get; set; }
        public virtual DbSet<PARAMETROS> PARAMETROS { get; set; }
        public virtual DbSet<PARTICIPANTE> PARTICIPANTE { get; set; }
        public virtual DbSet<PERIODO_BALANCE> PERIODO_BALANCE { get; set; }
        public virtual DbSet<PERIODOBALANCE_PARAMETRO> PERIODOBALANCE_PARAMETRO { get; set; }
        public virtual DbSet<PERMISO_AMBIENTAL> PERMISO_AMBIENTAL { get; set; }
        public virtual DbSet<PERSONAL_DGA> PERSONAL_DGA { get; set; }
        public virtual DbSet<PROFESION> PROFESION { get; set; }
        public virtual DbSet<REPORTE> REPORTE { get; set; }
        public virtual DbSet<REPRESENTANTE_LEGAL> REPRESENTANTE_LEGAL { get; set; }
        public virtual DbSet<TERCERO> TERCERO { get; set; }
        public virtual DbSet<TERCERO_INSTALACION> TERCERO_INSTALACION { get; set; }
        public virtual DbSet<TERCERO_INSTALACION_PROYECTO> TERCERO_INSTALACION_PROYECTO { get; set; }
        public virtual DbSet<TIPO_DOCUMENTO> TIPO_DOCUMENTO { get; set; }
        public virtual DbSet<TIPO_INSTALACION> TIPO_INSTALACION { get; set; }
        public virtual DbSet<TIPO_PERSONAL_DGA> TIPO_PERSONAL_DGA { get; set; }
        public virtual DbSet<TIPO_VIA> TIPO_VIA { get; set; }
        public virtual DbSet<VIA_EXCEPCION> VIA_EXCEPCION { get; set; }
        public virtual DbSet<ZONA> ZONA { get; set; }
        public virtual DbSet<DEPARTAMENTOS> DEPARTAMENTOS { get; set; }
        public virtual DbSet<GAE> GAE { get; set; }
        public virtual DbSet<INSTMP> INSTMP { get; set; }
        public virtual DbSet<USO_CPC> USO_CPC { get; set; }
        public virtual DbSet<QRY_CONTACTOS_MRQ> QRY_CONTACTOS_MRQ { get; set; }
        public virtual DbSet<QRY_EMPRESA_MRQ> QRY_EMPRESA_MRQ { get; set; }
        public virtual DbSet<QRY_INDICADORES_PML> QRY_INDICADORES_PML { get; set; }
        public virtual DbSet<QRY_INSTALACION_EMPRESA_MRQ> QRY_INSTALACION_EMPRESA_MRQ { get; set; }
        public virtual DbSet<QRY_SECTOR> QRY_SECTOR { get; set; }
        public virtual DbSet<QRY_TAMANIO_EMPRESA_MRQ> QRY_TAMANIO_EMPRESA_MRQ { get; set; }
        public virtual DbSet<QRYINSTALACION> QRYINSTALACION { get; set; }
        public virtual DbSet<QRYINSTALACION_TERCERO> QRYINSTALACION_TERCERO { get; set; }
        public virtual DbSet<QRYINSTALACION2> QRYINSTALACION2 { get; set; }
        public virtual DbSet<QRYINSTALACIONTODAS> QRYINSTALACIONTODAS { get; set; }
        public virtual DbSet<QRYTERCERO> QRYTERCERO { get; set; }
        public virtual DbSet<QRYTERCEROREPLEGAL> QRYTERCEROREPLEGAL { get; set; }
        /*
        public virtual int SP_AYUDA_TXT1(ObjectParameter rESP)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_AYUDA_TXT1", new ObjectParameter[] { rESP });
        }

        public virtual int UTL_FILE_TEST_WRITE(string pATH, string fILENAME, string fIRSTLINE, string sECONDLINE)
        {
            var pATHParameter = pATH != null ?
                new ObjectParameter("PATH", pATH) :
                new ObjectParameter("PATH", typeof(string));
    
            var fILENAMEParameter = fILENAME != null ?
                new ObjectParameter("FILENAME", fILENAME) :
                new ObjectParameter("FILENAME", typeof(string));
    
            var fIRSTLINEParameter = fIRSTLINE != null ?
                new ObjectParameter("FIRSTLINE", fIRSTLINE) :
                new ObjectParameter("FIRSTLINE", typeof(string));
    
            var sECONDLINEParameter = sECONDLINE != null ?
                new ObjectParameter("SECONDLINE", sECONDLINE) :
                new ObjectParameter("SECONDLINE", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UTL_FILE_TEST_WRITE", pATHParameter, fILENAMEParameter, fIRSTLINEParameter, sECONDLINEParameter);
        }*/
    }
}
