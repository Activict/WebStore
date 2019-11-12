using System;
using System.Collections.Generic;
using System.IO;
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

        // GET: Shop/Category/name
        public ActionResult Category(string name)
        {
            List<ProductVM> productVMList;

            using (Db db = new Db())
            {
                CategoryDTO categoryDTO = db.Categories.Where(m => m.Slug == name).FirstOrDefault();

                int catId = categoryDTO.Id;

                productVMList = db.Products.ToArray().Where(m => m.CategoryId == catId).Select(m => new ProductVM(m)).ToList();

                ViewBag.CategoryName = categoryDTO.Slug;

                //var productCat = db.Products.Where(m => m.CategoryId == catId).FirstOrDefault();

                //if (productCat == null)
                //{
                //    var catName = db.Categories.Where(m => m.Slug == name).Select(m => m.Name).FirstOrDefault();
                //    ViewBag.CategoryName = catName;
                //}
                //else
                //{
                //    ViewBag.CategoryName = productCat.CategoryName;
                //}
            }

            return View(productVMList);
        }

        // GET: Shop/product-details/name
        [ActionName("product-details")]
        public ActionResult ProductDetails(string name)
        {
            ProductDTO dto;
            ProductVM model;

            int id = 0;

            using (Db db = new Db())
            {
                if (!db.Products.Any(m => m.Slug.Equals(name)))
                {
                    return RedirectToAction("Index", "Shop");
                }
                else
                {
                    dto = db.Products.Where(m => m.Slug == name).FirstOrDefault();
                }

                id = dto.Id;

                model = new ProductVM(dto);
            }

            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                .Select(fn => Path.GetFileName(fn));

            return View("ProductDetails", model);
        }
    }
}