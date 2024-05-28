using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepo _categoryRepo;
        //constractor
        public CategoryController(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        // Category/ AllCategory
        public IActionResult AllCategory()
        {
            var caregories = _categoryRepo.GetAll();
            return View(caregories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(category);
                return RedirectToAction(nameof(AllCategory));
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return BadRequest();
            var category = _categoryRepo.GetById(id.Value);
            if(category is null)
                return NotFound();
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _categoryRepo.Update(category);
                    return RedirectToAction(nameof(AllCategory))
                }
                catch(System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message); 
                }
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest("Category ID is required.");

            var category = _categoryRepo.GetById(id.Value);
            if (category == null)
                return NotFound("Category not found.");

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(int id)
        {
            var category = _categoryRepo.GetById(id);
            if (category == null)
                return NotFound("Category not found.");

            try
            {
                _categoryRepo.Delete(category);
                return RedirectToAction(nameof(AllCategory));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error deleting category: {ex.Message}");
                return View(category);
            }
        }
    }
}
