using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DailyStatement.Models
{
    
    public class WorkCategory
    {
        [Key]
        public int WorkCategoryId { get; set; }

        [Required, MaxLength(50)]
        [DisplayName("工作類型")]
        public string Name { get; set; }

        public virtual List<DailyInfo> DailyInfos { get; set; }
        public virtual List<Prediction> Predictions { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("排序")]
        public int OrderBy { get; set; }
    }
}