using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        //GET
        public IActionResult Upsert(int? Id)
        {
            Company company = new();

            if (Id == null || Id == 0)
            {
                return View(company);
            }
            else
            {
                company = _unitOfWork.Company.GetFirstOrDefault(i => i.Id == Id);
                return View(company);
            }
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
               
                if(obj.Id == 0) 
                {
                    _unitOfWork.Company.Add(obj);
                    TempData["success"] = "Successfuly Created Company";
                } else
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["success"] = "Successfuly Updated Company";
                }
                
                _unitOfWork.Save();
                
                return RedirectToAction("Index");
            }
            return View(obj);
        }
       
     


        #region API_CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? Id)
        {
            var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == Id);
            if (obj == null)
            {
                return Json(new {success = false, message="Error while deleting"});
            }
            
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion

    }
}