using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebStore.Models.Data;
using WebStore.Models.ViewModels.Pages;

namespace WebStore.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            List<PageVM> pageList;
            using(Db db = new Db())
            {
                pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }

            return View(pageList);
        }

        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            // Check model
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                // Short review Slug
                string slug;

                // PageDTO
                PagesDTO dto = new PagesDTO();

                dto.Title = model.Title.ToUpper();

                // Check Slug and uniq Name Page
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                if (db.Pages.Any(x => x.Title == model.Title))
                {
                    ModelState.AddModelError("", "That title already exist.");
                    return View(model);
                }
                else if (db.Pages.Any(x => x.Slug == slug)) 
                {
                    ModelState.AddModelError("", "That Slug already exist.");
                    return View(model);
                }

                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                db.Pages.Add(dto);
                db.SaveChanges();

                TempData["SM"] = "You have added a new page";
            }

            return RedirectToAction("Index");
        }

        // GET: Admin/Pages/PageDetails
        public ActionResult PageDetails(int id)
        {
            PageVM model;
            using (Db db = new Db())
            {
                PagesDTO page = db.Pages.Find(id);

                if (page == null)
                {
                    return Content("The page does not exist.");
                }

                model = new PageVM(page);
            }
            return View(model);
        }

        // GET: Admin/Pages/EditPage
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            PageVM model;
            using (Db db = new Db())
            {
                // PagesDTO page = db.Pages.FirstOrDefault(m => m.Id == id);
                PagesDTO page = db.Pages.Find(id);

                if (page == null)
                {
                    return Content("The page does not exist.");
                }

                model = new PageVM(page);
            }

            return View(model);
        }

        // POST: Admin/Pages/EditPage
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                int id = model.Id;
                string slug = "home";
                PagesDTO page = db.Pages.Find(id);
                page.Title = model.Title;
                
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                if (db.Pages.Where(m => m.Id != id).Any(m => m.Title == model.Title))
                {
                    ModelState.AddModelError("", "That title already exist.");
                    return View(model);
                }
                else if (db.Pages.Where(m => m.Id != id).Any(m => m.Slug == slug))
                {
                    ModelState.AddModelError("", "That slug already exist.");
                    model.Slug = "";
                    return View(model);
                }

                page.Slug = slug;
                page.Body = model.Body;
                page.HasSidebar = model.HasSidebar;

                db.SaveChanges();
            }

            TempData["SM"] = "You have edited the page.";

            return RedirectToAction("Index");
        }

        // GET: Admin/Pages/DeletePage/id
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                PagesDTO page = db.Pages.Find(id);

                db.Pages.Remove(page);
                db.SaveChanges();
            }
            TempData["SM"] = "You have daleted page.";

            return RedirectToAction("Index");
        }

        // POST: /Admin/Pages/RecorderPages
        [HttpPost]
        public void RecorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                int count = 1;
                PagesDTO page;

                foreach (var pageId in id)
                {
                    page = db.Pages.Find(pageId);
                    page.Sorting = count;

                    db.SaveChanges();
                    count++;
                }
            }
        }

        // GET: /Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            SidebarVM model;
            using (Db db = new Db())
            {
                SidebarDTO dto = db.Sidebars.Find(1);

                model = new SidebarVM(dto);
            }

            return View(model);
        }
        // POST: /Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            using (Db db = new Db())
            {
                SidebarDTO dto = db.Sidebars.Find(1);
                dto.Body = model.Body;
                dto.Id = model.Id;
                db.SaveChanges();
            }
            TempData["SM"] = "You have edited the sidebar.";
            return RedirectToAction("EditSidebar");
        }
    }
}