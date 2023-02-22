namespace SIM.Areas.General.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using SIM.Data;
    using DevExpress.Web.ASPxTreeList;
    using SIM.Data.General;

    public class TIPO_CONTACTO
    {
        public string S_CODIGO { get; set; }
        public string S_NOMBRE { get; set; }
    }

    public class ModelsToListGeneral
    {
        public static IEnumerable GetTiposDocumento()
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            return dbSIM.TIPO_DOCUMENTO.ToList();
        }

        public static IEnumerable GetTiposContacto()
        {
            List<TIPO_CONTACTO> tiposContacto = new List<TIPO_CONTACTO>();
            tiposContacto.Add(new TIPO_CONTACTO { S_CODIGO = "R", S_NOMBRE = "Rep. Legal" });

            return tiposContacto;
        }

        public static dynamic GetTiposDocumentoNatural()
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            var tiposDocumentoConsulta = from tiposDocumento in dbSIM.TIPO_DOCUMENTO
                                         where tiposDocumento.ID_TIPODOCUMENTO != 2
                                         select new
                                         {
                                             tiposDocumento.ID_TIPODOCUMENTO,
                                             tiposDocumento.S_ABREVIATURA,
                                             tiposDocumento.S_DESCRIPCION,
                                             tiposDocumento.S_NOMBRE,
                                             tiposDocumento.S_TIPOPERSONA
                                         };

            return tiposDocumentoConsulta;
        }

        public static IEnumerable GetTiposDocumentoJuridica()
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            var tiposDocumentoConsulta = from tiposDocumento in dbSIM.TIPO_DOCUMENTO
                                         where tiposDocumento.ID_TIPODOCUMENTO == 2
                                         select new
                                         {
                                             tiposDocumento.ID_TIPODOCUMENTO,
                                             tiposDocumento.S_ABREVIATURA,
                                             tiposDocumento.S_DESCRIPCION,
                                             tiposDocumento.S_NOMBRE,
                                             tiposDocumento.S_TIPOPERSONA
                                         };

            return tiposDocumentoConsulta;
        }

        public static IEnumerable GetActividadesEconomicas()
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            return dbSIM.ACTIVIDAD_ECONOMICA.ToList();
        }

        public static dynamic GetTiposPersonal()
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            var tiposPersonal = from tiposper in dbSIM.TIPO_PERSONAL_DGA
                                where tiposper.ID_TIPOPERSONAL != 0
                                select new
                                {
                                    tiposper.ID_TIPOPERSONAL,
                                    tiposper.S_NOMBRE,
                                    tiposper.S_DESCRIPCION
                                };

            return tiposPersonal;

        }

        public static IEnumerable GetMunicipios()
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            var lcobjDatos = dbSIM.Database.SqlQuery<DIVIPOLA>("SELECT DIVIPOLA.* "
                + "FROM GENERAL.DIVIPOLA DIVIPOLA INNER JOIN GENERAL.DIVIPOLA_AUTORIDADAMBIENTAL DIVIPOLA_AUTORIDADAMBIENTAL "
                + "ON DIVIPOLA.ID_DIVIPOLA = DIVIPOLA_AUTORIDADAMBIENTAL.ID_DIVIPOLA "
                + "WHERE DIVIPOLA_AUTORIDADAMBIENTAL.ID_AUTORIDADAMBIENTAL = 1 ORDER BY S_NOMBRE");

            return lcobjDatos.ToList();
        }

        public static IEnumerable GetTiposVia()
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            var datos = from tiposVia in dbSIM.TIPO_VIA
                        select new
                        {
                            tiposVia.ID_TIPOVIA,
                            S_NOMBRE = tiposVia.S_NOMBRE + " (" + tiposVia.S_ABREVIATURA + ")"
                        };

            return datos.ToList();
        }

        public static IEnumerable GetLetrasVia()
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            var datos = from letrasVia in dbSIM.LETRA_VIA
                        select new
                        {
                            letrasVia.ID_LETRAVIA,
                            S_NOMBRE = letrasVia.S_NOMBRE
                        };

            return datos.ToList();
        }

        public static IEnumerable GetTiposInstalaciones()
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            var datos = (from tipoInstalacion in dbSIM.TIPO_INSTALACION
                         select new
                         {
                             tipoInstalacion.ID_TIPOINSTALACION,
                             tipoInstalacion.S_NOMBRE
                         }).OrderBy(r => r.S_NOMBRE);

            return datos.ToList();
        }

        public static IEnumerable GetTiposDeclaraciones()
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            /*var datos = (from tipoDeclaracion in dbSIM.TIPO_DECLARACION
                         select new
                         {
                             tipoDeclaracion.ID_TIPODECLARACION,
                             tipoDeclaracion.S_NOMBRE
                         }).OrderBy(r => r.S_NOMBRE);

            return datos.ToList();*/
            return null;
        }

        public static IEnumerable GetEstados()
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            var datos = (from estado in dbSIM.ESTADO
                         select new
                         {
                             estado.ID_ESTADO,
                             estado.S_NOMBRE
                         }).OrderBy(r => r.S_NOMBRE);

            return datos.ToList();
        }

        public static IEnumerable GetPermisosAmbientales()
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            return dbSIM.PERMISO_AMBIENTAL.OrderBy(id => id.ID_PERMISOAMBIENTAL).ToList();
        }

        public static IEnumerable GetPermisosAmbientales(EntitiesSIMOracle dbSIM)
        {
            return dbSIM.PERMISO_AMBIENTAL.OrderBy(id => id.ID_PERMISOAMBIENTAL).ToList();
        }

        public static IEnumerable GetAnosDGA(int? idTercero)
        {
            if (idTercero != null)
            {
                EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
                //IEnumerable<int> anos = Enumerable.Range(2008, DateTime.Today.Year - 2008 + 1);
                IEnumerable<int> anos = Enumerable.Range(DateTime.Today.Year, 1); // Solo Permite seleccionar el año actual

                var anosDGAActuales = (from anosDGA in dbSIM.DGA
                                       where anosDGA.ID_TERCERO == idTercero
                                       select anosDGA.D_ANO.Year).ToList();

                var listaAnos = (from anosPosibles in anos
                                 //where !(from anosActuales in anosDGAActuales select anosActuales).ToList().Contains(anosPosibles) // En comentario porque solamente debe permitir el año actual
                                 select new { N_ANO = anosPosibles }).ToList();

                return listaAnos;
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable GetProfesiones()
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            return dbSIM.PROFESION.Where(profesion => profesion.S_ACTIVO == "1").OrderBy(profesion => profesion.S_NOMBRE).ToList();
        }

        public static void VirtualModeCreateChildren(TreeListVirtualModeCreateChildrenEventArgs e)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            if (e.NodeObject == null)
            {
                e.Children = dbSIM.ACTIVIDAD_ECONOMICA.Where(ae => ae.ID_ACTIVIDADECONOMICAPADRE == null).OrderBy(ae => ae.S_CODIGO).ToList();
            }
            else
            {
                e.Children = dbSIM.ACTIVIDAD_ECONOMICA.Where(ae => ae.ID_ACTIVIDADECONOMICAPADRE == ((ACTIVIDAD_ECONOMICA)e.NodeObject).ID_ACTIVIDADECONOMICA).OrderBy(ae => ae.S_CODIGO).ToList();
            }
        }
        public static void VirtualModeNodeCreating(TreeListVirtualModeNodeCreatingEventArgs e)
        {
            ACTIVIDAD_ECONOMICA actividadEconomica = (ACTIVIDAD_ECONOMICA)e.NodeObject;

            e.NodeKeyValue = actividadEconomica.ID_ACTIVIDADECONOMICA;
            e.SetNodeValue("S_NOMBRE", actividadEconomica.S_NOMBRE + " (" + actividadEconomica.S_CODIGO + ")");
        }
    }
}
