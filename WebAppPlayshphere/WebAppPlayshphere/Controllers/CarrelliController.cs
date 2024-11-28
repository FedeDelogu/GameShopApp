using Microsoft.AspNetCore.Mvc;
using WebAppPlayshphere.DAO;
using WebAppPlayshphere.Models;
namespace WebAppPlayshphere.Controllers
{
    public class CarrelliController : Controller
    {
        public IActionResult Dettagli(int id)
        {
            Console.WriteLine("ID PASSATO DIO LADRO!!!: "+id);
            return View(DAOCarrello.GetIstance().Find(id));
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Ordina(int id)
        {
            DAOCarrello.GetIstance().Ordina(DAOCarrello.GetIstance().Find(id));
            return RedirectToAction("Completed");
        }
        public IActionResult Completed()
        {
            return View();
        }//viva
        public IActionResult UpdateUnits(int qt, int id, int idCarrello,int idPiattaforma)
        {
            Carrello c = (Carrello)DAOCarrello.GetIstance().Find(idCarrello);
            Videogioco videogioco = (Videogioco)DAOVideogioco.GetIstance().Find(id);

            // Cerca un videogioco con lo stesso ID
            var chiave = c.Videogiochi.Keys.FirstOrDefault(v => v.Id == videogioco.Id);

            if (chiave == null)
            {
                Console.WriteLine($"Il videogioco con ID {id} non è presente nel carrello.");
                return RedirectToAction("Dettagli", new { id = idCarrello });
            }

            // Ora puoi accedere alla quantità
            int quantitaCorrente = c.Videogiochi[chiave];

            if (quantitaCorrente > qt)
            {
                DAOCarrello.GetIstance().Remove(id, idCarrello,quantitaCorrente - qt);
            }
            else
            {
                Console.WriteLine("QUANTITA DA AGGIUNGERE: "+ (qt - quantitaCorrente));
                DAOCarrello.GetIstance().Insert(idCarrello, id, qt-quantitaCorrente, idPiattaforma);
            }
           

            // Reindirizza all'azione "Dettagli" passando il parametro id
            return RedirectToAction("Dettagli", new { id = idCarrello });
        }

       

    }
}
