using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using WebAppPlayshphere.DAO;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Chat()
        {
            bool risultatoQuery = false;
            bool primaChat = false;
            if (HttpContext.Session.GetString("UtenteLoggato") != null)
            {
                string u = HttpContext.Session.GetString("UtenteLoggato");
                Utente utente = JsonConvert.DeserializeObject<Utente>(u);
                Chat chat = (Chat)DAOChat.GetInstance().FindByUtente(utente.Id);
                if (chat == null)
                {
                    chat = new Chat();
                    chat.DataCreazione = DateTime.Now;
                    chat.IdUtente = utente.Id;
                    risultatoQuery = DAOChat.GetInstance().Create(chat);
                    // ERRORE CREAZIONE CHAT
                    if (!risultatoQuery)
                    {
                        Console.WriteLine("Errore nella creazione della chat (CHAT CONTROLLER)");
                    }
                    var c = DAOChat.GetInstance().FindByUtente(utente.Id);
                    if (c == null)
                    {
                        Console.WriteLine("ERRORE NEL RECUPERO DELLA CHAT APPENA CREATA (CHAT CONTROLLER)");
                        return null;
                    }
                    // ASSEGNO L ID DELLA CHAT APPENA CREATA ALL OGGETTO CHAT
                    chat.Id = c.Id;
                } // fine if (chat == null)
                else // RAMO CON CHAT GIA ESISTENTE
                {
                    Console.WriteLine($"UTENTE ID PER CERCARE I MESSAGGI {utente.Id}");
                    // CONTROLLO CHE CI SIANO MESSAGGI PER L UTENTE
                    if (DAOMessaggi.GetInstance().FindByUtente(utente.Id) != null)
                    {
                        // SE CI SONO MESSAGGI LI AGGIUNGO ALLA CHAT
                        foreach (var v in DAOMessaggi.GetInstance().FindByUtente(utente.Id))
                        {
                            chat.Messaggi.Add((Messaggio)v);
                        }
                    }
                }
                var utente_e_chat = new {
                    utente = utente,
                    chat = chat,
                };
                return View(utente_e_chat);
            }// fine if (utenteloggato != null)
            return View("/Home/Index");
        }
        public IActionResult InviaMex(Messaggio messaggio)
        {
            Console.WriteLine($"messaggio in arriva da {messaggio.IdUtente}, contenuto : {messaggio.Contenuto}");
            return RedirectToAction("Chat", "Chat");
        }
        // metodo solo per admin
        public IActionResult Elenco()
        {
           return View(DAOChat.GetInstance().ReadLista());
        }
        public IActionResult ChatAdmin(string idchat, string idutente)
        {
            Utente utente = (Utente)DAOUtente.GetInstance().Find(int.Parse(idutente));
            Chat chat = (Chat)DAOChat.GetInstance().FindByUtente(utente.Id);
            if (DAOMessaggi.GetInstance().FindByUtente(int.Parse(idutente)) != null) // chat con messaggi
            {
                if(DAOMessaggi.GetInstance().FindByUtente(int.Parse(idutente)).Count == 0)
                {
                    Console.WriteLine($"NON CI SONO MESSAGGI PER L UTENTE CON ID {idutente}");
                    foreach (var v in DAOMessaggi.GetInstance().FindByUtente(int.Parse(idutente)))
                    {
                        Console.WriteLine("MESSAGGIO: " + ((Messaggio)v).Contenuto);
                    }
                    var utente_e_chat = new
                    {
                        utente = utente,
                        chat = chat,
                    };
                    return View("Chat", utente_e_chat);
                }
                else // chat senza messaggi
                {
                    foreach (var v in DAOMessaggi.GetInstance().FindByUtente(utente.Id))
                    {
                        chat.Messaggi.Add((Messaggio)v);
                    }
                    var utente_e_chat = new
                    {
                        utente = utente,
                        chat = chat,
                    };
                    return View("Chat", utente_e_chat);
                }
            }
            return Content("ERRORE SISTEMA"); // DA CANCELLARE !!!!!!!!!

        }
    }
}

