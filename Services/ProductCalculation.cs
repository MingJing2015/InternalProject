using InternalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternalProject.Services
{
    public static class ProductCalculation
    {
        const decimal QST = 0.15m;
        const decimal GST = 0.05m;

        public static decimal QstTax(CartVM model)
        {
            var qstTax = 0m;
            var price = 0m;

            foreach (CartItemVM item in model.Products)
            {
               price += item.discountPrice * item.quantity;
            }

            qstTax = price * QST;
            return qstTax;
        }
        
        public static decimal GstTax(CartVM model)
        {
            var gstTax = 0m;
            var price = 0m;

            foreach (CartItemVM item in model.Products)
            {
                price += item.discountPrice * item.quantity;
            }

            gstTax = price * GST;
            return gstTax;
        }

        public static decimal SubTotal(CartVM model)
        {
            decimal price = 0m;

            foreach (CartItemVM item in model.Products)
            {
                price += item.discountPrice * item.quantity;
            }

            return price;
        }

        public static decimal TotalPrice(CartVM model)
        {
            var totalprice = 0m;
            var price = 0m;

            foreach (CartItemVM item in model.Products)
            {
                price += item.discountPrice * item.quantity;
            }

            totalprice = (price + (price * GST) + (price * QST));
            return totalprice;
        }
    }
}