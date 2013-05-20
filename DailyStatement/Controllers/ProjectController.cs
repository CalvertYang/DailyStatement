using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DailyStatement.Models;
using KendoGridBinder;

namespace DailyStatement.Controllers
{
    [Authorize(Roles = "超級管理員,一般管理員,助理,業務,會計")]
    public class ProjectController : Controller
    {
        private DailyStatementContext db = new DailyStatementContext();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Grid(KendoGridRequest request)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var projects = db.Projects.Include("Predictions").OrderByDescending(d => d.StartOn).ToList();
            foreach (var x in projects)
            {
                if (x.ProjectNo == null) x.ProjectNo = "";
                if (x.CustomerCode == null) x.CustomerCode = "";
                if (x.CustomerName == null) x.CustomerName = ""; 
            }

            var grid = new KendoGrid<Project>(request, projects);
            return Json(grid);
        }

        //
        // GET: /Project/Details/5

        public ActionResult Details(int id = 0)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            // List<WorkCategory> category = db.Categories.Except(project.Predictions.Select(x => x.WorkCategory).ToList()).ToList();
            List<WorkCategory> category = db.Categories.ToList();
            foreach (var c in project.Predictions)
            {
                category.Remove(c.WorkCategory);
            }
            if(category.Count > 0)
            {
                if(project.Predictions == null)
                {
                    project.Predictions = new List<Prediction>();
                }
                foreach (var c in category)
                {
                    project.Predictions.Add(new Prediction { WorkCategory = c, PredictHours = 0 });
                }
            }
            
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsEditedPredictions(Project project)
        {
            project.Predictions.ForEach(x => x.WorkCategory = db.Categories.Find(x.WorkCategory.WorkCategoryId));
            
            if (ModelState.IsValid)
            {
                foreach (var i in project.Predictions)
                {
                    if (i.PredictionId == 0)
                    {
                        db.Categories.Attach(i.WorkCategory);
                        db.Entry(i).State = EntityState.Added;
                    }
                    else
                    {
                        db.Categories.Attach(i.WorkCategory);
                        db.Entry(i).State = EntityState.Modified;
                    }
                }
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = project.ProjectId });
        }

        //
        // GET: /Project/Create

        public ActionResult Create()
        {
            ViewBag.Engineeers = new SelectList(db.Employees.Where(e => e.Rank.Name == "工程師"), "EmployeeId", "Name");
            ViewBag.Sales = new SelectList(db.Employees.Where(e => e.Rank.Name == "業務"), "EmployeeId", "Name");
            return View();
        }

        //
        // POST: /Project/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Sale, Engineer")]Project project, int SalesId = 0, int EngineeersId = 0)
        {
            if (SalesId != 0)
                project.Sale = db.Employees.Where(e => e.EmployeeId == SalesId).SingleOrDefault();
            if (EngineeersId != 0)
                project.Engineer = db.Employees.Where(e => e.EmployeeId == EngineeersId).SingleOrDefault();

            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Engineeers = new SelectList(db.Employees.Where(e => e.Rank.Name == "工程師"), "EmployeeId", "Name");
            ViewBag.Sales = new SelectList(db.Employees.Where(e => e.Rank.Name == "業務"), "EmployeeId", "Name");
            
            return View(project);
        }

        //
        // GET: /Project/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Project project = db.Projects.Include("Sale").Include("Engineer").Where(p => p.ProjectId == id).FirstOrDefault();
            if (project == null)
            {
                return HttpNotFound();
            }

            if (project.Sale != null)
            {
                ViewBag.Sales = new SelectList(db.Employees.Where(e => e.Rank.Name == "業務"), "EmployeeId", "Name", project.Sale.EmployeeId);
            }
            else
            {
                project.Sale = new Employee();
                ViewBag.Sales = new SelectList(db.Employees.Where(e => e.Rank.Name == "業務"), "EmployeeId", "Name", 0);
            }

            if (project.Engineer != null)
            {
                ViewBag.Engineeers = new SelectList(db.Employees.Where(e => e.Rank.Name == "工程師"), "EmployeeId", "Name", project.Engineer.EmployeeId);
            }
            else
            {
                project.Engineer = new Employee();
                ViewBag.Engineeers = new SelectList(db.Employees.Where(e => e.Rank.Name == "工程師"), "EmployeeId", "Name", 0);
            }
            
            return View(project);
        }

        //
        // POST: /Project/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "Sale, Engineer")]Project project)
        {
            if (project.Sale.EmployeeId != 0)
            {
                project.Sale = db.Employees.Where(e => e.EmployeeId == project.Sale.EmployeeId).SingleOrDefault();
            }
            else
            {
                project.Sale = null;
            }

            if (project.Engineer.EmployeeId != 0)
            {
                project.Engineer = db.Employees.Where(e => e.EmployeeId == project.Engineer.EmployeeId).SingleOrDefault();
            }
            else
            {
                project.Engineer = null;
            }

            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Engineeers = new SelectList(db.Employees.Where(e => e.Rank.Name == "工程師"), "EmployeeId", "Name");
            ViewBag.Sales = new SelectList(db.Employees.Where(e => e.Rank.Name == "業務"), "EmployeeId", "Name");
            
            return View(project);
        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // 檢查帳號是否已存在
        public JsonResult CheckProjectNoDup(string projectNo, int projectId = 0)
        {
            var proj = db.Projects.Where(p => p.ProjectNo == projectNo && p.ProjectId != projectId ).FirstOrDefault();

            if (proj != null)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }


        [Authorize(Roles = "超級管理員,一般管理員,工程師,助理,業務")]
        private int UserId(string account)
        {
            var emp = db.Employees.Where(e => e.Account == account).SingleOrDefault();
            if (emp == null)
            {
                return 0;
            }
            return emp.EmployeeId;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}