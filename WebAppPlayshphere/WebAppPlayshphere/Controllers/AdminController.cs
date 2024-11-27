using Microsoft.AspNetCore.Mvc;
using WebAppPlayshphere.DAO;

namespace WebAppPlayshphere.Controllers
{
    public class AdminController : Controller
    {
        // PAGINA INIZIALE ADMIN
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
