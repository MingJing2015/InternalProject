using InternalProject.Models;
using InternalProject.RepositoryModel;
using InternalProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InternalProject.Controllers
{
    [Authorize(Roles = "Seller")]
    public class SellerController : Controller
    {
        private DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
        
        // GET: Seller
        public ActionResult Index()
        {
            return RedirectToAction("Products");
        }

        public ActionResult Products(string user)
        {
            ProductRepo pr = new ProductRepo();
            return View(pr.GetAllProductsByUser(user));
        }

        public ActionResult SellerProfile(string user)
        {
            SellerRepo sr = new SellerRepo();
            return View(sr.GetSellerInfo(user));
        }
        [HttpGet]
        public ActionResult EditSeller()
        {
            SellerRepo sr = new SellerRepo();
            return View(sr.GetSellerInfo());
        }
        [HttpPost]
        public ActionResult EditSeller(SellerVM model)
        {
            SellerRepo ur = new SellerRepo();
            bool status = ur.EditUserInfo(model);
            if (status)
            {
                return RedirectToAction("SellerProfile");
            }
            return RedirectToAction("EditSeller");
        }

        [HttpGet]
        public ActionResult EditProduct(int productID)
        {
            ProductRepo pr = new ProductRepo();

            return View(pr.GetProductByID(productID,true));
        }

        [HttpPost]
        public ActionResult EditProduct(ProductVM model, HttpPostedFileBase imageFile)
        {
            if (imageFile != null)
            {
                model.imgUrl = User.Identity.Name + "/" + imageFile.FileName;
                ImageService.UploadImage(imageFile, Server.MapPath("~/Images/" + User.Identity.Name));
            }
            ProductRepo pr = new ProductRepo();
            bool status = pr.EditProductInfo(model);
            if (status)
            {
                return RedirectToAction("Products");
            }
            return RedirectToAction("EditProduct", new { productID = model.productID });
        }

        public ActionResult DetailProduct(int id)
        {
            ProductRepo pr = new ProductRepo();
            return View(pr.GetProductByID(id));
        }

        [HttpGet]
        public ActionResult DeleteProduct(ProductVM model)
        {
            ProductRepo pr = new ProductRepo();

            return View(pr.GetProductByID(model.productID));
        }

        [HttpPost]
        public ActionResult DeleteProduct(int productID)
        {
            ProductRepo pr = new ProductRepo();
            bool status = pr.DeleteProduct(productID);
            if (status)
            {
                return RedirectToAction("Products");
            }
            return RedirectToAction("DeleteProduct");
        }

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
        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(ProductVM model, HttpPostedFileBase imgUrl)
        {
            if (ModelState.IsValid)
            {
                ProductRepo pr = new ProductRepo();
                model.vendor = User.Identity.Name;
                model.imgUrl = User.Identity.Name + "/" + imgUrl.FileName;
                bool status = pr.AddingProduct(model);
                if (status)
                {
                    // TODO validate adding image
                    // TODO use GUID instead of userName if possible
                    ImageService.UploadImage(imgUrl, Server.MapPath("~/Images/" + User.Identity.Name));
                    return RedirectToAction("Products");
                }
            }
            return View(model);
        }





        //[HttpPost]
        //public ActionResult AddProduct()
        //{

        //}


        //public ActionResult Products()
        //{
        //    List<ProductVM> ps = new List<ProductVM>();
        //    ps.Add(ProductRepo.SampleProduct(0));
        //    return View(ps);
        //}
        //public ActionResult SellerProfile()
        //{
        //    SellerRepo details = new SellerRepo();

        //    return View(SellerRepo.SampleAccount(0));
        //}

        //[HttpGet]
        //public ActionResult EditSeller()
        //{
        //    SellerRepo details = new SellerRepo();

        //    return View(SellerRepo.SampleAccount(0));
        //}
        //[HttpPost]
        //public ActionResult EditSeller(string username, string password, string firstName, string lastName, string email,
        //    string phone, string address)
        //{
        //    return RedirectToAction("SellerProfile");
        //}

        //[HttpGet]
        //public ActionResult AddProduct()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult AddProduct(string name)
        //{
        //    return RedirectToAction("Products");
        //}
        //[HttpGet]
        //public ActionResult EditProduct(int productID)
        //{
        //    ProductRepo pr = new ProductRepo();

        //    return View(ProductRepo.SampleProduct(productID));
        //}
        //[HttpPost]
        //public ActionResult EditProduct(string name, decimal price, decimal discountPrice, string imgUrl)
        //{
        //    return RedirectToAction("Products");
        //}
        //[HttpGet]
        //public ActionResult DeleteProduct(int productID)
        //{
        //    ProductRepo pr = new ProductRepo();
        //    return View(ProductRepo.SampleProduct(productID));
        //}
        //[HttpPost]
        //public ActionResult DeleteProduct()
        //{


        //    return RedirectToAction("Products");
        //}
        //public ActionResult Search()
        //{
        //    List<ProductVM> ps = new List<ProductVM>();
        //    ps.Add(ProductRepo.SampleProduct(0));
        //    return View(ps);
        //}

    }
}