using Entity;
using Repository;
using System;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
namespace Services
{
    public class BlogService : IBlogRepository
    {
        public DataTable SearchTopicFromFile(string searchText)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// GET Blog detail by blog id and created by 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public Blog GetBlogInfo(int id, int? createdBy)
        {
            using (var context = new BlogManagementEntities())
            {

                try
                {
                    if (createdBy == 0)
                        return context.Blogs
                       .Include("User").FirstOrDefault(x => x.BlogID == id);
                    return context.Blogs
                        .Include("User").FirstOrDefault(x => x.BlogID == id && x.CreatedBy == createdBy);
                }
                catch (Exception)
                {
                    return null;
                }

            }
        }



        /// <summary>
        /// Save Blog information
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Save(Blog entity)
        {
            using (var context = new BlogManagementEntities())
            {

                try
                {
                    context.Blogs.Add(entity);
                    return context.SaveChanges();
                }
                catch (Exception)
                {
                    return 0;
                }

            }
        }
        /// <summary>
        /// Save Blog information
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(Blog entity)
        {

            using (var context = new BlogManagementEntities())
            {

                try
                {
                    var original = context.Blogs.Find(entity.BlogID);
                    entity.BlogCreatedDate = original.BlogCreatedDate;
                    entity.CreatedBy = original.CreatedBy;
                    entity.isActive = original.isActive;

                    context.Entry(original).CurrentValues.SetValues(entity);
                    return context.SaveChanges();

                }
                catch (Exception)
                {
                    return 0;
                }

            }
        }

        /// <summary>
        /// Delete Blog information(Making is inActive)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete(Blog entity)
        {
            using (var context = new BlogManagementEntities())
            {

                try
                {
                    var original = context.Blogs.Find(entity.BlogID);

                    entity.BlogCreatedDate = original.BlogCreatedDate;
                    entity.CreatedBy = original.CreatedBy;
                    entity.BlogUpdatedBy = original.BlogUpdatedBy;
                    entity.BlogUpdatedDate = original.BlogUpdatedDate;
                    entity.IP = original.IP;
                    entity.BlogSubTitle = original.BlogSubTitle;
                    entity.BlogText = original.BlogText;
                    entity.BlogTitle = original.BlogTitle;

                    context.Entry(original).CurrentValues.SetValues(entity);
                    return context.SaveChanges();

                }
                catch (Exception)
                {
                    return 0;
                }

            }
        }


        /// <summary>
        /// Get Blogs for home page Order by latest records first
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public string GetBlogsJson(int pageNo, int pageSize)
        {
            using (var context = new BlogManagementEntities())
            {

                try
                {
                    var data = (from blog in context.Blogs
                                join user in context.Users on blog.CreatedBy equals user.UserID
                                where blog.isActive == true && user.isActive == true
                                select new
                                {
                                    blog.BlogID,
                                    blog.BlogText,
                                    blog.BlogTitle,
                                    blog.BlogCreatedDate,

                                    user.UserFirstName,
                                    user.UserLastName

                                })
                                .OrderByDescending(p => p.BlogCreatedDate)
                                .Skip(pageSize * (pageNo - 1))
                                .Take(pageSize)
                                .AsEnumerable()
                                .Select(x => new
                                {
                                    ID = x.BlogID,
                                    Text = Substring(StripHtml(x.BlogText).Replace("&nbsp;", " "), 100),
                                    Title = x.BlogTitle,
                                    Date = x.BlogCreatedDate.Value.ToString("D"),
                                    Name = x.UserFirstName + " " + Convert.ToString(x.UserLastName),


                                });
                    var jsonSerialiser = new JavaScriptSerializer() { MaxJsonLength = 1000000 };
                    return jsonSerialiser.Serialize(data.ToList());

                }
                catch (Exception)
                {
                    return "error";
                }

            }

        }

        /// <summary>
        /// Get Blogs for home page Order by latest records first
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public string GetBlogsJsonFromAdmin(int pageNo, int pageSize)
        {
            using (var context = new BlogManagementEntities())
            {

                try
                {
                    // where blog.isActive == true && user.isActive == true
                    var data = (from blog in context.Blogs
                                join user in context.Users on blog.CreatedBy equals user.UserID

                                select new
                                {
                                    blog.BlogID,
                                    blog.BlogTitle,
                                    blog.BlogCreatedDate,
                                    blog.BlogSubTitle,
                                    blog.isActive,

                                    user.UserFirstName,
                                    user.UserLastName

                                })
                                .OrderByDescending(p => p.BlogCreatedDate)
                                .Skip(pageSize * (pageNo - 1))
                                .Take(pageSize)
                                .AsEnumerable()
                                .Select(x => new
                                {
                                    ID = x.BlogID,
                                    Title = Substring(x.BlogTitle, 50),
                                    SubTitle = Substring(Convert.ToString(x.BlogSubTitle), 50),
                                    Date = x.BlogCreatedDate.Value.ToString("D"),
                                    Name = x.UserFirstName + " " + Convert.ToString(x.UserLastName),
                                    isActive = x.isActive

                                });
                    var jsonSerialiser = new JavaScriptSerializer() { MaxJsonLength = 1000000 };
                    return jsonSerialiser.Serialize(data.ToList());

                }
                catch (Exception)
                {
                    return "error";
                }

            }

        }
        /// <summary>
        /// Method to replace HTML Tags from string
        /// </summary>
        /// <param name="Txt"></param>
        /// <returns></returns>
        protected string StripHtml(string Txt)
        {
            return Regex.Replace(Txt, "<(.|\\n)*?>", string.Empty);
        }

        /// <summary>
        /// Method to replace HTML Tags from string
        /// </summary>
        /// <param name="Txt"></param>
        /// <returns></returns>
        protected string Substring(string Txt, int length)
        {
            if (!string.IsNullOrEmpty(Txt))
            {
                return Txt.Length > length ? Txt.Substring(0, length) : Txt;
            }
            return Txt;
        }


        public Blog Get(int id)
        {
            throw new NotImplementedException();
        }
        public Blog SaveRecord(Blog entity)
        {
            throw new NotImplementedException();
        }
        public Blog UpdateRecord(Blog entity)
        {
            throw new NotImplementedException();
        }
    }
}
