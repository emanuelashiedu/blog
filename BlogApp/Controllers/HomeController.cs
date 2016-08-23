using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using Services;
using BlogApp.Model;
namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() { return View(); }
        public ActionResult Contact() { return View(); }
        public ActionResult About() { return View(); }

        /// <summary>
        /// Blog Information Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Blog()
        {
            var objDetail = new BlogDetail();

            var objBal = new BlogService();
            var objHeaderDetail = new BlogDetailHeader();
            var id = RouteData.Values["id"];
            try
            {
                
                objDetail.BlogInfo = objBal.GetBlogInfo(Convert.ToInt32(id), 0);
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
       
        /// <summary>
        /// Home page blog list
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        
        [HttpGet]
        public string GetBlogs(int pageNo)
        {
            var objBal = new BlogService();
            return objBal.GetBlogsJson(pageNo, Helpers.StaticValues.PageSize);

        }
    }
}
