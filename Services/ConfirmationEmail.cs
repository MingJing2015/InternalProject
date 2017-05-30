using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace InternalProject.Services
{
    public class ConfirmationEmail
    {
        public static void SendEmail(string type, string email, string callbackUrl)
        {
            string subject = "";
            string message = "";
            if (type == "REGISTER")
            {
                subject = "One more step to Complete your registeration";
                message = "Please confirm your account by clicking this link: <a href=\""
                               + callbackUrl + "\">Confirm Registration</a>";
            }
            else if (type == "RESET")
            {
                subject = "Reset password for your BinaryBase account.";
                message = "Please reset your password by clicking <a href=\""
                                        + callbackUrl + "\">here</a>";
            }

            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress(email, email.Substring(0, email.IndexOf('@'))));

                // From
                mailMsg.From = new MailAddress("no-reply@BinaryBase.com", "BinaryBase");

                // Subject and multipart/alternative Body
                mailMsg.Subject = subject;
                string text = message;
                string html = @"<p>" + message + @"</p>";

                mailMsg.AlternateViews.Add(
                        AlternateView.CreateAlternateViewFromString(text,
                        null, MediaTypeNames.Text.Plain));
                mailMsg.AlternateViews.Add(
                        AlternateView.CreateAlternateViewFromString(html,
                        null, MediaTypeNames.Text.Html));

                // Init SmtpClient and send
                string sendGridID = System.Configuration.ConfigurationManager.AppSettings["SendGridID"];
                string sendGridPassword = System.Configuration.ConfigurationManager.AppSettings["SendGridPassword"];

                SmtpClient smtpClient
                = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials
                = new System.Net.NetworkCredential(sendGridID, sendGridPassword);
                smtpClient.Credentials = credentials;
                smtpClient.Send(mailMsg);
                System.Diagnostics.Debug.WriteLine("Sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine("Failed");
            }

        }
    }
}