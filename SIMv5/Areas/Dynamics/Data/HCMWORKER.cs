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
    [Table("ASSETBOOK")]
    public class HcmWorker
    {
        [Key]
        public long RECID { get; set; } //
        public long Person { get; set; } //
    }
}