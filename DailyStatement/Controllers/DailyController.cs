using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DailyStatement.Models;
using DailyStatement.ViewModel;
using KendoGridBinder;

namespace DailyStatement.Controllers
{
    [Authorize]
    public class DailyController : Controller
    {
        private DailyStatementContext db = new DailyStatementContext();

        //
        // GET: /Daily/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Grid(KendoGridRequest request)
        {
            // Sample here: https://github.com/rwhitmire/KendoGridBinder
            db.Configuration.ProxyCreationEnabled = false;

            var emp = db.Employees.Include("Rank").Where(e => e.Account == User.Identity.Name).FirstOrDefault();

            List<DailyInfoForIndex> dailies = new List<DailyInfoForIndex>();
            if (emp.Rank.Name == "一般人員")
            {
                dailies = (from d in db.Dailies.Include("WorkCategory")
                           where d.EmployeeId == emp.EmployeeId
                               select new DailyInfoForIndex
                               {
                                   DailyInfoId = d.DailyInfoId,
                                   CreateDate = d.CreateDate,
                                   EmployeeId = d.DailyInfoId,
                                   Customer = d.Customer,
                                   ProjectNo = d.ProjectNo,
                                   WorkContent = d.WorkContent,
                                   WorkingHours = d.WorkingHours,
                                   WorkCategory = d.WorkCategory.Name,
                                   EmployeeName = d.Employee.Name
                               }).OrderByDescending(d => d.CreateDate).ToList();
            }
            else
            {
                dailies = (from d in db.Dailies.Include("WorkCategory")
                               select new DailyInfoForIndex
                               {
                                   DailyInfoId = d.DailyInfoId,
                                   CreateDate = d.CreateDate,
                                   EmployeeId = d.DailyInfoId,
                                   Customer = d.Customer,
                                   ProjectNo = d.ProjectNo,
                                   WorkContent = d.WorkContent,
                                   WorkingHours = d.WorkingHours,
                                   WorkCategory = d.WorkCategory.Name,
                                   EmployeeName = d.Employee.Name
                               }).OrderByDescending(d => d.CreateDate).ToList();
            }

            var grid = new KendoGrid<DailyInfoForIndex>(request, dailies);
            return Json(grid);

        }

        //
        // GET: /Daily/Details/5

        public ActionResult Details(int id = 0)
        {
            DailyInfo dailyinfo = db.Dailies.Find(id);
            if (dailyinfo == null)
            {
                return HttpNotFound();
            }
            return View(dailyinfo);
        }

        //
        // GET: /Daily/Create

        public ActionResult Create()
        {
            ViewData["Categories"] = new SelectList(db.Categories.ToList(), "WorkCategoryId", "Name", "");
            return View();
        }

        //
        // POST: /Daily/Create

        [HttpPost]
        public ActionResult Create([Bind(Exclude = "EmployeeId")] DailyInfo dailyinfo)
        {
            dailyinfo.Employee = db.Employees.Where(e => e.Account == User.Identity.Name).FirstOrDefault();
            
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

        public ActionResult Edit(int id = 0)
        {
            DailyInfo dailyinfo = db.Dailies.Find(id);

            ViewBag.CurrentEmployeeId = dailyinfo.EmployeeId;

            if (dailyinfo == null)
            {
                return HttpNotFound();
            }

            ViewData["Categories"] = new SelectList(db.Categories.ToList(), "WorkCategoryId", "Name", "");
            return View(dailyinfo);
        }

        //
        // POST: /Daily/Edit/5

        [HttpPost]
        public ActionResult Edit(DailyInfo dailyinfo)
        {
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
        public ActionResult DeleteConfirmed(int id)
        {
            DailyInfo dailyinfo = db.Dailies.Find(id);
            db.Dailies.Remove(dailyinfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EmployeeList()
        {
            return View(db.Employees.ToList());
        }


        // UNDONE: 目前先不採用
        [HttpPost]
        public JsonResult GetEmployeeNameList(KendoGridRequest request)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var emp = db.Employees.Where(e => e.Account == User.Identity.Name).FirstOrDefault();
            
            
            if (emp.Rank == "3")
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

        public ActionResult ReportSearch()
        {
            List<SelectListItem> months = new List<SelectListItem>();
            for (int i = 1; i < 13; i++)
            {
                months.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            ViewBag.Months = new SelectList(months,"Text","Value");
            var emp = db.Employees.Where(e => e.Account == User.Identity.Name).FirstOrDefault();
            if(emp.Rank == "3")
            {
                ViewBag.LoginUser = emp;
            }else
            {
               
                ViewBag.LoginUser = emp;
                ViewBag.Employee = new SelectList(db.Employees,"EmployeeId","Name");
            }
            return View();
        }

        public ActionResult Report(int employeeId, DateTime formDate, DateTime toDate )
        {

            return View();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}