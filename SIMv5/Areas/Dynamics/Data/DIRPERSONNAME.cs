namespace SIM.Areas.Dynamics.Data
{
    using DevExpress.Xpo;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// 
    /// </summary>
    [Table("DIRPERSONNAME")]
    public class DirPersonName
    {
        public long RECID { get; set; }
        public string FirstName { get; set; } //
        public string LastName { get; set; } //
        public string MiddleName { get; set; } //
        [Key]
        public Int64 Person { get; set; } //
        public string AP_CO_SecondLastName { get; set; } //
    }
}