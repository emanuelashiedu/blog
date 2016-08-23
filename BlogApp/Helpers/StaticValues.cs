using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace BlogApp.Helpers
{
    public static class StaticValues
    {
        public static string GetIpAddress()
        {
            var context = HttpContext.Current;
            var ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ipAddress)) return context.Request.ServerVariables["REMOTE_ADDR"];
            var addresses = ipAddress.Split(',');
            return addresses.Length != 0 ? addresses[0] : context.Request.ServerVariables["REMOTE_ADDR"];
        }
        public enum Roles
        {
            SuperAdmin = 1,
            Admin=2
        }

        public static void WriteLog(string ex)
        {
            using (var writer = new StreamWriter("errorlog.txt", true))
            {
                writer.WriteLine(ex);
            }
        }
        public static void CreateFormAuthCookie(string userName, bool isPer, string userData)
        {
            try
            {

                var ticket = new FormsAuthenticationTicket(
                  1,                                      // ticket version
                  userName,                               // authenticated username
                  DateTime.Now,                           // issueDate
                  DateTime.Now.AddDays(1),                // expiryDate
                  isPer,                                  // true to persist across browser sessions
                  userData,                               // can be used to store additional user data
                  FormsAuthentication.FormsCookiePath);   // the path for the cookie

                // Encrypt the ticket using the machine key
                var encryptedTicket = FormsAuthentication.Encrypt(ticket);

                // Add the cookie to the request to save it
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) {HttpOnly = false};
                HttpContext.Current.Response.Cookies.Add(cookie);

            }
            catch (Exception ex) { }


        }
        /*******0)User Name 1) User ID 2) User First Name 3) User Last Name 4)  Role 5) Created Date 6) Login Time****************/
        public static string[] ReadCookie()
        {
            string[] data = null;
            try
            {
                var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    //Extract the forms authentication cookie
                    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    // If caching roles in userData field then extract
                    if (authTicket != null) data = authTicket.UserData.Split('?');
                }
            }
            catch (Exception ex) { }
            return data;

        }
        public static void DeleteCookie()
        {
            if (HttpContext.Current == null) return;
            var cookieCount = HttpContext.Current.Request.Cookies.Count;
            for (var i = 0; i < cookieCount; i++)
            {
                var cookie = HttpContext.Current.Request.Cookies[i];
                if (cookie == null) continue;
                var cookieName = cookie.Name;
                var expiredCookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1) };
                HttpContext.Current.Response.Cookies.Add(expiredCookie); // overwrite it
            }
            // clear cookies server side
            HttpContext.Current.Request.Cookies.Clear();
        }
        /// <summary>
        /// ENCRYPT STRING
        /// </summary>
        /// <returns></returns>
        public static string Encrypt(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            var keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(ViKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return ReplaceCharacters(Convert.ToBase64String(cipherTextBytes));
        }
        /// <summary>
        /// DECRYPT STRING
        /// <returns></returns>
        public static string Decrypt(string encryptedText)
        {
            encryptedText = RedoReplaceCharacters(encryptedText);
            var cipherTextBytes = Convert.FromBase64String(encryptedText);
            var keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(ViKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var plainTextBytes = new byte[cipherTextBytes.Length];

            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

        public static string ReplaceCharacters(string text)
        {
            try
            {
                return text.Replace("+", "aplus").Replace("&", "a_mp").Replace("`", "tilt").Replace("<", "lthan")
                    .Replace(">", "gthan").Replace("'", "s_core").Replace("/", "fslash").Replace("+", "p_lus").Replace("-", "m_inus").Replace("=", "e_qual");
            }
            catch (Exception ex) { return text; }
        }
        public static string RedoReplaceCharacters(string text)
        {
            try
            {
                return text.Replace("aplus", "+").Replace("a_mp", "&").Replace("tilt", "`").Replace("lthan", "<")
                    .Replace("gthan", ">").Replace("s_core", "'").Replace("fslash", "/").Replace("p_lus", "+").Replace("m_inus", "-").Replace("e_qual", "=");
            }
            catch (Exception ex) { return text; }
        }


        /// <summary>
        /// Static and Common Properties Region
        /// </summary>
        #region

        public static readonly int PageSize = 10;

        private const string PasswordHash = "eOXP-3FD";
        private const string SaltKey = "S@GlwdUuH";
        private const string ViKey = "@1G78HTYqas6Lkl9";

        public static readonly string BaseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;


        public static readonly string ApplicationNameText = Resources.StaticContent.ApplicationNameText;
        public static readonly string AdminLoginHeaderText = Resources.StaticContent.AdminLoginHeaderText;
        public static readonly string AdminCreateBlogText = Resources.StaticContent.AdminCreateBlogText;
        public static readonly string AdminUpdateBlogText = Resources.StaticContent.AdminUpdateBlogText;
        public static readonly string NewAdminUserText = Resources.StaticContent.NewAdminUserText;
        public static readonly string LogoutText = Resources.StaticContent.LogoutText;
        public static readonly string HeaderApplicationName = Resources.StaticContent.HeaderApplicationName;
        public static readonly string HeaderApplicationText = Resources.StaticContent.HeaderApplicationText;
        public static readonly string HomePageHeaderImage = Resources.StaticContent.HomePageHeaderImage;
        public static readonly string BlogPageHeaderImage = Resources.StaticContent.BlogPageHeaderImage;
        public static readonly string ContactUsText = Resources.StaticContent.ContactUsText;
        public static readonly string AboutUsText = Resources.StaticContent.AboutUsText;
        

        #endregion
    }
}