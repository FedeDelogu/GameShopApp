﻿using Microsoft.AspNetCore.Mvc;
using Utility;
using WebAppPlayshphere.DAO;
using WebAppPlayshphere.Models;

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

        

        public IActionResult Login(Dictionary<string, string> credenziali)
        {
            // Le chiavi del dictionary sono i name di HTML
            string user = credenziali["username"];
            string password = credenziali["passw"];

            if (DAOUtente.GetInstance().Find(user, password))
            {
                Entity e = DAOUtente.GetInstance().Find(user);
                // Memorizzo chi ha fatto login
                _utenteLoggato = (Utente)e;

                _logger.LogInformation($"Utente Loggato: {_utenteLoggato.Username} alle {DateTime.Now}");

                return View(e);
            }
            else
                return RedirectToAction("Index");
        }

        public IActionResult Registrazione()
        {
            return View();
        }

        public IActionResult Salva(Dictionary<string, string> credenziali)
        {
            Entity e = new Utente();
            e.FromDictionary(credenziali);
            ((Utente)e).Ruolo = 0;

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
