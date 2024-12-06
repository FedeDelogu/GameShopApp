using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text.Json;
using Utility;
using WebAppPlayshphere.DAO;
using WebAppPlayshphere.Factory;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.Controllers
{
    public class VideogiochiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AggiungiRecensione([FromForm] Dictionary<string, string> recensione)
        {
            foreach (var item in recensione)
            {
                Console.WriteLine(item.Key + " " + item.Value);
            }
            Entity e = new Recensione();
            if (recensione != null)
            {
                if (recensione["valido"] == "1")
                {
                    recensione["valido"] = "true";
                }

                e = RecensioneFactory.CreateRecensione(recensione);
                if (DAORecensione.GetIstance().Create(e))
                {
                    Console.WriteLine("Recensione creata");
                    int id = Convert.ToInt32(recensione["idvideogioco"]);
                    Console.WriteLine("id da passare " + id);
                    return RedirectToAction("Dettagli", new { id = id });
                }
            }
            return Content("ROTTO TUTTO");
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
        public IActionResult Filtra(string ricerca)
        {
            List<Videogioco> vg = new List<Videogioco>();
            if (ricerca == "")
            {
                List<Entity> ris = DAOVideogioco.GetIstance().Read();
                foreach(Entity e in ris)
                {
                    vg.Add((Videogioco)e);
                }
            }
            else
            {
                vg = DAOVideogioco.GetIstance().Ricerca(ricerca);
            }
            return Json(vg);
        }

        public IActionResult FiltraPiattaforma(int idPiattaforma)
        {
            List<Videogioco> vg = new List<Videogioco>();
            if (idPiattaforma == -1)
            {
                List<Entity> ris = DAOVideogioco.GetIstance().Read();
                foreach (Entity e in ris) {
                    vg.Add((Videogioco)e);
                }
                return Json(vg);
            }
             vg = DAOVideogioco.GetIstance().GetByPiattaforma(idPiattaforma);

            return Json(vg);
        }

        public IActionResult FiltraGeneri(string generi)
        {
            List<Videogioco> vg = new List<Videogioco>();
            if (generi == "")
            {
                List<Entity> ris = DAOVideogioco.GetIstance().Read();
                foreach (Entity e in ris)
                {
                    vg.Add((Videogioco)e);
                }
            }
            else
            {
                vg = DAOVideogioco.GetIstance().RicercaGeneri(generi);
                Console.WriteLine("Lunghezza vg: " + vg.Count);
            }
            return Json(vg);
        }


        public IActionResult OrdinaPer(string operazione)
        {
            List<Videogioco> ris = new();
            List<Entity> giochi = new List<Entity>();
            string query = "";
            switch (operazione)
            {
                case "1":
                    query = "Prezzo DESC";
                    break;
                case "2":
                    query = "Prezzo ASC";
                    break;
                case "3":
                    query = "Rilascio DESC";
                    break;
                case "4":
                    query = "Rilascio ASC";
                    break;
                default:
                    Console.WriteLine("sei nel default");
                    giochi = DAOVideogioco.GetIstance().Read();
                    break;
               
            }
            if (query == "") {
                if (operazione != "")
                {
                    if (operazione == "5")
                    {
                        Console.WriteLine("sei nel decrescente");
                        giochi=giochi.OrderByDescending(e => ((Videogioco)e).Valutazione()).ToList();
                    }
                    else
                    {
                        Console.WriteLine("sei nel crescente");
                        giochi=giochi.OrderBy(e => ((Videogioco)e).Valutazione()).ToList();
                    }
                    foreach(var gioco in giochi)
                    {
                        Console.WriteLine("VALUTAZIONE: "+((Videogioco)gioco).Valutazione());
                        ris.Add((Videogioco)gioco);
                    }
                }
            }
            else
            {
                ris=DAOVideogioco.GetIstance().Order(query);
            }
            
            return Json(ris);
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

        /*RISERVATI ALL'ADMIN*/

        public IActionResult CatalogoJson()
        {
            if (DAOVideogioco.GetIstance().Read() == null)
            {
                Console.WriteLine("ERRORE CATALOGO");
                return View("Home/Index");
            }
            List<Entity> listaGiochi = DAOVideogioco.GetIstance().Read();
            List<Videogioco> videogiochi = new List<Videogioco>();
            foreach (var v in listaGiochi)
            {
                videogiochi.Add((Videogioco)v);
            }
            return Json(videogiochi);
        }

        [HttpPost]
        public IActionResult EliminaProdotto([FromBody] dynamic requestDelete)
        {

            if (requestDelete.TryGetProperty("id", out JsonElement idElement))
            {
                int idDelete = Convert.ToInt32(idElement.ToString());

                // Validazione dei dati
                if (idDelete <= 0) // Verifica se l'ID e il ruolo sono validi
                {
                    return BadRequest(new { success = false, message = "Valore id non valido" });
                }

                bool delete = DAOVideogioco.GetIstance().Delete(idDelete); // SE id > 0 allora fa il ban

                return Json(new { success = delete, message = delete ? "Recensione Eliminata." : "Errore durante l'aggiornamento." });
            }

            return BadRequest(new { success = false, message = "ID Mancante" });

        }

    }
}
