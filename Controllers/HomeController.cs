using InternalProject.Models;
using InternalProject.RepositoryModel;
using InternalProject.Services;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace InternalProject.Controllers
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateAccount(RegisterUserVM model, int accountType)
        {
            string message;
            string EncodedResponse = Request.Form["g-recaptcha-response"];
            bool IsCaptchaValid = (ReCaptcha.Validate(EncodedResponse) == "True" ? true : false);

            if (!IsCaptchaValid) 
            {
                //Invalid Request, you are a robot
                message = "Are you a robot ?";

                TempData["ErrorMessage"] = message;
                return View(model);
            }

            if (ModelState.IsValid)
            {
                RegisterUserVM newUser = new RegisterUserVM()
                {
                    userName        = model.userName,
                    email           = model.email,
                    password        = model.password,
                    confirmPassword = model.password
                };

                AspNetUsersRepo anur = new AspNetUsersRepo();
                bool result = anur.RegisterUser(newUser, Url, Request, out message);

                if (result)
                {
                    anur.AssignRole(newUser, accountType);

                    // TODO: add validation here if user exist etc.
                    AccountRepo ar = new AccountRepo();
                    ar.AddAccount(model);

                    if (accountType == 1)
                    {
                        SellerRepo sr = new SellerRepo();
                        if (sr.RegisterSeller(model))
                        {
                            return RedirectToAction("Login");
                        }
                    }
                    else if (accountType == 2)
                    {
                        UsersRepo ur = new UsersRepo();
                        if (ur.RegisterUser(model))
                        {
                            return RedirectToAction("Login");
                        }
                    }
                }
                else
                {
                    message = "Something went wrong on our end, please try again later.";
                }
            }
            message = "Something went wrong in your input.";
            TempData["ErrorMessage"] = message;

            return View(model);
        }

        
        public ActionResult Cart()
        {
            return View();
        }
        
        //-----------------------------------
        //-----------------------------------
        //----------WITH DATABASE------------
        //-----------------------------------
        //-----------------------------------

        // GET: Home.
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (User.IsInRole("Seller"))
                {
                    return RedirectToAction("Index", "Seller");
                }
                else if (User.IsInRole("User"))
                {
                    return RedirectToAction("Index", "User");
                }
            }
            ProductRepo pr = new ProductRepo();
            return View(pr.GetAllProducts());
        }

        public ActionResult Details(int id)
        {
            ProductRepo pr = new ProductRepo();
            return View(pr.GetProductByID(id));
        }

        // POST: Search, and list search result 
        [HttpPost]
        public ActionResult Search(string search)
        {
            ProductRepo pr = new ProductRepo();
            var query = pr.SearchProducts(search);
            ViewBag.itemNumber = query.Count();
            if (ViewBag.itemNumber == 0)
            {
                query = pr.GetAllProducts();
            }
            return View(query);
        }



        [HttpGet]
        public ActionResult DesignDocument()
        {
            return View();
        }

        [HttpGet]
        public ActionResult WebApi()
        {
            return View();
        }


        //-----------------------------------
        //-----------------------------------
        //----------OWIN SECURITY------------
        //-----------------------------------
        //-----------------------------------


        [HttpGet]
        public ActionResult Login(int id = 0)
        {
            var ctx = Request.GetOwinContext();
            IAuthenticationManager authenticationManager = ctx.Authentication;

            if (Request.IsAuthenticated)
            {
                // Already logined
                return RedirectToAction("Index", "Home");
            }

            if (id == 1)
            {

                TempData["ErrorMessage"] = "Please login to checkout.";
            }
            else if (id == 2)
            {
                TempData["ErrorMessage"] = "Entered username does not exist.";
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM login)
        {
            string message = "";
            DateTime start = DateTime.Now;

            AspNetUsersRepo userRepo = new AspNetUsersRepo();

            if (ModelState.IsValid)
            {
                if (userRepo.ValidLogin(login, out message))
                {
                    userRepo.SignInUser(login, HttpContext);
                    DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
                    var hasRole = db.AspNetUsers.Where(u => u.UserName == login.UserName).FirstOrDefault().AspNetRoles.FirstOrDefault();
                    string currentUserRole = "User";
                    if (hasRole != null)
                    {
                        currentUserRole = db.AspNetUsers.Where(u => u.UserName == login.UserName).FirstOrDefault().AspNetRoles.FirstOrDefault().Name;
                    }
                    if (currentUserRole == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (currentUserRole == "Seller")
                    {
                        return RedirectToAction("Index", "Seller");
                    }
                    else if (currentUserRole == "User")
                    {
                        return RedirectToAction("Index", "User");
                    }
                }
            }
            else
            {
                message = "Invalid submission";
            }

            System.Threading.Thread.Sleep(1000);

            TempData["ErrorMessage"] = message;
            return View();
        }

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            IAuthenticationManager authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();

            return RedirectToAction("Login", "Home");
        }

        public ActionResult EmailConfirmation(string userID, string code)
        {
            AspNetUsersRepo userRepo = new AspNetUsersRepo();
            if (userRepo.ConfirmLink(userID, code))
            {
                // success message
                TempData["message"] = "Email confirmed";
            }
            else
            {
                // fail message
                TempData["message"] = "Email NOT confirmed";
            }
            return View();
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            TempData["message"] = "";
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            string message;
            string EncodedResponse = Request.Form["g-recaptcha-response"];
            bool IsCaptchaValid = (ReCaptcha.Validate(EncodedResponse) == "True" ? true : false);

            if (!IsCaptchaValid)
            {
                //Invalid Request, you are a robot
                message = "Are you a robot ?";

                TempData["ErrorMessage"] = message;
                return View(model);
            }
            if (ModelState.IsValid)
            {
                AspNetUsersRepo anur = new AspNetUsersRepo();

                bool result = anur.EmailForgotPassword(model.Email, Url, Request);

                // TODO: remove?
                //String token = anur.GetUserToken(model.Email);
                //String userID = anur.GetUserID(model.Email);


                ViewBag.ShowFlag = 1;
                if (result)
                {
                    // success message
                    TempData["message"] = "Go to Email reset password";
                    ViewBag.Message = "Go to your email, and reset password.";
                }
                else
                {
                    // fail message
                    TempData["message"] = "Email NOT reset password, something is wrong";
                    ViewBag.Message = "Something is wrong !";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword(string userID, string code)
        {
            ViewBag.PasswordToken = code;
            ViewBag.UserID = userID;

            //TempData["message"] = "";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model,
                                          string passwordToken, string userID)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AspNetUsersRepo userRepo = new AspNetUsersRepo();
            string message;
            if (userRepo.ResetUserPassword(model.Password, model.ConfirmPassword, passwordToken, userID, out message))
            {
                // success message
                //TempData["message"] = "Password reset";
                return RedirectToAction("ResetPasswordConfirmation", "Home");
            }
            else
            {
                // fail message
                //TempData["message"] = "Password NOT reset";
                ViewBag.Message = "Something is wrong !";
                ViewBag.ShowFlag = 1;
            }
            return View();
        }

        //
        // GET: /Home/ResetPasswordConfirmation
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UpdatePassword(string userName)
        {
            ViewBag.userName = userName;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePassword(UpdatePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // failed input message
                ViewBag.Message = "Something is wrong input !";
                ViewBag.ShowFlag = 1;
                return View(model);
            }

            AspNetUsersRepo userRepo = new AspNetUsersRepo();

            // From Usertable and account
            //UsersRepo ur = new UsersRepo();
            //AccountVM user = ur.GetUserInfo();
            //string email = user.email;

            // From AspNetUsers 
            // UserStore and UserManager manages data retreival.
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            var user = manager.FindByName(User.Identity.Name);
            var email = user.Email;
            //IdentityUser identityUser = manager.FindByName(userName);
            //IdentityUser identityUser = manager.FindByEmail(email);
            //email = identityUser.Email;

            string token = userRepo.GetUserToken(email);
            string userID = userRepo.GetUserID(email);
            string message;

            if ( !manager.CheckPassword(user, model.CurrentPassword))
            {
                ViewBag.Message = "Current password is wrong input !";
                ViewBag.ShowFlag = 1;
                return View(model);
            }

            if (userRepo.ResetUserPassword(model.NewPassword, model.ConfirmPassword, token, userID, out message))
            {
                // successed message
                //TempData["message"] = "Password update";
                return RedirectToAction("UpdatePasswordConfirmation", "Home");
            }
            else
            {
                // failed message
                //TempData["message"] = "Password NOT update";
                ViewBag.Message = "Something is wrong !";
                ViewBag.ShowFlag = 1;
            }
            return View(model);
        }

        // GET: /Home/UpdatePasswordConfirmation
        public ActionResult UpdatePasswordConfirmation()
        {
            return View();
        }
    }
}