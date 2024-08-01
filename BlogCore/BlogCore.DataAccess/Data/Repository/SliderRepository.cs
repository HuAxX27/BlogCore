
using BlogCore.Data;
using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;

namespace BlogCore.DataAccess.Data.Repository
{
    public class SliderRepository : Repository<Slider>, ISliderRepository
    {

        private readonly ApplicationDbContext _context;

        public SliderRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

        public void Update(Slider slider)
        {
            var objFromDb = _context.Sliders.FirstOrDefault(s => s.Id == slider.Id);
            objFromDb.Name = slider.Name;
            objFromDb.State = slider.State;
            objFromDb.UrlImage = slider.UrlImage;
            //_context.SaveChanges();
        }
    }
}
