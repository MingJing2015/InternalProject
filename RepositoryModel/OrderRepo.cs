using InternalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternalProject.RepositoryModel
{
    public class OrderRepo
    {
        public int AddOrder()
        {
            // TODO: implement validation and remove try/catch if possible
            try
            {
                DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
                CartRepo cr = new CartRepo();
                CartVM cart = cr.GetAllItemsByUser();
                
                OrderTable order = new OrderTable()
                {
                    userName = HttpContext.Current.User.Identity.Name,
                    addressID = null,    // TODO: add address
                    timeStamp = DateTime.Now
                };
                db.OrderTables.Add(order);

                foreach (CartItemVM item in cart.Products)
                {
                    ProductOrder po = new ProductOrder()
                    {
                        orderID = order.orderID,
                        productID = item.productID,
                        quantity = item.quantity
                    };
                    db.ProductOrders.Add(po);
                }
                db.SaveChanges();
                
                return order.orderID;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public IEnumerable<OrderVM> DisplayAll()
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
            var orders = db.OrderTables.Where(o=>o.userName==HttpContext.Current.User.Identity.Name)
                .AsEnumerable().Select(
                o => new OrderVM()
                {
                    orderId = o.orderID,
                    timeStamp = o.timeStamp,
                    summary = o.ProductOrders.Select(po=>
                    new ProductOrderVM() {
                        productName = po.Product.name,
                        quantity = (int) po.quantity
                    }).ToList()
                });
            return orders;
        }
        
        public string summarize(OrderTable ot)
        {
            string result = "";
            IEnumerable<ProductOrder> po = ot.ProductOrders;
            System.Diagnostics.Debug.Print(po.Count().ToString());
            foreach (var item in po)
            {
                result += item.quantity + " of " + item.Product.name + ", ";
            }
            return result;
        }
    }
}