using Microsoft.AspNetCore.Mvc;
using Utility;
using WebAppPlayshphere.DAO;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.Controllers
{
    public class HomeController :Controller
    {
        public IActionResult Index()
        {
            
           return View();   
                        }

        public IActionResult Regolamento()
        {

            return View();
        }
        public IActionResult Privacy()
        {

            return View();
        }
        public IActionResult Termini()
        {

            return View();
        }
        public IActionResult ChiSiamo()
        {

            return View();
        }
    }
}
