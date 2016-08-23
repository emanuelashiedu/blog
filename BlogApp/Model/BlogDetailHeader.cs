using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entity;
namespace BlogApp.Model
{
    public class BlogDetailHeader
    {
        public string BlogTitle { get; set; }
        public string BlogSubTitle { get; set; }
        public string PostedBy { get; set; }
        public string PostedDate { get; set; }
    }
    public class BlogDetail
    {
        public Blog BlogInfo { get; set; }
        public BlogDetailHeader BlogHeaderInfo { get; set; }
    }
}