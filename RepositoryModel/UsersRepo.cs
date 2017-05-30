using InternalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternalProject.RepositoryModel
{
    public class UsersRepo
    {
        public IEnumerable<AccountVM> GetAllUsers()
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
            var users = db.UserTables.Select(
                u => new AccountVM()
                {
                    address = u.Account.Addresses.FirstOrDefault().houseNumber
                            + " " + u.Account.Addresses.FirstOrDefault().street
                            + ", " + u.Account.Addresses.FirstOrDefault().city
                            + ", " + u.Account.Addresses.FirstOrDefault().province,
                    addressID = u.Account.Addresses.FirstOrDefault().addressID,
                    addressHouseNumber = u.Account.Addresses.FirstOrDefault().houseNumber,
                    addressStreet = u.Account.Addresses.FirstOrDefault().street,
                    addressCity = u.Account.Addresses.FirstOrDefault().city,
                    addressProvince = u.Account.Addresses.FirstOrDefault().province,
                    firstName = u.firstName,
                    lastName = u.lastName,
                    password = "******",
                    phone = u.phone,
                    username = u.userName
                });
            return users;
        }

        public IEnumerable<AccountVM> SearchUsers(string search)
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
            var users = db.UserTables.Where(s => s.userName.ToUpper().Contains(search.ToUpper())
                              || s.firstName.ToUpper().Contains(search.ToUpper())
                              || s.lastName.ToUpper().Contains(search.ToUpper())).Select(
                u => new AccountVM()
                {
                    address = u.Account.Addresses.FirstOrDefault().houseNumber
                            + " " + u.Account.Addresses.FirstOrDefault().street
                            + ", " + u.Account.Addresses.FirstOrDefault().city
                            + ", " + u.Account.Addresses.FirstOrDefault().province,
                    addressID = u.Account.Addresses.FirstOrDefault().addressID,
                    addressHouseNumber = u.Account.Addresses.FirstOrDefault().houseNumber,
                    addressStreet = u.Account.Addresses.FirstOrDefault().street,
                    addressCity = u.Account.Addresses.FirstOrDefault().city,
                    addressProvince = u.Account.Addresses.FirstOrDefault().province,
                    firstName = u.firstName,
                    lastName = u.lastName,
                    password = "******",
                    phone = u.phone,
                    username = u.userName
                });
            return users;
        }

        public AccountVM GetUserInfo(string username=null)
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
            if (username == null)
            {
                username = HttpContext.Current.User.Identity.Name;
            }//else only admin can get info by username
            var user = db.UserTables.Where(u => u.userName == username).Select(
                u => new AccountVM()
                {
                    address = u.Account.Addresses.FirstOrDefault().houseNumber
                            + " " + u.Account.Addresses.FirstOrDefault().street
                            + ", " + u.Account.Addresses.FirstOrDefault().city
                            + ", " + u.Account.Addresses.FirstOrDefault().province,
                    addressID = u.Account.Addresses.FirstOrDefault().addressID,
                    addressHouseNumber = u.Account.Addresses.FirstOrDefault().houseNumber,
                    addressStreet = u.Account.Addresses.FirstOrDefault().street,
                    addressCity = u.Account.Addresses.FirstOrDefault().city,
                    addressProvince = u.Account.Addresses.FirstOrDefault().province,
                    firstName = u.firstName,
                    lastName = u.lastName,
                    password = "******",
                    phone = u.phone,
                    username = u.userName
                }).FirstOrDefault();
            return user;
        }

        public bool RegisterUser(RegisterUserVM model)
        {
            // TODO: incomplete
            // TODO: implement validation and remove try/catch if possible
            try
            {
                DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

                Account account = new Account()
                {
                    userName = model.userName,
                    accountType = 2 // TODO account type for normal user
                };

                UserTable user = new UserTable()
                {
                    userName = model.userName,
                    firstName = model.firstName,
                    lastName = model.lastName,
                    phone = model.phone
                };

                //Address address = new Address()
                //{
                //    userName = model.username,
                //    houseNumber = model.addressHouseNumber,
                //    street = model.addressStreet,
                //    city = model.addressCity,
                //    province = model.addressProvince
                //};

                // TODO: need to implement adding to AspNet tables
                db.Accounts.Add(account);
                db.UserTables.Add(user);
                //db.Addresses.Add(address);

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RegisterUser(SellerVM model)
        {
            // TODO: incomplete
            // TODO: implement validation and remove try/catch if possible
            try
            {
                DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

                Account account = new Account()
                {
                    userName = model.username,
                    accountType = 2 // TODO account type for normal user
                };

                UserTable user = new UserTable()
                {
                    userName = model.username,
                    firstName = model.firstName,
                    lastName = model.lastName,
                    phone = model.phone
                };

                Address address = new Address()
                {
                    userName = model.username,
                    houseNumber = model.addressHouseNumber,
                    street = model.addressStreet,
                    city = model.addressCity,
                    province = model.addressProvince
                };

                // TODO: need to implement adding to AspNet tables
                db.Accounts.Add(account);
                db.UserTables.Add(user);
                db.Addresses.Add(address);

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool EditUserInfo(AccountVM model)
        {
            // TODO: implement validation and remove try/catch if possible
            // TODO: change password
            try
            {
                DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

                var username = HttpContext.Current.User.Identity.Name;

                // for editing the information as admin
                // fix this if statement with role
                if (username=="admin")
                {
                    username = model.username;
                }

                var user = db.UserTables.Where(u => u.userName == username).FirstOrDefault();
                user.firstName = model.firstName;
                user.lastName = model.lastName;
                user.phone = model.phone;

                var address = db.Addresses.Where(a => a.userName == username).FirstOrDefault();
                address.houseNumber = model.addressHouseNumber;
                address.street = model.addressStreet;
                address.city = model.addressCity;
                address.province = model.addressProvince;

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteUser(string username)
        {
            try
            {
                DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
                
                var user = db.UserTables.Where(u => u.userName == username).FirstOrDefault();

                var addresses = db.Addresses.Where(a => a.userName == username);
                foreach (var item in addresses)
                {
                    db.Addresses.Remove(item);
                }

                var carts = db.Carts.Where(c => c.userName == username);
                foreach (var item in carts)
                {
                    db.Carts.Remove(item);
                }

                db.UserTables.Remove(user);
                db.Accounts.Remove(db.Accounts.Where(a => a.userName == username).FirstOrDefault());

                // TODO: need to implement removing from AspNet tables
                //var iuser = db.AspNetUsers.Where(u => u.UserName == username).FirstOrDefault();
                //db.AspNetUsers.Remove(iuser);


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