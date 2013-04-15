using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DailyStatement.Models;
using DailyStatement.ViewModel;
using KendoGridBinder;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Data.SqlClient;
using CrystalDecisions.Shared;
using System.Data.Objects;

namespace DailyStatement.Controllers
{
	[Authorize]
	public class DailyController : Controller
	{
		private DailyStatementContext db = new DailyStatementContext();
		
		//
		// GET: /Daily/

		[Authorize(Roles = "超級管理員,一般管理員,一般人員")]
		public ActionResult Index()
		{
			var emp = db.Employees.Include("Rank").Where(e => e.Account == User.Identity.Name).FirstOrDefault();
			DateTime today = DateTime.Today;
			DateTime tomorrow = DateTime.Today.AddDays(1);
			var todaySumHours = db.Dailies.Where(d => d.EmployeeId == emp.EmployeeId && d.CreateDate >= today && d.CreateDate < tomorrow).Select(d => (int?)d.WorkingHours).Sum() ?? 0;

			var todayWintrissHours = db.Dailies.Where(d => d.EmployeeId == emp.EmployeeId && d.CreateDate >= today && d.CreateDate < tomorrow && d.Project.ProjectId == 2).Select(d => (int?)d.WorkingHours).Sum() ?? 0;
			

			if (emp.Rank.Name == "一般人員")
			{
				if (todaySumHours < 8)
				{
					string notify = string.Format("本日未達工時仍有 {0} 小時。", 8 - todaySumHours);
					ViewBag.NotifyOfSumHours = notify;
				}

				if (todayWintrissHours < 1)
				{
					ViewBag.NotifyOfInternal = "本日內部紀錄(WINTRISS)尚未填寫。";
				}
				
			}
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "超級管理員,一般管理員,一般人員")]
		public JsonResult Grid(KendoGridRequest request)
		{
			// Sample here: https://github.com/rwhitmire/KendoGridBinder
			db.Configuration.ProxyCreationEnabled = false;

			var emp = db.Employees.Include("Rank").Where(e => e.Account == User.Identity.Name).FirstOrDefault();

			List<DailyInfoForIndex> dailies = new List<DailyInfoForIndex>();
			if (emp.Rank.Name == "一般人員")
			{
				dailies = (from d in db.Dailies.Include("WorkCategory").Include("Project")
						   where d.EmployeeId == emp.EmployeeId
							   select new DailyInfoForIndex
							   {
								   DailyInfoId = d.DailyInfoId,
								   CreateDate = d.CreateDate,
								   EmployeeId = d.DailyInfoId,
								   Customer = d.Project.CustomerName,
								   ProjectNo = d.Project.ProjectNo,
								   WorkContent = d.WorkContent,
								   WorkingHours = d.WorkingHours,
								   WorkCategory = d.WorkCategory.Name,
								   EmployeeName = d.Employee.Name
							   }).OrderByDescending(d => d.CreateDate).ToList();
			}
			else
			{
				dailies = (from d in db.Dailies.Include("WorkCategory").Include("Project")
							   select new DailyInfoForIndex
							   {
								   DailyInfoId = d.DailyInfoId,
								   CreateDate = d.CreateDate,
								   EmployeeId = d.DailyInfoId,
								   Customer = d.Project.CustomerName,
								   ProjectNo = d.Project.ProjectNo,
								   WorkContent = d.WorkContent,
								   WorkingHours = d.WorkingHours,
								   WorkCategory = d.WorkCategory.Name,
								   EmployeeName = d.Employee.Name
							   }).OrderByDescending(d => d.CreateDate).ToList();
			}

			var grid = new KendoGrid<DailyInfoForIndex>(request, dailies);
			return Json(grid);
		}

		[HttpPost]
		[Authorize(Roles = "超級管理員,一般管理員,一般人員")]
		public JsonResult GetProjects()
		{
			return Json(db.Projects.Select(x => new { ProjectId = x.ProjectId, ProjectNo = x.ProjectNo }).ToList());
		}

