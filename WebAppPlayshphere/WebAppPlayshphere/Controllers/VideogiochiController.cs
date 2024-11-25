using Microsoft.AspNetCore.Mvc;
using Utility;
using WebAppPlayshphere.DAO;

namespace WebAppPlayshphere.Controllers
{
    public class VideogiochiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dettagli(int id)
        {
            // Questo if serve per vedere se il parametro id è stato caricato
            //  nel caso in cui id non si carichi avrà il valore 0
            if (id > 0)
            {
                Entity e = DAOVideogioco.GetIstance().Find(id);
                if (e != null)
                    return View(e);
                else
                    return Content("Non c'è niente");
            }
            else
            {
                Console.WriteLine($"Parametro ID: {id}");
                return Content("Non c'è niente");
            }
        }

        public IActionResult Catalogo()
        {
            return View(DAOVideogioco.GetIstance().Read());
        }
    }
}
