using BlogCore.Data;
using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;

namespace BlogCore.DataAccess.Data.Repository
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {

        private readonly ApplicationDbContext _context;

        public ArticleRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

        public void Update(Article article)
        {
            var objFromDb = _context.Articles.FirstOrDefault(s => s.Id == article.Id);
            objFromDb.Name = article.Name;
            objFromDb.Description = article.Description;
            objFromDb.UrlImage = article.UrlImage;
            objFromDb.CategoryId = article.CategoryId;

            //_context.SaveChanges();
        }
    }
}
