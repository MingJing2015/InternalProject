using InternalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternalProject.RepositoryModel
{
    public class AccountRepo
    {
        public bool AddAccount(RegisterUserVM accountVM)
        {
            // TODO: implement validation and remove try/catch if possible
            try
            {
                DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

                UserTable account = new UserTable()
                {
                    userName = accountVM.userName,
                    firstName = accountVM.firstName,
                    lastName = accountVM.lastName,
                    phone = accountVM.phone
                };

                db.UserTables.Add(account);
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