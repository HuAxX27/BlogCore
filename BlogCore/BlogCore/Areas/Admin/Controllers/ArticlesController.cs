using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticlesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticlesController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ArticleViewModel articleViewModel = new ArticleViewModel()
            {
                Article = new Article(),
                ListCategory = _unitOfWork.Category.GetListCategory()
            };

            return View(articleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticleViewModel articleViewModel)
        {
            if (ModelState.IsValid)
            {
                string rootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (articleViewModel.Article.Id == 0 && files.Count() > 0 )
                {
                    string fileName = Guid.NewGuid().ToString();
                    var loaded = Path.Combine(rootPath, @"images\articles");
                    var extension = Path.GetExtension(files[0].FileName);


                    using (var fileStreams = new FileStream(Path.Combine(loaded, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    articleViewModel.Article.UrlImage = @"\images\articles\" + fileName + extension;
                    articleViewModel.Article.CreatedDate = DateTime.Now.ToString();

                    _unitOfWork.Article.Add(articleViewModel.Article);
                    _unitOfWork.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Image", "Debes seleccionar una imagen");
                }
            }

            articleViewModel.ListCategory = _unitOfWork.Category.GetListCategory();
            return View(articleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticleViewModel articleViewModel)
        {
            if (ModelState.IsValid)
            {
                string rootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                var articleFromDb = _unitOfWork.Article.Get(articleViewModel.Article.Id);


                if (files.Count() > 0)
                {
                    //New image for the article
                    string fileName = Guid.NewGuid().ToString();
                    var loaded = Path.Combine(rootPath, @"images\articles");
                    var extension = Path.GetExtension(files[0].FileName);
                    var newExtension = Path.GetExtension(files[0].FileName);

                    var pathImage = Path.Combine(rootPath, articleFromDb.UrlImage.TrimStart('\\'));

                    if (System.IO.File.Exists(pathImage))
                    {
                        System.IO.File.Delete(pathImage);
                    }

                    //Load file
                    using (var fileStreams = new FileStream(Path.Combine(loaded, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    articleViewModel.Article.UrlImage = @"\images\articles\" + fileName + extension;
                    articleViewModel.Article.CreatedDate = DateTime.Now.ToString();

                    _unitOfWork.Article.Update(articleViewModel.Article);
                    _unitOfWork.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    articleViewModel.Article.UrlImage = articleFromDb.UrlImage;

                }

                _unitOfWork.Article.Update(articleViewModel.Article);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            articleViewModel.ListCategory = _unitOfWork.Category.GetListCategory();
            return View(articleViewModel);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticleViewModel articleViewModel = new ArticleViewModel()
            {
                Article = new Article(),
                ListCategory = _unitOfWork.Category.GetListCategory()
            };

            if(id != null)
            {
                articleViewModel.Article = _unitOfWork.Article.Get(id.GetValueOrDefault());
            }

            return View(articleViewModel);
        }



        #region Calls to API

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Article.GetAll(includeProperties:"Category") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Article.Get(id);
            string rootPath = _webHostEnvironment.WebRootPath;
            var pathImage = Path.Combine(rootPath, objFromDb.UrlImage.TrimStart('\\'));

            if (System.IO.File.Exists(pathImage))
            {
                System.IO.File.Delete(pathImage);
            }

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error borrando articulo" });
            }
            _unitOfWork.Article.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Articulo borrado correctamente" });
        }


        #endregion

    }
}
