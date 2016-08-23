using System;
using System.Web.Mvc;
using Entity;
using Services;
using BlogApp.Model;
namespace BlogApp.Controllers
{
    public class AdminController : Controller
    {

        public ActionResult Index()
        {
            var userInfo = Helpers.StaticValues.ReadCookie();

            if (userInfo == null)
                return RedirectToAction(Resources.StaticContent.AdminLogin);
            return View();
        }

        public ActionResult Login()
        {
            var userInfo = Helpers.StaticValues.ReadCookie();
            if (userInfo != null)
                return RedirectToAction(Resources.StaticContent.AdminAddBlog);
            return View();
        }
        public ActionResult Logout()
        {
            Helpers.StaticValues.DeleteCookie();
            return RedirectToAction(Resources.StaticContent.AdminLogin);
        }
        public ActionResult BlogList()
        {
            var loginUserInfo = Helpers.StaticValues.ReadCookie();
            if (loginUserInfo == null) return View();
            if (Convert.ToInt16(loginUserInfo[4]) == Convert.ToInt16(Helpers.StaticValues.Roles.SuperAdmin))
                return View();
            Helpers.StaticValues.DeleteCookie();
            return RedirectToAction(Resources.StaticContent.AdminLogin);
        }
        public ActionResult UpdateBlog()
        {
            var loginUserInfo = Helpers.StaticValues.ReadCookie();
            if (loginUserInfo == null) return View();
            if (Convert.ToInt16(loginUserInfo[4]) == Convert.ToInt16(Helpers.StaticValues.Roles.SuperAdmin))
                return View();
            Helpers.StaticValues.DeleteCookie();
            return RedirectToAction(Resources.StaticContent.AdminLogin);
        }
        /// <summary>
        /// Create admin user pageload
        /// </summary>
        /// <returns></returns>
        public ActionResult AddUsers()
        {
            var loginUserInfo = Helpers.StaticValues.ReadCookie();
            if (loginUserInfo == null) return View();
            if (Convert.ToInt16(loginUserInfo[4]) == Convert.ToInt16(Helpers.StaticValues.Roles.SuperAdmin))
                return View();
            Helpers.StaticValues.DeleteCookie();
            return RedirectToAction(Resources.StaticContent.AdminLogin);
        }

        /// <summary>
        /// Add new admin User
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public int AddNewUser([System.Web.Http.FromBody]User userInfo)
        {
            var loginUserInfo = Helpers.StaticValues.ReadCookie();
            if (loginUserInfo == null) return 0;
            if (Convert.ToInt16(loginUserInfo[4]) != Convert.ToInt16(Helpers.StaticValues.Roles.SuperAdmin))
                return 0;
            var obj = new UserService();
            userInfo.CreatedDate = DateTime.Now;
            userInfo.IP = Helpers.StaticValues.GetIpAddress();
            userInfo.isActive = true;
            userInfo.UserRole = Convert.ToInt16(Helpers.StaticValues.Roles.Admin);
            return obj.Save(userInfo);
        }

        /// <summary>
        ///Get Admin  Users List 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetUserList()
        {
            var loginUserInfo = Helpers.StaticValues.ReadCookie();
            if (loginUserInfo == null) return null;
            if (Convert.ToInt16(loginUserInfo[4]) != Convert.ToInt16(Helpers.StaticValues.Roles.SuperAdmin))
                return null;
            var obj = new UserService();
            return obj.GetUsers(Convert.ToInt32(loginUserInfo[1]));
        }

