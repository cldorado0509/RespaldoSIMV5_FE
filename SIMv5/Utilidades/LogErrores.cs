﻿using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Threading;
using SIM.Data;

namespace SIM.Utilidades
{
    public class LogErrores
    {
        /// <summary>
        /// Permite almacenar un error presentado en el sistema en el log de Errores.
        /// </summary>
        /// <param name="IdCodigoUsuarioForma"></param>
        /// <param name="DetalleTecnico"></param>
        /// <returns></returns>
        public Exception GuardarError(long IdCodigoUsuarioForma, Exception e, String DetalleTecnico)
        {
            string rutaLog = ConfigurationManager.AppSettings["RutaErrores"];
            //String RutaXmlErrores = System.AppDomain.CurrentDomain.BaseDirectory + "LogErrores.xml";
            String RutaXmlErrores = rutaLog + "\\LogErrores" + DateTime.Today.ToString("yyyyMMdd") + ".xml";
            // PermisosArchivo rights = new PermisosArchivo(RutaXmlErrores);
            if (Utilidades.PermisosArchivo.VerificaAccesoDisco(RutaXmlErrores))
            {
                if (!System.IO.File.Exists(RutaXmlErrores)) File.Create(RutaXmlErrores);
                //{

                //    if (!rights.canWrite() || !rights.canRead())
                //    {
                //        throw new Exception("El archivo para el LOG No posee los permisos suficientes para el registro de las acciones.");
                //    }
                //}

                try
                {
                    if (System.IO.File.Exists(RutaXmlErrores))
                    {
                        if (e.Source == null || e.Source == "") e.Source = "Sin especificar fuente";
                        AddXMLLogError(RutaXmlErrores, IdCodigoUsuarioForma, e, DetalleTecnico);
                    }
                    else
                    {
                        if (e.Source == null) e.Source = "Sin especificar fuente";
                        CrearXMLLogError(RutaXmlErrores, IdCodigoUsuarioForma, e, DetalleTecnico);
                    }
                    return e;
                }
                catch (Exception er)
                {
                    throw new InvalidOperationException("Error al Insertar el Registro en el Archivo de Log de Errores del Sistema.", er);
                }
            }
            else return new Exception("No se pudo crear el archivo para el log de errores");
        }

        /// <summary>
        /// Crea el Archivo XML para almacenar los Errores del Sistema
        /// </summary>
        /// <param name="ArchivoXML">Ruta del Archivo XML</param>
        /// <param name="IdCodigoUsuarioForma">Identifica el Formulario y  el Usuario</param>
        /// <param name="e">Excepción devuelta por la aplicación</param>
        /// <param name="DetalleTecnico">Detalle Técnico del error</param>
        public void CrearXMLLogError(String ArchivoXML, long IdCodigoUsuarioForma, Exception e, String DetalleTecnico)
        {
            String Estacion = System.Environment.MachineName;
            String UsuarioRed = System.Environment.UserName;

            try
            {
                XmlTextWriter myXmlTextWriter = new XmlTextWriter(ArchivoXML, null);
                myXmlTextWriter.Formatting = Formatting.Indented;
                myXmlTextWriter.WriteStartDocument(false);
                myXmlTextWriter.WriteComment("Log de Errores del sistema");
                myXmlTextWriter.WriteStartElement("Errores");
                myXmlTextWriter.WriteStartElement("Error");
                myXmlTextWriter.WriteStartElement("Detalles");
                myXmlTextWriter.WriteElementString("Fecha", DateTime.Now.Date.ToShortDateString());
                myXmlTextWriter.WriteElementString("Hora", DateTime.Now.ToShortTimeString());
                myXmlTextWriter.WriteElementString("Estacion", Estacion);
                myXmlTextWriter.WriteElementString("Usuario", UsuarioRed);
                myXmlTextWriter.WriteElementString("Codigo_Usuario_Formulario", IdCodigoUsuarioForma.ToString());
                myXmlTextWriter.WriteElementString("Descripcion", Utilidades.Data.ObtenerError(e) + " - " + e.Source.ToString());
                myXmlTextWriter.WriteElementString("SQL", DetalleTecnico);
                myXmlTextWriter.WriteEndElement();
                myXmlTextWriter.WriteEndElement();
                myXmlTextWriter.WriteEndElement();
                myXmlTextWriter.Flush();
                myXmlTextWriter.WriteEndDocument();
                myXmlTextWriter.Close();
            }
            catch (Exception er)
            {
                throw new InvalidOperationException("Error al Insertar el Registro en el Archivo de Log de Errores del Sistema.", er);
            }
        }

