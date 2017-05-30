using InternalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternalProject.RepositoryModel;

namespace InternalProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // JM Todo need this method ???
        //public ActionResult Search()
        //{
        //    // TODO: requires Database implementation
        //    UsersRepo details = new UsersRepo();
        //    //  ???   return View(UsersRepo.listAccount);
        //    return View();
        //}

        // Users -------------------------------------------------------------------------------------
        // GET: Admin Users list
        public ActionResult UsersList(string searchString)
        {
            UsersRepo ur = new UsersRepo();

            if ((searchString == null) || searchString.Length <= 0)
                return View(ur.GetAllUsers());
            else
                return View(ur.SearchUsers(searchString));
        }

        // GET: Admin User Create
        public ActionResult UserCreate()
        {
            return View();
        }

        // POST: Admin User Create
        [HttpPost]
        public ActionResult UserCreate(RegisterUserVM account)
        {
            UsersRepo ur = new UsersRepo();
            if (ModelState.IsValid)
            {
                if (ur.RegisterUser(account))
                {
                    return RedirectToAction("UsersList", "Admin");
                }
            }
            return View();
        }

        // GET: Admin User Edit
        public ActionResult UserEdit(string username)
        {
            UsersRepo ur = new UsersRepo();
            return View(ur.GetUserInfo(username));
        }

        // POST: Admin User Edit update
        [HttpPost]
        public ActionResult UserEdit(AccountVM account)
        {
            UsersRepo ur = new UsersRepo();
            bool status = ur.EditUserInfo(account);
            if (status)
            {
                return RedirectToAction("UsersList", "Admin");
            }
            return RedirectToAction("UserEdit", new { username = account.username });
        }

        // GET: Admin User Delete
        public ActionResult UserDelete(string username)
        {
            UsersRepo ur = new UsersRepo();
            ur.DeleteUser(username);
            return RedirectToAction("UsersList", "Admin");
        }

        // GET: Admin User Detail
        public ActionResult UserDetail(string username)
        {
            UsersRepo ur = new UsersRepo();
            return View(ur.GetUserInfo(username));
        }
        // Products ----------------------------------------------------------------------------------

        // GET: Admin list Products
        public ActionResult ProductsList(string searchString)
        {
            // TODO: requires Database implementation
            ProductRepo pr = new ProductRepo();

            if( (searchString == null) || searchString.Length <= 0 )  
                return View(pr.GetAllProducts());
            else
                return View(pr.SearchProducts(searchString));
        }

        // GET: Admin Product  Edit
        //  ?? [ValidateAntiForgeryToken]     for debug  ??? 20170405   Why use :   @Html.AntiForgeryToken()  
        public ActionResult ProductEdit(int productID)
        {
            ProductRepo pr = new ProductRepo();
            return View(pr.GetProductByID(productID));
        }

        // Comment :    @Html.AntiForgeryToken()    in ProductEdit.cshtml   for debug  ??? 20170405


        // HttpPost: Admin Product  Edit
        [HttpPost]
        public ActionResult ProductEdit(ProductVM product)
        {
            ProductRepo pr = new ProductRepo();
            pr.EditProductInfo(product);

            return RedirectToAction("ProductsList", "Admin");
        }

        // GET: Admin Product  Delete
        public ActionResult ProductDelete(int productID)
        {
            ProductRepo pr = new ProductRepo();
            pr.DeleteProduct(productID);
            return RedirectToAction("ProductsList", "Admin");
        }

        // GET: Admin Product Create
        public ActionResult ProductCreate()
        {
            return View();
        }

        // POST: Admin Product Create
        [HttpPost]
        public ActionResult ProductCreate(ProductVM product)
        {
            ProductRepo pr = new ProductRepo();
            pr.AddingProduct(product);

            return RedirectToAction("ProductsList", "Admin");
        }

        // GET: Admin Product Detail
        public ActionResult ProductDetail(int productID)
        {
            ProductRepo pr = new ProductRepo();
            return View(pr.GetProductByID(productID));
        }

        // Sellers --------------------------------------------------------------------------
        // GET: Admin Sellers list
        public ActionResult SellersList(string searchString)
        {
            SellerRepo pr = new SellerRepo();

            if ((searchString == null) || searchString.Length <= 0)
                return View(pr.GetAllSellers());
            else
                return View(pr.SearchSellers(searchString));
        }


        // GET: Admin User Create
        public ActionResult SellerCreate()
        {
            return View();
        }

        // POST: Admin User Create
        [HttpPost]
        public ActionResult SellerCreate(RegisterUserVM seller)
        {
            SellerRepo sr = new SellerRepo();
            if (ModelState.IsValid)
            {
                if (sr.RegisterSeller(seller))
                {
                    return RedirectToAction("SellersList", "Admin");
                }
            }
            return View();
        }

        // GET: Admin User Edit
        public ActionResult SellerEdit(String username)
        {
            SellerRepo sr = new SellerRepo();
            return View(sr.GetSellerInfo(username));
        }

        // POST: Admin User Edit update
        [HttpPost]
        public ActionResult SellerEdit(SellerVM seller)
        {
            SellerRepo sr = new SellerRepo();
            if (ModelState.IsValid)
            {
                if (sr.EditUserInfo(seller))
                {
                    return RedirectToAction("SellersList", "Admin");
                }
            }
            return View();
        }

        // GET: Admin User Delete
        public ActionResult SellerDelete(String username)
        {
            SellerRepo sr = new SellerRepo();
            if (sr.DeleteSeller(username))
            {
                return RedirectToAction("SellersList", "Admin");
            }
            return RedirectToAction("SellersList", "Admin");
        }

        // GET: Admin Seller Detail
        public ActionResult SellerDetail(String username)
        {
            SellerRepo sr = new SellerRepo();
            return View(sr.GetSellerInfo(username));
        }

        // POST: Search, and list search result 
        [HttpPost]
        public ActionResult Search(string search)
        {
            ProductRepo pr = new ProductRepo();
            var query = pr.SearchProducts(search);
            ViewBag.itemNumber = query.Count();
            return View(query);
        }



    }
}