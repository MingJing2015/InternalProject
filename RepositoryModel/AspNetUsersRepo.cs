using InternalProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using InternalProject.Services;
using System.Web.Mvc;

namespace InternalProject.RepositoryModel
{
    public class AspNetUsersRepo
    {
        // Additional Security Measures of the Identity Model
        // Registrant Confirmation
        const string EMAIL_CONFIRMATION = "EmailConfirmation";
        const string PASSWORD_RESET = "ResetPassword";

        //to store reset tokens locally
        void CreateTokenProvider(UserManager<IdentityUser> manager, string tokenType)
        {
            manager.UserTokenProvider = new EmailTokenProvider<IdentityUser>();
        }

        public void SignInUser(LoginVM login, HttpContextBase HttpContext)
        {
            IAuthenticationManager authenticationManager
                                   = HttpContext.GetOwinContext()
                                    .Authentication;
            authenticationManager
                   .SignOut(DefaultAuthenticationTypes.ExternalCookie);

            var identity = new ClaimsIdentity(new[] {
                                            new Claim(ClaimTypes.Name, login.UserName),
                                        },
                                DefaultAuthenticationTypes.ApplicationCookie,
                                ClaimTypes.Name, ClaimTypes.Role);
            // SignIn() accepts ClaimsIdentity and issues logged in cookie. 
            authenticationManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = false
            }, identity);
        }

        public bool ValidLogin(LoginVM login, out string message)
        {
            // UserStore and UserManager manages data retreival.
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(userStore)
            {
                UserLockoutEnabledByDefault = true,
                DefaultAccountLockoutTimeSpan = new TimeSpan(0, 10, 0),
                MaxFailedAccessAttemptsBeforeLockout = 3
            };

            // Username and Email.
            var user = userManager.FindByName(login.UserName);
            message = "";
            if (user == null)
            {
                message = "Invalid username or password.";
                return false;
            }

            // User is locked out.
            if (userManager.SupportsUserLockout && userManager.IsLockedOut(user.Id))
            {
                message = "This account has been locked out. Please try again later.";
                return false;
            }

            // Validated user was locked out but now can be reset.
            if (userManager.CheckPassword(user, login.Password) && userManager.IsEmailConfirmed(user.Id))
            {
                if (userManager.SupportsUserLockout && userManager.GetAccessFailedCount(user.Id) > 0)
                {
                    userManager.ResetAccessFailedCount(user.Id);
                }
            }
            // Login is invalid so increment failed attempts.
            else
            {
                bool lockoutEnabled = userManager.GetLockoutEnabled(user.Id);
                if (!userManager.IsEmailConfirmed(user.Id))
                {
                    // email has not been confirmed
                    message = "Please confirm your email before logging in.";
                }
                else if (userManager.SupportsUserLockout && userManager.GetLockoutEnabled(user.Id))
                {
                    message = "Invalid username or password.";
                    userManager.AccessFailed(user.Id);
                }
                else
                {
                    message = "Something went wrong.";
                }
                return false;
            }
            return true;
        }

        // Attempts to registers user and if successful sends an email to user
        public bool RegisterUser(RegisterUserVM newUser, UrlHelper Url, HttpRequestBase Request, out string message)
        {
            DB_110727_binarybaseEntities db = new DB_110727_binarybaseEntities();
            var email = db.AspNetUsers.Where(x => x.Email == newUser.email).FirstOrDefault();
            if (email != null)
            {
                message = "That email have been used already.";
                return false;
            }

            var userStore = new UserStore<IdentityUser>();

            // enables lockouts for 10 minutes after 3 failed logins:
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore)
            {
                UserLockoutEnabledByDefault = true,
                DefaultAccountLockoutTimeSpan = new TimeSpan(0, 10, 0),
                MaxFailedAccessAttemptsBeforeLockout = 3
            };

            IdentityUser identityUser = new IdentityUser()
            {
                UserName = newUser.userName,
                Email = newUser.email
            };

            IdentityResult result = manager.Create(identityUser, newUser.password);

            if (result.Succeeded)
            {
                // create/send email
                CreateTokenProvider(manager, EMAIL_CONFIRMATION);

                string code = manager.GenerateEmailConfirmationToken(identityUser.Id);
                string callbackUrl = Url.Action("EmailConfirmation", "Home",
                                                new { userId = identityUser.Id, code = code },
                                                    protocol: Request.Url.Scheme);

                ConfirmationEmail.SendEmail("REGISTER", identityUser.Email, callbackUrl);
                message = identityUser.UserName;

                return true;
            }
            else
            {
                message = result.Errors.FirstOrDefault().ToString();
                return false;
            }
        }


        // create the ConfirmEmail action method:
        public bool ConfirmLink(string userID, string code)
        {
            var userStore                     = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            CreateTokenProvider(manager, EMAIL_CONFIRMATION);
            try
            {
                IdentityResult result = manager.ConfirmEmail(userID, code);
                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool EmailForgotPassword(string email, UrlHelper Url, HttpRequestBase Request)
        {
            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            var user = manager.FindByEmail(email);
            if (user != null)
            {
                CreateTokenProvider(manager, PASSWORD_RESET);

                var code = manager.GeneratePasswordResetToken(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Home",
                                             new { userId = user.Id, code = code },
                                             protocol: Request.Url.Scheme);
                ConfirmationEmail.SendEmail("RESET", user.Email, callbackUrl);
                return true;
            }
            return false;
        }

        // For update password
        public String GetUserID(string email)
        {
            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);

            var user = manager.FindByEmail(email);
            if (user != null)
            {
                return user.Id;
            }
            return null;
        }

        // For update password
        public String GetUserToken(string email)
        {
            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);

            var user = manager.FindByEmail(email);
            if (user != null)
            {
                CreateTokenProvider(manager, PASSWORD_RESET);

                var code = manager.GeneratePasswordResetToken(user.Id);

                return code;
            }
            return null;
        }

        public bool ResetUserPassword(string password, string passwordConfirm, string passwordToken, string userID, out string message)
        {
            if (password == passwordConfirm)
            {
                var userStore = new UserStore<IdentityUser>();
                UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
                var user = manager.FindById(userID);
                user.EmailConfirmed = true;
                CreateTokenProvider(manager, PASSWORD_RESET);

                IdentityResult result = manager.ResetPassword(userID, passwordToken, password);
                if (result.Succeeded)
                {
                    message = "The password has been reset.";

                    return true;
                }
                else
                {
                    message = "The password has not been reset.";
                    return false;
                }
            }
            else
            {
                message = "Both passwords must be same.";
                return false;
            }
        }

        public void AssignRole(RegisterUserVM newUser, int accountType)
        {
            string roleName = "User";
            if (accountType == 1)
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
