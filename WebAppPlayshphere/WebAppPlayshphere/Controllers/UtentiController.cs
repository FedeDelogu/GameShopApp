using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Utility;
using WebAppPlayshphere.DAO;
using WebAppPlayshphere.Models;
using Newtonsoft.Json;
using System.Text.Json;

namespace WebAppPlayshphere.Controllers
{
    public class UtentiController : Controller
    {
        ILogger<UtentiController> _logger;

        // Il costruttore in questo caso mi serve per creare il foglio bianco.
        // Si valorizza tramite Injection
        public UtentiController(ILogger<UtentiController> l)
        {
            _logger = l;
        }

        // Mi serve per tenere in memoria chi ha fatto login
        public static Utente _utenteLoggato = null;
        static int tentativiAccesso = -1;

        public IActionResult Index()
        {
            // Ogni volta che carico questa pagina sale di 1
            // Al primo caricamento vale 0
            tentativiAccesso++;
            _logger.LogInformation($"Tentativo {tentativiAccesso} alle {DateTime.Now}");
            return View(tentativiAccesso);
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Profilo()
        {
            if( _utenteLoggato.Anagrafica == null )
            {
                Entity AnagraficaVuota = new Anagrafica
                {
                    Nome = "",
                    Cognome="",
                    Indirizzo="",
                    Telefono="",
                    Citta="",
                    Cap = "",


                };
                _utenteLoggato.Anagrafica = (Anagrafica)AnagraficaVuota;
            }
            return View(_utenteLoggato);
        }
        public IActionResult Recensioni()
        {
            if (_utenteLoggato.Anagrafica == null)
            {
                Entity AnagraficaVuota = new Anagrafica
                {
                    Nome = "",
                    Cognome = "",
                    Indirizzo = "",
                    Telefono = "",
                    Citta = "",
                    Cap = "",
                };
                _utenteLoggato.Anagrafica = (Anagrafica)AnagraficaVuota;
            }

            // Recupera le recensioni dell'utente loggato
            List<Recensione> recensioni = DAORecensione.GetIstance().RecensioniUtente(_utenteLoggato.Id); // Recupera le recensioni dall'ID dell'utente loggato

            // Crea un dizionario che lega l'utente alla sua lista di recensioni
            var model = new Dictionary<Utente, List<Recensione>>()
            {
                { _utenteLoggato, recensioni }
            };

            // Passa il dizionario alla vista
            return View(model);
        }



        public IActionResult Accedi(Dictionary<string, string> credenziali)
        {
            // Le chiavi del dictionary sono i name di HTML
            string user = credenziali["username"];
            string password = credenziali["password"];
            Console.WriteLine($"{user} {password}");

            if (DAOUtente.GetInstance().Find(user, password))
            {
                
                Entity e = DAOUtente.GetInstance().Find(user);
                // Memorizzo chi ha fatto login
                _utenteLoggato = (Utente)e;
                Utente utenteFront = new Utente
                {
                    Id = _utenteLoggato.Id,
                    Ruolo = ((Utente)_utenteLoggato).Ruolo,
                    Email = ((Utente)_utenteLoggato).Email
                    // Anagrafica = (Anagrafica)DAOAnagrafica.GetIstance().Find(entity.Id)
                };
                HttpContext.Session.SetString("UtenteLoggato", JsonConvert.SerializeObject(utenteFront));
                _logger.LogInformation($"Utente Loggato: {_utenteLoggato.Username} alle {DateTime.Now}");
                if (((Utente)e).Ruolo == 0)
                {
                    return RedirectToAction("Dashboard","Admin");
                }
                return RedirectToAction("Index","Home");
            }
            else
                return RedirectToAction("Login");
        }
        public IActionResult ModificaAnagrafica(Dictionary<string, string> dati)
        {
           
            var utenteLoggato = JsonConvert.DeserializeObject<Utente>(HttpContext.Session.GetString("UtenteLoggato"));

                foreach(var l in dati)
                    Console.WriteLine(l.Key +" "+ l.Value);

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

            // Verifica se è un'operazione di aggiornamento o di creazione
            if (utenteLoggato.Anagrafica.Nome!="")
            {
               
                var succ = DAOAnagrafica.GetInstance().Update(utenteLoggato);
                if (succ)
                {
                    return View("Profilo", utenteLoggato);
                }
                else
                {
                    return RedirectToAction("Profilo", utenteLoggato);
                }
            }
            else 
            
            {
                // Se l'anagrafica è nuova, usa il metodo Create
                var succ = DAOAnagrafica.GetInstance().Create(utenteLoggato);
                if (succ)
                {
                    return View("Profilo", utenteLoggato);
                }
                else
                {
                    return RedirectToAction("Profilo", utenteLoggato);
                }
            }
        }

        public IActionResult Registrazione()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Azione(string par1, string par2)
        {
            return RedirectToAction("");
        }
        public IActionResult Salva(Dictionary<string, string> credenziali)
        {
            foreach(var l in credenziali)
            {
                Console.WriteLine(l.Key+" "+l.Value);
            }

            Entity e = new Utente
            {
               Email= credenziali["Email"],
               Password = credenziali["Password"],
            };
            
            ((Utente)e).Ruolo = 1;

            if (DAOUtente.GetInstance().Create(e))
            {
                // Dove lo mando se la registrazione avviene?
                return RedirectToAction("Login");
            }
            else
            {
                // Dove lo mando se non riesco a salvare la registrazione?
                return RedirectToAction("Registrazione");
            }
        }

        public IActionResult Logout()
        {
            _logger.LogInformation($"Logout {_utenteLoggato.Username} alle {DateTime.Now}");
            // 'Sloggo' l'utente resettando la variabile statica
            _utenteLoggato = null;
            // Ripulisco i tentativi di accesso
            tentativiAccesso = -1;
            HttpContext.Session.Clear();
            // Reindirizzo al Login per il prossimo accesso
            return RedirectToAction("Index","Home");
        }

        /* RISERVATO ALL'ADMIN */

        [HttpGet]
        public IActionResult ListaUtenti()
        {
            var entities = DAOUtente.GetInstance().Read(); // Lista di Entity
            List<Utente> utenti = new List<Utente>();

            foreach (var entity in entities)
            {
                // Mappa ogni entity a un oggetto Utente
                Utente utente = new Utente
                {
                    Id = entity.Id,
                    Email = ((Utente)entity).Email,
                    Password = ((Utente)entity).Password,
                    Ruolo = ((Utente)entity).Ruolo,
                    Dob = ((Utente)entity).Dob,
                    Anagrafica = (Anagrafica)DAOAnagrafica.GetInstance().Find(entity.Id)
                };

                // Aggiungi l'oggetto Utente alla lista
                utenti.Add(utente);
            }

            return Json(utenti);
        }


        [HttpPost]
        public IActionResult BanUtente([FromBody] dynamic requestBan)
        {

            if (requestBan.TryGetProperty("id", out JsonElement idElement))
            {
                int iduser = Convert.ToInt32(idElement.ToString());

                // Validazione dei dati
                if (iduser <= 0) // Verifica se l'ID e il ruolo sono validi
                {
                    return BadRequest(new { success = false, message = "Valore id non valido" });
                }
                                
                bool ban = DAOUtente.GetInstance().Ban(iduser); // SE id > 0 allora fa il ban

                return Json(new { success = ban, message = ban ? "Ruolo aggiornato." : "Errore durante l'aggiornamento." });
            }

            return BadRequest(new { success = false, message = "ID Mancante" });
        }

        [HttpPost]
        public IActionResult SbloccaUtente([FromBody] dynamic requestSban)
        {

            if (requestSban.TryGetProperty("id", out JsonElement idElement))
            {
                int iduser = Convert.ToInt32(idElement.ToString());

                // Validazione dei dati
                if (iduser <= 0) // Verifica se l'ID e il ruolo sono validi
                {
                    return BadRequest(new { success = false, message = "Valore id non valido" });
                }

                bool sban = DAOUtente.GetInstance().Sban(iduser); // SE id > 0 allora fa il ban

                return Json(new { success = sban, message = sban ? "Ruolo aggiornato." : "Errore durante l'aggiornamento." });
            }

            return BadRequest(new { success = false, message = "ID Mancante" });
        }

    }
}