        /// <summary>
        /// Adiciona Registro al Archivo Log Xml
        /// </summary>
        /// <param name="ArchivoXML">Ruta del Archivo XML</param>
        /// <param name="IdCodigoUsuarioForma">Identifica el Formulario y  el Usuario</param>
        /// <param name="e">Excepción devuelta por la aplicación</param>
        /// <param name="DetalleTecnico">Detalle Técnico del error</param>
        public void AddXMLLogError(String ArchivoXML, long IdCodigoUsuarioForma, Exception e, String DetalleTecnico)
        {
            String Estacion = System.Environment.MachineName;
            String UsuarioRed = System.Environment.UserName;

            try
            {
                XmlDocument Arbol = new XmlDocument();
                XmlNode Nodo;
                XmlNode NodoErrores;
                XmlNode NodoError;

                Arbol.Load(ArchivoXML);

                NodoErrores = Arbol.SelectSingleNode("Errores");

                NodoError = Arbol.CreateElement("Error");

                Nodo = Arbol.CreateElement("Fecha");
                Nodo.InnerText = DateTime.Now.Date.ToShortDateString();
                NodoError.AppendChild(Nodo);


                Nodo = Arbol.CreateElement("Hora");
                Nodo.InnerText = DateTime.Now.ToShortTimeString();
                NodoError.AppendChild(Nodo);

                Nodo = Arbol.CreateElement("Estacion");
                Nodo.InnerText = Estacion;
                NodoError.AppendChild(Nodo);

                Nodo = Arbol.CreateElement("Usuario");
                Nodo.InnerText = UsuarioRed;
                NodoError.AppendChild(Nodo);

                Nodo = Arbol.CreateElement("Codigo_Usuario_Formulario");
                Nodo.InnerText = IdCodigoUsuarioForma.ToString();
                NodoError.AppendChild(Nodo);

                Nodo = Arbol.CreateElement("Descripcion");
                Nodo.InnerText = Utilidades.Data.ObtenerError(e) + " - " + e.Source.ToString();
                NodoError.AppendChild(Nodo);

                Nodo = Arbol.CreateElement("SQL");
                Nodo.InnerText = DetalleTecnico;
                NodoError.AppendChild(Nodo);

                NodoErrores.AppendChild(NodoError);

                Arbol.Save(ArchivoXML);
            }
            catch (IOException)
            {
                throw new InvalidOperationException("El archivo esta ocupado y no se puede grabar el error");
            }
            catch (Exception er)
            {
                throw new InvalidOperationException("Error al Insertar el Registro en el Archivo de Log de Errores del Sistema.", er);
            }
        }

        public static string ObtenerError(Exception rfobjError)
        {
            string lcstrMensajeError;
            Exception lcobjErrorInterno;

            lcstrMensajeError = rfobjError.Message;

            lcobjErrorInterno = rfobjError.InnerException;
            while (lcobjErrorInterno != null)
            {
                lcstrMensajeError += "\r\n\r\n" + lcobjErrorInterno.Message;
                lcobjErrorInterno = lcobjErrorInterno.InnerException;
            }

            if (rfobjError is System.Data.Entity.Validation.DbEntityValidationException)
            {
                System.Data.Entity.Validation.DbEntityValidationException error = (System.Data.Entity.Validation.DbEntityValidationException)rfobjError;

                foreach (var entityValidationError in error.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationError.ValidationErrors)
                    {
                        lcstrMensajeError += "\r\n" + validationError.ErrorMessage;
                    }
                }
            }

