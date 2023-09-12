namespace SIM.Utilidades
{
    using SIM.Data;
    using SIM.Data.Tramites;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class DatosTarea
    {
        public decimal CODTAREA { get; set; }
        public decimal CODFUNCIONARIO { get; set; }
        public decimal? ORDEN { get; set; }
        public decimal? CODTAREAANTERIOR { get; set; }
        public decimal? CODGRUPOT { get; set; }
    }

    public class IndicesValores
    {
        public int CODINDICE { get; set; }
        public string VALOR { get; set; }
    }

    public class Documento
    {
        public int TipoDocumento { get; set; }
        public decimal Codfuncionario { get; set; }
        public decimal IdUsuario { get; set; }
        public decimal CodSerie { get; set; }
        public int Paginas { get; set; }
        public byte[] Archivo { get; set; }
        public string Extension { get; set; }
    }

    public class IndicesDocumento
    {
        public int CODINDICE { get; set; }
        public string VALOR { get; set; }
    }

    public class Tramites
    {
        static EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public static long ObtieneTareaAnterior(long codTramite, long codTarea, long orden)
        {
            if (codTramite <= 0) return -1;
            try
            {
                decimal? TarAnt = (from Utar in dbSIM.TBTRAMITETAREA
                                   where Utar.CODTRAMITE == codTramite && Utar.CODTAREA == codTarea && Utar.ORDEN == orden
                                   select Utar.CODTAREA_ANTERIOR).FirstOrDefault();
                return long.Parse(TarAnt.ToString());
            }
            catch
            {
                return -1;
            }
        }

        private DatosTarea ObtenerDatosTareaActual(long codTramite)
        {
            try
            {
                var TarAnt = (from Tta in dbSIM.TBTRAMITETAREA
                              where Tta.CODTRAMITE == codTramite
                              && Tta.COPIA == 0
                              && Tta.ORDEN == (from TT in dbSIM.TBTRAMITETAREA where TT.CODTRAMITE == codTramite && TT.COPIA == 0 select TT.ORDEN).Max()
                              orderby Tta.ORDEN
                              select new DatosTarea
                              {
                                  CODTAREA = Tta.CODTAREA,
                                  CODFUNCIONARIO = Tta.CODFUNCIONARIO,
                                  ORDEN = Tta.ORDEN,
                                  CODTAREAANTERIOR = Tta.CODTAREA_ANTERIOR
                              }).FirstOrDefault();
                return TarAnt;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        private DatosTarea ObtenerDatosTareaAnterior(long codTramite)
        {
            try
            {

                var TarAnt = (from Tta in dbSIM.TBTRAMITETAREA
                              where Tta.CODTRAMITE == codTramite
                              && Tta.COPIA == 0
                              && Tta.ORDEN == (from TT in dbSIM.TBTRAMITETAREA where TT.CODTRAMITE == codTramite && TT.COPIA == 0 select TT.ORDEN).Max() - 1
                              orderby Tta.ORDEN
                              select new DatosTarea
                              {
                                  CODTAREA = Tta.CODTAREA,
                                  CODFUNCIONARIO = Tta.CODFUNCIONARIO,
                                  CODGRUPOT = Tta.CODGRUPOT
                              }).FirstOrDefault();
                return TarAnt;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        private bool TareaEsSubproceso(decimal CodTarea)
        {
            try
            {
                var Proceso = (from Tar in dbSIM.TBTAREA
                               join Pro in dbSIM.TBPROCESO on Tar.CODPROCESO equals Pro.CODPROCESO
                               where Tar.CODTAREA == CodTarea
                               select Pro.CODPROCESO).FirstOrDefault();
                var EsSub = (from Tar in dbSIM.TBTAREA
                             where Tar.CODSUBPROCESO == Proceso
                             select Tar.CODTAREA).FirstOrDefault();
                if (EsSub > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private bool ValidarFuncionarioActivo(decimal CodigoFuncionario)
        {
            if (CodigoFuncionario == 0) return false;
            var Funcionario = (from fun in dbSIM.TBFUNCIONARIO
                               where fun.CODFUNCIONARIO == CodigoFuncionario
                               select fun.ACTIVO).FirstOrDefault();
            if (Funcionario != null)
            {
                if (Funcionario == "1") return true;
                else return false;
            }
            else return false;
        }

        private bool TareaFinal(decimal CodTarea)
        {
            if (CodTarea == 0) return false;
            var TareaFin = (from Tar in dbSIM.TBTAREA
                            where Tar.CODTAREA == CodTarea
                            select Tar.FIN).FirstOrDefault();
            if (TareaFin == 0) return false;
            else return true;
        }

        public string DevolverTramite(long CodigoTramite, long CodigoTarea, string Funcionario, int Orden, string Comentario, string ArrayMotivos)
        {
            string _Rpta = "";
            DatosTarea TareaActual = ObtenerDatosTareaActual(CodigoTramite);
            DatosTarea TareaAnterior = ObtenerDatosTareaAnterior(CodigoTramite);
            if (!TareaEsSubproceso(TareaActual.CODTAREA))
            {
                if (ValidarFuncionarioActivo(TareaAnterior.CODFUNCIONARIO))
                {
                    try
                    {
                        if (SePuedeDevolver(TareaAnterior.CODTAREA, TareaActual.CODTAREA))
                        {
                            var _TareaFinal = (from tar in dbSIM.TBTRAMITETAREA
                                               where tar.CODTRAMITE == CodigoTramite
                                               && tar.COPIA == 0
                                               && tar.ORDEN == (from TT in dbSIM.TBTRAMITETAREA where TT.CODTRAMITE == CodigoTramite && TT.COPIA == 0 select TT.ORDEN).Max()
                                               select tar).FirstOrDefault();
                            if (_TareaFinal != null)
                            {
                                // ArrayMotivos = ArrayMotivos.Trim().EndsWith(",") ? ArrayMotivos.Trim().Substring(0, ArrayMotivos.Length - 1) : ArrayMotivos;
                                string[] _Motivos = ArrayMotivos.Split(',');
                                string _TxtMotivos = "DEVOLUCIÓN – Motivos: ";
                                if (_Motivos.Length > 0)
                                {
                                    DEVOLUCION_TAREA dev = new DEVOLUCION_TAREA();
                                    foreach (string DevItem in _Motivos)
                                    {
                                        if (DevItem != "")
                                        {
                                            string[] _ArrayMotivo = DevItem.Split(';');
                                            dev.CODFUCNIONARIO = decimal.Parse(Funcionario);
                                            dev.CODTRAMITE = CodigoTramite;
                                            dev.CODTAREA = TareaActual.CODTAREA;
                                            dev.CODTAREA_DEV = TareaAnterior.CODTAREA;
                                            dev.ORDEN = Orden;
                                            dev.ID_MOTIVODEV = long.Parse(_ArrayMotivo[0]);
                                            dbSIM.DEVOLUCION_TAREA.Add(dev);
                                            dbSIM.SaveChanges();
                                            _TxtMotivos += _ArrayMotivo[1] + ", ";
                                        }
                                    }
                                    _TxtMotivos = _TxtMotivos.Trim().EndsWith(",") ? _TxtMotivos.Trim().Substring(0, _TxtMotivos.Trim().Length - 1) : _TxtMotivos.Trim();
                                    Comentario = _TxtMotivos + " MENSAJE: " + Comentario;
                                }
                                _TareaFinal.FECHAFIN = DateTime.Now;
                                _TareaFinal.ESTADO = 1;
                                _TareaFinal.COMENTARIO = Comentario;
                                dbSIM.Entry(_TareaFinal).State = System.Data.Entity.EntityState.Modified;
                                dbSIM.SaveChanges();
                                TBTRAMITETAREA tta = new TBTRAMITETAREA();
                                tta.CODTRAMITE = CodigoTramite;
                                tta.CODTAREA = TareaAnterior.CODTAREA;
                                tta.CODTAREA_ANTERIOR = TareaActual.CODTAREA;
                                tta.CODFUNCIONARIO = TareaAnterior.CODFUNCIONARIO;
                                tta.COMENTARIO = "";
                                tta.FECHAINI = DateTime.Now;
                                tta.FECHAFIN = null;
                                tta.ORDEN = Orden + 1;
                                tta.ESTADO = 0;
                                tta.COPIA = 0;
                                tta.RECIBIDA = 0;
                                tta.DEVOLUCION = "1";
                                dbSIM.TBTRAMITETAREA.Add(tta);
                                dbSIM.SaveChanges();
                                TBTAREACOMENTARIO tac = new TBTAREACOMENTARIO();
                                tac.CODTRAMITE = CodigoTramite;
                                tac.CODTAREA = TareaActual.CODTAREA;
                                tac.CODFUNCIONARIO = TareaActual.CODFUNCIONARIO;
                                tac.COMENTARIO = Comentario;
                                tac.FECHA = DateTime.Now;
                                tac.IMPORTANCIA = "0";
                                dbSIM.TBTAREACOMENTARIO.Add(tac);
                                dbSIM.SaveChanges();
                            }
                            else _Rpta = "Error: Ocurrió un problema localizando la última tarea del trámite!";
                        }
                        else _Rpta = "Error: La tarea no se puede devolver ya que la actual y la anterior son de procesos diferentes!";
                    }
                    catch (Exception ex) { _Rpta = "Error: " + ex.Message; }
                }
                else _Rpta = "Error: El funcionario al que se retorna la tarea no se encuentra activo!";
            }
            else _Rpta = "Error: La tarea es de un subproceso, favor devolverla por SIMV4";
            return _Rpta;
        }

        public bool SePuedeDevolver(decimal _TareaAnterior, decimal _TareaActual)
        {
            bool _Rpta = true;
            if (_TareaAnterior == 0 || _TareaActual == 0) return false;
            var TareaAnt = (from Tar in dbSIM.TBTAREA
                            where Tar.CODTAREA == _TareaAnterior
                            select Tar.CODPROCESO).FirstOrDefault();
            var TareaAct = (from Tar in dbSIM.TBTAREA
                            where Tar.CODTAREA == _TareaActual
                            select Tar.CODPROCESO).FirstOrDefault();
            if (TareaAnt == 0 || TareaAct == 0) return false;
            if (TareaAct == TareaAnt) _Rpta = true; else _Rpta = false;
            return _Rpta;
        }

        public static decimal ObtenerFuncMenosCargaTarea(int _IdTarea)
        {
            decimal _resp = -1;
            try
            {
                var Carga = (from Tres in dbSIM.TBTAREARESPONSABLE
                             where Tres.CODTAREA == _IdTarea
                             select new
                             {
                                 Tres.CODFUNCIONARIO,
                                 CANTIDAD = (from Tta in dbSIM.TBTRAMITETAREA where Tta.COPIA == 0 && Tta.ESTADO == 0 && Tta.CODFUNCIONARIO == Tres.CODFUNCIONARIO select Tta).Count()
                             }
                            ).ToList();
                if (Carga.Count > 0)
                {
                    var CargaOrd = Carga.OrderBy(o => o.CANTIDAD);
                    _resp = CargaOrd.First().CODFUNCIONARIO;
                }

            }
            catch (Exception e)
            {
                return _resp;
            }
            return _resp;
        }

        public static List<string> ObtenerCodFuncionariosResponsables(int IdTarea)
        {
            var _Resp = new List<string>();
            try
            {
                var FuncResp = (from Fres in dbSIM.TBTAREARESPONSABLE
                                join Fun in dbSIM.QRY_FUNCIONARIO_ALL on Fres.CODFUNCIONARIO equals Fun.CODFUNCIONARIO
                                where Fres.CODTAREA == IdTarea && Fun.ACTIVO == "1"
                                select new
                                {
                                    Fres.CODFUNCIONARIO,
                                    Fun.NOMBRES
                                }).ToList();
                if (FuncResp.Count > 0)
                {
                    foreach (var item in FuncResp) _Resp.Add(item.CODFUNCIONARIO.ToString() + ";" + item.NOMBRES);
                    return _Resp;
                }
                else return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static int[] CrearTramite(int codProceso, int codTarea, int? codTareaPadre, string comentarios, string mensaje, int codFuncionarioInicial, int codFuncionarioResponsable, int[] codFuncionariosCopias, List<IndicesValores> Indices)
        {
            return CrearTramite(codProceso, codTarea, codTareaPadre, comentarios, mensaje, codFuncionarioInicial, codFuncionarioResponsable, codFuncionariosCopias, Indices, true);
        }

        public static int[] CrearTramite(int codProceso, int codTarea, int? codTareaPadre, string comentarios, string mensaje, int codFuncionarioInicial, int codFuncionarioResponsable, int[] codFuncionariosCopias, List<IndicesValores> Indices, bool sinDocumento)
        {
            decimal codNuevoTramite = 0;
            decimal codTareaInicioProceso = 0;
            int codTareaSiguiente = 0;
            int codFuncionarioSiguiente = 0;
            DateTime fechaActual = DateTime.Now;

            TBINDICETRAMITE indiceTramite;
            TBTRAMITETAREA nuevoTramiteTarea;
            TBTRAMITE nuevoTramite = new TBTRAMITE();
            nuevoTramite.CODPROCESO = Convert.ToDecimal(codProceso);
            nuevoTramite.FECHAINI = fechaActual;
            nuevoTramite.COMENTARIOS = comentarios;
            nuevoTramite.MENSAJE = mensaje;
            nuevoTramite.PRIORIDAD = 0;
            nuevoTramite.ESTADO = 0;

            dbSIM.Entry(nuevoTramite).State = System.Data.Entity.EntityState.Added;
            dbSIM.SaveChanges();

            codNuevoTramite = nuevoTramite.CODTRAMITE;

            if (Indices != null)
            {
                foreach (IndicesValores valores in Indices)
                {
                    indiceTramite = new TBINDICETRAMITE();
                    indiceTramite.CODTRAMITE = codNuevoTramite;
                    indiceTramite.CODINDICE = valores.CODINDICE;
                    indiceTramite.VALOR = valores.VALOR;
                    indiceTramite.FECHAREGISTRO = fechaActual;
                    indiceTramite.FECHAACTUALIZA = fechaActual;
                    dbSIM.Entry(indiceTramite).State = System.Data.Entity.EntityState.Added;
                    dbSIM.SaveChanges();
                }
            }

            codTareaInicioProceso = dbSIM.TBTAREA.Where(t => t.CODPROCESO == codProceso && t.INICIO == 1).Select(t => t.CODTAREA).FirstOrDefault();

            if (codTarea == 0)
            {
                string sql = "SELECT CODTAREASIGUIENTE FROM TRAMITES.TBDETALLEREGLA WHERE CODTAREA = " + codTareaInicioProceso.ToString();

                codTareaSiguiente = dbSIM.Database.SqlQuery<int>(sql).FirstOrDefault();

                codFuncionarioSiguiente = dbSIM.TBTAREARESPONSABLE.Where(tr => tr.CODTAREA == codTareaSiguiente).Select(tr => tr.CODFUNCIONARIO).FirstOrDefault();
            }
            else
            {
                codTareaSiguiente = codTarea;
                codFuncionarioSiguiente = codFuncionarioResponsable;
            }

            // Se crea la tarea inicial del proceso
            nuevoTramiteTarea = new TBTRAMITETAREA();
            nuevoTramiteTarea.CODTRAMITE = codNuevoTramite;
            nuevoTramiteTarea.CODTAREA = codTareaInicioProceso;
            nuevoTramiteTarea.CODTAREA_ANTERIOR = null;
            nuevoTramiteTarea.CODTAREA_SIGUIENTE = codTareaSiguiente;
            nuevoTramiteTarea.CODFUNCIONARIO = Convert.ToDecimal(codFuncionarioInicial);
            nuevoTramiteTarea.FECHAINI = fechaActual;
            nuevoTramiteTarea.COPIA = 0;
            nuevoTramiteTarea.ORDEN = 1;
            nuevoTramiteTarea.ESTADO = 1;
            nuevoTramiteTarea.DELEGADO = 0;
            nuevoTramiteTarea.RECHAZADO = 0;
            nuevoTramiteTarea.COMENTARIO = (sinDocumento ? "Inicio Trámite Sin Documento" : comentarios);
            nuevoTramiteTarea.RECIBIDA = 0;
            nuevoTramiteTarea.IMPORTANTE = 0;
            nuevoTramiteTarea.FECHAFIN = fechaActual;
            nuevoTramiteTarea.DEVOLUCION = "0";

            dbSIM.Entry(nuevoTramiteTarea).State = System.Data.Entity.EntityState.Added;
            dbSIM.SaveChanges();

            codTareaPadre = Convert.ToInt32(codTareaInicioProceso);

            nuevoTramiteTarea = new TBTRAMITETAREA();
            nuevoTramiteTarea.CODTRAMITE = codNuevoTramite;
            nuevoTramiteTarea.CODTAREA = codTareaSiguiente;
            nuevoTramiteTarea.CODTAREA_ANTERIOR = codTareaPadre;
            nuevoTramiteTarea.CODTAREA_SIGUIENTE = null;
            nuevoTramiteTarea.CODFUNCIONARIO = Convert.ToDecimal(codFuncionarioSiguiente);
            nuevoTramiteTarea.FECHAINI = fechaActual;
            nuevoTramiteTarea.COPIA = 0;
            nuevoTramiteTarea.ORDEN = 2;
            nuevoTramiteTarea.ESTADO = 0;
            nuevoTramiteTarea.DELEGADO = 0;
            nuevoTramiteTarea.RECHAZADO = 0;
            nuevoTramiteTarea.COMENTARIO = comentarios;
            nuevoTramiteTarea.RECIBIDA = 0;
            nuevoTramiteTarea.IMPORTANTE = 0;
            nuevoTramiteTarea.FECHAFIN = null;
            nuevoTramiteTarea.DEVOLUCION = "0";

            dbSIM.Entry(nuevoTramiteTarea).State = System.Data.Entity.EntityState.Added;
            dbSIM.SaveChanges();

            if (codFuncionariosCopias != null)
            {
                foreach (int codFuncionario in codFuncionariosCopias)
                {
                    if (codFuncionario != codFuncionarioSiguiente)
                    {
                        nuevoTramiteTarea = new TBTRAMITETAREA();
                        nuevoTramiteTarea.CODTRAMITE = codNuevoTramite;
                        nuevoTramiteTarea.CODTAREA = codTareaSiguiente;
                        nuevoTramiteTarea.CODTAREA_ANTERIOR = codTareaPadre;
                        nuevoTramiteTarea.CODTAREA_SIGUIENTE = null;
                        nuevoTramiteTarea.CODFUNCIONARIO = Convert.ToDecimal(codFuncionario);
                        nuevoTramiteTarea.FECHAINI = fechaActual;
                        nuevoTramiteTarea.COPIA = 1;
                        nuevoTramiteTarea.ORDEN = 2;
                        nuevoTramiteTarea.ESTADO = 0;
                        nuevoTramiteTarea.DELEGADO = 0;
                        nuevoTramiteTarea.RECHAZADO = 0;
                        nuevoTramiteTarea.COMENTARIO = comentarios;
                        nuevoTramiteTarea.RECIBIDA = 0;
                        nuevoTramiteTarea.IMPORTANTE = 0;
                        nuevoTramiteTarea.FECHAFIN = null;
                        nuevoTramiteTarea.DEVOLUCION = "0";

                        dbSIM.Entry(nuevoTramiteTarea).State = System.Data.Entity.EntityState.Added;
                        dbSIM.SaveChanges();
                    }
                }
            }

            return new int[] { Convert.ToInt32(codNuevoTramite), codTareaSiguiente };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodTramite"></param>
        /// <returns></returns>
        public static bool ExisteTramite(decimal CodTramite)
        {
            var _Resp = false;
            try
            {
                var Tramite = (from Tra in dbSIM.TBTRAMITE where Tra.CODTRAMITE == CodTramite select Tra).FirstOrDefault();
                if (Tramite != null) return _Resp = true;
            }
            catch (Exception e)
            {
                _Resp = false;
            }
            return _Resp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        public static decimal ObtenerCodiogoFuncionario(decimal IdUsuario)
        {
            var resp = -1;
            try
            {
                resp = dbSIM.USUARIO_FUNCIONARIO.Where(w => w.ID_USUARIO == IdUsuario).Select(s => s.CODFUNCIONARIO).FirstOrDefault();
            }
            catch
            {
                return resp;
            }
            return resp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodTramite"></param>
        /// <param name="_doc"></param>
        /// <param name="_indices"></param>
        /// <returns></returns>
        public static bool AdicionaDocumentoTramite(decimal CodTramite, Documento _doc, List<IndicesDocumento> _indices)
        {
            bool _resp = false;
            try
            {
                var Codproceso = dbSIM.TBTRAMITE.Where(w => w.CODTRAMITE == CodTramite).Select(s => s.CODPROCESO).FirstOrDefault();
                if (Codproceso > 0)
                {
                    var CodDocumento = dbSIM.TBTRAMITEDOCUMENTO.Where(f => f.CODTRAMITE == CodTramite).Select(s => s.CODDOCUMENTO).Max() + 1;
                    var Ruta = dbSIM.TBRUTAPROCESO.Where(w => w.CODPROCESO == Codproceso).Select(s => s.PATH).FirstOrDefault();
                    var RutaDoc = Ruta += "\\" + SIM.Utilidades.Archivos.GetRutaDocumento((ulong)CodTramite, 100);
                    if (!File.Exists(Ruta)) Directory.CreateDirectory(Ruta);
                    RutaDoc += CodTramite.ToString() + "-" + CodDocumento.ToString().Trim() + "." + _doc.Extension;
                    if (File.Exists(RutaDoc)) File.Delete(RutaDoc);
                    if (_doc.Archivo.Length > 0) Cryptografia.GrabaMemoryStream(new MemoryStream(_doc.Archivo), RutaDoc);
                    TBTRAMITEDOCUMENTO _NuevoDoc = new TBTRAMITEDOCUMENTO();
                    _NuevoDoc.CODTRAMITE = CodTramite;
                    _NuevoDoc.CODDOCUMENTO = CodDocumento;
                    _NuevoDoc.CODSERIE = _doc.CodSerie;
                    _NuevoDoc.CODFUNCIONARIO = _doc.Codfuncionario;
                    _NuevoDoc.TIPODOCUMENTO = 1;
                    _NuevoDoc.RUTA = RutaDoc;
                    _NuevoDoc.MAPAARCHIVO = "M";
                    _NuevoDoc.MAPABD = "M";
                    _NuevoDoc.PAGINAS = _doc.Paginas;
                    _NuevoDoc.ID_USUARIO = _doc.IdUsuario;
                    _NuevoDoc.CIFRADO = "0";
                    _NuevoDoc.FECHACREACION = DateTime.Now;
                    dbSIM.TBTRAMITEDOCUMENTO.Add(_NuevoDoc);
                    dbSIM.SaveChanges();
                    TBTRAMITE_DOC _RelTraDoc = new TBTRAMITE_DOC();
                    _RelTraDoc.CODTRAMITE = _NuevoDoc.CODTRAMITE;
                    _RelTraDoc.CODDOCUMENTO = _NuevoDoc.CODDOCUMENTO;
                    _RelTraDoc.ID_DOCUMENTO = _NuevoDoc.ID_DOCUMENTO;
                    dbSIM.TBTRAMITE_DOC.Add(_RelTraDoc);
                    dbSIM.SaveChanges();
                    if (_indices.Count > 0)
                    {
                        foreach (var i in _indices)
                        {
                            TBINDICEDOCUMENTO _Ind = new TBINDICEDOCUMENTO();
                            _Ind.CODTRAMITE = _NuevoDoc.CODTRAMITE;
                            _Ind.CODDOCUMENTO = _NuevoDoc.CODDOCUMENTO;
                            _Ind.CODINDICE = i.CODINDICE;
                            _Ind.CODSERIE = _NuevoDoc.CODSERIE;
                            _Ind.VALOR = i.VALOR;
                            _Ind.ID_DOCUMENTO = _NuevoDoc.ID_DOCUMENTO;
                            dbSIM.TBINDICEDOCUMENTO.Add(_Ind);
                            dbSIM.SaveChanges();
                        }
                        string _Sql = $"INSERT INTO TRAMITES.BUSQUEDA_DOCUMENTO (COD_SERIE,COD_DOCUMENTO,S_INDICE,COD_TRAMITE,FECHADIGITALIZA,ID_DOCUMENTO) SELECT DOC.CODSERIE AS COD_SERIE,DOC.CODDOCUMENTO AS COD_DOCUMENTO,'TRAMITE: ' || DOC.CODTRAMITE || ' ' || (SELECT LISTAGG(IND.INDICE || ': ' || IDO.valor, ' - ') WITHIN GROUP(ORDER BY IDO.CODDOCUMENTO) AS valor FROM TRAMITES.TBINDICEDOCUMENTO IDO INNER JOIN TRAMITES.TBINDICESERIE IND ON IDO.CODINDICE = IND.CODINDICE WHERE DOC.CODTRAMITE = IDO.CODTRAMITE AND DOC.CODDOCUMENTO = IDO.CODDOCUMENTO GROUP BY DOC.CODTRAMITE) AS S_INDICE,DOC.CODTRAMITE AS COD_TRAMITE,DOC.FECHACREACION AS FECHADIGITALIZA, DOC.ID_DOCUMENTO FROM TRAMITES.TBTRAMITEDOCUMENTO DOC WHERE DOC.CODTRAMITE ={CodTramite} AND DOC.CODDOCUMENTO ={CodDocumento}";
                        dbSIM.Database.ExecuteSqlCommandAsync(_Sql);
                    }
                    _resp = true;
                }
                else _resp = false;
            }
            catch (Exception e)
            {
                _resp = false;
            }

            return _resp;
        }
    }
}