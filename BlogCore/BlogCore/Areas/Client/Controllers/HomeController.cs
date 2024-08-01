using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogCore.Areas.Client.Controllers
{
    [Area("Client")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Index()
        {

            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Sliders = _unitOfWork.Slider.GetAll(),
                Articles = _unitOfWork.Article.GetAll()
            };

            ViewBag.IsHome = true;

            return View(homeViewModel);
        }

        [HttpGet]
        public IActionResult Details(int id) 
        {
            var articleFromDb = _unitOfWork.Article.Get(id);

            return View(articleFromDb);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