		[Authorize(Roles = "超級管理員,一般管理員,一般人員")]
		public ActionResult Create()
		{
			ViewData["Categories"] = new SelectList(db.Categories.ToList(), "WorkCategoryId", "Name", "");
			ViewData["Projects"] = new SelectList(db.Projects.OrderBy(p => p.ProjectNo).ToList(), "ProjectId", "ProjectNo", "");
			if (!User.IsInRole("一般人員"))
			{
				int empId = db.Employees.Where(e => e.Account == User.Identity.Name).FirstOrDefault().EmployeeId;
				ViewData["EmployeeList"] = new SelectList(db.Employees.ToList(), "EmployeeId", "Name", empId);
			}

			return View();
		}

		//
		// POST: /Daily/Create

		[HttpPost]
		[Authorize(Roles = "超級管理員,一般管理員,一般人員")]
		public ActionResult Create(Project project, int? WorkCategoryId, string WorkContent, DateTime CreateDate, int WorkingHours, int? EmployeeList )
		{
			DailyInfo dailyinfo = new DailyInfo();
			dailyinfo.Project = db.Projects.Find(project.ProjectId);
			dailyinfo.WorkCategoryId = WorkCategoryId;
			dailyinfo.WorkContent = WorkContent;
			dailyinfo.CreateDate = CreateDate;
			dailyinfo.WorkingHours = WorkingHours;
			dailyinfo.Employee = db.Employees.Where(e => (EmployeeList == null && e.Account == User.Identity.Name) || (EmployeeList != null && e.EmployeeId == EmployeeList)).FirstOrDefault();

			if (ModelState.IsValid)
			{
				db.Dailies.Add(dailyinfo);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(dailyinfo);
		}

		//
		// GET: /Daily/Edit/5

		[Authorize(Roles = "超級管理員,一般管理員,一般人員")]
		public ActionResult Edit(int id = 0)
		{
			DailyInfo dailyinfo = db.Dailies.Find(id);

			ViewBag.CurrentEmployeeId = dailyinfo.EmployeeId;

			if (dailyinfo == null)
			{
				return HttpNotFound();
			}

			ViewData["Categories"] = new SelectList(db.Categories.ToList(), "WorkCategoryId", "Name", "");
			ViewData["Projects"] = new SelectList(db.Projects.OrderBy(p => p.ProjectNo).ToList(), "ProjectId", "ProjectNo", "");
			if (!User.IsInRole("一般人員"))
			{
				ViewData["EmployeeList"] = new SelectList(db.Employees.ToList(), "EmployeeId", "Name", dailyinfo.EmployeeId);
			}
			
			return View(dailyinfo);
		}

		//
		// POST: /Daily/Edit/5

		[HttpPost]
		[Authorize(Roles = "超級管理員,一般管理員,一般人員")]
		public ActionResult Edit(int DailyInfoId, Project project, int? WorkCategoryId, string WorkContent, DateTime CreateDate, int WorkingHours, int? EmployeeList, byte[] RowVersion)
		{
			DailyInfo dailyinfo = db.Dailies.Where(d => d.DailyInfoId == DailyInfoId && d.RowVersion == RowVersion).FirstOrDefault();
			
			if (dailyinfo == null)
			{
				return View("Error");
			};
			
			dailyinfo.Project = db.Projects.Find(project.ProjectId);
			dailyinfo.WorkCategoryId = WorkCategoryId;
			dailyinfo.WorkContent = WorkContent;
			dailyinfo.CreateDate = CreateDate;
			dailyinfo.WorkingHours = WorkingHours;
			
			if (EmployeeList != null)
			{
				dailyinfo.EmployeeId = Convert.ToInt32(EmployeeList);
			}

			if (ModelState.IsValid)
			{
				db.Entry(dailyinfo).State = EntityState.Modified;
				db.SaveChanges();

				return RedirectToAction("Index");
			}
			return View(dailyinfo);
		}

		//
		// GET: /Daily/Delete/5

		//public ActionResult Delete(int id = 0)
		//{
		//    DailyInfo dailyinfo = db.Dailies.Find(id);
		//    if (dailyinfo == null)
		//    {
		//        return HttpNotFound();
		//    }
		//    return View(dailyinfo);
		//}

		//
		// POST: /Daily/Delete/5
		
		[HttpPost, ActionName("Delete")]
		[Authorize(Roles = "超級管理員,一般管理員,一般人員")]
		public ActionResult DeleteConfirmed(int id)
		{
			DailyInfo dailyinfo = db.Dailies.Find(id);
			db.Dailies.Remove(dailyinfo);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		[Authorize(Roles = "超級管理員,一般管理員,一般人員")]
		public ActionResult EmployeeList()
		{
			return View(db.Employees.ToList());
		}


		// UNDONE: 目前先不採用
		[HttpPost]
		[Authorize(Roles = "超級管理員,一般管理員,一般人員")]
		public JsonResult GetEmployeeNameList(KendoGridRequest request)
		{
			db.Configuration.ProxyCreationEnabled = false;

			if (User.IsInRole("一般人員"))
			{
				return Json("{}");
			}
			else
			{
				var employee = db.Employees.ToList();
				var grid = new KendoGrid<Employee>(request, employee);
				return Json(grid);
			}
		}

		[Authorize(Roles = "超級管理員,一般管理員,一般人員,助理")]
		public ActionResult ReportSearch()
		{
			List<SelectListItem> months = new List<SelectListItem>();
			for (int i = 1; i < 13; i++)
			{
				months.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
			}
			List<SelectListItem> years = new List<SelectListItem>();
			int yearStart = DateTime.Now.Year;
			for (int i = yearStart ; i > yearStart - 3; i--)
			{
				years.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
			}


			ViewBag.Months = new SelectList(months, "Text", "Value");

			ViewBag.Years = new SelectList(years, "Text", "Value");

			ViewBag.Employee = new SelectList(db.Employees, "EmployeeId", "Name", UserId(User.Identity.Name));

			ViewBag.WorkCategory = new SelectList(db.Categories, "WorkCategoryId", "Name");

			 ViewData["Projects"] = new SelectList(db.Projects.OrderBy(p => p.ProjectNo).ToList(), "ProjectId", "ProjectNo", "");
			return View();
		}


		[ValidateAntiForgeryToken]
		[Authorize(Roles = "超級管理員,一般管理員,一般人員,助理")]
		public ActionResult ReportWeekForSingle(int employeeId, DateTime fromDate, DateTime toDate )
		{
			string query = String.Format(@"Select T2.[ProjectNo] + ' - ' +
										(Select top 1 T3.[CustomerName] From [dbo].[Projects] T3 Where T3.[ProjectId] = T1.[Project_ProjectId]) as [WorkName],
										SUM(CASE (DATEPART(Weekday, T1.[CreateDate])) WHEN '1' THEN T1.[WorkingHours] ELSE 0 END) AS [Sunday],
										SUM(CASE (DATEPART(Weekday, T1.[CreateDate])) WHEN '2' THEN T1.[WorkingHours] ELSE 0 END) AS [Monday],
										SUM(CASE (DATEPART(Weekday, T1.[CreateDate])) WHEN '3' THEN T1.[WorkingHours] ELSE 0 END) AS [Tuesday],
										SUM(CASE (DATEPART(Weekday, T1.[CreateDate])) WHEN '4' THEN T1.[WorkingHours] ELSE 0 END) AS [Wednesday],
										SUM(CASE (DATEPART(Weekday, T1.[CreateDate])) WHEN '5' THEN T1.[WorkingHours] ELSE 0 END) AS [Thursday],
										SUM(CASE (DATEPART(Weekday, T1.[CreateDate])) WHEN '6' THEN T1.[WorkingHours] ELSE 0 END) AS [Friday],
										SUM(CASE (DATEPART(Weekday, T1.[CreateDate])) WHEN '7' THEN T1.[WorkingHours] ELSE 0 END) AS [Saturday]
										From
											[dbo].[DailyInfoes] T1,
											[dbo].[Projects] T2
										Where 
											T2.[ProjectId] = T1.[Project_ProjectId]
											AND T1.[EmployeeId] = {0}
											AND T1.[CreateDate] Between '{1}' AND '{2}'
										Group By
											T1.[EmployeeId], T1.[Project_ProjectId], T2.[ProjectNo]", employeeId, fromDate.ToShortDateString(), toDate.ToShortDateString());
			var report = db.Database.SqlQuery<WeekReportOfSingle>(query).ToList();

			ViewBag.TotalOfAll = (db.Dailies.Where(d => d.EmployeeId == employeeId && (d.CreateDate >= fromDate && d.CreateDate <= toDate)).Count()>0)?db.Dailies.Where(d => d.EmployeeId == employeeId && (d.CreateDate >= fromDate && d.CreateDate <= toDate)).Select(d => d.WorkingHours).Sum():0;
			ViewBag.EmployeeId = employeeId;
			ViewBag.EmployeeName = db.Employees.Where(e => e.EmployeeId == employeeId).SingleOrDefault().Name;
			CultureInfo ci = CultureInfo.CurrentCulture;
			int weekNum = ci.Calendar.GetWeekOfYear(fromDate, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
			ViewBag.WeekNum = weekNum;
			ViewBag.FromDate = fromDate;
			ViewBag.ToDate = toDate;
			ViewBag.Date = fromDate.ToString("yyyy 年 MM 月 dd 日") + " ~ " + toDate.ToString("yyyy 年 MM 月 dd 日");
			var reportList = db.Dailies.Where(d => d.EmployeeId == employeeId && (d.CreateDate >= fromDate && d.CreateDate <= toDate)).Select(d => new { Date = d.CreateDate, Content = d.WorkContent });

			if (reportList != null)
			{
				ViewBag.Monday = new List<string>();
				ViewBag.Tuesday = new List<string>();
				ViewBag.Wednesday = new List<string>();
				ViewBag.Thursday = new List<string>();
				ViewBag.Friday = new List<string>();
				ViewBag.Overtime = new List<string>();
				foreach (var i in reportList)
				{
					switch (i.Date.DayOfWeek.ToString())
					{ 
						case "Monday":
							ViewBag.Monday.Add(i.Content);
							break;
						case "Tuesday":
							ViewBag.Tuesday.Add(i.Content);
							break;
						case "Wednesday":
							ViewBag.Wednesday.Add(i.Content);
							break;
						case "Thursday":
							ViewBag.Thursday.Add(i.Content);
							break;
						case "Friday":
							ViewBag.Friday.Add(i.Content);
							break;
						default:
							ViewBag.Overtime.Add(i.Content);
							break;
					}
				}
			}

			return View(report);
		}

		[HttpPost]
		[Authorize(Roles = "超級管理員,一般管理員,一般人員,助理")]
		public ActionResult GenerateWeekReport(int employeeId, string weekOfYear, DateTime fromDate, DateTime toDate)
		{
			try
			{
				ReportDocument rpt = new ReportDocument();
				rpt.Load(Server.MapPath("~/Report/WeekReport.rpt"));

				DailyStatementDS ds = new DailyStatementDS();

				string conn = System.Configuration.ConfigurationManager.ConnectionStrings["DailyStatementContext"].ConnectionString;
				// Get data from DailyInfoes
				string condition = String.Format("SELECT * FROM [dbo].[DailyInfoes] WHERE [EmployeeId] = {0} AND ([CreateDate] >= '{1}' AND [CreateDate] <= '{2}')", employeeId, fromDate.ToShortDateString(), toDate.ToShortDateString());
				SqlDataAdapter da = new SqlDataAdapter(condition, conn);
				da.Fill(ds.DailyInfoes);
				// Get data from Employees
				condition = String.Format("SELECT * FROM [dbo].[Employees] WHERE [EmployeeId] = {0}", employeeId);
				da = new SqlDataAdapter(condition, conn);
				da.Fill(ds.Employees);
				// Get data from Projects
				condition = "SELECT * FROM [dbo].[Projects]";
				da = new SqlDataAdapter(condition, conn);
				da.Fill(ds.Projects);
				// Due to SetParameterValue always return error, so use datatable to store parameter
				ds.ParameterForWeekRpt.Rows.Add(employeeId, fromDate, toDate, weekOfYear);
				
				rpt.SetDataSource(ds);

				Stream stream = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
				return File(stream, "application/pdf");
			}
			catch (Exception e)
			{
				return Content(e.ToString());
			}
		}

		[HttpPost]
		[Authorize(Roles = "超級管理員,一般管理員,助理")]
		public ActionResult GenerateProjectReport(int projectNo = 0, int workCategoryId = 0)
		{
			try
			{
				ReportDocument rpt = new ReportDocument();
				rpt.Load(Server.MapPath("~/Report/ProjectReport.rpt"));
				//rpt.Load(Server.MapPath("~/Report/Test.rpt"));

				DailyStatementDS ds = new DailyStatementDS();

				string conn = System.Configuration.ConfigurationManager.ConnectionStrings["DailyStatementContext"].ConnectionString;
				// Get data from DailyInfoes
				string condition = String.Format("SELECT * FROM [dbo].[DailyInfoes] WHERE (({0} = 0 AND [Project_ProjectId] >= 0) OR [Project_ProjectId] = {0}) AND (({1} = 0 AND [WorkCategoryId] >= 0) OR [WorkCategoryId] = {1})", projectNo, workCategoryId);
				SqlDataAdapter da = new SqlDataAdapter(condition, conn);
				da.Fill(ds.DailyInfoes);
				// Get data from Employees
				condition = "SELECT * FROM [dbo].[Employees]";
				da = new SqlDataAdapter(condition, conn);
				da.Fill(ds.Employees);
				// Get data from WorkCategories
				condition = "SELECT * FROM [dbo].[WorkCategories]";
				da = new SqlDataAdapter(condition, conn);
				da.Fill(ds.WorkCategories);
				// Get data from Projects
				condition = String.Format("SELECT * FROM [dbo].[Projects] WHERE [ProjectId] = {0}", projectNo);
				da = new SqlDataAdapter(condition, conn);
				da.Fill(ds.Projects);
				// Due to SetParameterValue always return error, so use datatable to store parameter
				ds.ParameterForProjectRpt.Rows.Add(projectNo, workCategoryId);

				rpt.SetDataSource(ds);

				Stream stream = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
				return File(stream, "application/pdf");
			}
			catch (Exception e)
			{
				return Content(e.ToString());
			}
		}

		[HttpPost]
		[Authorize(Roles = "超級管理員,一般管理員,助理")]
		public ActionResult GenerateProjectSummaryReport(int projectId = 0)
		{
			try
			{
				ReportDocument rpt = new ReportDocument();
				rpt.Load(Server.MapPath("~/Report/ProjectSummaryReport.rpt"));

				DailyStatementDS ds = new DailyStatementDS();

				string conn = System.Configuration.ConfigurationManager.ConnectionStrings["DailyStatementContext"].ConnectionString;
				// Get data from DailyInfoes
				string condition = String.Format("SELECT T1.* FROM [dbo].[DailyInfoes] T1 INNER JOIN [dbo].[Projects] T2 on T2.[ProjectId] = T1.[Project_ProjectId] WHERE (({0} = 0 AND T2.[ProjectId] >= 0) OR T2.[ProjectId] = {0})", projectId);
				SqlDataAdapter da = new SqlDataAdapter(condition, conn);
				da.Fill(ds.DailyInfoes);
				// Get data from WorkCategories
				condition = "SELECT * FROM [dbo].[WorkCategories]";
				da = new SqlDataAdapter(condition, conn);
				da.Fill(ds.WorkCategories);
				// Get data from Projects
				condition = String.Format("SELECT * FROM [dbo].[Projects] WHERE (({0} = 0 AND [ProjectId] >= 0) OR [ProjectId] = {0})", projectId);
				da = new SqlDataAdapter(condition, conn);
				da.Fill(ds.Projects);
				// Get data from Predictions
				condition = String.Format("SELECT * FROM [dbo].[Predictions] WHERE (({0} = 0 AND [Project_ProjectId] >= 0) OR [Project_ProjectId] = {0})", projectId);
				da = new SqlDataAdapter(condition, conn);
				da.Fill(ds.Predictions);
				// Due to SetParameterValue always return error, so use datatable to store parameter
				ds.ParameterForProjectRpt.Rows.Add(projectId, null);

				rpt.SetDataSource(ds);

				Stream stream = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
				return File(stream, "application/pdf");
			}
			catch (Exception e)
			{
				return Content(e.ToString());
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ReportSummaryOfYear(int projectid = 0)
		{
		   
			if (projectid == 0)
			{
				return GenerateProjectSummaryReport(projectid);
			}
			else
			{
				var project = db.Projects.Find(projectid);
				//var current = from daily in db.Dailies.Include("WorkCategory")
				//              group daily by new { daily.WorkCategory } into g
				//              select new { Category = g.Key.WorkCategory.Name, SumHours = g.Sum(daily => daily.WorkingHours) };
				var current = from category in db.Categories
							  join daily in db.Dailies on category.WorkCategoryId equals daily.WorkCategoryId into daily_join
							  from daily2 in daily_join.Where(d => d.Project.ProjectId == projectid).DefaultIfEmpty()
							  orderby category.WorkCategoryId ascending
							  group new { category, daily2 } by new { category.WorkCategoryId, category.Name } into g
							  select new
							  {
								  Category = g.Key.Name,
								  SumHours = g.Sum(p => p.daily2.WorkingHours) == null ? 0 : g.Sum(p => p.daily2.WorkingHours)
							  };

				ViewBag.Predictions = project.Predictions.ToList();
				ViewBag.Current = current;
				ViewBag.ProjectId = projectid;
				ViewBag.ProjectNo = project.ProjectNo;

			}
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "超級管理員,一般管理員,一般人員,助理")]
		public ActionResult AnalysisHoursPersonal(int years, int months)
		{
			// TODO: here
			int numberOfDays = DateTime.DaysInMonth(years, months);
			DateTime startDay = new DateTime(years,months,1);
			DateTime lastDay = new DateTime(years, months, numberOfDays);
			CultureInfo ci = new CultureInfo("zh-TW");
			List<DateTime> holidays = new List<DateTime>();
			for (int i = 1; i <= ci.Calendar.GetDaysInMonth(years, months); i++)
			{
				DateTime dt = new DateTime(years, months, i);
				if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
					holidays.Add(dt);
			}


			List<PersonalWorkingHours> pwh = new List<PersonalWorkingHours>();
			List<Employee> employee = new List<Employee>();
			employee = db.Employees.ToList();
			foreach (var emp in employee)
			{
				PersonalWorkingHours p = new PersonalWorkingHours();
				p.EmployeeName = emp.Name;
				// Wintriss
				p.InternalHours = db.Dailies.Where(d => d.EmployeeId == emp.EmployeeId &&
								  d.Project.ProjectId == 2 &&
								  !d.Project.ProjectNo.StartsWith("N/A") &&
								  d.CreateDate >= startDay &&
								  d.CreateDate <= lastDay
								  ).Select(d => (int?)d.WorkingHours).Sum() ?? 0;
				// CN,CP,CO,C*,LINPO
				p.ProjectHours = db.Dailies.Where(d => d.EmployeeId == emp.EmployeeId &&
								  !d.Project.ProjectNo.StartsWith("ST") &&
								  !d.Project.ProjectNo.StartsWith("DO") &&
								  !d.Project.ProjectNo.StartsWith("CR") &&
								  !d.Project.ProjectNo.StartsWith("N/A") &&
								  // d.Project.ProjectId != 13 &&
								  d.Project.ProjectId != 2 &&
								  d.CreateDate >= startDay &&
								  d.CreateDate <= lastDay
								  ).Select(d => (int?)d.WorkingHours).Sum() ?? 0;
				// ST
				p.UndefineHours = db.Dailies.Where(d => d.EmployeeId == emp.EmployeeId &&
								  d.Project.ProjectNo.StartsWith("ST") &&
								  !d.Project.ProjectNo.StartsWith("N/A") &&
								  d.CreateDate >= startDay &&
								  d.CreateDate <= lastDay
								  ).Select(d => (int?)d.WorkingHours).Sum() ?? 0;
				// DO
				p.DemoHours = db.Dailies.Where(d => d.EmployeeId == emp.EmployeeId &&
								  d.Project.ProjectNo.StartsWith("DO") &&
								  !d.Project.ProjectNo.StartsWith("N/A") &&
								  d.CreateDate >= startDay &&
								  d.CreateDate <= lastDay
								  ).Select(d => (int?)d.WorkingHours).Sum() ?? 0;
				// CR
				p.ResearchHours = db.Dailies.Where(d => d.EmployeeId == emp.EmployeeId &&
								  d.Project.ProjectNo.StartsWith("CR") &&
								  !d.Project.ProjectNo.StartsWith("N/A") &&
								  d.CreateDate >= startDay &&
								  d.CreateDate <= lastDay
								  ).Select(d => (int?)d.WorkingHours).Sum() ?? 0;
				// 加班
				p.Overtime = db.Dailies.Where(d => d.EmployeeId == emp.EmployeeId &&
								  d.WorkCategoryId == 13 &&
								  !d.Project.ProjectNo.StartsWith("N/A") &&
								  d.CreateDate >= startDay &&
								  d.CreateDate <= lastDay &&
								  !holidays.Contains(d.CreateDate)
								  ).Select(d => (int?)d.WorkingHours).Sum() ?? 0;

			}
			return View(pwh);
		}

		[Authorize(Roles = "超級管理員,一般管理員,助理")]
		public ActionResult CategoryIndex()
		{
			return View();
		}

		[Authorize(Roles = "超級管理員,一般管理員,助理")]
		public ActionResult CategoryCreate()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "超級管理員,一般管理員,助理")]
		public ActionResult CategoryCreate(WorkCategory workCategory)
		{
			if (ModelState.IsValid)
			{
				db.Categories.Add(workCategory);
				db.SaveChanges();
				return RedirectToAction("CategoryIndex");
			}

			return View(workCategory);
		}

		[Authorize(Roles = "超級管理員,一般管理員,助理")]
		public ActionResult CategoryEdit(int id = 0)
		{
			WorkCategory workCategory = db.Categories.Find(id);
			if (workCategory == null)
			{
				return HttpNotFound();
			}

			return View(workCategory);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CategoryEdit(WorkCategory workCategory)
		{
			if (ModelState.IsValid)
			{
				db.Entry(workCategory).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("CategoryIndex");
			}

			return View(workCategory);
		}

		[HttpPost, ActionName("CategoryDelete")]
		[Authorize(Roles = "超級管理員,一般管理員,一般人員")]
		public ActionResult CategoryDeleteConfirmed(int id)
		{
			WorkCategory workCategory = db.Categories.Find(id);
			db.Categories.Remove(workCategory);
			db.SaveChanges();
			return RedirectToAction("CategoryIndex");
		}

		// 回傳所有帳號相關資料
		[HttpPost]
		[Authorize(Roles = "超級管理員,一般管理員,助理")]
		public JsonResult CategoryGrid(KendoGridRequest request)
		{
			db.Configuration.ProxyCreationEnabled = false;
			var categories = (from c in db.Categories
							  select new CategoryForIndex
							  {
								  WorkCategoryId = c.WorkCategoryId,
								  Name = c.Name
							  }).ToList();

			var grid = new KendoGrid<CategoryForIndex>(request, categories);

			return Json(grid);
		}
		
		[Authorize(Roles = "超級管理員,一般管理員,一般人員,助理")]
		private int UserId(string account)
		{
			var emp = db.Employees.Where(e => e.Account == account).SingleOrDefault();
			if (emp == null)
			{
				return 0;
			}
			return emp.EmployeeId;
		}

		[Authorize(Roles = "超級管理員,一般管理員,一般人員,助理")]
		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}