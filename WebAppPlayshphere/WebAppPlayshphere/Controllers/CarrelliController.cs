using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using WebAppPlayshphere.DAO;
using WebAppPlayshphere.Models;
namespace WebAppPlayshphere.Controllers
{
    public class CarrelliController : Controller
    {
        public IActionResult Dettagli(int id)
        {
            return View(DAOCarrello.GetIstance().Find(id));
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Ordina(int id)
        {
            Ordine ord = DAOCarrello.GetIstance().Ordina(DAOCarrello.GetIstance().Find(id));
            return RedirectToAction("Completed",  ord);
        }
        public IActionResult Completed(Ordine o)
        {
            return View(o);
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
        public IActionResult Rimuovi(int id, int idVid, int  idPiattaforma)
        {
            int idCarrello = id;
            DAOCarrello.GetIstance().Remove(idVid, id, idPiattaforma);
            return RedirectToAction("Dettagli", new { id=idCarrello });
        }

        public IActionResult Checkout(int id) 
        {
            return View(DAOCarrello.GetIstance().Find(id));
        }

        public IActionResult ModificaAnagrafica(Dictionary<string, string> dati)
        {

            var utenteLoggato = JsonConvert.DeserializeObject<Utente>(HttpContext.Session.GetString("UtenteLoggato"));

            foreach (var l in dati)
                Console.WriteLine(l.Key + " " + l.Value);

            // Aggiorna l'anagrafica
            utenteLoggato.Anagrafica = new Anagrafica
            {
                Nome = dati["Nome"],
                Cognome = dati["Cognome"],
                Indirizzo = dati["Indirizzo"],
                Telefono = dati["Telefono"],
                Citta = dati["Citta"],
                Stato = dati["Stato"],
                Cap = dati["Cap"]
            };

            UtentiController._utenteLoggato.Anagrafica = new Anagrafica
             {
                 Nome = dati["Nome"],
                 Cognome = dati["Cognome"],
                 Indirizzo = dati["Indirizzo"],
                 Telefono = dati["Telefono"],
                 Citta = dati["Citta"],
                 Stato = dati["Stato"],
                 Cap = dati["Cap"]
             };

            // Verifica se è un'operazione di aggiornamento o di creazione
            if (utenteLoggato.Anagrafica.Nome != "")
            {

                var succ = DAOAnagrafica.GetInstance().Update(utenteLoggato);
                if (succ)
                {
                    HttpContext.Session.SetString("UtenteLoggato", JsonConvert.SerializeObject(utenteLoggato));
                    Console.WriteLine(utenteLoggato.Anagrafica.ToString());
                    return RedirectToAction("Checkout", new { id = utenteLoggato.Id, });
                }
                else
                {
                    return Content("HAI SBAGLIATO QUALCOSA");
                }
            }
            else

            {
                // Se l'anagrafica è nuova, usa il metodo Create
                var succ = DAOAnagrafica.GetInstance().Create(utenteLoggato);
                if (succ)
                {
                    HttpContext.Session.SetString("UtenteLoggato", JsonConvert.SerializeObject(utenteLoggato));
                    return RedirectToAction("Checkout");
                }
                else
                {
                    return Content("HAI SBAGLIATO QUALCOSA");
                }
            }
        }

    }
}
