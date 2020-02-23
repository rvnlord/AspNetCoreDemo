using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AspNetCoreDemo.Models;
using AspNetCoreDemo.Security;
using AspNetCoreDemo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCoreDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;
        private readonly IDataProtector _protector;

        public HomeController(IEmployeeRepository employeeRepository, IWebHostEnvironment env, ILogger<HomeController> logger, 
            IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _employeeRepository = employeeRepository;
            _env = env;
            _logger = logger;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.EmployeeIdRouteValue);
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployees().Select(e =>
            {
                e.EncryptedId = _protector.Protect(e.Id.ToString());
                return e;
            });
            return View(model);
        }

        public ViewResult Details(string id)
        {
            //throw new Exception("Error showing Details");

            _logger.LogTrace("Trace Log");
            _logger.LogDebug("Debug Log");
            _logger.LogInformation("Information Log");
            _logger.LogWarning("Warning Log");
            _logger.LogError("Error Log");
            _logger.LogCritical("Critical Log");

            if (!int.TryParse(id, out var decryptedId) && !int.TryParse(_protector.Unprotect(id), out decryptedId))
                decryptedId = -1;

            var canDecrypt = decryptedId != -1;
            var employee = _employeeRepository.GetEmployee(canDecrypt ? decryptedId : 1);

            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", canDecrypt ? decryptedId : 1);
            }

            var model = new HomeDetailsViewModel { Employee = employee, PageTitle = "Details" };
            return View(model);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View("Edit", new EmployeeEditViewModel());
        }

        [HttpPost]
        public IActionResult Create(EmployeeEditViewModel model)
        {
            if (!ModelState.IsValid) 
                return View();

            var newEmployee = new Employee
            {
                Name = model.Name,
                Email = model.Email,
                Department = model.Department,
                PhotoPath = ProcessUploadedFiles(model)
            };

            _employeeRepository.Add(newEmployee);
            return RedirectToAction("Details", new { newEmployee.Id });
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            var employee = _employeeRepository.GetEmployee(id);
            var employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (!ModelState.IsValid) 
                return View(model);

            var employee = _employeeRepository.GetEmployee(model.Id);
            employee.Name = model.Name;
            employee.Email = model.Email;
            employee.Department = model.Department;
            employee.PhotoPath = ProcessUploadedFiles(model);

            _employeeRepository.Update(employee);
            return RedirectToAction("index");
        }

        
        private string ProcessUploadedFiles(EmployeeEditViewModel model)
        {
            if (model.Photo == null) 
                return model.ExistingPhotoPath;

            if (model.ExistingPhotoPath != null)
            {
                var oldFilePath = Path.Combine(_env.WebRootPath, "images", model.ExistingPhotoPath);
                System.IO.File.Delete(oldFilePath);
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
            var uniqueFileName = Guid.NewGuid() + "_" + model.Photo.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            model.Photo.CopyTo(fileStream);

            return uniqueFileName;
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var employee = _employeeRepository.Delete(id);
            if (employee == null)
                return RedirectToPage("/NotFound");

            return RedirectToAction("Index");
        }

    }
}
