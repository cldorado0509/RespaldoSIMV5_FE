using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.BPMN.Entidades
{
    public class Procesos
    {

        public int ID_PROCESO { get; set; }
        public string NOMBRE { get; set; }
        public string DESCRIPCION { get; set; }
        public bool HABILITADO { get; set; }
        public string VERSION { get; set; }


    }
}