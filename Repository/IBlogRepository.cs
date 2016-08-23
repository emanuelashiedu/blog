using Entity;
using System.Data;
namespace Repository
{
    public interface IBlogRepository : IRepository<Blog, int>
    {
        DataTable SearchTopicFromFile(string searchText);
    }
}
