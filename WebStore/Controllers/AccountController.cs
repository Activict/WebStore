using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebStore.Models.Data;
using WebStore.Models.ViewModels.Account;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        // GET: account/create-account
        [ActionName("create-account")]
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        [ActionName("create-account")]
        [HttpPost]
        public ActionResult CreateAccount(UserVM model)
        {
            if (!ModelState.IsValid)
            {
                return (View("CreateAccount", model));
            }

            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Password do not match!");
                return View("CreateAccount", model);
            }

            using (Db db = new Db())
            {
                if (db.Users.Any(m => m.UserName.Equals(model.UserName)))
                {
                    ModelState.AddModelError("", $"User name {model.UserName} is taken.");
                    model.UserName = "";
                    return View("CreateAccount", model);
                }

                UserDTO userDTO = new UserDTO()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.UserName,
                    Password = model.Password
                };

                db.Users.Add(userDTO);
                db.SaveChanges();

                int id = userDTO.Id;

                UserRoleDTO userRoleDTO = new UserRoleDTO()
                {
                    UserId = id,
                    RoleId = 2
                };

                db.UserRoles.Add(userRoleDTO);
                db.SaveChanges();
            }

            TempData["SM"] = "You are now registered and can login.";
            
            return RedirectToAction("Login");
        }

        // GET: Account/Login
        [HttpGet]
        public ActionResult Login()
        {
            string userName = User.Identity.Name;

            if (!string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("user-profile");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUserVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isValid = false;
            using (Db db = new Db())
            {
                if (db.Users.Any(m => m.UserName.Equals(model.UserName) && m.Password.Equals(model.Password)))
                {
                    isValid = true;
                }

                if (!isValid)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    return Redirect(FormsAuthentication.GetRedirectUrl(model.UserName, model.RememberMe));
                }
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}