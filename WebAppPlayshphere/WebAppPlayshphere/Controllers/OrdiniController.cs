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
        public IActionResult GetByUtente(int id)
        {
            return View("Elenco",DAOOrdine.GetInstance().FindByUtente(id));
        }
        public IActionResult GetByData(string data)
        {
            return View("Elenco", DAOOrdine.GetInstance().FindByData(data));
        }
        public IActionResult UpdateStato(int id,string stato)
        {
            Console.WriteLine($"{stato}, {id}");
            if(DAOOrdine.GetInstance().Update(stato, id))
            {
                Console.WriteLine("MODIFICA AVVENUTA CON SUCCESSO !");
            }
            return RedirectToAction("Elenco");
        }
    }
}
