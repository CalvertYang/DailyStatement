using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace DailyStatement.Models
{
    public class EmployeeConfigurations:EntityTypeConfiguration<Employee>
    {
        public EmployeeConfigurations()
        {
            // Fluent API 設定建置在此
            HasRequired(e => e.Rank).WithMany(r => r.Employees).WillCascadeOnDelete(false);
        }
    }

    public class DailyInfoConfigurations : EntityTypeConfiguration<DailyInfo>
    {
        public DailyInfoConfigurations()
        {
            // Fluent API 設定建置在此
            HasRequired(d => d.Employee).WithMany(e => e.DailyInfos).WillCascadeOnDelete(false);
            HasRequired(d => d.Project).WithMany(p => p.DailyInfoes).WillCascadeOnDelete(false);
        }
    }

    public class WorkCategoryConfigurations : EntityTypeConfiguration<WorkCategory>
    {
        public WorkCategoryConfigurations()
        {
            // Fluent API 設定建置在此
        }
    }

    public class ProjectConfigurations : EntityTypeConfiguration<Project>
    {
        public ProjectConfigurations()
        {
            // Fluent API 設定建置在此
        }
    }

    public class PredictionConfigurations : EntityTypeConfiguration<Prediction>
    {
        public PredictionConfigurations()
        {
            // Fluent API 設定建置在此
        }
    }

    public class RankConfigurations : EntityTypeConfiguration<Rank>
    {
        public RankConfigurations()
        {
            // Fluent API 設定建置在此
        }
    }
}