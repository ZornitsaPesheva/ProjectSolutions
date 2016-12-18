using Solutions.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solutions.Enums;

namespace Solutions.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: Post/List
        public ActionResult List()
        {
            using (var database = new ApplicationDbContext())
            {
                var posts = database.Posts
                    .Include(a => a.Author)
                    .ToList();

                return View(posts);
            }
        }

        // GET: Post/Create
        public ActionResult Create()
        {
            using (var database = new ApplicationDbContext())
            {
                var model = new PostViewModel();
                model.Chapters = database.Chapters
                    .ToList();

                model.Languages = Enum.GetNames(typeof(Languages)).ToList();
                model.Verifies = Enum.GetNames(typeof(Verify)).ToList();

                return View(model);
            }
        }

        // POST: Post/Create
        [HttpPost]
        public ActionResult Create(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new ApplicationDbContext())
                {
                    var authorId = database.Users
                        .Where(u => u.UserName == this.User.Identity.Name)
                        .First()
                        .Id;

                    var post = new Post(authorId, model.Title, model.Link, model.ChapterId, model.Language, model.Verify);

                    database.Posts.Add(post);
                    database.SaveChanges();

                    return RedirectToAction("ListPosts", "Home", new { @chapterId = model.ChapterId });
                }
            }
            return View(model);
        }

        // GET: Post/Delete



        // GET: Post/Edit



        // POST: Post/Edit





    }
}