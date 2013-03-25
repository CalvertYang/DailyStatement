using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DailyStatement.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [DisplayName("案號")]
        public string ProjectNo { get; set; }

        [DisplayName("備註")]
        public string Comment { get; set; }

        [DisplayName("開始日")]
        public DateTime? StartOn { get; set; }

        [DisplayName("結案日")]
        public DateTime? EndOn { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual List<Prediction> Predictions { get; set; }
    }
}