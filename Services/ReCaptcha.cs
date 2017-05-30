using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternalProject.Services
{
    using Newtonsoft.Json;
    public class ReCaptcha
    {
        // Validating Recaptcha 2 (No CAPTCHA reCAPTCHA) in ASP.NET's server side
        // http://stackoverflow.com/questions/27764692/validating-recaptcha-2-no-captcha-recaptcha-in-asp-nets-server-side
        // https://www.google.com/recaptcha/admin#site/337380147

        public static string Validate(string EncodedResponse)
        {
            var client = new System.Net.WebClient();

            string PrivateKey = "6LczAxwUAAAAAGOJJeIXplIx1NukXOxTBsaklc7_";

            var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey, EncodedResponse));

            var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ReCaptcha>(GoogleReply);

            return captchaResponse.Success;
        }

        [JsonProperty("success")]
        public string Success
        {
            get { return m_Success; }
            set { m_Success = value; }
        }

        private string m_Success;
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get { return m_ErrorCodes; }
            set { m_ErrorCodes = value; }
        }

        private List<string> m_ErrorCodes;
    }
}