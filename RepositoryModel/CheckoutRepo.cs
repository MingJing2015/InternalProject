using InternalProject.Models;
using InternalProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternalProject.RepositoryModel
{
    public class CheckoutRepo
    {
        public CheckoutVM GetCheckoutInfo()
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

            var username = HttpContext.Current.User.Identity.Name;

            var cart = db.Carts.Where(c => c.userName == username).Select(
                c => new CartItemVM()
                {
                    productID = c.productID,
                    userName = c.userName,
                    productName = c.Product.name,
                    price = (decimal)c.Product.price,
                    discountPrice = (decimal)c.Product.discountPrice,
                    quantity = (int)c.quantity,
                    imgUrl = "http://www.dakotajang.me/binarybase/Images/" + c.Product.imgUrl,
                });
            
            string summary = "";
            foreach (var item in cart)
            {
                summary += item.productName + ":" + item.quantity.ToString() + "/n";
            }

            var cartsCast = new CartVM()
            {
                Products = cart.ToList()
            };
            decimal subtotal = ProductCalculation.SubTotal(cartsCast);
            decimal gst = ProductCalculation.GstTax(cartsCast);
            decimal pst = ProductCalculation.QstTax(cartsCast);
            decimal totalPrice = ProductCalculation.TotalPrice(cartsCast);



            CheckoutVM checkoutInfo = db.UserTables.Where(u => u.userName == username).Select(
                u => new CheckoutVM()
                {
                    firstName = u.firstName,
                    lastName = u.lastName,
                    summary = summary,
                    address = u.Account.Addresses.FirstOrDefault().houseNumber
                            + " " + u.Account.Addresses.FirstOrDefault().street
                            + ", " + u.Account.Addresses.FirstOrDefault().city
                            + ", " + u.Account.Addresses.FirstOrDefault().province,
                    products = cart.ToList(),
                    subtotal = subtotal,
                    gst = gst,
                    pst = pst,
                    total = totalPrice
                }).FirstOrDefault();
            return checkoutInfo;
        }
    }
}