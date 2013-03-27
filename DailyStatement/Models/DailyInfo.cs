using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DailyStatement.Models
{
    public class DailyInfo
    {
        [Key]
        public int DailyInfoId { get; set; }

        public int? WorkCategoryId { get; set; }

        [DisplayName("工作類型")]
        public virtual WorkCategory WorkCategory { get; set; }

        [ConcurrencyCheck, Required, MaxLength(200)]
        [DisplayName("案號")]
        [Description("若尚未成案，則設為 'N'")]
        public string ProjectNo { get; set; }

        [Required, MaxLength(100)]
        [DisplayName("客戶名稱")]
        public string Customer { get; set; }

        [Required]
        [DisplayName("工作內容")]
        public string WorkContent { get; set; }

        [DisplayName("日期")]
        public DateTime CreateDate { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [DisplayName("工時")]
        public int WorkingHours { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
      
        public virtual Project Project { get; set; }
    }
}