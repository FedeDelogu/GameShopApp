using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;
using System.Globalization;
using Utility;
using WebAppPlayshphere.DAO;
using WebAppPlayshphere.Models;

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
                {
                    var utenteLoggato = GetUtenteLoggato();

                    ViewBag.UtenteLoggato = utenteLoggato;

                    return View(e);
                }
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
            if(DAOVideogioco.GetIstance().Read() == null)
            {
                Console.WriteLine("ERRORE CATALOGO");
                return View("Home/Index");
            }
            return View(DAOVideogioco.GetIstance().Read());
        }

        /*
         * METODI UTILIZZABILI SOLO DALL ADMIN (BISOGNA BLOCCARE IL CONTENUTO PER GLI UTENTI NON ADMIN)
         */

        public IActionResult FormAggiungi()
        {
            Console.WriteLine("FORM AGGIUNGI INVIATO");
            Entity e = new Videogioco();
            return View(e);
        }
        public IActionResult FormModifica(int id)
        {
            return View(DAOVideogioco.GetIstance().Find(id));
        }
        public IActionResult Elimina(int id) // OPERAZIONE VALIDA SOLO SE SI E' ADMIN
        {
            //////////////////////////////////////////////////////////
            ///
            // AGGIUNGERE CONTROLLO SE L'UTENTE E' ADMIN
            // QUI -> SE L'UTENTE NON E' ADMIN TORNO AL CATALOGO
            //
            //////////////////////////////////////////////////////////


            // SE L ID E' MINORE DI 0 TORNO AL CATALOGO
            if (id < 0)
            {
                return View("Catalogo", DAOVideogioco.GetIstance().Read());
            }
            // SE L ID E' MAGGIORE DI 0 TENTO DI ELIMINARE IL VIDEOGIOCO
            else
            {
                if (DAOVideogioco.GetIstance().Delete(id))
                {
                    Console.WriteLine("Videogioco eliminato");
                    return View("Catalogo", DAOVideogioco.GetIstance().Read());
                }
                else
                {
                    Console.WriteLine("ERRORE : Videogioco non eliminato");
                    return View("Catalogo", DAOVideogioco.GetIstance().Read());
                }
            }
        } // fine metodo elimina
        public IActionResult Modifica([FromForm] Dictionary<string, string> videogioco, [FromForm] List<string> piattaforme)
        {
            Console.WriteLine("VideogiochiController - Modifica");
            Entity v = new Videogioco();
            List<Entity> pi = DAOPiattaforma.GetIstance().Read();

            videogioco.Remove("piattaforme");
            videogioco["prezzo"] = videogioco["prezzo"].Replace(".", ",");
            v.FromDictionary(videogioco);
            foreach (var piattaforma in pi)
            {
                if(piattaforme.Contains(((Piattaforma)piattaforma).Nome))
                {
                    ((Videogioco)v).Piattaforme.Add((Piattaforma)piattaforma);
                }
            }
            if (DAOVideogioco.GetIstance().Update(v))
            {
                Console.WriteLine("Videogioco modificato");
                return View("Catalogo", DAOVideogioco.GetIstance().Read());
            }
            return View("Catalogo", DAOVideogioco.GetIstance().Read());
        } // FINE METODO MODIFICA
        public IActionResult Aggiungi([FromForm] Dictionary<string, string> videogioco, [FromForm] List<string> piattaforme)
        {
            Console.WriteLine("VideogiochiController - Aggiungi");
            Entity v = new Videogioco();
            List<Entity> pi = DAOPiattaforma.GetIstance().Read();

            videogioco.Remove("piattaforme");
            videogioco["prezzo"] = videogioco["prezzo"].Replace(".", ",");
            v.FromDictionary(videogioco);
            foreach (var piattaforma in pi)
            {
                if (piattaforme.Contains(((Piattaforma)piattaforma).Nome))
                {
                    ((Videogioco)v).Piattaforme.Add((Piattaforma)piattaforma);
                }
            }
            if (DAOVideogioco.GetIstance().Create(v))
            {
                Console.WriteLine("Videogioco modificato");
                return View("Catalogo", DAOVideogioco.GetIstance().Read());
            }
            return View("Catalogo", DAOVideogioco.GetIstance().Read());
        }
        public IActionResult AggiungiCarrello(int idVid, int idUtente, int idPiattaforma)
        {
            Console.WriteLine("ci sei fratello");
            DAOCarrello.GetIstance().Insert(idUtente, idVid, 1, idPiattaforma);
            return RedirectToAction("Dettagli", new { id = idVid });
        }

        public IActionResult Categoria(string categoria)
        {
            return View(DAOVideogioco.GetIstance().filtroCategoria(categoria));
        }
        private Utente GetUtenteLoggato()
        {
            var utenteLoggato = HttpContext.Session.GetString("UtenteLoggato");
            if (utenteLoggato != null)
            {
                return JsonConvert.DeserializeObject<Utente>(utenteLoggato);
            }
            return null;
        }

    }
}
