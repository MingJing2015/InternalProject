using InternalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternalProject.RepositoryModel
{
    public class AspNetUserRolesRepo
    {   
        public void AssignRole(RegisterUserVM newUser, int accountType)
        {
            string roleName = "User";
            if (accountType==1)
            {
                roleName = "Seller";
            }
            else if (accountType == 2)
            {
                roleName = "User";
            }

            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
            var user = db.AspNetUsers.Where(u => u.UserName == newUser.userName).FirstOrDefault();
            var role = db.AspNetRoles.Where(r => r.Name == roleName).FirstOrDefault();
            user.AspNetRoles.Add(role);
            db.SaveChanges();
        }
    }
}