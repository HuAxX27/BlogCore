using BlogCore.Models;

namespace BlogCore.DataAccess.Data.Repository.IRepository
{
    public interface ISliderRepository : IRepository<Slider>
    {
        void Update(Slider slider);
    }
}
