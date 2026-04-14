using InvoiceManagementSystem.Models;

using Microsoft.AspNetCore.Mvc;
namespace InvoiceManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login() 
        { 
        return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model) 
        {
            if (model.Username == "admin" && model.Password == "admin123") 
            {
             return RedirectToAction("Index", "Invoice");
            }
            ViewBag.Error = "Invalid Credentials";
            return View();
        }
    }
}