        /// <summary>
        /// Make Admin User Inactive/Active
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public int InactiveUser([System.Web.Http.FromBody]User userInfo)
        {
            var loginUserInfo = Helpers.StaticValues.ReadCookie();
            if (loginUserInfo == null) return 0;
            if (Convert.ToInt16(loginUserInfo[4]) != Convert.ToInt16(Helpers.StaticValues.Roles.SuperAdmin)) return 0;
            var obj = new UserService();
            return obj.Delete(userInfo);
        }
        [HttpPost]
        public ActionResult Login([System.Web.Http.FromBody]User userInfo)
        {
            var obj = new UserService();
            userInfo = obj.AuthenticateUser(userInfo);
            if (userInfo != null)
            {
                var logintime = DateTime.Now.ToString("f");
                Helpers.StaticValues.CreateFormAuthCookie(userInfo.UserName, true, userInfo.UserName + '?' + userInfo.UserID + '?' + userInfo.UserFirstName + '?' + userInfo.UserLastName + '?' + userInfo.UserRole + '?' + userInfo.CreatedDate + '?' + logintime);
                return RedirectToAction(Resources.StaticContent.AdminAddBlog);
            }
            ViewBag.Error = Resources.StaticContent.LoginErrorMessage;
            return View(Resources.StaticContent.AdminLogin);
        }

        //Post method to add details    
        [ValidateInput(false)]
        [HttpPost]
        public int AddBlog(Blog obj)
        {
            var userInfo = Helpers.StaticValues.ReadCookie();
            var objBal = new BlogService();
            obj.CreatedBy = Convert.ToInt32(userInfo[1]);
            obj.BlogCreatedDate = DateTime.Now;
            obj.isActive = true;
            obj.IP = Helpers.StaticValues.GetIpAddress();
            return objBal.Save(obj);

        }
        /// <summary>
        /// Admin page blog list
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>

        //Post method to add details    
        [ValidateInput(false)]
        [HttpPost]
        public int UpdateBlog(Blog obj)
        {
            var userInfo = Helpers.StaticValues.ReadCookie();
            var objBal = new BlogService();
            obj.CreatedBy = Convert.ToInt32(userInfo[1]);
            obj.BlogCreatedDate = DateTime.Now;
            obj.isActive = true;

            obj.IP = Helpers.StaticValues.GetIpAddress();

            return objBal.Update(obj);

        }
        /// <summary>
        /// Update blog list
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        /// 
        public ActionResult Blog()
        {
            var objDetail = new BlogDetail();

            var objupdateBal = new BlogService();
            var objHeaderDetail = new BlogDetailHeader();
            var id = RouteData.Values["id"];
            try
            {

                objDetail.BlogInfo = objupdateBal.GetBlogInfo(Convert.ToInt32(id), 0);
                if (objDetail.BlogInfo != null)
                {
                    objHeaderDetail.BlogTitle = objDetail.BlogInfo.BlogTitle;
                    objHeaderDetail.BlogSubTitle = objDetail.BlogInfo.BlogSubTitle;
                    if (objDetail.BlogInfo.BlogCreatedDate != null)
                        objHeaderDetail.PostedDate = objDetail.BlogInfo.BlogCreatedDate.Value.ToString("D");
                    objHeaderDetail.PostedBy = objDetail.BlogInfo.User.UserFirstName + " " + Convert.ToString(objDetail.BlogInfo.User.UserLastName);
                    objDetail.BlogHeaderInfo = objHeaderDetail;
                }
            }
            catch { }
            return View(objDetail);

        }       

        [HttpGet]
        public string GetBlogs(int pageNo)
        {
            var objBal = new BlogService();
            return objBal.GetBlogsJsonFromAdmin(pageNo, Helpers.StaticValues.PageSize);

        }
        /// <summary>
        /// Make Admin User Inactive/Active
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public int InactiveBlog([System.Web.Http.FromBody]Blog blogInfo)
        {
            var loginUserInfo = Helpers.StaticValues.ReadCookie();
            if (loginUserInfo == null) return 0;
            if (Convert.ToInt16(loginUserInfo[4]) != Convert.ToInt16(Helpers.StaticValues.Roles.SuperAdmin)) return 0;
            var obj = new BlogService();
            return obj.Delete(blogInfo);
        }
    }
}
