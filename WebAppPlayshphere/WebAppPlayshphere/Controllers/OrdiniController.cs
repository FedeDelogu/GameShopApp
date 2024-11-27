using Microsoft.AspNetCore.Mvc;
using WebAppPlayshphere.DAO;

namespace WebAppPlayshphere.Controllers
{
    public class OrdiniController : Controller
    {
        public IActionResult Elenco()
        {
            return View(DAOOrdine.GetInstance().Read());
        }
        public IActionResult UpdateStato(string stato)
        {
            return View(DAOOrdine.GetInstance().Read());
        }
    }
}
