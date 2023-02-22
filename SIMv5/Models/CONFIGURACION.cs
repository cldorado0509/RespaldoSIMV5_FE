using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Models
{
    public class CONFIGURACION
    {
        public int Tipo;
        public string Nombre;
        public Dictionary<string,CAMPO> Campos;
    }

    public class CAMPO
    {
        public string Nombre;
        public string Titulo;
        public bool Requerido;
        public int Longitud; // -1 si no tiene límite
        public string Tipo; // N Numero, S String
        public string Formato; // Solo Aplica para Numero
        public string Alineacion; // I Izquierda, D Derecha - Solo Aplica para String
        public string Control; // T TextBox, C ComboBox, CE ComboBoxExtendido, CT Constante (No Control)
        public int AnchoControl;
        public string[] ControlesDependencia; // Solo Aplica para ComboBox y ComboBox Extendido (Nombre de Campos Separados por comas ',')
        public string SQL; // Solo Aplica para ComboBox y ComboBox Extendido. Para CT Constante, poner el campo constante en esta variable
        public string ColumnasCombo; // Solo Aplica para ComboBox Extendido ([Nombre Columna]&[Titulo]&[Ancho]&[Visible S/N], ...)
        public string ColumnasVisualizar; // Solo Aplica para ComboBox Extendido ([Nombre Columna], ...)
        public string ColumnasValor; // Solo Aplica para ComboBox Extendido ([Nombre Columna]&[Formato], ...)
        public string Limpiar; // Limpia el campo al generar el Id (S/N)
        public string ControlTexto; // Determina si el texto del campo es el que se pone en el tiquete (S/N)
        public Dictionary<string, CAMPO> ControlesDependientes;
        public IEnumerable<DATOS> Datos; // Solo Aplica para ComboBox
    }

    public class DATOS
    {
        public int ID { get; set; }
        public string DESCRIPCION { get; set; }
    }

    public class DATOSEXT
    {
        public string ID_S { get; set; }
        public int ID_I { get; set; }
        public long ID_L { get; set; }
        public string S1 { get; set; }
        public string S2 { get; set; }
        public string S3 { get; set; }
        public string S4 { get; set; }
        public string S5 { get; set; }
        public string S6 { get; set; }
        public string S7 { get; set; }
        public string S8 { get; set; }
        public string S9 { get; set; }
        public int I1 { get; set; }
        public int I2 { get; set; }
        public int I3 { get; set; }
        public int I4 { get; set; }
        public int I5 { get; set; }
        public int I6 { get; set; }
        public int I7 { get; set; }
        public int I8 { get; set; }
        public int I9 { get; set; }
        public long L1 { get; set; }
        public long L2 { get; set; }
        public long L3 { get; set; }
        public long L4 { get; set; }
        public long L5 { get; set; }
        public long L6 { get; set; }
        public long L7 { get; set; }
        public long L8 { get; set; }
        public long L9 { get; set; }
    }
}