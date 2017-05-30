using InternalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternalProject.RepositoryModel
{
    public class ProductRepo
    {
        public IEnumerable<ProductVM> GetAllProducts()
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
            var products = db.Products.Select(
                p => new ProductVM() {
                    discountPrice = (decimal) p.discountPrice,
                    imgUrl = "http://www.dakotajang.me/binarybase/Images/" + p.imgUrl,
                    manufacturer = p.userName,
                    name = p.name,
                    price = (decimal) p.price,
                    productID = p.productID,
                    summary = p.summary,
                    vendor = p.Vendor.vendorName
                });
            return products;
        }

        public IEnumerable<ProductVM> GetAllProductsByUser(string username = null)
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

            if (username == null)
            {
                username = HttpContext.Current.User.Identity.Name;
            }

            var products = db.Products.Where(p=>p.userName == username).Select(
                p => new ProductVM()
                {
                    discountPrice = (decimal)p.discountPrice,
                    imgUrl = "http://www.dakotajang.me/binarybase/Images/" + p.imgUrl,
                    manufacturer = p.manufacturer,
                    name = p.name,
                    price = (decimal)p.price,
                    productID = p.productID,
                    summary = p.summary,
                    vendor = p.Vendor.vendorName
                });
            return products;
        }

        public ProductVM GetProductByID(int id, bool edit=false)
        {
            string imageLocation = "http://www.dakotajang.me/binarybase/Images/";
            if (edit)
            {
                imageLocation = "";
            }
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
            var product = db.Products.Where(p=> p.productID==id).Select(
                p => new ProductVM()
                {
                    discountPrice = (decimal)p.discountPrice,
                    imgUrl = imageLocation + p.imgUrl,
                    manufacturer = p.manufacturer,
                    name = p.name,
                    price = (decimal)p.price,
                    productID = p.productID,
                    summary = p.summary,
                    vendor = p.Vendor.vendorName
                }).FirstOrDefault();
            return product;
        }

        public bool EditProductInfo(ProductVM model)
        {
            // TODO: implement validation and remove try/catch if possible
            // TODO: change password
            try
            {
                DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

                var username = HttpContext.Current.User.Identity.Name;
                if (model.vendor!=null && username=="admin")
                {
                    username = db.Vendors.Where(v => v.vendorName == model.vendor).FirstOrDefault().userName;

                }var product = db.Products.Where(p => p.productID == model.productID).FirstOrDefault();
                product.name = model.name;
                product.userName = username;
                product.price = model.price;
                product.summary = model.summary;
                product.discountPrice = model.discountPrice;
                product.imgUrl = model.imgUrl;
                product.manufacturer = model.manufacturer;


                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteProduct(int productID)
        {
            try
            {
                DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

                var carts = db.Carts.Where(c => c.productID == productID);
                foreach (var item in carts)
                {
                    db.Carts.Remove(item);
                }

                Product prod = db.Products.Find(productID);
                db.Products.Remove(prod);           

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<ProductVM> SearchProducts(string search)
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
            var products = db.Products.Where(s => s.name.ToUpper().Contains(search.ToUpper())
                              || s.Vendor.vendorName.ToUpper().Contains(search.ToUpper())).Select(
                p => new ProductVM()
                {
                    discountPrice = (decimal)p.discountPrice,
                    imgUrl = "http://www.dakotajang.me/binarybase/Images/" + p.imgUrl,
                    manufacturer = p.manufacturer,
                    name = p.name,
                    price = (decimal)p.price,
                    productID = p.productID,
                    summary = p.summary,
                    vendor = p.Vendor.vendorName
                });
            return products;
        }


        public bool AddingProduct(ProductVM model)
        {
            // TODO: incomplete
            // TODO: implement validation and remove try/catch if possible
            try
            {
                DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

                Product product = new Product()
                {
                    name = model.name,
                    userName = HttpContext.Current.User.Identity.Name,
                    summary = model.summary,
                    price = model.price,
                    discountPrice= model.discountPrice,
                    imgUrl = model.imgUrl
                };

                db.Products.Add(product);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}