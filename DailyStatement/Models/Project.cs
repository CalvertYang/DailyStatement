using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DailyStatement.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [DisplayName("案號")]
        [Remote("CheckProjectNoDup", "Project", HttpMethod = "POST", ErrorMessage = "您輸入案號已經有了！", AdditionalFields = "ProjectId")]
        public string ProjectNo { get; set; }

        [DisplayName("客戶名稱")]
        public string CustomerName { get; set; }

        [DisplayName("備註")]
        public string Comment { get; set; }

        [DisplayName("開始日")]
        public DateTime? StartOn { get; set; }

        [DisplayName("是否結案")]
        public bool IsClosed { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual List<Prediction> Predictions { get; set; }
    }
}