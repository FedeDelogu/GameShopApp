using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Utility;
using WebAppPlayshphere.DAO;
using WebAppPlayshphere.Models;
using Newtonsoft.Json;

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
        static Utente _utenteLoggato = null;
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

        [Authorize]
        public ActionResult Profilo()
        {
            return View();
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

                HttpContext.Session.SetString("UtenteLoggato", JsonConvert.SerializeObject(_utenteLoggato));
                _logger.LogInformation($"Utente Loggato: {_utenteLoggato.Username} alle {DateTime.Now}");
                if (((Utente)e).Ruolo == 0)
                {
                    return RedirectToAction("Index","Admin");
                }
                return RedirectToAction("Index","Home", new {id=e.Id});
            }
            else
                return RedirectToAction("Login");
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
            Entity e = new Utente();
            e.FromDictionary(credenziali);
            ((Utente)e).Ruolo = 1;

            if (DAOUtente.GetInstance().Create(e))
            {
                // Dove lo mando se la registrazione avviene?
                return RedirectToAction("Index");
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
            // Reindirizzo al Login per il prossimo accesso
            return RedirectToAction("Index");
        }
    }
}
