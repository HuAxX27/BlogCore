namespace BlogCore.DataAccess.Data.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        //Aqui se deben agregar los repositorios
        ICategoryRepository Category { get; }

        IArticleRepository Article { get; }

        ISliderRepository Slider { get; }

        void Save();
    }
}
