using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BlogCore.DataAccess.Data.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);

        IEnumerable<SelectListItem> GetListCategory();
    }
}
