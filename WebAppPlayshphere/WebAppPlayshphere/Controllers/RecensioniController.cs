using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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
        public IActionResult RecensioniGioco(int idvideogioco)
        {
            Console.WriteLine($"ID VIDEOGIOCO : {idvideogioco}");
            return View(DAORecensione.GetIstance().RecensioniGioco(idvideogioco));
        }

        /* RISERVATO ALL'ADMIN */

        [HttpGet]
        public IActionResult ListaRecensioni()
        {
            var entities = DAORecensione.GetIstance().Read(); // Lista di Entity
            List<Recensione> recensioni = new List<Recensione>();
            

            foreach (var entity in entities)
            {
                recensioni.Add((Recensione)entity);    
            };

            return Json(recensioni);
        }

        [HttpPost]
        public IActionResult ApprovaRecensione([FromBody] dynamic requestApprove)
        {

            if (requestApprove.TryGetProperty("id", out JsonElement idElement))
            {
                int idReview = Convert.ToInt32(idElement.ToString());

                // Validazione dei dati
                if (idReview <= 0) // Verifica se l'ID e il ruolo sono validi
                {
                    return BadRequest(new { success = false, message = "Valore id non valido" });
                }

                bool approved = DAORecensione.GetIstance().Approve(idReview); // SE id > 0 allora fa il ban

                return Json(new { success = approved, message = approved ? "Recensione Approvata." : "Errore durante l'aggiornamento." });
            }

            return BadRequest(new { success = false, message = "ID Mancante" });

        }

        [HttpPost]
        public IActionResult DisapprovaRecensione([FromBody] dynamic requestDisapprove)
        {

            if (requestDisapprove.TryGetProperty("id", out JsonElement idElement))
            {
                int idReview = Convert.ToInt32(idElement.ToString());

                // Validazione dei dati
                if (idReview <= 0) // Verifica se l'ID e il ruolo sono validi
                {
                    return BadRequest(new { success = false, message = "Valore id non valido" });
                }

                bool disapproved = DAORecensione.GetIstance().Disapprove(idReview); // SE id > 0 allora fa il ban

                return Json(new { success = disapproved, message = disapproved ? "Recensione Disapprovata ." : "Errore durante l'aggiornamento." });
            }

            return BadRequest(new { success = false, message = "ID Mancante" });

        }

        [HttpPost]
        public IActionResult EliminaRecensione([FromBody] dynamic requestDelete)
        {

            if (requestDelete.TryGetProperty("id", out JsonElement idElement))
            {
                int idDelete = Convert.ToInt32(idElement.ToString());

                // Validazione dei dati
                if (idDelete <= 0) // Verifica se l'ID e il ruolo sono validi
                {
                    return BadRequest(new { success = false, message = "Valore id non valido" });
                }

                bool delete = DAORecensione.GetIstance().Delete(idDelete); // SE id > 0 allora fa il ban

                return Json(new { success = delete, message = delete ? "Recensione Eliminata." : "Errore durante l'aggiornamento." });
            }

            return BadRequest(new { success = false, message = "ID Mancante" });

        }

    }
}