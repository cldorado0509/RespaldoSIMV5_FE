using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using SIM.Areas.Seguridad.Models;
using SIM.Data.Seguridad;

namespace SIM.Areas.Models
{
    /// <summary>
    /// Clase contexto para heredar y sobreescribir el metodo SaveChanges con el fin de realizar la auditoria a los cambios que realiza cada usuario
    /// </summary>
    public partial class DBContext : DbContext
    {
        public DBContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public int SaveChangesAnonymous()
        {
            return base.SaveChanges();
        }

        /*public override int SaveChanges()
        {
            // Se valida que se tenga activa la auditoria
            if ((System.Configuration.ConfigurationManager.AppSettings["ActivarAuditoria"].ToUpper() != "SI") || Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(System.Web.HttpContext.Current.User.Identity) == null)
                return base.SaveChanges();
            
            // se obtiene el id del usuario logeado
            int userId = int.Parse(Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(System.Web.HttpContext.Current.User.Identity));
                        
            //se separa los add para poder aplicar cambios y tomar los id asignados desde base de datos
            var entriesDelMod = this.ChangeTracker.Entries().Where(p => p.State == EntityState.Deleted || p.State == EntityState.Modified);
            var entriesAdd = this.ChangeTracker.Entries().Where(p => p.State == EntityState.Added).ToList();
            List<LOG_AUDITORIA> cambios = new List<LOG_AUDITORIA>();
            
            // Se obtienen los cambios realizados para cada entidad que se modifica o elimina
            foreach (var ent in entriesDelMod)
            {
                foreach (LOG_AUDITORIA x in getCambiosRealizados(ent, userId))
                    cambios.Add(x);
            }

            // Se aplican los cambios para obtener los id de los registros nuevos
            int rSaveChanges = base.SaveChanges();

            // Se obtienen los nuevos registros insertados
            foreach (var ent in entriesAdd)
            {
                foreach (LOG_AUDITORIA x in getCambiosRealizados(ent, userId))
                    cambios.Add(x);
            }

            // Si se realizaron cambios en el modelo de datos se registran en la auditoria del sistema
            if (cambios.Count > 0)
            {
                foreach (LOG_AUDITORIA x in cambios)
                   // dbSeguridad.LOG_AUDITORIA.Add(x);
                    ((EntitiesSIMOracle)this).LOG_AUDITORIA.Add(x);
                base.SaveChanges();
            }
            return rSaveChanges;
        }

        private List<LOG_AUDITORIA> getCambiosRealizados(DbEntityEntry dbEntry, int userId)
        {
            var objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(dbEntry.Entity);
            List<LOG_AUDITORIA> cambios = new List<LOG_AUDITORIA>();
            string cIdnombre = string.Empty;
            string cIdvalor = string.Empty;

            DateTime fechaRegistro = DateTime.Now;
            string tabla = objectStateEntry.EntitySet.Name;
            //Se concatenan los diferentes coampos que componen el primary key
            foreach (var k in objectStateEntry.EntitySet.ElementType.KeyMembers)
            {
                cIdnombre = ";" + cIdnombre + k.Name;
                cIdvalor = ";" + cIdvalor + (dbEntry.OriginalValues.GetValue<object>(k.Name) == null ? string.Empty : dbEntry.OriginalValues.GetValue<object>(k.Name).ToString());
            }
            cIdnombre = cIdnombre.Substring(1);
            cIdvalor = cIdvalor.Substring(1);


            if (objectStateEntry.State == EntityState.Deleted)
            {
                ////****** Si se cambia el modelo para guardar en ligar del id del registro almacenar todos los campos con susrepectivos valores
                //string sCampo = string.Empty;
                //string sValor = string.Empty;
                //foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                //{
                //    sCampo = sCampo + propertyName + ";";
                //    string v = dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? string.Empty : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString();
                //    sValor = sValor + v + ";";
                //}

                cambios.Add(new LOG_AUDITORIA()
                {
                    ID_USUARIO = userId,
                    D_REGISTRO = fechaRegistro,
                    S_TIPO = "D", // Deleted
                    S_TABLA = tabla,
                    S_ID_CAMPO = cIdnombre,
                    S_ID_VALOR = cIdvalor
                    //S_CAMPO = sCampo, //este si se quieren almacenar todos los valores en lugar de solo el registro
                    //S_OLD_VALOR = sValor //este si se quieren almacenar todos los valores en lugar de solo el registro
                }
                    );
            }
            else if (objectStateEntry.State == EntityState.Modified)
            {
                foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                {
                    // Se capturan solo las columnas que cambiaron
                    if (!object.Equals(dbEntry.OriginalValues.GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName)))
                    {
                        cambios.Add(new LOG_AUDITORIA()
                        {
                            ID_USUARIO = userId,
                            D_REGISTRO = fechaRegistro,
                            S_TIPO = "M",    // Modified
                            S_TABLA = tabla,
                            S_ID_CAMPO = cIdnombre,
                            S_ID_VALOR = cIdvalor,
                            S_CAMPO = propertyName,
                            S_OLD_VALOR = dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? null : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString(),
                            S_NEW_VALOR = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                        }
                            );
                    }
                }
            }
            else  //EntityState.Added pero ya no tiene el estado porque los cambios fueron aceptados
            {
                ////****** Si se cambia el modelo para guardar en ligar del id del registro almacenar todos los campos con susrepectivos valores
                //string sCampo = string.Empty;
                //string sValor = string.Empty;
                //foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                //{
                //    sCampo = sCampo + propertyName + ";";
                //    string v = dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? string.Empty : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString();
                //    sValor = sValor + v + ";";
                //}

                cambios.Add(new LOG_AUDITORIA()
                {
                    ID_USUARIO = userId,
                    D_REGISTRO = fechaRegistro,
                    S_TIPO = "A",    // Added
                    S_TABLA = tabla,
                    S_ID_CAMPO = cIdnombre,
                    S_ID_VALOR = cIdvalor
                    //S_CAMPO = sCampo, //este si se quieren almacenar todos los valores en lugar de solo el registro
                    //S_NEW_VALOR = sValor //este si se quieren almacenar todos los valores en lugar de solo el registro
                }
                    );
            }

            return cambios;
        }*/
    }
}