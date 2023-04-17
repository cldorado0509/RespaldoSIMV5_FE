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
    [Table("HCMWORKER")]
    public class HcmWorker
    {
        [Key]
        public long RECID { get; set; } //
        public long Person { get; set; } //
    }
}