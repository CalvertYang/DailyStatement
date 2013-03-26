using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace DailyStatement.Models
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [DisplayName("帳號")]
        [Remote("CheckAccountDup", "Employee", HttpMethod = "POST", ErrorMessage = "您輸入的帳號已經有人註冊過了！", AdditionalFields = "EmployeeId")]
        public string Account { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("密碼")]
        public string Password { get; set; }

        [DisplayName("建立日期")]
        public DateTime CreateDate { get; set; }

        [DisplayName("最後登入日期")]
        public DateTime? LastLoginDate { get; set; }

        public int RankId { get; set; }

        [DisplayName("權限")]
        [Description("1: 超級管理員, 2: 一般管理員, 3: 一般人員")]
        public Rank Rank { get; set; }

        [Required, MaxLength(50)]
        [DisplayName("姓名")]
        public string Name { get; set; }

        [Required, EmailAddress, MaxLength(150)]
        [DisplayName("電子郵件")]
        [Remote("CheckEmailDup", "Employee", HttpMethod = "POST", ErrorMessage = "您輸入的電子郵件已經有人註冊過了！", AdditionalFields = "EmployeeId")]
        public string Email { get; set; }

        [Required]
        [DisplayName("接收通知訊息")]
        [Description("通知訊息用於通知管理員那些員工在一定天數內未登入")]
        public bool RecvNotify { get; set; }

        [Required]
        [DisplayName("啟用")]
        public bool Activity { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual List<DailyInfo> DailyInfos { get; set; }

        public string Company { get; set; }
    }

    

}