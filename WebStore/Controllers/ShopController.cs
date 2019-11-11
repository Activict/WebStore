using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebStore.Models.Data;
using WebStore.Models.ViewModels.Shop;

namespace WebStore.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }

        public ActionResult CategoryMenuPartial()
        {
            List<CategoryVM> categoryVMList;

            using (Db db = new Db())
            {
                categoryVMList = db.Categories.ToArray().OrderBy(m => m.Sorting).Select(m => new CategoryVM(m)).ToList();
            }

            return PartialView("_CategoryMenuPartial", categoryVMList);
        }
    }
}