using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DailyStatement.Models
{
    public class Prediction
    {
        [Key]
        public int PredictionId { get; set; }

        [ConcurrencyCheck]
        [DisplayName("工作類別")]
        public virtual WorkCategory WorkCategory { get; set; }

        [DisplayName("預測工時")]
        public int PredictHours { get; set; }
    }
}