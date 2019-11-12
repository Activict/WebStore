using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebStore.Models.Data;
using WebStore.Models.ViewModels.Cart;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            if (cart.Count == 0 || Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return View();
            }

            decimal total = 0m;

            foreach (var item in cart)
            {
                total += item.Total;
            }

            ViewBag.GrandTotal = total;

            return View(cart);
        }

        public ActionResult CartPartial()
        {
            CartVM model = new CartVM();
            int qty = 0;
            decimal price = 0m;

            if (Session["cart"] != null)
            {
                var list = (List<CartVM>)Session["cart"];

                foreach (var item in list)
                {
                    qty += item.Quantity;
                    price += item.Price * item.Quantity;
                }

                model.Quantity = qty;
                model.Price = price;
            }
            else
            {
                model.Quantity = 0;
                model.Price = 0m;
            }

            return PartialView("_CartPartial", model);
        }

        public ActionResult AddToCartPartial(int id)
        {
            List<CartVM> cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            CartVM model = new CartVM();

            using (Db db = new Db())
            {
                ProductDTO product = db.Products.Find(id);

                var productInCart = cart.FirstOrDefault(m => m.ProductId == id);

                if (productInCart == null)
                {
                    cart.Add(new CartVM()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Quantity = 1,
                        Price = product.Price,
                        Image = product.ImageName
                    });
                }
                else
                {
                    productInCart.Quantity++;
                }
            }
            int qty = 0;
            decimal price = 0m;

            foreach (var item in cart)
            {
                qty += item.Quantity;
                price += item.Price * item.Quantity;
            }

            model.Quantity = qty;
            model.Price = price;

            Session["cart"] = cart;

            return PartialView("_AddToCartPartial", model);
        }

        public JsonResult IncrementProduct(int productId)
        {
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            using (Db db = new Db())
            {
                CartVM model = cart.FirstOrDefault(m => m.ProductId == productId);

                model.Quantity++;

                var result = new { qty = model.Quantity, price = model.Price };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DecrementProduct(int productId)
        {
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            using (Db db = new Db())
            {
                CartVM model = cart.FirstOrDefault(m => m.ProductId == productId);

                if (model.Quantity > 1)
                {
                    model.Quantity--;
                }
                else
                {
                    model.Quantity = 0;
                    cart.Remove(model);
                }

                var result = new { qty = model.Quantity, price = model.Price };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public void RemoveProduct(int productId)
        {
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            using (Db db = new Db())
            {
                CartVM model = cart.FirstOrDefault(m => m.ProductId == productId);

                cart.Remove(model);
            }
        }
    }
}