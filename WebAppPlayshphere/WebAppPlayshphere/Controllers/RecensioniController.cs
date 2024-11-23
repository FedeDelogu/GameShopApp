using Microsoft.AspNetCore.Mvc;
using WebAppPlayshphere.DAO;

namespace WebAppPlayshphere.Controllers
{
    public class RecensioniController : Controller
    {
        public IActionResult Elenco()
        {
            Console.WriteLine("RecensioniController - Elenco");
            return View(DAORecensione.GetIstance().Read());
        }
    }
}
