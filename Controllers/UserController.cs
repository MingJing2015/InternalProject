using InternalProject.Models;
using InternalProject.RepositoryModel;
using InternalProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternalProject.Controllers
{
    [Authorize(Roles ="User")]
    public class UserController : Controller
    {
        
        public ActionResult Checkout()
        {
            CheckoutRepo cr = new CheckoutRepo();
            return View(cr.GetCheckoutInfo());
        }

        [HttpGet]
        public ActionResult EditCheckout()
        {
            UsersRepo ur = new UsersRepo();
            return View(ur.GetUserInfo());
        }
        [HttpPost]
        public ActionResult EditCheckout(string email, string firstName, string lastName,
             string address, string phone, string ccard , string exp)
        {
            return RedirectToAction("Checkout");
        }

        public ActionResult Success()
        {
            OrderRepo or = new OrderRepo();
            ViewBag.orderID = or.AddOrder();
            if (ViewBag.orderID==-1)
            {
                TempData["msg"] = "Something went wrong with our database. Please contact the administrator.";
                return RedirectToAction("Cart");
            }
            CartRepo cr = new CartRepo();
            cr.RemoveCart();
            return View();
        }

        //-----------------------------------
        //-----------------------------------
        //----------WITH DATABASE------------
        //-----------------------------------
        //-----------------------------------
        public ActionResult Index()
        {
            ProductRepo pr = new ProductRepo();
            return View(pr.GetAllProducts());
        }

        public ActionResult Details(int id)
        {
            ProductRepo pr = new ProductRepo();
            return View(pr.GetProductByID(id));
        }

        public ActionResult Search(string search)
        {
            ProductRepo pr = new ProductRepo();
            var query = pr.SearchProducts(search);
            ViewBag.itemNumber = query.Count();
            if (ViewBag.itemNumber==0)
            {
                query = pr.GetAllProducts();
            }
            return View(query);
        }


        public ActionResult UserProfile()
        {
            UsersRepo ur = new UsersRepo();
            return View(ur.GetUserInfo());
        }


        [HttpGet]
        public ActionResult EditUser()
        {
            UsersRepo ur = new UsersRepo();
            return View(ur.GetUserInfo());
        }

        [HttpPost]
        public ActionResult EditUser(AccountVM model)
        {
            UsersRepo ur = new UsersRepo();
            bool status = ur.EditUserInfo(model);
            if (status)
            {
                return RedirectToAction("UserProfile");
            }
            return RedirectToAction("EditUser");
        }

        public ActionResult Cart()
        {
            CartRepo cr = new CartRepo();
            CartVM cart = cr.GetAllItemsByUser(User.Identity.Name);

            ViewBag.subtotal = ProductCalculation.SubTotal(cart);

            return View(cart);
        }

        [HttpPost]
        public ActionResult Cart(CartVM cart)
        {
            CartRepo cr = new CartRepo();

            ViewBag.UpdateCart = "";
            if (cr.UpdateCart(cart))
            {
                ViewBag.UpdateCart = "Saved Changes";
            }

            CartVM updatedCart = cr.GetAllItemsByUser(User.Identity.Name);

            ViewBag.subtotal = ProductCalculation.SubTotal(updatedCart);

            return View(updatedCart);
        }

        [HttpGet]
        public ActionResult DeleteItem(int id)
        {
            CartRepo cr = new CartRepo();
            cr.RemoveItem(id);
            CartVM cart = cr.GetAllItemsByUser(User.Identity.Name);
            return RedirectToAction("Cart",new { cart = cart });
        }

        [HttpPost]
        public ActionResult AddProductToCart(int productID)
        {
            CartRepo cr = new CartRepo();
            bool success = cr.AddProduct(productID);

            // TODO display "fail to add" message?
            if (success)
            {
                return RedirectToAction("Cart");
            }
            else
            {
                return RedirectToAction("Cart");
            }
        }

        public ActionResult PurchaseHistory()
        {
            OrderRepo or = new OrderRepo();
            return View(or.DisplayAll());
        }
    }
}