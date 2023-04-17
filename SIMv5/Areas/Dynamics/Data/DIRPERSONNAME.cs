namespace SIM.Areas.Dynamics.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// 
    /// </summary>
    [Table("DIRPERSONNAME")]
    public class DirPersonName
    {
        [Key]
        public long Person { get; set; } 
        public long RECID { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string MiddleName { get; set; } 
        public string AP_CO_SecondLastName { get; set; }
       
    }
}