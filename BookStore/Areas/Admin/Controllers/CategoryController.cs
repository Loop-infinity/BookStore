using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        //api call
        //not possible in razor pages


        //can also use ActionResult instead but u ll be restricted to using the predefined ones and not customised ones
        //IActionResult is an interface, we can create a custom response as a return, when you use ActionResult you can return only predefined ones for returning a View or a resource.With IActionResult we can return a response, or error as well.On the other hand, ActionResult is an abstract class, and you would need to make a custom class that inherits.
        public IActionResult getAllCategories()
        {
            var categories = _unitOfWork.Category.GetAll();
            return Json(categories);
        }

        public IActionResult Upsert(int? id)
        {
            try
            {
                Category category = new Category();
                //add new book
                if (id == null)
                {
                    return View();
                }

                //edit book
                if (id != null)
                {
                    category = _unitOfWork.Category.Get((int)id);
                    return View(category);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(int? id, string name)
        {
            Category category = new Category();

            //edit
            if (ModelState.IsValid == false)
                return View();

            if (id != null)
            {
                category.Id = (int)id;
                category.Name = name;

                _unitOfWork.Category.Update(category);
            }

            //add new
            if (id ==null)
            {
                category.Name = name;
                _unitOfWork.Category.Add(category);
                
            }

            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id) 
        {
            Category category = new Category();
            category = _unitOfWork.Category.GetFirstOrDefault(s => s.Id == id);

            if(category != null)
            {
                _unitOfWork.Category.Remove(category);
                _unitOfWork.Save();
                var deleteMsg = Json(new { success = true, message = "Delete Successfull"} );
                return deleteMsg;
            }
            else
            {
                var deleteMsg = Json(new { success = false, message = "Delete Failed"} );
                return deleteMsg;
            }
        }
    }
}