            return lcstrMensajeError;
        }
    }
    public class LogEventos
    {
        private static EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// Guarda un rachivo de eventos especificos del sistema 
        /// </summary>
        /// <param name="_Usuario">Nombre del usuario que genera el evento</param>
        /// <param name="_Formulario">Identificador del formulario en ele cual se genra el evento</param>
        /// <param name="_Fecha">Fecha y hora en la cual se genra el evento</param>
        /// <param name="_Evento">Evento generado</param>
        static public void GuardaEvento(string _Formulario, long _CodFuncionario, string _Evento)
        {
            try
            {
                var _Usuario = (from Fun in dbSIM.QRY_FUNCIONARIO_ALL
                                join UFu in dbSIM.USUARIO_FUNCIONARIO on Fun.CODFUNCIONARIO equals UFu.CODFUNCIONARIO
                                join Usr in dbSIM.USUARIO on UFu.ID_USUARIO equals Usr.ID_USUARIO
                                where Fun.CODFUNCIONARIO == _CodFuncionario
                                select new
                                {
                                    Funcionario = Fun.NOMBRES,
                                    Usuario = (Usr.S_NOMBRES + " " + Usr.S_APELLIDOS)
                                }).FirstOrDefault();
                string rutaLog = ConfigurationManager.AppSettings["RutaErrores"];
                String RutaXmlEventos = rutaLog + "\\LogEventos" + DateTime.Today.ToString("yyyyMMdd") + ".xml";
                //               PermisosArchivo rights = new PermisosArchivo(RutaXmlEventos);
                if (Utilidades.PermisosArchivo.VerificaAccesoDisco(RutaXmlEventos))
                {
                    if (!System.IO.File.Exists(RutaXmlEventos)) File.Create(RutaXmlEventos);
                    //{
                    //    if (!rights.canWrite() || !rights.canRead())
                    //    {
                    //        throw new Exception("El archivo para el LOG No posee los permisos suficientes para el registro de las acciones.");
                    //    }
                    //}
                    if (_Evento == "") throw new Exception("No se indicó un evento para registrar!!");
                    try
                    {
                        if (System.IO.File.Exists(RutaXmlEventos))
                        {
                            AddXMLLogEvento(RutaXmlEventos, _Usuario.Usuario, _Formulario, _CodFuncionario, _Evento);
                        }
                        else
                        {
                            CrearXMLLogEvento(RutaXmlEventos, _Usuario.Usuario, _Formulario, _CodFuncionario, _Evento);
                        }
                    }
                    catch (Exception er)
                    {
                        throw new InvalidOperationException("Error al Insertar el Registro en el Archivo de Log de Eventos del Sistema.", er);
                    }
                }
            }
            catch (Exception er)
            {
                throw new InvalidOperationException("Error al Insertar el Registro en el Archivo de Log de Eventos del Sistema.", er);
            }
        }


        /// <summary>
        /// Adiciona Registro al Archivo Log Xml
        /// </summary>
        /// <param name="ArchivoXML">Ruta del Archivo XML</param>
        /// <param name="IdCodigoUsuarioForma">Identifica el Formulario y  el Usuario</param>
        /// <param name="e">Excepción devuelta por la aplicación</param>
        /// <param name="DetalleTecnico">Detalle Técnico del error</param>
        private static void AddXMLLogEvento(string ArchivoXML, string Usuario, string _Formulario, long _CodFuncionario, string DetalleEvento)
        {
            String Estacion = System.Environment.MachineName;

            try
            {
                XmlDocument Arbol = new XmlDocument();
                XmlNode Nodo;
                XmlNode NodoEventos;
                XmlNode NodoEvento;
                Arbol.Load(ArchivoXML);
                NodoEventos = Arbol.SelectSingleNode("Eventos");
                NodoEvento = Arbol.CreateElement("Evento");
                Nodo = Arbol.CreateElement("Fecha");
                Nodo.InnerText = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                NodoEvento.AppendChild(Nodo);
                Nodo = Arbol.CreateElement("Estacion");
                Nodo.InnerText = Estacion;
                NodoEvento.AppendChild(Nodo);
                Nodo = Arbol.CreateElement("Usuario");
                Nodo.InnerText = Usuario;
                NodoEvento.AppendChild(Nodo);
                Nodo = Arbol.CreateElement("CodFuncionario");
                Nodo.InnerText = _CodFuncionario.ToString();
                NodoEvento.AppendChild(Nodo);
                Nodo = Arbol.CreateElement("Formulario");
                Nodo.InnerText = _Formulario;
                NodoEvento.AppendChild(Nodo);
                Nodo = Arbol.CreateElement("Evento");
                Nodo.InnerText = DetalleEvento;
                NodoEvento.AppendChild(Nodo);
                NodoEventos.AppendChild(NodoEvento);
                Arbol.Save(ArchivoXML);
            }
            catch (IOException)
            {
                throw new InvalidOperationException("El archivo esta ocupado y no se puede grabar el error");
            }
            catch (Exception er)
            {
                throw new InvalidOperationException("Error al Insertar el Registro en el Archivo de Log de Errores del Sistema.", er);
            }
        }

        private static void CrearXMLLogEvento(string ArchivoXML, string Usuario, string _Formulario, long _CodFuncionario, string DetalleEvento)
        {
            String Estacion = System.Environment.MachineName;
            try
            {
                XmlTextWriter myXmlTextWriter = new XmlTextWriter(ArchivoXML, null);
                myXmlTextWriter.Formatting = Formatting.Indented;
                myXmlTextWriter.WriteStartDocument(false);
                myXmlTextWriter.WriteComment("Log de Eventos del sistema");
                myXmlTextWriter.WriteStartElement("Eventos");
                myXmlTextWriter.WriteStartElement("Evento");
                myXmlTextWriter.WriteStartElement("Detalles");
                myXmlTextWriter.WriteElementString("Fecha", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                myXmlTextWriter.WriteElementString("Estacion", Estacion);
                myXmlTextWriter.WriteElementString("Usuario", Usuario);
                myXmlTextWriter.WriteElementString("CodFuncionario", _CodFuncionario.ToString());
                myXmlTextWriter.WriteElementString("Formulario", _Formulario);
                myXmlTextWriter.WriteElementString("Evento", DetalleEvento);
                myXmlTextWriter.WriteEndElement();
                myXmlTextWriter.WriteEndElement();
                myXmlTextWriter.WriteEndElement();
                myXmlTextWriter.Flush();
                myXmlTextWriter.WriteEndDocument();
                myXmlTextWriter.Close();
            }
            catch (Exception er)
            {
                throw new InvalidOperationException("Error al Insertar el Registro en el Archivo de Log de Eventos del Sistema.", er);
            }
        }

    }

    public class Log
    {
        private static ReaderWriterLock pvrwlWriterLock = new ReaderWriterLock();

        public static void EscribirRegistro(string rfstrArchivo, string rfstrTexto)
        {
            System.Diagnostics.EventLog lcobjLog;

            string lcstrContenido = "";
            string lcstrRutaArchivo;
            FileStream lcfilArchivo;
            StreamWriter lcwriArchivo;

            lcstrRutaArchivo = rfstrArchivo;

            try
            {
                pvrwlWriterLock.AcquireWriterLock(500);

                lcfilArchivo = new FileStream(lcstrRutaArchivo, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                lcwriArchivo = new StreamWriter(lcfilArchivo);

                lcstrContenido = DateTime.Now.ToString("HH:mm:ss") + " - " + rfstrTexto + "\r\n";

                lcwriArchivo.Write(lcstrContenido);
                lcwriArchivo.Close();
                lcfilArchivo.Close();
            }
            catch (Exception lcobjError)
            {
                if (!System.Diagnostics.EventLog.SourceExists("SIP"))
                    System.Diagnostics.EventLog.CreateEventSource("SIP", "Application");

                lcobjLog = new System.Diagnostics.EventLog();
                lcobjLog.Source = "SIP";
                lcobjLog.WriteEntry(Utilidades.LogErrores.ObtenerError(lcobjError));
            }
            finally
            {
                // Ensure that the lock is released.
                pvrwlWriterLock.ReleaseReaderLock();
            }
        }
    }
}