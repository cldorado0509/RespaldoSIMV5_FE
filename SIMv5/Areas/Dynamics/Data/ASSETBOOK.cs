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
    [Table("ASSETBOOK")]
    public class AssetBook
    {
        public long RECID { get; set; }
        [Key]
        public string AssetId { get; set; } 
        public int Status { get; set; } 
    }
}