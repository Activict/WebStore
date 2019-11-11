using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebStore.Models.Data;
using WebStore.Models.ViewModels.Pages;

namespace WebStore.Controllers
{
    public class PagesController : Controller
    {
        // GET: Index/{page}
        public ActionResult Index(string page="")
        {
            if (page == "")
            {
                page = "home";
            }

            PageVM model;
            PagesDTO dto;

            // если нет передавамой страницы
            using (Db db = new Db())
            {
                if (!db.Pages.Any(m => m.Slug.Equals(page)))
                {
                    return RedirectToAction("Index", new { page = "" });
                }
            }

            using (Db db = new Db())
            {
                dto = db.Pages.Where(m => m.Slug == page).FirstOrDefault();
            }

            ViewBag.PageTitle = dto.Title;

            if (dto.HasSidebar == true)
            {
                ViewBag.Sidebar = "Yes";
            }
            else
            {
                ViewBag.Sidebar = "No";
            }

            model = new PageVM(dto);

            return View(model);
        }

        // GET:
        public ActionResult PagesMenuPartial()
        {
            List<PageVM> pageVMList;

            using (Db db = new Db())
            {
                pageVMList = db.Pages.ToArray().OrderBy(m => m.Sorting).Where(m => m.Slug != "home")
                    .Select(m => new PageVM(m)).ToList();
            }

            return PartialView("_PagesMenuPartial", pageVMList);
        }

        public ActionResult SidebarPartial()
        {
            SidebarVM model;

            using (Db db = new Db())
            {
                SidebarDTO dto = db.Sidebars.Find(1);

                model = new SidebarVM(dto);
            }

            return PartialView("_SidebarPartial", model);
        }
    }
}