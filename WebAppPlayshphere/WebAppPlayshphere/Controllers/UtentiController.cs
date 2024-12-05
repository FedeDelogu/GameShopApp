using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Utility;
using WebAppPlayshphere.DAO;
using WebAppPlayshphere.Models;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;

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
       // public static Utente _utenteLoggato = null;
        static int tentativiAccesso = -1;

        public IActionResult Index()
        {
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
            var utenteLoggato = GetUtenteLoggato();
            Console.WriteLine(utenteLoggato.ToString());
            if (utenteLoggato == null)
            {
                return RedirectToAction("Login");
            }

            // Recupera l'anagrafica dell'utente loggato
            utenteLoggato.Anagrafica = (Anagrafica)DAOAnagrafica.GetInstance().Find(utenteLoggato.Id);

            if (utenteLoggato.Anagrafica == null)
            {
                utenteLoggato.Anagrafica = new Anagrafica
                {
                    Nome = "",
                    Cognome = "",
                    Indirizzo = "",
                    Telefono = "",
                    Citta = "",
                    Cap = "",
                };
            }
            return View(utenteLoggato);
        }

        public IActionResult Recensioni()
        {
            var utenteloggato = GetUtenteLoggato();
            if (utenteloggato== null)
            {
                // Reindirizza al login se l'utente non è autenticato
                return RedirectToAction("Login");
            }
       
            if (utenteloggato.Anagrafica == null)
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
                utenteloggato.Anagrafica = (Anagrafica)AnagraficaVuota;
            }
            
            // Recupera le recensioni dell'utente loggato
            List<Recensione> recensioni = DAORecensione.GetIstance().RecensioniUtente(utenteloggato.Id); // Recupera le recensioni dall'ID dell'utente loggato

            // Crea un dizionario che lega l'utente alla sua lista di recensioni
            var model = new Dictionary<Utente, List<Recensione>>()
            {
                { utenteloggato, recensioni }
            };

            // Passa il dizionario alla vista
            return View(model);
        }



        public IActionResult Accedi(Dictionary<string, string> credenziali)
        {
            string user = credenziali["username"];
            string password = credenziali["password"];
            Console.WriteLine($"{user} {password}");

            if (DAOUtente.GetInstance().Find(user, password))
            {
                Entity e = DAOUtente.GetInstance().Find(user);
                Utente utenteFront = new Utente
                {
                    Id = ((Utente)e).Id,
                    Ruolo = ((Utente)e).Ruolo,
                    Email = ((Utente)e).Email,
                   
                   
                };

                // Memorizza l'utente nella sessione
                HttpContext.Session.SetString("UtenteLoggato", JsonConvert.SerializeObject(utenteFront));
                _logger.LogInformation($"Utente Loggato: {utenteFront.Email} alle {DateTime.Now}");

                if (((Utente)e).Ruolo == 0)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult ModificaAnagrafica(Dictionary<string, string> dati)
        {
            var utenteloggato = GetUtenteLoggato();
            if (utenteloggato== null)
            {
                return RedirectToAction("Login");
            }

            HttpContext.Session.SetString("UtenteLoggato", JsonConvert.SerializeObject(utenteloggato));

                foreach(var l in dati)
                    Console.WriteLine(l.Key +" "+ l.Value);

            // Aggiorna l'anagrafica
            utenteloggato.Anagrafica = new Anagrafica
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
            if (utenteloggato.Anagrafica.Nome!="")
            {
               
                var succ = DAOAnagrafica.GetInstance().Update(utenteloggato);
                if (succ)
                {
                    return View("Profilo", utenteloggato);
                }
                else
                {
                    return RedirectToAction("Profilo", utenteloggato);
                }
            }
            else 
            
            {
                // Se l'anagrafica è nuova, usa il metodo Create
                var succ = DAOAnagrafica.GetInstance().Create(utenteloggato);
                if (succ)
                {
                    return View("Profilo", utenteloggato);
                }
                else
                {
                    return RedirectToAction("Profilo", utenteloggato);
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
               Dob = DateTime.Parse(credenziali["Dob"]) // Imposta la data di nascita direttamente


            };
            
            ((Utente)e).Ruolo = 1;

            if (DAOUtente.GetInstance().Create(e))
            {
                try
                {
                    // Crea il messaggio email
                    Utente u = (Utente)e;
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("hellsgames2024@gmail.com"); // Indirizzo mittente
                    mail.To.Add(u.Email); // Indirizzo destinatario
                    mail.Subject = "Registrazione Completata";
                    mail.Body = $"Benvenut* {u.Username} nella famiglia Hell's Games™ \n" +
                        "Goditi il nostro servizio infernale, navigando nell'ade dei nostri prezzi superbi!!!.\n";
                    
                    // Percorso del file immagine locale
                    //string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img/HellsGames.png");
                    string imagePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "img", "HellsGames.png");

                    if (System.IO.File.Exists(imagePath)) // Controlla che il file esista
                    {
                        Attachment attachment = new Attachment(imagePath, MediaTypeNames.Image.Jpeg); // Specifica il tipo MIME
                        mail.Attachments.Add(attachment);
                    }
                    else
                    {
                        Console.WriteLine("File immagine non trovato!");
                    }


                    // Configura il client SMTP per Gmail
                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587); // Server e porta Gmail
                    smtpClient.Credentials = new NetworkCredential("hellsgames2024@gmail.com", "fkit fiur dpsv bqgr"); // Credenziali Gmail
                    smtpClient.EnableSsl = true; // Abilita SSL/TLS

                    // Invia l'email
                    smtpClient.Send(mail);
                    Console.WriteLine("Email inviata con successo!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errore durante l'invio dell'email: " + ex.Message);
                }

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
            _logger.LogInformation($"Logout alle {DateTime.Now}");
            HttpContext.Session.Clear(); // Pulisce tutta la sessione
            return RedirectToAction("Index", "Home");
        }


        /* PAGINE PERSONALI DELL'UTENTE */
        public IActionResult StoricoAcquisti()
        {
            var utenteloggato = GetUtenteLoggato();
            if (utenteloggato== null)
            {
                return RedirectToAction("Login");
            }

            if (utenteloggato.Anagrafica == null)
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
                utenteloggato.Anagrafica = (Anagrafica)AnagraficaVuota;
            }

            // Recupera le recensioni dell'utente loggato
            List<Ordine> ordini = DAOOrdine.GetInstance().OrdiniUtente(utenteloggato.Id); // Recupera le recensioni dall'ID dell'utente loggato

            // Crea un dizionario che lega l'utente alla sua lista di recensioni
            var model = new Dictionary<Utente, List<Ordine>>()
            {
                { utenteloggato, ordini }
            };

            // Passa il dizionario alla vista
            return View(model);
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
        private Utente GetUtenteLoggato()
        {
            var utenteLoggato = HttpContext.Session.GetString("UtenteLoggato");
            if (utenteLoggato != null)
            {
                return JsonConvert.DeserializeObject<Utente>(utenteLoggato);
            }
            return null;
        }

        [HttpGet("Utenti/GetIdUtenteLoggato")]
        public IActionResult GetIdUtenteLoggato()
        {
            var utente = GetUtenteLoggato();  // Recupera l'utente loggato dalla sessione

            // Verifica che l'utente esista e che abbia un Id
            if (utente == null || utente.Id == 0)
            {
                return NotFound();  // Se non c'è nessun utente loggato, restituisci un errore
            }

            return Json(new { Id = utente.Id });  // Restituisci solo l'ID dell'utente come JSON
        }


    }
}
