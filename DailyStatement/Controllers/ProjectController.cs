﻿using System;
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
            var proj = db.Projects.Include("Predictions").OrderByDescending(d => d.StartOn).ToList();
            
            var grid = new KendoGrid<Project>(request, proj);
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
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsEditedPredictions(int projectId, int predictionId, int predictHours )
        {
            var project = db.Projects.Find(projectId);
            if (project == null)
            {
                return HttpNotFound();
            }
            Prediction prediction = project.Predictions.Where(x => x.PredictionId == predictionId).SingleOrDefault();
            if (prediction == null)
            {
                return HttpNotFound();
            }
            prediction.PredictHours = predictHours;
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.Entry(prediction).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Details");
        }

        //
        // GET: /Project/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Project/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        //
        // GET: /Project/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        //
        // POST: /Project/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }


        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "超級管理員,一般管理員,一般人員")]
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}