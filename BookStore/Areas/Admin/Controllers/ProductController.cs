using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webhostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webhostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            Product p = new Product();
            return View();
        }

        public IActionResult GetAllProducts()
        {
            return Json(_unitOfWork.Product.GetAll(includeProperties : "Category"));
        }

        public IActionResult Upsert(int? id)
        {
            Product product = new Product();
            var categories = _unitOfWork.Category.GetAll().Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            

            ProductViewModel productViewModel = new ProductViewModel();
            productViewModel.Categories = categories;
            
            //add new book
            if (id == null)
            {
                return View(productViewModel);
            }

            //edit book

            if (id != null)
            { 
                product = _unitOfWork.Product.Get((int)id);
                productViewModel.Product = product;

                return View(productViewModel);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel productViewModel)
        {
            var categories = _unitOfWork.Category.GetAll().Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            productViewModel.Categories = categories;
            if (ModelState.IsValid)
            {    
                string webRootPath = _webhostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if(files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString(); //global unique identifier
                    var uploadPath = Path.Combine(webRootPath,@"images\products");
                    var fileExtentention = Path.GetExtension(files[0].FileName);
                    var finalUploadPath = Path.Combine(uploadPath, fileName+fileExtentention);

                    //Edit - Delete old file
                    if (productViewModel.Product.ImageUrl != null)
                    {
                        var imgPath = Path.Combine(webRootPath,productViewModel.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(imgPath))
                        {
                            System.IO.File.Delete(imgPath);
                        }
                    }

                    //copy file to the path
                    using (var filestream = new FileStream(finalUploadPath, FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    
                    productViewModel.Product.ImageUrl = @"images/products/" + fileName + fileExtentention;
                }
                else //no file 
                {
                    //edit then apply same name
                    if(productViewModel.Product.Id != 0)
                    {
                        var objFrmDb = _unitOfWork.Product.Get(productViewModel.Product.Id);
                        productViewModel.Product.ImageUrl = objFrmDb.ImageUrl;
                    }
                    else
                    {
                        //create then return back
                        return View(productViewModel);
                    }

                    
                }



            
                //edit
                if (productViewModel.Product.Id != 0)
                {
                    _unitOfWork.Product.Update(productViewModel.Product);
                }

            //add new
                if (productViewModel.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productViewModel.Product);
                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                //var categories = _unitOfWork.Category.GetAll().Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                //productViewModel.Categories = categories;
                return View(productViewModel);
            }
            
        }

        public IActionResult Delete(int id)
        {
            Product product = new Product();
            string webRootPath = _webhostEnvironment.WebRootPath;
            product = _unitOfWork.Product.GetFirstOrDefault(s => s.Id == id);

            if (product != null)
            {
                //delete image
                    var imgPath = Path.Combine(webRootPath, product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(imgPath))
                    {
                        System.IO.File.Delete(imgPath);
                    }
                //remove product
                _unitOfWork.Product.Remove(product);
                _unitOfWork.Save();
                var deleteMsg = Json(new { success = true, message = "Delete Successfull" });
                return deleteMsg;
            }
            else
            {
                var deleteMsg = Json(new { success = false, message = "Delete Failed" });
                return deleteMsg;
            }
        }
    }
}
