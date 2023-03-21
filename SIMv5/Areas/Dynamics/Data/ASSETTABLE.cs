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
    [Table("ASSETTABLE")]
    public class AssetTable
    {

        public long RECID { get; set; } 
        [Key]
        public string AssetId { get; set; } 
        public string Name { get; set; } 
        public long WorkerResponsible { get; set; } 
    }
}