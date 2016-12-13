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
        // GET: Course
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: Courses/List
        public ActionResult List()
        {
            using (var database = new ApplicationDbContext())
            {
                // Get Courses from database
                var courses = database.Courses
                    .ToList();

                return View(courses);
            }
        }

        // GET: Courses/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                var course = database.Courses
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
        }

        // POST: Course/Edit
        [HttpPost]
        public ActionResult Edit(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new ApplicationDbContext())
                {
                    var course = database.Courses
                        .FirstOrDefault(a => a.Id == model.Id);

                    course.Name = model.Name;

                    database.Entry(course).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            using (var database = new  ApplicationDbContext())
            {
                var model = new CourseViewModel();
                model.Modules = database.Modules
                    .OrderBy(c => c.Name)
                    .ToList();

                return View(model);
            }
        }

        // POST: Course/Create
        [HttpPost]
        public ActionResult Create(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new ApplicationDbContext())
                {
                    var course = new Course(model.Name, model.ModuleId);

                    database.Courses.Add(course);
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

    }
}