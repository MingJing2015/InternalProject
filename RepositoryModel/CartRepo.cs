using InternalProject.Models;
using System;
using System.Linq;
using System.Web;

namespace InternalProject.RepositoryModel
{
    public class CartRepo
    {

        public CartVM GetAllItemsByUser(string username = null)
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

            if (username == null)
            {
                username = HttpContext.Current.User.Identity.Name;
            }

            var carts = db.Carts.Where(c=>c.userName == username).Select(
                c=> new CartItemVM() {
                    productID = c.productID,
                    userName = c.userName,
                    productName = c.Product.name,
                    price =(decimal)c.Product.price,
                    discountPrice =(decimal)c.Product.discountPrice,
                    quantity = (int)c.quantity,
                    imgUrl = "http://www.dakotajang.me/binarybase/Images/" + c.Product.imgUrl,
                });
            var cartsCast = new CartVM()
            {
                Products = carts.ToList()
            };
            return cartsCast;
        }

        public CartItemVM GetItemById(int id)
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

            var cart = db.Carts.Where(c => c.productID == id && c.userName== HttpContext.Current.User.Identity.Name).Select(
                c => new CartItemVM()
                {
                    productID = c.productID,
                    userName = c.userName,
                    productName = c.Product.name,
                    price = (decimal)c.Product.price,
                    discountPrice = (decimal)c.Product.discountPrice,
                    quantity = (int)c.quantity,
                    imgUrl = "http://www.dakotajang.me/binarybase/Images/" + c.Product.imgUrl,
                }).FirstOrDefault();
            return cart;
        }



        public bool AddProduct(int productID)
        {
            CartItemVM item = GetItemById(productID);


            // TODO: implement validation and remove try/catch if possible
            try
            {
                DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

                if (item == null)
                {
                    // New product add to cart
                    Cart cartItem = new Cart()
                    {
                        productID = productID,
                        userName = HttpContext.Current.User.Identity.Name,
                        quantity = 1
                    };

                    db.Carts.Add(cartItem);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    var cart = db.Carts.Where(c => c.productID == productID ).FirstOrDefault();
                    cart.quantity++;

                    // update quantity
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveItem(int productID)
        {
            try
            {
                DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

                var username = HttpContext.Current.User.Identity.Name;

                var carts = db.Carts.Where(c => c.productID == productID && c.userName == username);
                foreach (var item in carts)
                {
                    db.Carts.Remove(item);
                }

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EditItemInfo(CartItemVM model)
        {
            try
            {
                DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
                var username = model.userName;
                var cart = db.Carts.Where(c => c.productID == model.productID && username == model.userName).FirstOrDefault();
                cart.Product.name = model.productName;
                cart.quantity = model.quantity;
                cart.productID = model.productID;
                cart.Product.discountPrice = model.discountPrice;
                cart.Product.price = model.price;
                cart.Product.imgUrl = model.imgUrl;

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateCart(CartVM cart)
        {
            try
            {
                DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
                var username = HttpContext.Current.User.Identity.Name;

                foreach (CartItemVM cartItem in cart.Products)
                {
                    var query = db.Carts.Where(c => c.productID == cartItem.productID).FirstOrDefault();
                    query.quantity = (int)cartItem.quantity;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void RemoveCart()
        {
            CartVM cart = GetAllItemsByUser();

            foreach (CartItemVM item in cart.Products)
            {
                RemoveItem(item.productID);
            }
        }
    }
}