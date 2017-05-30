using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InternalProject.Controllers
{
    public class DocumentController : ApiController
    {
        // GET: api/Document
        public IEnumerable<string> Get()
        {

            return new string[] {
                "Products API document",
                "1. Get all document",
                "URL: http://...",
                "GET: api/Document",
                "For example: http://localhost:4510/api/Document"

            };
        }

        // GET: api/Document/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Document
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Document/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Document/5
        public void Delete(int id)
        {
        }
    }
}
