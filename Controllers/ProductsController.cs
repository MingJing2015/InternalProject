using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using InternalProject;
using System.Web.Http.Cors;
using InternalProject.Models;

namespace InternalProject.Controllers
{
    public class ProductsController : ApiController
    {
        //?????  change into every method : private DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

        // Disable lazy loading otherwise the REST service returns
        // all data in the database.
        //db.Products.LazyLoadingEnabled = false;


        // The first parameter restricts the domains that can access
        // this service. In this case it is open to all.
        [EnableCors(origins: "*", headers: "*", methods: "*")]

        // GET: api/Products
        public IQueryable<Product> GetProducts()
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

            // Disable lazy loading otherwise the REST service returns
            // all data in the database.
            db.Configuration.LazyLoadingEnabled = false;

            return db.Products;
        }

        //// GET: api/Products/5
        //[ResponseType(typeof(Product))]
        //public IHttpActionResult GetProduct(int id)
        //{
        //    DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

        //    // Disable lazy loading otherwise the REST service returns
        //    // all data in the database.
        //    db.Configuration.LazyLoadingEnabled = false;

        //    Product product = db.Products.Find(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(product);
        //}

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // GET: api/Products/*****-*******-******-*****
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(string id)
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
            var user = db.AspNetUsers.Where(u => u.Id == id).FirstOrDefault();
            string username = null;
            if (user != null)
            {
                username = user.UserName;
            }
            var products = db.Products.Where(a => a.userName == username).Select(
                p => new ProductVM()
                {
                    productID = p.productID,
                    name = p.name,
                    imgUrl = p.imgUrl,
                    price = (decimal)p.price,
                    discountPrice = (decimal)p.discountPrice,
                    vendor = p.Vendor.vendorName,
                    summary = p.summary,
                    manufacturer = p.manufacturer
                });
            //Product product = db.Products.Find(id);
            if (products.Count() == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

            // Disable lazy loading otherwise the REST service returns
            // all data in the database.
            db.Configuration.LazyLoadingEnabled = false;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.productID)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

            // Disable lazy loading otherwise the REST service returns
            // all data in the database.
            db.Configuration.LazyLoadingEnabled = false;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.productID }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

            // Disable lazy loading otherwise the REST service returns
            // all data in the database.
            db.Configuration.LazyLoadingEnabled = false;

            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

            // Disable lazy loading otherwise the REST service returns
            // all data in the database.
            db.Configuration.LazyLoadingEnabled = false;

            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

            // Disable lazy loading otherwise the REST service returns
            // all data in the database.
            db.Configuration.LazyLoadingEnabled = false;

            return db.Products.Count(e => e.productID == id) > 0;
        }
    }
}