using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using System.Web;
using DevExpress.Web.Mvc;
using SIM.Areas.ControlVigilancia.Models;
using System.Security.Claims;
using System.Data.Linq.SqlClient;
using Newtonsoft.Json;
using System.Data.OleDb;
using System.Data;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using DevExpress.BarCodes;
using System.Drawing.Imaging;
using SIM.Areas.Fotografia.Controllers;
using System.Data.Entity.Core.Objects;
using Excel;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using SIM.Utilidades;
using PdfSharp.Drawing;
using System.Text;
using PdfSharp.Pdf.Content;
using System.Web.Hosting;
using SIM.Areas.Tramites.Models;
using DevExpress.Pdf;
using DevExpress.Office.Utils;
using System.Globalization;
using Xceed.Words.NET;
using System.Net.Http.Headers;
using System.Data.Entity;

namespace SIM.Areas.General.Controllers
{
    public class FloraAPIController : ApiController
    {
        private class Arbol
        {
            public string S_NOMBRE_CIENTIFICO { get; set; }
            public string S_NOMBRE_COMUN { get; set; }
            public string S_TIPO_ARBOL { get; set; }
            public string S_FOTO { get; set; }
            public decimal CORX { get; set; }
            public decimal CORY { get; set; }
        }

        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema General y Seguridad
        /// </summary>
        EntitiesSIMOracle db = new EntitiesSIMOracle();

        /// <summary>
        /// Consulta lista de árboles registrados
        /// </summary>
        /// <returns>Lista de árboles</returns>
        [ActionName("Arboles")]
        public object GetArboles()
        {
            var arboles = ((DbContext)db).Database.SqlQuery<Arbol>("SELECT * FROM CONTROL.VWS_FLORA WHERE ROWNUM <= 200");

            return arboles.ToList();
        }
    }
}