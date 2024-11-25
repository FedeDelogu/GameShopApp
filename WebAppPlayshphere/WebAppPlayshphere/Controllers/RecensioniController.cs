using Microsoft.AspNetCore.Mvc;
using Utility;
using WebAppPlayshphere.DAO;
using WebAppPlayshphere.Factory;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.Controllers
{
    public class RecensioniController : Controller
    {
        public IActionResult Elenco()
        {
            Console.WriteLine("RecensioniController - Elenco");
            return View(DAORecensione.GetIstance().Read());
        }
        public IActionResult FormModifica(int id)
        {
            Console.WriteLine("RecensioniController - Form Modifica");
            if (DAORecensione.GetIstance().Find(id) == null)
            {
                Console.WriteLine($"{id} : Recensione non trovata");
            }
            return View(DAORecensione.GetIstance().Find(id));
        }
        public IActionResult Modifica([FromForm] Dictionary<string, string> recensione)
        {
            Entity e = new Recensione();
            if (recensione != null)
            {
                e = RecensioneFactory.CreateRecensione(recensione);
                if (DAORecensione.GetIstance().Update(e))
                {
                    Console.WriteLine("Recensione modificata");
                    return View("Elenco", DAORecensione.GetIstance().Read());
                }
            }
            return View("Elenco");
        }
        public IActionResult Dettagli(int id)
        {
            Console.WriteLine("RecensioniController - Dettagli");
            return View(DAORecensione.GetIstance().Find(id));
        }
    }
}