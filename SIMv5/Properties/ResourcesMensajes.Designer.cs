﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIM.Properties {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ResourcesMensajes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourcesMensajes() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SIM.Properties.ResourcesMensajes", typeof(ResourcesMensajes).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Tu contraseña ha sido cambiada..
        /// </summary>
        public static string msjContrasenaActualizada {
            get {
                return ResourceManager.GetString("msjContrasenaActualizada", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a La contraseña actual es incorrecta..
        /// </summary>
        public static string msjContrasenaIncorrecta {
            get {
                return ResourceManager.GetString("msjContrasenaIncorrecta", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Tu contraseña ha sido seteada..
        /// </summary>
        public static string msjContrasenaSeteada {
            get {
                return ResourceManager.GetString("msjContrasenaSeteada", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Ya existe un usuario registrado con ese correo. Por favor seleccione otro..
        /// </summary>
        public static string msjEmailYaExiste {
            get {
                return ResourceManager.GetString("msjEmailYaExiste", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Tu login externo ha sido removido..
        /// </summary>
        public static string msjLoginRemovido {
            get {
                return ResourceManager.GetString("msjLoginRemovido", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Nombre de usuario o contraseña incorrectos..
        /// </summary>
        public static string msjUsuarioContrasenaIncorrectos {
            get {
                return ResourceManager.GetString("msjUsuarioContrasenaIncorrectos", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a La fecha de vencimiento de su cuenta de usuario ya se cumplió. Consulte con el Administrador del Sistema..
        /// </summary>
        public static string msjUsuarioInactivoPorFecha {
            get {
                return ResourceManager.GetString("msjUsuarioInactivoPorFecha", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a El usuario es inválido..
        /// </summary>
        public static string msjUsuarioInvalido {
            get {
                return ResourceManager.GetString("msjUsuarioInvalido", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a El nombre de usuario ya está registrado. Por favor seleccione otro..
        /// </summary>
        public static string msjUsuarioYaExiste {
            get {
                return ResourceManager.GetString("msjUsuarioYaExiste", resourceCulture);
            }
        }
    }
}