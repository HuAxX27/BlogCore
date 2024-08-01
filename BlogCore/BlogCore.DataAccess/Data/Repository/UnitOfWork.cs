using BlogCore.Data;
using BlogCore.DataAccess.Data.Repository.IRepository;

namespace BlogCore.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(_context);
            Article = new ArticleRepository(_context);
            Slider = new SliderRepository(_context);
        }

        public ICategoryRepository Category { get; private set; }

        public IArticleRepository Article { get; private set; }

        public ISliderRepository Slider { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
