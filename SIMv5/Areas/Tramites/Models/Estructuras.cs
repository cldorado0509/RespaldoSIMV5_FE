using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Tramites.Models
{
    public class DOCUMENTO_TEMPORAL
    {
        public int ID_DOCUMENTO { get; set; }
        public string S_DESCRIPCION { get; set; }
        public string S_RUTA { get; set; }
    }

    public class INDICE_DOCUMENTO
    {
        public int CODINDICE { get; set; }
        public string VALOR { get; set; }
    }

    /********* FORMULARIOS CONTROL Y VIGILANCIA, ACTUACIONES JURIDICAS ***********/
    public class CARACTERISTICA
    {
        private string fDESPLIEGUE_FORM;

        public int ID_CARACTERISTICA { get; set; }
        public string S_DESCRIPCION { get; set; }
        public string S_CARDINALIDAD { get; set; }
        public int N_ORDEN { get; set; }
        public string DESPLIEGUE_FORM { get { return (fDESPLIEGUE_FORM == null ? "" : fDESPLIEGUE_FORM); } set { fDESPLIEGUE_FORM = value; } }
        [JsonIgnore]
        public int ID_CARACTETERISTICA_PADRE { get; set; } /**/
        public ITEM PLANTILLA { get; set; }
        public List<CARACTERISTICA> CARACTERISTICAS { get; set; }
        public List<ITEM> ITEMCARACTERISTICA { get; set; }
    }

    public class ITEM
    {
        public int ID_CARACTERISTICA_ESTADO { get; set; }
        public string NOMBRE { get; set; }
        public int ID_ESTADO { get; set; }
        [JsonIgnore]
        public int ID_CARACTERISTICA { get; set; } /**/
        public List<VARIABLE> VARIABLES { get; set; }
    }

    public class VARIABLE
    {
        private string fS_FORMULA;
        private string fS_AYUDA;
        private string fID_VALOR;
        private string fS_VALOR;
        private string fD_VALOR;

        public int ID_VARIABLE { get; set; }
        [JsonIgnore]
        public int ID_CARACTERISTICA { get; set; } /**/
        [JsonIgnore]
        public int ID_CARACTERISTICA_ESTADO { get; set; } /**/
        public string S_NOMBRE { get; set; }
        public int ID_TIPO_DATO { get; set; }
        public string B_REQUERIDO { get; set; }
        public string S_FORMULA { get { return (fS_FORMULA == null ? "" : fS_FORMULA); } set { fS_FORMULA = value; } }
        public string S_AYUDA { get { return (fS_AYUDA == null ? "" : fS_AYUDA); } set { fS_AYUDA = value; } }
        public string ID_VALOR { get { return (fID_VALOR == null ? "" : fID_VALOR); } set { fID_VALOR = value; } }
        public decimal? N_VALOR { get; set; }
        public string S_VALOR { get { return (fS_VALOR == null ? "" : fS_VALOR); } set { fS_VALOR = value; } }
        public string D_VALOR { get { return (fD_VALOR == null ? "" : fD_VALOR); } set { fD_VALOR = value; } }
        public List<OPCION> OPCIONES { get; set; }
    }

    public class OPCION
    {
        [JsonIgnore]
        public int ID_VARIABLE { get; set; }
        public int ID_OPCION { get; set; }
        public string DESCRIPCION { get; set; }
    }

    public class RECURSO_FORMULARIOS
    {
        public int ID_RECURSO { get; set; }
        public string NOMBRE { get; set; }
        public List<FORMULARIO_FORMULARIOS> FORMULARIOS { get; set; }
    }

    public class FORMULARIO_FORMULARIOS
    {
        public int ID { get; set; }
        [JsonIgnore]
        public int ID_RECURSO { get; set; }
        public string NOMBRE { get; set; }
        public string URL { get; set; }
        public string TBL_ITEM { get; set; }
        public string TBL_ESTADOS { get; set; }
        public string S_CAMPO_NOMBRE { get; set; }
        public string S_CAMPO_ID_VISITA { get; set; }
        public string S_CAMPO_ID_ITEM { get; set; }
        public string TBL_FOTOS { get; set; }
        public string TBL_GEO { get; set; }
        public string S_CARDINALIDAD { get; set; }
        public string S_URL_MAPA { get; set; }
        public List<ITEM_FORMULARIOS> ITEMS { get; set; }
    }

    public class ITEM_FORMULARIOS
    {
        public int ID_ITEM { get; set; }
        [JsonIgnore]
        public int ID_FORMULARIO { get; set; }
        public string NOMBRE { get; set; }
        public List<ESTADO_FORMULARIOS> ESTADOS { get; set; }
        public int ESTADO { get; set; }
    }

    public class ESTADO_FORMULARIOS
    {
        public int ID_ESTADO { get; set; }
        [JsonIgnore]
        public int ID_FORMULARIO { get; set; }
        [JsonIgnore]
        public int ID_ITEM { get; set; }
        public string S_NOMBRE { get; set; }
        public string S_DESCRIPCION { get; set; }
        public int ID_VISITA { get; set; }
        public DateTime D_INICIO { get; set; }
    }
    /*******************************/

    /********* ENCUESTAS ***********/
    public class ENCUESTA
    {
        [JsonIgnore]
        public Nullable<int> ID_SOLUCION { get; set; }
        public int ID_ENCUESTA { get; set; }
        public string S_NOMBRE { get; set; }
        public string S_DESCRIPCION { get; set; }
        public string S_TIPO { get; set; }
        public Nullable<decimal> N_VALOR { get; set; }
        public int ESTADO { get; set; }
        [JsonConverter(typeof(DateConverter))]
        public Nullable<DateTime> D_SOLUCION { get; set; }
        public List<PREGUNTA> PREGUNTAS { get; set; }
    }

    public class PREGUNTA
    {
        private DateTime? dValor;
        private string xValor;
        private string yValor;
        private string sValor;
        private string sObservacion;
        private string sRango;
        private string sAyuda;

        [JsonIgnore]
        public int ID_ENCUESTA { get; set; }
        [JsonIgnore]
        public Nullable<int> ID_SOLUCION { get; set; }
        public string X_VALOR { get { return (xValor == null ? "" : xValor); } set { xValor = value; } }
        public string Y_VALOR { get { return (yValor == null ? "" : yValor); } set { yValor = value; } }
        public int ID_PREGUNTA { get; set; }
        public string S_NOMBRE { get; set; }
        public int ID_TIPOPREGUNTA { get; set; }
        public string S_AYUDA { get { return (sAyuda == null ? "" : sAyuda); } set { sAyuda = value; } }
        public string S_REQUERIDA { get; set; }
        public int N_FACTOR { get; set; }
        public int N_NIVEL { get; set; }
        public int N_TIPO_TITULO { get; set; } // 1. Normal, 2. Pequeña, 3. Mediano, 4. Grande
        public Nullable<int> ID_VALOR { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public Nullable<decimal> N_VALOR { get; set; }
        public string S_VALOR { get { return (sValor == null ? "" : sValor); } set { sValor = value; } }
        /*[JsonConverter(typeof(DateConverter))]
        public Nullable<DateTime> D_VALOR { get { return (dValor == null ? new DateTime(1900, 1, 1) : dValor); } set { dValor = value; } }*/
        public string D_VALOR { get; set; }
        public string S_OBSERVACION { get { return (sObservacion == null ? "" : sObservacion); } set { sObservacion = value; } }
        public int ID_SOLUCION_PREGUNTAS { get; set; }
        [JsonIgnore] 
        public string SQL_OPCION_EXTERNO { get; set; }
        [JsonIgnore]
        public Nullable<int> ID_OPCION_RESPUESTA_DOM { get; set; }
        public List<OPCIONES> OPCIONES { get; set; }
        public List<DEPENDENCIA> DEPENDENCIA { get; set; }
        public string RANGO { get { return (sRango == null ? "" : sRango); } set { sRango = value; } }

        public bool ShouldSerializeRANGO()
        {
            
            return (ID_TIPOPREGUNTA == 4 || ID_TIPOPREGUNTA == 10);
        }
    }

    public class OPCIONES
    {
        private string sValor;
        private string sCodigo;

        public int ID_RESPUESTA { get; set; }
        [JsonIgnore]
        public Nullable<int> ID_PREGUNTA { get; set; }
        public string S_VALOR { get { return (sValor == null ? "" : sValor); } set { sValor = value; } }
        public string S_CODIGO { get { return (sCodigo == null ? "" : sCodigo); } set { sCodigo = value; } }
        public int SELECTED { get; set; }
    }

    public class OPCIONESSELECCIONADAS
    {
        public int ID_PREGUNTA { get; set; }
        public int ID_VALOR { get; set; }
    }

    public class DEPENDENCIA
    {
        public int ID_PREGUNTA_PADRE { get; set; }
        public int ID_PREGUNTA_HIJA { get; set; }
        public string ID_HIJO { get; set; }
        public int TIPO { get; set; }
        public string OPCIONES { get; set; }
    }

    public class Estructuras
    {
        public static void EliminarNodosSinHojas(List<CARACTERISTICA> caracteristicas)
        {
            for (int cont = caracteristicas.Count - 1; cont >= 0; cont--)
            {
                if (EliminarNodosSinHojas(caracteristicas[cont]))
                {
                    caracteristicas.RemoveAt(cont);
                }
            }
        }

        public static bool EliminarNodosSinHojas(CARACTERISTICA caracteristica)
        {
            bool resultado = true;

            if (caracteristica.PLANTILLA.VARIABLES.Count > 0)
            {
                resultado = false;
            }

            if (caracteristica.CARACTERISTICAS.Count > 0)
            {
                for (int cont = caracteristica.CARACTERISTICAS.Count - 1; cont >= 0; cont--)
                {
                    if (EliminarNodosSinHojas(caracteristica.CARACTERISTICAS[cont]))
                    {
                        caracteristica.CARACTERISTICAS.RemoveAt(cont);
                    }
                }
            }

            if (caracteristica.CARACTERISTICAS.Count > 0)
            {
                resultado = false;
            }

            return resultado;
        }
    }

    public class DateConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null && value is DateTime && value.ToString().Trim() != "")
            {
                var date = Convert.ToDateTime(value);

                if (date.Year == 1900)
                {
                    writer.WriteValue("");
                }
                else
                {
                    var niceLookingDate = date.ToString("yyyy-MM-dd HH:mm:ss");
                    writer.WriteValue(niceLookingDate);
                }
            }
            else
            {
                writer.WriteValue("");
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }
    }

    public class DecimalConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is decimal)
            {
                try
                {
                    if ((decimal)Convert.ToInt32(value) == (decimal)value)
                    {
                        writer.WriteValue(Convert.ToInt32(value));
                    }
                    else
                    {
                        writer.WriteValue((decimal)value);
                    }
                }
                catch
                {
                    writer.WriteValue((decimal)value);
                }
            }
            else
            {
                writer.WriteValue(value);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }
    }
}