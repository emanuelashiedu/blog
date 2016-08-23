using Entity;
using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
namespace Services
{
    public class UserService : IUserRepository
    {

        /// <summary>
        /// Authenticate User on login
        /// </summary>
        /// <param name="uinfo"></param>
        /// <returns></returns>
        public User AuthenticateUser(User uinfo)
        {
            using (var context = new BlogManagementEntities())
            {

                try
                {
                    return context.Users.FirstOrDefault(u => u.UserName == uinfo.UserName && u.UserPassword == uinfo.UserPassword);

                }
                catch (Exception)
                {
                    return null;
                }

            }
        }
        public int Save(User entity)
        {
            using (var context = new BlogManagementEntities())
            {

                try
                {
                    context.Users.Add(entity);
                    return context.SaveChanges();
                }
                catch (Exception)
                {
                    return 0;
                }

            }
        }
        public int Update(User entity)
        {
            throw new NotImplementedException();
        }
        public int Delete(User entity)
        {
            using (var context = new BlogManagementEntities())
            {

                try
                {
                    var original = context.Users.Find(entity.UserID);

                    entity.UserName = original.UserName;
                    entity.UserPassword = original.UserPassword;
                    entity.UserFirstName = original.UserFirstName;
                    entity.UserLastName = original.UserLastName;
                    entity.IP = original.IP;
                    entity.CreatedDate = original.CreatedDate;
                    entity.UserRole = original.UserRole;

                    context.Entry(original).CurrentValues.SetValues(entity);
                    return context.SaveChanges();

                }
                catch (Exception)
                {
                    return 0;
                }

            }
        }
        public string GetUsers(int currentUserID)
        {
            using (var context = new BlogManagementEntities())
            {

                try
                {
                    var data = context.Users.Where(x => x.UserID != currentUserID)
                       .Select(x => new
                       {
                           UserID = x.UserID,
                           FirstName = x.UserFirstName,
                           LastName = x.UserLastName,
                           UserName = x.UserName,
                           isActive = x.isActive
                       });
                    var jsonSerialiser = new JavaScriptSerializer() { MaxJsonLength = 500000 };
                    return jsonSerialiser.Serialize(data.ToList());                         

                }
                catch (Exception)
                {
                    return null;
                }

            }

        }




        public User Get(int id)
        {
            throw new NotImplementedException();
        }
        public User SaveRecord(User entity)
        {
            throw new NotImplementedException();
        }
        public User UpdateRecord(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
