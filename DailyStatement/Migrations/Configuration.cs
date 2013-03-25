namespace DailyStatement.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DailyStatement.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<DailyStatement.Models.DailyStatementContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DailyStatement.Models.DailyStatementContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            if (context.Employees.Count() == 0)
            {
                context.Employees.AddOrUpdate(new Employee
                {
                    EmployeeId = 1,
                    Account = "admin",
                    Password = "F50102B11D7088627E8C6DC39C0DFC7C181D2D0B",
                    Name = "預設管理員(Admin)",
                    Email = "admin@winspection.com",
                    RecvNotify = false,
                    Activity = true,
                    Rank = new Rank { RankId = 1, Name = "超級管理員" },
                    CreateDate = DateTime.Now,
                    LastLoginDate = null
                });
            }

            if (context.Categories.Count() == 0)
            {
                context.Categories.AddOrUpdate(
                    new WorkCategory { WorkCategoryId = 1, Name = "裝機" },
                    new WorkCategory { WorkCategoryId = 2, Name = "維修" },
                    new WorkCategory { WorkCategoryId = 3, Name = "測試" },
                    new WorkCategory { WorkCategoryId = 4, Name = "DEMO" },
                    new WorkCategory { WorkCategoryId = 5, Name = "KOM" },
                    new WorkCategory { WorkCategoryId = 6, Name = "驗收" },
                    new WorkCategory { WorkCategoryId = 7, Name = "教育訓練" },
                    new WorkCategory { WorkCategoryId = 8, Name = "研發" },
                    new WorkCategory { WorkCategoryId = 9, Name = "設計規劃" },
                    new WorkCategory { WorkCategoryId = 10, Name = "其他" },
                    new WorkCategory { WorkCategoryId = 11, Name = "備料" },
                    new WorkCategory { WorkCategoryId = 12, Name = "組裝" }
                );
            }

            if (context.Ranks.Count() == 0)
            {
                context.Ranks.AddOrUpdate(
                    //new Rank { RankId = 1, Name = "超級管理員" },
                    new Rank { RankId = 2, Name = "一般管理員" },
                    new Rank { RankId = 3, Name = "一般人員" }
                );
            }
        }
    }
}
