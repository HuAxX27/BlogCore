using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SlidersController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            if (ModelState.IsValid)
            {
                string rootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var loaded = Path.Combine(rootPath, @"images\sliders");
                    var extension = Path.GetExtension(files[0].FileName);


                    using (var fileStreams = new FileStream(Path.Combine(loaded, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    slider.UrlImage = @"\images\sliders\" + fileName + extension;

                    _unitOfWork.Slider.Add(slider);
                    _unitOfWork.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Image", "Debes seleccionar una imagen");
                }
            }
            return View(slider);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Slider slider = new Slider();
            slider = _unitOfWork.Slider.Get(id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {
            if (ModelState.IsValid)
            {
                string rootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                var sliderFromDb = _unitOfWork.Slider.Get(slider.Id);


                if (files.Count() > 0)
                {
                    //New image for the article
                    string fileName = Guid.NewGuid().ToString();
                    var loaded = Path.Combine(rootPath, @"images\sliders");
                    var extension = Path.GetExtension(files[0].FileName);
                    var newExtension = Path.GetExtension(files[0].FileName);

                    var pathImage = Path.Combine(rootPath, sliderFromDb.UrlImage.TrimStart('\\'));

                    if (System.IO.File.Exists(pathImage))
                    {
                        System.IO.File.Delete(pathImage);
                    }

                    //Load file
                    using (var fileStreams = new FileStream(Path.Combine(loaded, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    slider.UrlImage = @"\images\sliders\" + fileName + extension;

                    _unitOfWork.Slider.Update(slider);
                    _unitOfWork.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    slider.UrlImage = sliderFromDb.UrlImage;

                }

                _unitOfWork.Slider.Update(slider);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(slider);
        }


        #region Calls to API

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Slider.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Slider.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error borrando slider" });
            }
            _unitOfWork.Slider.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Slider borrado correctamente" });
        }

        #endregion

    }
}
