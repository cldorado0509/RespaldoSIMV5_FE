using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using SIM.Data.Control;
using SIM.Utilidades.SLExcelUtility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace SIM.Areas.EncuestaExterna
{
    public class PreguntaEncuestaPlantilla
    {
        public int ID_ENCUESTA { get; set; }
        public string S_ENCUESTA { get; set; }
        public int ID_PREGUNTA { get; set; }
        public string S_NOMBRE { get; set; }
        public int ID_TIPOPREGUNTA { get; set; }
        public string S_REQUERIDA { get; set; }
        public string SQL_OPCION_EXTERNO { get; set; }
        public int? ID_OPCION_RESPUESTA_DOM { get; set; }
    }

    public class PreguntaEncuestaOpciones
    {
        public int ID_RESPUESTA { get; set; }
        public string S_VALOR { get; set; }
        public string S_CODIGO { get; set; }
        public int? N_ORDEN { get; set; }
    }

    public class PreguntaCarga
    {
        public int ID_PREGUNTA { get; set; }
        public int ID_ENCUESTA { get; set; }
        public int ID_TIPOPREGUNTA { get; set; }
        public bool REQUERIDA { get; set; }
        public Dictionary<string, int> OPCIONES { get; set; }
        public int ORDEN { get; set; }
    }

    public class RegistroCarga
    {
        private EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public List<PreguntaCarga> Preguntas;
        public List<string> ValoresClave;

        private int idTercero;
        private int idInstalacion;
        private int idVigencia;
        private string valorVigencia;

        private int? idPreguntaClave;

        public RegistroCarga(int idTercero, int idInstalacion, int idVigencia, string valorVigencia)
        {
            this.idTercero = idTercero;
            this.idInstalacion = idInstalacion;
            this.idVigencia = idVigencia;
            this.valorVigencia = valorVigencia;
        }

        public void CargarConfiguracionPreguntas()
        {
            VIGENCIA vigencia = dbSIM.VIGENCIA.Where(v => v.ID_VIGENCIA == idVigencia).FirstOrDefault();

            idPreguntaClave = vigencia.ID_PREGUNTA_CLAVE;

            string sql = "SELECT e.ID_ENCUESTA, e.S_NOMBRE AS S_ENCUESTA, p.ID_PREGUNTA, p.S_NOMBRE, p.ID_TIPOPREGUNTA, p.SQL_OPCION_EXTERNO, ID_OPCION_RESPUESTA_DOM, NVL(ep.S_REQUERIDA, '0') AS S_REQUERIDA " +
                            "FROM CONTROL.VIGENCIA v INNER JOIN " +
                            "CONTROL.ENCUESTA_VIGENCIA ev ON v.ID_VIGENCIA = ev.ID_VIGENCIA INNER JOIN " +
                            "CONTROL.ENC_ENCUESTA e ON ev.ID_ENCUESTA = e.ID_ENCUESTA INNER JOIN " +
                            "CONTROL.FORMULARIO_ENCUESTA fe ON ev.ID_ENCUESTA = fe.ID_ENCUESTA AND fe.ID_FORMULARIO = 14 INNER JOIN " +
                            "CONTROL.ENC_ENCUESTA_PREGUNTA ep ON ev.ID_ENCUESTA = ep.ID_ENCUESTA INNER JOIN " +
                            "CONTROL.ENC_PREGUNTA p ON ep.ID_PREGUNTA = p.ID_PREGUNTA " +
                            "WHERE v.ID_VIGENCIA = " + idVigencia.ToString() + " AND p.ID_TIPOPREGUNTA NOT IN (3, 8, 9, 11, 13, 14) " +
                            "ORDER BY fe.N_ORDEN, ep.N_ORDEN";

            var preguntas = dbSIM.Database.SqlQuery<PreguntaEncuestaPlantilla>(sql).ToList();

            Preguntas = new List<PreguntaCarga>();
            foreach (var pregunta in preguntas)
            {
                Preguntas.Add(new PreguntaCarga { ID_PREGUNTA = pregunta.ID_PREGUNTA, ID_ENCUESTA = pregunta.ID_ENCUESTA, ID_TIPOPREGUNTA = pregunta.ID_TIPOPREGUNTA, OPCIONES = (pregunta.ID_TIPOPREGUNTA == 2 || pregunta.ID_TIPOPREGUNTA == 12 ? this.ListaOpciones(pregunta.ID_PREGUNTA, pregunta.ID_TIPOPREGUNTA, pregunta.SQL_OPCION_EXTERNO, pregunta.ID_OPCION_RESPUESTA_DOM) : null), REQUERIDA = (pregunta.S_REQUERIDA == "1"), ORDEN = 0 });
            }

            if (idPreguntaClave != null)
            {
                // Respuestas para la pregunta clave, para después comparar que no se repita
                string sqlClaves = "SELECT sp.S_VALOR " +
                            "FROM CONTROL.FRM_GENERICO_ESTADO ge INNER JOIN " +
                            "CONTROL.VIGENCIA_SOLUCION vs ON ge.ID_ESTADO = vs.ID_ESTADO INNER JOIN " +
                            "CONTROL.ENC_SOLUCION s ON vs.ID_ESTADO = s.ID_ESTADO INNER JOIN " +
                            "CONTROL.ENC_SOLUCION_PREGUNTAS sp ON s.ID_SOLUCION = sp.ID_SOLUCION " +
                            "WHERE ge.ID_TERCERO = " + idTercero.ToString() + " AND ge.ID_INSTALACION = " + idInstalacion.ToString() + " AND ge.ACTIVO = '0' AND vs.ID_VIGENCIA = " + idVigencia.ToString() + " AND vs.VALOR = '" + valorVigencia + "' AND sp.ID_PREGUNTA = " + idPreguntaClave.ToString();

                this.ValoresClave = dbSIM.Database.SqlQuery<string>(sqlClaves).ToList();
            }
        }

        public bool CargarEncabezados(List<string> encabezados)
        {
            int idPregunta;
            bool resultado = true;

            for (int i = 1; i < encabezados.Count; i++)
            {
                try
                {
                    idPregunta = Convert.ToInt32(encabezados[i].Substring(6, 6));

                    var pregunta = Preguntas.Find(p => p.ID_PREGUNTA == idPregunta);

                    if (pregunta == null)
                    {
                        resultado = false;

                        Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "CargaMasiva [CargarEncabezados - IdTercero: " + idTercero.ToString() + " IdInstalacion: " + idInstalacion.ToString() + " IdVigencia: " + idVigencia.ToString() + " ValorVigencia: " + valorVigencia + "] [" + GetExcelColumnName(i) + "3] : Id de Pregunta Inválida (" + idPregunta.ToString() + ").");
                    }
                    else
                    {
                        pregunta.ORDEN = i;
                    }
                }
                catch (Exception error)
                {
                    resultado = false;

                    Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "CargaMasiva [CargarEncabezados - IdTercero: " + idTercero.ToString() + " IdInstalacion: " + idInstalacion.ToString() + " IdVigencia: " + idVigencia.ToString() + " ValorVigencia: " + valorVigencia + "] [" + GetExcelColumnName(i) + "3] : Error procesando encabezado. Dato de Pregunta Inválido. " + Utilidades.LogErrores.ObtenerError(error));
                }
            }

            var preguntasSinMapear = Preguntas.Where(p => p.ORDEN == 0);
            if (preguntasSinMapear.Count() > 0)
            {
                resultado = false;

                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "CargaMasiva [CargarEncabezados - IdTercero: " + idTercero.ToString() + " IdInstalacion: " + idInstalacion.ToString() + " IdVigencia: " + idVigencia.ToString() + " ValorVigencia: " + valorVigencia + "]: Error procesando encabezado. Faltan preguntas por mapear en el archivo de Excel (" + string.Join<string>(",", preguntasSinMapear.Select(p => p.ID_PREGUNTA.ToString())) + ")");
            }

            Preguntas = Preguntas.OrderBy(p => p.ORDEN).ToList();

            return resultado;
        }

        public string ValidarRegistro(int fila, List<string> registro)
        {
            string respuesta = "";

            foreach (var pregunta in Preguntas)
            {
                try
                {
                    if (pregunta.ORDEN >= registro.Count && pregunta.REQUERIDA)
                    {
                        respuesta += "\r\n[" + GetExcelColumnName(pregunta.ORDEN + 1) + fila.ToString() + "] Dato Requerido.";
                    }
                    else if (pregunta.ORDEN >= registro.Count && !pregunta.REQUERIDA)
                    {
                        continue;
                    }

                    var dato = registro[pregunta.ORDEN].Trim();

                    if (pregunta.REQUERIDA && dato == "")
                    {
                        respuesta += "\r\n[" + GetExcelColumnName(pregunta.ORDEN + 1) + fila.ToString() + "] Dato Requerido.";
                    }

                    if (dato == "")
                        continue;

                    if (pregunta.ID_PREGUNTA == idPreguntaClave)
                    {
                        if (ValoresClave.Contains(dato))
                        {
                            respuesta += "\r\n[" + GetExcelColumnName(pregunta.ORDEN + 1) + fila.ToString() + "] Valor Clave Repetido.";

                            continue;
                        }
                        else
                        {
                            ValoresClave.Add(dato);
                        }
                    }

                    switch (pregunta.ID_TIPOPREGUNTA)
                    {
                        case 1: // SI/NO
                            if (dato.ToUpper() != "SI" && dato.ToUpper() != "NO")
                            {
                                respuesta += "\r\n[" + GetExcelColumnName(pregunta.ORDEN + 1) + fila.ToString() + "] Dato Inválido, valores posibles: SI, NO.";
                            }
                            break;
                        case 4: // Numérico
                            try
                            {
                                Convert.ToDecimal(dato);
                            } catch
                            {
                                respuesta += "\r\n[" + GetExcelColumnName(pregunta.ORDEN + 1) + fila.ToString() + "] Dato Inválido, el valor debe ser numérico.";
                            }
                            break;
                        case 6: // Fecha
                            try
                            {
                                new DateTime(1900, 1, 1).AddDays(Convert.ToDouble(dato) - 2);
                            }
                            catch
                            {
                                respuesta += "\r\n[" + GetExcelColumnName(pregunta.ORDEN + 1) + fila.ToString() + "] Dato Inválido, el valor debe ser fecha.";
                            }
                            break;
                        case 2: // Selección Simple, Dominio Simple
                        case 12:
                            try
                            {
                                if (!pregunta.OPCIONES.ContainsKey(dato.Split('|')[0]))
                                {
                                    respuesta += "\r\n[" + GetExcelColumnName(pregunta.ORDEN + 1) + fila.ToString() + "] Dato Inválido, selección inválida.";
                                }
                            }
                            catch
                            {
                                respuesta += "\r\n[" + GetExcelColumnName(pregunta.ORDEN + 1) + fila.ToString() + "] Dato Inválido, el formato del dato seleccionado es inválido.";
                            }
                            break;
                    }
                }
                catch (Exception error)
                {
                    respuesta += "\r\n[" + GetExcelColumnName(pregunta.ORDEN + 1) + fila.ToString() + "] Dato Inválido, error desconocido.";
                }
            }

            return respuesta;
        }

        public string RegistrarRegistro(int fila, List<string> registro)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            string respuesta = "";

            int idEstado = CrearEstadoEncuesta(idUsuario, idTercero, idInstalacion, idVigencia, valorVigencia, registro[0]);

            foreach (var pregunta in Preguntas)
            {
                try
                {
                    if (pregunta.ORDEN >= registro.Count && !pregunta.REQUERIDA)
                    {
                        continue;
                    }

                    var dato = registro[pregunta.ORDEN].Trim();

                    if (dato == "")
                        continue;

                    RegistrarRespuesta(idEstado, pregunta, dato.ToString());
                }
                catch (Exception error)
                {
                    respuesta += "\r\n[" + GetExcelColumnName(pregunta.ORDEN) + fila.ToString() + "] Dato Inválido, error desconocido.";
                }
            }

            return respuesta;
        }

        private int CrearEstadoEncuesta(int idUsuario, int idTercero, int idInstalacion, int idVigencia, string valorVigencia, string nombreEstado)
        {
            FRM_GENERICO_ESTADO nuevoEstado = new FRM_GENERICO_ESTADO();
            VIGENCIA_SOLUCION vigenciaEstado = new VIGENCIA_SOLUCION();

            nuevoEstado.ID_ENCUESTA = 0;
            nuevoEstado.COD_USURARIO = idUsuario.ToString();
            nuevoEstado.ID_TERCERO = idTercero;
            nuevoEstado.ID_INSTALACION = idInstalacion;
            nuevoEstado.NOMBRE = nombreEstado;
            nuevoEstado.ID_VIGENCIA = idVigencia;
            nuevoEstado.VALOR = Convert.ToDecimal(valorVigencia);
            nuevoEstado.ACTIVO = "0";
            nuevoEstado.RADICADO = "0";
            nuevoEstado.CODRADICADO = 0;
            nuevoEstado.S_CLAVE = null;

            dbSIM.Entry(nuevoEstado).State = EntityState.Added;
            dbSIM.SaveChanges();

            vigenciaEstado.ID_VIGENCIA = idVigencia;
            vigenciaEstado.VALOR = valorVigencia;
            vigenciaEstado.ID_ESTADO = nuevoEstado.ID_ESTADO;

            dbSIM.Entry(vigenciaEstado).State = EntityState.Added;
            dbSIM.SaveChanges();

            return Convert.ToInt32(nuevoEstado.ID_ESTADO);
        }

        private void RegistrarRespuesta(int idEstado, PreguntaCarga pregunta, string valor)
        {
            ENC_SOLUCION_PREGUNTAS solucionPregunta;

            ENC_SOLUCION solucion = dbSIM.ENC_SOLUCION.Where(s => s.ID_ENCUESTA == pregunta.ID_ENCUESTA && s.ID_ESTADO == idEstado).FirstOrDefault();

            if (solucion == null)
            {
                solucion = new ENC_SOLUCION();

                solucion.ID_ENCUESTA = pregunta.ID_ENCUESTA;
                solucion.ID_ESTADO = idEstado;
                solucion.ID_FORMULARIO = 14;

                dbSIM.Entry(solucion).State = EntityState.Added;
                dbSIM.SaveChanges();
            }

            solucionPregunta = new ENC_SOLUCION_PREGUNTAS();

            solucionPregunta.ID_SOLUCION = solucion.ID_SOLUCION;
            solucionPregunta.ID_PREGUNTA = pregunta.ID_PREGUNTA;
            solucionPregunta.ID_VALOR = (pregunta.ID_TIPOPREGUNTA == 2 || pregunta.ID_TIPOPREGUNTA == 12 ? (int?)(pregunta.OPCIONES[valor.Split('|')[0]]) : (pregunta.ID_TIPOPREGUNTA == 1 ? (int?)(valor == "SI" ? 1 : 0) : null));
            solucionPregunta.S_VALOR = (pregunta.ID_TIPOPREGUNTA == 5 ? valor.Trim() : null);
            solucionPregunta.D_VALOR = (pregunta.ID_TIPOPREGUNTA == 6 ? (DateTime?)new DateTime(1900, 1, 1).AddDays(Convert.ToDouble(valor) - 2) : null);
            solucionPregunta.N_VALOR = (pregunta.ID_TIPOPREGUNTA == 4 ? (decimal?)Convert.ToDecimal(valor) : null);

            dbSIM.Entry(solucionPregunta).State = EntityState.Added;
            dbSIM.SaveChanges();
        }

        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        private Dictionary<string, int> ListaOpciones(int idPregunta, int idTipoPregunta, string sqlOpcionExterno, int? idOpcionRespuestaDOM)
        {
            Dictionary<string, int> resultado = new Dictionary<string, int>();
            string sqlOpciones = "";

            switch (idTipoPregunta)
            {
                case 2: // Selección Simple
                    sqlOpciones = "SELECT ID_RESPUESTA, S_CODIGO, S_VALOR, N_ORDEN " +
                        "FROM CONTROL.ENC_OPCION_RESPUESTA " +
                        "WHERE ID_PREGUNTA =  " + idPregunta.ToString() + " " +
                        "ORDER BY N_ORDEN";
                    break;
                case 12: // Dominio Simple
                    if (sqlOpcionExterno != null && sqlOpcionExterno.Trim() != "")
                    {
                        sqlOpciones = sqlOpcionExterno;
                    }
                    else if (idOpcionRespuestaDOM != null)
                    {
                        var opcionesDOM = dbSIM.ENC_OPCION_RESPUESTA_DOM.Where(ord => ord.ID_OPCION_RESPUESTA_DOM == idOpcionRespuestaDOM).FirstOrDefault();

                        sqlOpciones = opcionesDOM.S_SQL;
                    }
                    break;
            }

            if (sqlOpciones != "") // Se optienen los datos de las opciones y se registra en el archivo de Excel
            {
                var opciones = dbSIM.Database.SqlQuery<PreguntaEncuestaOpciones>(sqlOpciones).ToList();

                foreach (var opcion in opciones)
                {
                    resultado.Add(opcion.S_CODIGO, opcion.ID_RESPUESTA);
                }
            }

            return resultado;
        }
    }

    public class CargaMasiva
    {
        private EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        //public byte[] PlantillaCargaMasiva(string pathPlantillas, int vigencia, string valor)
        public byte[] PlantillaCargaMasiva(string pathPlantillas, int vigencia)
        {
            //string path = HostingEnvironment.MapPath("~/App_Data/Plantillas/Encuestas/" + v.ToString());
            string path = pathPlantillas + "/" + vigencia.ToString();

            var vigenciaEncuesta = dbSIM.VIGENCIA.Where(r => r.ID_VIGENCIA == vigencia).FirstOrDefault();
            string fileName = Path.Combine(path, vigenciaEncuesta.S_NOMBRE_ARCHIVO + ".xlsx");

            //return File(System.IO.File.ReadAllBytes(fileName), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", vigencia.S_NOMBRE_ARCHIVO + " " + valor + ".xlsx");
            return System.IO.File.ReadAllBytes(fileName);
        }

        public void GenerarPlantillaCargaMasiva(string pathPlantillas, int vigencia)
        {
            int posCol = 2;
            int posColOpciones = 1;
            int numOpciones = 0;
            Row row;

            //string path = HostingEnvironment.MapPath("~/App_Data/Plantillas/Encuestas/" + v.ToString());
            string path = pathPlantillas + "\\" + vigencia.ToString();

            var vigenciaEncuesta = dbSIM.VIGENCIA.Where(r => r.ID_VIGENCIA == vigencia).FirstOrDefault();
            string fileName = Path.Combine(path, vigenciaEncuesta.S_NOMBRE_ARCHIVO + ".xlsx");

            if (vigenciaEncuesta != null)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (FileStream archivoPlantilla = new FileStream(fileName, FileMode.Create))
                {
                    using (SpreadsheetDocument document = SpreadsheetDocument.Create(archivoPlantilla, SpreadsheetDocumentType.Workbook))
                    //using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
                    {
                        WorkbookPart workbookPart = document.AddWorkbookPart();
                        workbookPart.Workbook = new Workbook();

                        Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                        // Hoja de Datos
                        WorksheetPart worksheetPartDatos = workbookPart.AddNewPart<WorksheetPart>();
                        worksheetPartDatos.Worksheet = new Worksheet();

                        // Hoja de Opciones
                        WorksheetPart worksheetPartOpciones = workbookPart.AddNewPart<WorksheetPart>();
                        worksheetPartOpciones.Worksheet = new Worksheet();

                        Sheet sheetDatos = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPartDatos), SheetId = 1, Name = "Datos" };
                        sheets.Append(sheetDatos);

                        Sheet sheetOpciones = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPartOpciones), SheetId = 2, Name = "Opciones" };
                        sheets.Append(sheetOpciones);

                        SheetData sheetDataDatos = new SheetData();
                        worksheetPartDatos.Worksheet = new Worksheet(sheetDataDatos);

                        SheetData sheetDataOpciones = new SheetData();
                        worksheetPartOpciones.Worksheet = new Worksheet(sheetDataOpciones);

                        Row titulo = new Row();
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(vigenciaEncuesta.NOMBRE);
                        titulo.AppendChild(cell);
                        titulo.RowIndex = 1;
                        sheetDataDatos.AppendChild(titulo);

                        sheetDataDatos.AppendChild(new Row() { RowIndex = 2 });
                        sheetDataDatos.AppendChild(new Row() { RowIndex = 3 });
                        sheetDataDatos.AppendChild(new Row() { RowIndex = 4 });
                        sheetDataDatos.AppendChild(new Row() { RowIndex = 5 });
                        sheetDataDatos.AppendChild(new Row() { RowIndex = 6 });
                        sheetDataDatos.AppendChild(new Row() { RowIndex = 7 });
                        sheetDataDatos.AppendChild(new Row() { RowIndex = 8 });

                        string sql = "SELECT e.ID_ENCUESTA, e.S_NOMBRE AS S_ENCUESTA, p.ID_PREGUNTA, p.S_NOMBRE, p.ID_TIPOPREGUNTA, p.SQL_OPCION_EXTERNO, ID_OPCION_RESPUESTA_DOM " +
                            "FROM CONTROL.VIGENCIA v INNER JOIN " +
                            "CONTROL.ENCUESTA_VIGENCIA ev ON v.ID_VIGENCIA = ev.ID_VIGENCIA INNER JOIN " +
                            "CONTROL.ENC_ENCUESTA e ON ev.ID_ENCUESTA = e.ID_ENCUESTA INNER JOIN " +
                            "CONTROL.FORMULARIO_ENCUESTA fe ON ev.ID_ENCUESTA = fe.ID_ENCUESTA AND fe.ID_FORMULARIO = 14 INNER JOIN " +
                            "CONTROL.ENC_ENCUESTA_PREGUNTA ep ON ev.ID_ENCUESTA = ep.ID_ENCUESTA INNER JOIN " +
                            "CONTROL.ENC_PREGUNTA p ON ep.ID_PREGUNTA = p.ID_PREGUNTA " +
                            "WHERE v.ID_VIGENCIA = " + vigencia.ToString() + " " +
                            "ORDER BY fe.N_ORDEN, ep.N_ORDEN";

                        var preguntas = dbSIM.Database.SqlQuery<PreguntaEncuestaPlantilla>(sql).ToList();

                        DataValidations dataValidations = new DataValidations();

                        int idEncuestaActual = -1;

                        Random r = new Random();

                        foreach (var pregunta in preguntas)
                        {
                            if (pregunta.ID_ENCUESTA != idEncuestaActual)
                            {
                                var celdaPregunta = InsertCell(4, posCol, sheetDataDatos);
                                celdaPregunta.CellValue = new CellValue(pregunta.S_ENCUESTA);
                                celdaPregunta.DataType = new EnumValue<CellValues>(CellValues.String);

                                idEncuestaActual = pregunta.ID_ENCUESTA;
                            }

                            if (pregunta.ID_TIPOPREGUNTA == 11)
                            {
                                var celdaPregunta = InsertCell(5, posCol, sheetDataDatos);
                                celdaPregunta.CellValue = new CellValue(pregunta.S_NOMBRE);
                                celdaPregunta.DataType = new EnumValue<CellValues>(CellValues.String);
                            }
                            else
                            {
                                var celdaIdPregunta = InsertCell(3, posCol, sheetDataDatos);
                                celdaIdPregunta.CellValue = new CellValue(DateTime.Today.AddDays(r.Next(36000) * (-1)).ToString("MMyydd") + pregunta.ID_PREGUNTA.ToString("000000") + DateTime.Today.AddDays(r.Next(36000) * (-1)).ToString("ddyyMM"));
                                celdaIdPregunta.DataType = new EnumValue<CellValues>(CellValues.String);
                                ((Row)celdaIdPregunta.Parent).CustomHeight = true;
                                ((Row)celdaIdPregunta.Parent).Height = 0;

                                var celdaPregunta = InsertCell(6, posCol, sheetDataDatos);
                                celdaPregunta.CellValue = new CellValue(pregunta.S_NOMBRE);
                                celdaPregunta.DataType = new EnumValue<CellValues>(CellValues.String);

                                Cell opcionCell;
                                string columnName;
                                string columnNameList;
                                DataValidation dataValidation;

                                switch (pregunta.ID_TIPOPREGUNTA)
                                {
                                    case 1:
                                        numOpciones = 2;

                                        opcionCell = InsertCell(1, posColOpciones, sheetDataOpciones);
                                        opcionCell.CellValue = new CellValue("SI");
                                        opcionCell.DataType = new EnumValue<CellValues>(CellValues.String);

                                        opcionCell = InsertCell(2, posColOpciones, sheetDataOpciones);
                                        opcionCell.CellValue = new CellValue("NO");
                                        opcionCell.DataType = new EnumValue<CellValues>(CellValues.String);

                                        columnName = GetExcelColumnName(posCol);
                                        columnNameList = GetExcelColumnName(posColOpciones);

                                        dataValidation = new DataValidation()
                                        {
                                            Formula1 = new Formula1("Opciones!$" + columnNameList + "$1:$" + columnNameList + "$" + numOpciones.ToString()),
                                            Type = DataValidationValues.List,
                                            ShowInputMessage = false,
                                            ShowErrorMessage = false,
                                            SequenceOfReferences = new ListValue<StringValue>() { InnerText = columnName + "8" }
                                        };

                                        dataValidations.Append(dataValidation);

                                        posColOpciones++;
                                        break;
                                    case 2:
                                    case 12:
                                        string sqlOpciones = "";
                                        switch (pregunta.ID_TIPOPREGUNTA)
                                        {
                                            case 2: // Selección Simple
                                                sqlOpciones = "SELECT ID_RESPUESTA, S_CODIGO, S_VALOR, N_ORDEN " +
                                                    "FROM CONTROL.ENC_OPCION_RESPUESTA " +
                                                    "WHERE ID_PREGUNTA =  " + pregunta.ID_PREGUNTA.ToString() + " " +
                                                    "ORDER BY N_ORDEN";
                                                break;
                                            case 12: // Dominio Simple
                                                if (pregunta.SQL_OPCION_EXTERNO != null && pregunta.SQL_OPCION_EXTERNO.Trim() != "")
                                                {
                                                    sqlOpciones = pregunta.SQL_OPCION_EXTERNO;
                                                }
                                                else if (pregunta.ID_OPCION_RESPUESTA_DOM != null)
                                                {
                                                    var opcionesDOM = dbSIM.ENC_OPCION_RESPUESTA_DOM.Where(ord => ord.ID_OPCION_RESPUESTA_DOM == pregunta.ID_OPCION_RESPUESTA_DOM).FirstOrDefault();

                                                    sqlOpciones = opcionesDOM.S_SQL;
                                                }
                                                break;
                                        }

                                        if (sqlOpciones != "") // Se optienen los datos de las opciones y se registra en el archivo de Excel
                                        {
                                            var opciones = dbSIM.Database.SqlQuery<PreguntaEncuestaOpciones>(sqlOpciones).ToList();
                                            int posRow = 1;

                                            numOpciones = 0;
                                            foreach (var opcion in opciones)
                                            {
                                                opcionCell = InsertCell(posRow, posColOpciones, sheetDataOpciones);

                                                opcionCell.CellValue = new CellValue((opcion.S_CODIGO ?? "") + "|" + (opcion.S_VALOR ?? ""));
                                                opcionCell.DataType = new EnumValue<CellValues>(CellValues.String);

                                                posRow++;
                                                numOpciones++;
                                            }

                                            columnName = GetExcelColumnName(posCol);
                                            columnNameList = GetExcelColumnName(posColOpciones);

                                            dataValidation = new DataValidation()
                                            {
                                                Formula1 = new Formula1("Opciones!$" + columnNameList + "$1:$" + columnNameList + "$" + numOpciones.ToString()),
                                                Type = DataValidationValues.List,
                                                ShowInputMessage = false,
                                                ShowErrorMessage = false,
                                                SequenceOfReferences = new ListValue<StringValue>() { InnerText = columnName + "8" }
                                            };

                                            dataValidations.Append(dataValidation);

                                            posColOpciones++;
                                        }
                                        break;
                                }

                                posCol++;
                            }
                        }

                        if (posColOpciones > 1)
                        {
                            //DataValidations dataValidations = new DataValidations() { Count = (UInt32Value)1U };
                            dataValidations.Count = new UInt32Value((uint)(posColOpciones - 1));
                            worksheetPartDatos.Worksheet.Append(dataValidations);
                        }

                        var celdaItem = InsertCell(8, 1, sheetDataDatos);
                        celdaItem.CellValue = new CellValue(vigenciaEncuesta.ITEM + " 1");
                        celdaItem.DataType = new EnumValue<CellValues>(CellValues.String);

                        /*row = new Row();
                        row.CustomHeight = true;
                        row.Height = 0;
                        row.Append(colIDPreguntas);
                        sheetDataDatos.AppendChild(row);

                        row = new Row();
                        row.Append(colPreguntas);
                        sheetDataDatos.AppendChild(row);*/

                        //workbookPart.Workbook.Save();





                        /*string sqlPreguntasOpciones = "SELECT p.ID_PREGUNTA, p.ID_TIPOPREGUNTA, p.SQL_OPCION_EXTERNO, ID_OPCION_RESPUESTA_DOM " +
                            "FROM CONTROL.VIGENCIA v INNER JOIN " +
                            "CONTROL.ENCUESTA_VIGENCIA ev ON v.ID_VIGENCIA = ev.ID_VIGENCIA INNER JOIN " +
                            "CONTROL.FORMULARIO_ENCUESTA fe ON ev.ID_ENCUESTA = fe.ID_ENCUESTA AND fe.ID_FORMULARIO = 14 INNER JOIN " +
                            "CONTROL.ENC_ENCUESTA_PREGUNTA ep ON ev.ID_ENCUESTA = ep.ID_ENCUESTA INNER JOIN " +
                            "CONTROL.ENC_PREGUNTA p ON ep.ID_PREGUNTA = p.ID_PREGUNTA " +
                            "WHERE v.ID_VIGENCIA = " + vigencia.ToString() + " AND ID_TIPOPREGUNTA IN (2, 12)" +
                            "ORDER BY fe.N_ORDEN, ep.N_ORDEN";

                        var preguntasOpciones = dbSIM.Database.SqlQuery<ENC_PREGUNTA>(sql).ToList();

                        List<OpenXmlElement> colPreguntas = new List<OpenXmlElement>();
                        foreach (var pregunta in preguntasOpciones)
                        {
                            colPreguntas.Add(new Cell()
                            {
                                CellValue = new CellValue(pregunta + " 2"),
                                DataType = new EnumValue<CellValues>(CellValues.String),
                            });
                        }

                        row = new Row();

                        row.Append(colPreguntas);
                        sheetDataOpciones.AppendChild(row);*/
                    }
                }
                //archivoPlantilla.Close();
                //archivoPlantilla.Dispose();

                //var result = File(archivoPlantilla.GetBuffer(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", vigencia.S_NOMBRE_ARCHIVO + ".xlsx");
                //archivoPlantilla.Dispose();

                //return result;

                //return File(System.IO.File.ReadAllBytes(fileName), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", vigencia.S_NOMBRE_ARCHIVO + ".xlsx");
            }
        }

        //private Cell InsertCell(int rowIndex, int columnIndex, Worksheet worksheet)
        private Cell InsertCell(int rowIndex, int columnIndex, SheetData sheetData)
        {
            Row row = null;
            //var sheetData = worksheet.GetFirstChild<SheetData>();

            // Check if the worksheet contains a row with the specified row index.
            row = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex == rowIndex);
            if (row == null)
            {
                row = new Row() { RowIndex = (uint)rowIndex };
                sheetData.Append(row);
            }

            // Convert column index to column name for cell reference.
            var columnName = GetExcelColumnName(columnIndex);
            var cellReference = columnName + rowIndex;      // e.g. A1

            // Check if the row contains a cell with the specified column name.
            var cell = row.Elements<Cell>()
                       .FirstOrDefault(c => c.CellReference.Value == cellReference);
            if (cell == null)
            {
                cell = new Cell() { CellReference = cellReference };
                if (row.ChildElements.Count < columnIndex)
                    row.AppendChild(cell);
                else
                    row.InsertAt(cell, (int)columnIndex);
            }

            return cell;
        }

        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        private Cell ConstructCell(string value, CellValues dataType)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }

        public string CargarPlantillaDiligenciada(int idTercero, int idInstalacion, int idVigencia, string valorVigencia, HttpPostedFileBase file)
        {
            bool ok = true;
            StringBuilder resultado = new StringBuilder();

            try
            {
                RegistroCarga configuracionEncuesta = new RegistroCarga(idTercero, idInstalacion, idVigencia, valorVigencia);

                configuracionEncuesta.CargarConfiguracionPreguntas();

                SLExcelReader archivoPlantilla = new SLExcelReader();
                var datosArchivo = archivoPlantilla.ReadExcel(file);

                // Fila 3 - Encabezados con códigos de Pregunta
                // Fila 8+ - Filas de Datos

                // Validar Registros de Encabezados de Control
                if (datosArchivo.DataRows.Count >= 3)
                {
                    var encabezados = datosArchivo.DataRows[2];

                    if ((encabezados.Count - 1) < configuracionEncuesta.Preguntas.Count)
                    {
                        resultado.Append("Formato Inválido. La cantidad de preguntas del archivo no corresponden a la cantidad de preguntas de la encuesta.");
                        ok = false;
                    }
                    else
                    {
                        // Se alimenta la estructura para el control de validaciones por columna
                        if (!configuracionEncuesta.CargarEncabezados(encabezados))
                        {
                            resultado.Append("Formato Inválido. Archivo modificado, las preguntas no corresponden exactamente a la encuesta.");
                            ok = false;
                        }
                    }
                }

                // Validar Dominios y restricciones de los registros
                if (ok)
                {
                    for (int fila = 7; fila < datosArchivo.DataRows.Count && datosArchivo.DataRows[fila][0].Trim() != ""; fila++)
                    {
                        string validacionRegistro = configuracionEncuesta.ValidarRegistro(fila+1, datosArchivo.DataRows[fila]);

                        if (validacionRegistro != "")
                        {
                            resultado.Append(validacionRegistro);
                            ok = false;
                        }
                    }
                }

                // Registrar Encuestas
                if (ok)
                {
                    for (int fila = 7; fila < datosArchivo.DataRows.Count && datosArchivo.DataRows[fila][0].Trim() != ""; fila++)
                    {
                        string validacionRegistro = configuracionEncuesta.RegistrarRegistro(fila + 1, datosArchivo.DataRows[fila]);

                        if (validacionRegistro != "")
                        {
                            resultado.Append(validacionRegistro);
                            ok = false;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "CargaMasiva [CargarPlantillaDiligenciada - IdTercero: " + idTercero.ToString() + " IdInstalacion: " + idInstalacion.ToString() + " IdVigencia: " + idVigencia.ToString() + " ValorVigencia: " + valorVigencia + "] : " + Utilidades.LogErrores.ObtenerError(error));
                return "Error Cargando Archivo. Formato Inválido";
            }

            string resultadoFinal = resultado.ToString();

            if (resultadoFinal == "")
                return "Archivo Procesado Satisfactoriamente.";
            else
                return "Se presentaron errores cargando el archivo.\r\n" + resultadoFinal;
        }
    }
}