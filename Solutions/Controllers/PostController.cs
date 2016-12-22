using Solutions.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
        public ActionResult Create(int? chapterId)
        {
            using (var database = new ApplicationDbContext())
            {
                var model = new PostViewModel();
                model.Chapters = database.Chapters
                    .ToList();

                model.Languages = Enum.GetNames(typeof(Languages)).ToList();
                model.Verified = Enum.GetNames(typeof(Verify)).ToList();

                ViewBag.chapterId = chapterId;
                return View(model);
            }
        }

        // POST: Post/Create
        [HttpPost]
        public ActionResult Create(PostViewModel model, int chapterId)
        {
            if (ModelState.IsValid)
            {
                using (var database = new ApplicationDbContext())
                {
                    var authorId = database.Users
                        .Where(u => u.UserName == this.User.Identity.Name)
                        .First()
                        .Id;

                    var post = new Post(authorId, model.Title, model.Link, chapterId, model.Language, model.Verify);

                    database.Posts.Add(post);
                    database.SaveChanges();

                    return RedirectToAction("ListPosts", "Home", new { @chapterId = model.ChapterId });
                }
            }
            return View(model);
        }



        // GET: Post/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                var model = new PostViewModel();

                // Get post from the database
                var result = database.Posts
                    .Where(a => a.Id == id)
                    .Include(a => a.Author)
                    .Include(b => b.Chapter)
                    .First();

                // Check if post exists
                if (result == null)
                {
                    return HttpNotFound();
                }

                model.Title = result.Title;
                model.Link = result.Link;
                //model.Language = result.LanguageId;
                //model.Verify = result.Verified;

                model.ChapterId = result.ChapterId;
                model.AutorId = result.AuthorId;

                model.Languages = Enum.GetNames(typeof(Languages)).ToList();
                model.Verified = Enum.GetNames(typeof(Verify)).ToList();

                return View(model);
            }
        }





        //
        // POST: Post/Edit
        [HttpPost]
        [ActionName("Edit")]
        //public ActionResult Edit(int? id, FormCollection editedPost)
        public ActionResult Edit(PostViewModel editedPost, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Check if model state is valid
            if (ModelState.IsValid)
            {
                using (var database = new ApplicationDbContext())
                {
                    var model = new PostViewModel();

                    // Get post from the database
                    var solution = database.Posts
                        .Where(a => a.Id == id)
                        .Include(a => a.Author)
                        .Include(b => b.Chapter)
                        .First();

                    // Set post title and content
                    solution.Title = editedPost.Title;
                    solution.Link = editedPost.Link;
                    //solution.ChapterId = result.ChapterId;
                    //solution.AuthorId = this.User.Identity.Name;
                    //solution.LanguageId = Enum.GetNames(typeof(Languages)).ToString();
                    //solution.Verified = Enum.GetNames(typeof(Verify)).ToString();

                    solution.LanguageId = editedPost.Language;
                    solution.Verified = editedPost.Verify;

                    // Save post state in database
                    database.Entry(solution).State = EntityState.Modified;
                    database.SaveChanges();

                    // Redirect to the index page
                    //return RedirectToAction("Index");
                    return RedirectToAction("ListPosts", "Home", new { @chapterId = solution.ChapterId });
                }
            }

            // If model state is invalid, return the same view
            return View();
        }



        // GET: Post/Delete
        [Authorize]
        public ActionResult Delete(int? id)
        {
            var model = new PostViewModel();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                // Get post from database
                var post = database.Posts
                    .Where(a => a.Id == id)
                    .Include(a => a.Author)
                    .First();

                if (!IsUserAuthorizedToEdit(post))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                // Check if post exists
                if (post == null)
                {
                    return HttpNotFound();
                }
                model.Title = post.Title;
                model.Link = post.Link;
                model.Language = post.LanguageId;
                // Pass post to view
                return View(model);
            }
        }



        // POST: Post/Delete
        [HttpPost, Authorize]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            var model = new PostViewModel();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                // Get post from database
                var post = database.Posts
                    .Where(a => a.Id == id)
                    .Include(a => a.Author)
                    .First();
                var ChapId = post.ChapterId;

                // Check if post exists
                if (post == null)
                {
                    return HttpNotFound();
                }

                // Delete post from database
                database.Posts.Remove(post);
                database.SaveChanges();

                // Redirect to index page
                //return RedirectToAction("Index");
                return RedirectToAction("ListPosts", "Home", new { @chapterId = ChapId });
            }
        }


        
        private bool IsUserAuthorizedToEdit(Post post)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = post.IsAuthor(this.User.Identity.Name);

            return isAdmin || isAuthor;
        }

    }
}
