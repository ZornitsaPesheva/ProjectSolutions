using Solutions.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Solutions.Controllers
{
    public class CourseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Course
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: Courses/List
        public ActionResult List()
        {
            // Get Courses from database
            var courses = db.Courses
                .ToList();

            return View(courses);
        }

        // GET: Courses/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var course = db.Courses
                    .Where(a => a.Id == id)
                    .First();

            if (course == null)
            {
                return HttpNotFound();
            }

            var model = new CourseViewModel();
            model.Id = course.Id;
            model.Name = course.Name;

            return View(model);
        }

        // POST: Course/Edit
        [HttpPost]
        public ActionResult Edit(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var course = db.Courses
                        .FirstOrDefault(a => a.Id == model.Id);

                course.Name = model.Name;

                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "Modules", new { @id = course.ModuleId });

            }

            return View(model);
        }

        // GET: Course/Create
        public ActionResult Create(int moduleId, string moduleName)
        {
            using (var database = new ApplicationDbContext())
            {
                var model = new CourseViewModel();

                model.ModuleId = moduleId;
                ViewBag.ModuleName = moduleName;
                return View(model);
            }
        }

        // POST: Course/Create
        [HttpPost]
        public ActionResult Create(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {

                var course = new Course(model.Name, model.ModuleId);

                db.Courses.Add(course);
                db.SaveChanges();

                return RedirectToAction("Details", "Modules", new { @id = model.ModuleId });

            }

            return View(model);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);

        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            var moduleId = course.ModuleId;
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Details", "Modules", new { @id = moduleId });

        }

        public ActionResult Details(int? id)
        {
            if (id == null || !User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            course.Chapters = db.Chapters.Where(x => x.CourseId == course.Id).ToList();
            return View(course);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}