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
            HasMany(w => w.Predictions).WithRequired(p => p.WorkCategory).WillCascadeOnDelete(true);
        }
    }

    public class ProjectConfigurations : EntityTypeConfiguration<Project>
    {
        public ProjectConfigurations()
        {
            // Fluent API 設定建置在此
            HasOptional(p => p.Sale).WithMany(e => e.SaleFor).HasForeignKey(p => p.SaleId).WillCascadeOnDelete(false);
            HasOptional(p => p.Engineer).WithMany(e => e.EngineerFor).HasForeignKey(p => p.EngineerId).WillCascadeOnDelete(false);
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