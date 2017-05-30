using InternalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternalProject.RepositoryModel
{
    public class SellerRepo
    {

        public IEnumerable<SellerVM> GetAllSellers()
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
            var sellers = db.Vendors.Select(
                u => new SellerVM()
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
                    username = u.userName,
                    vendor = u.vendorName
                });
            return sellers;
        }

        public IEnumerable<SellerVM> SearchSellers(string search)
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
            var sellers = db.Vendors.Where(s => s.vendorName.ToUpper().Contains(search.ToUpper())
                              || s.userName.ToUpper().Contains(search.ToUpper())
                              || s.firstName.ToUpper().Contains(search.ToUpper())
                              || s.lastName.ToUpper().Contains(search.ToUpper())).Select(
                u => new SellerVM()
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
                    username = u.userName,
                    vendor = u.vendorName
                });
            return sellers;
        }

        public SellerVM GetSellerInfo(string username = null)
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

            if (username == null)
            {
                username = HttpContext.Current.User.Identity.Name;
            }

            var seller = db.Vendors.Where(u => u.userName == username).Select(
                u => new SellerVM()
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
                    vendor = u.vendorName,
                    username = u.userName
                }).FirstOrDefault();
            return seller;
        }

        public bool RegisterSeller(RegisterUserVM model)
        {
            // TODO: incomplete
            // TODO: implement validation and remove try/catch if possible
            try
            {
                DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

                Account account = new Account()
                {
                    userName = model.userName,
                    accountType = 1 // TODO account type for normal user
                };

                Vendor seller = new Vendor()
                {
                    userName = model.userName,
                    firstName = model.firstName,
                    lastName = model.lastName,
                    phone = model.phone,
                    vendorName = model.vendorName
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
                db.Vendors.Add(seller);
                //db.Addresses.Add(address);

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EditUserInfo(SellerVM model)
        {
            // TODO: implement validation and remove try/catch if possible
            // TODO: change password
            try
            {
                DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
                var username = HttpContext.Current.User.Identity.Name;

                // for editing the information as admin
                // fix this if statement with role
                if (username == "admin")
                {
                    username = model.username;
                }

                var seller = db.Vendors.Where(u => u.userName == username).FirstOrDefault();
                seller.firstName = model.firstName;
                seller.lastName = model.lastName;
                seller.phone = model.phone;
                seller.vendorName = model.vendor;

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

        public bool DeleteSeller(string username)
        {
            try
            {
                DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();

                var user = db.Vendors.Where(u => u.userName == username).FirstOrDefault();

                var addresses = db.Addresses.Where(a => a.userName == username);
                foreach (var item in addresses)
                {
                    db.Addresses.Remove(item);
                }

                var products = db.Products.Where(a => a.userName == username);
                foreach (var item in products)
                {
                    db.Carts.Remove(db.Carts.Where(c => c.productID == item.productID).FirstOrDefault());
                    db.Products.Remove(item);
                }

                db.Vendors.Remove(user);
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