using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DailyStatement.Models
{
    public class Project
    {
        public Project()
        {
            
        }

        #region Basic Required 
        [Key]
        public int ProjectId { get; set; }

        [DisplayName("案號")]
        [Remote("CheckProjectNoDup", "Project", HttpMethod = "POST", ErrorMessage = "您輸入案號已經有了！", AdditionalFields = "ProjectId")]
        public string ProjectNo { get; set; }

        [DisplayName("公司及案件名稱")]
        public string CustomerName { get; set; }
        #endregion

        #region Responsibility of Sales
        [DisplayName("客戶代碼")]
        public string CustomerCode { get; set; }

        [DisplayName("專案總金額")]
        public int? Price { get; set; }

        [DisplayName("負責業務")]
        public virtual Employee Sale { get; set; }

        [DisplayName("訂金金額")]
        public int? DownPayment { get; set; }

        [DisplayName("下訂單日期")]
        public DateTime? OrderOn { get; set; }

        [DisplayName("收訂金日期")]
        public DateTime? GetDownPaymentOn { get; set; }

        [DisplayName("交機款金額")]
        public int? HandoverPrice { get; set; }

        [DisplayName("交機款預估日期")]
        public DateTime? HandoverPriceExpectOn { get; set; }

        [DisplayName("裝機款金額")]
        public int? ConstitutePrice { get; set; }

        [DisplayName("裝機款預計收款日")]
        public DateTime? ConstitutePriceExpectOn { get; set; }

        [DisplayName("驗收款金額")]
        public int? CheckPrice { get; set; }

        [DisplayName("驗收款預估收款日期")]
        public DateTime? CheckPriceExpectOn { get; set; }

        [DisplayName("保固款金額")]
        public int? WarrantyPrice { get; set; }

        [DisplayName("保固款預估收款日")]
        public DateTime? WarrantyPriceExpectOn { get; set; }
        
        #endregion

        #region Responsibility of Engineer
        [DisplayName("負責工程師")]
        public virtual Employee Engineer { get; set; }

        [DisplayName("專案起始日")]
        public DateTime? StartOn { get; set; }

        [DisplayName("案件狀態")]
        public string Comment { get; set; }

        [DisplayName("保固起始日")]
        public DateTime? WarrantyStart { get; set; }

        [DisplayName("保固迄止日")]
        public DateTime? WarrantyEnd { get; set; }

        [DisplayName("結案日期")]
        public DateTime? ClosedOn { get; set; }

        [DisplayName("預估交機日期")]
        public DateTime? HandoverExpectOn { get; set; }

        [DisplayName("實際交機日期")]
        public DateTime? HandoverOn { get; set; }

        [DisplayName("預估裝機日期")]
        public DateTime? ConstituteExpectOn {get;set;}

        [DisplayName("實際裝機日期")]
        public DateTime? ConstituteOn { get; set; }

        [DisplayName("預估驗收日期")]
        public DateTime? CheckExpectOn {get;set;}

        [DisplayName("實際驗收日期")]
        public DateTime? CheckOn { get; set; }

        public virtual List<Prediction> Predictions { get; set; }
        public virtual List<DailyInfo> DailyInfoes { get; set; }
        #endregion

        #region Responsibility of Account
        [DisplayName("交機款實收日期")]
        public DateTime? HandoverPriceOn { get; set; }

        [DisplayName("裝機款實際收款日")]
        public DateTime? ConstitutePriceOn { get; set; }

        [DisplayName("驗收款實際收款日期")]
        public DateTime? CheckPriceOn { get; set; }

        [DisplayName("保固款實際收款日")]
        public DateTime? WarrantyPriceOn { get; set; }
        #endregion

        #region System
        [Timestamp]
        public byte[] RowVersion { get; set; }
        #endregion

       
       

       

       

      

       

       

       

     

       


    }
}