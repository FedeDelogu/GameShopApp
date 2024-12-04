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
            if (DAOMessaggi.GetInstance().FindByUtente(3) != null)
            {
                foreach (var v in DAOMessaggi.GetInstance().FindByUtente(3))
                {
                    Console.WriteLine("MESSAGGIO: " + ((Messaggio)v).Contenuto);
                }
            }
            else
            {
                Console.WriteLine("messaggi == null");
            }
            //Chat chat = (Chat)DAOChat.GetInstance().Find(1);
            if (HttpContext.Session.GetString("UtenteLoggato") != null)
            {
                // devo controllare se la chat esiste
                // mi serve : idutente, id della chat
                // per adesso ho l idutente

                string u = HttpContext.Session.GetString("UtenteLoggato");
                Utente utente = JsonConvert.DeserializeObject<Utente>(u);
                Chat chat = (Chat)DAOChat.GetInstance().FindByUtente(utente.Id);
                /*
                if (chat == null) // la chat non esiste va creata
                {
                    primaChat = true;
                    chat = new Chat();
                    chat.DataCreazione = DateTime.Now;
                    risultatoQuery = DAOChat.GetInstance().Create(chat);
                    if (!risultatoQuery)
                    {
                        Console.WriteLine("Errore nella creazione della chat");
                        //return null;
                    }
                    else
                    {
                        var c = DAOChat.GetInstance().FindByUtente(utente.Id);
                        if (c == null)
                        {
                            Console.WriteLine("ERRORE NELLA RICERCA DELLA CHAT");
                            return null;
                        }
                        chat.Id = c.Id;

                    }
                    // aggiungo il partecipante
                    risultatoQuery = DAOChat.GetInstance().AddPartecipante(chat.Id, utente.Id);
                    if (!risultatoQuery)
                    {
                        Console.WriteLine("Errore aggiunta del partecipante");
                    }
                }*/
                // se la chat esiste
                //else
                //{
                    Console.WriteLine($"UTENTE ID PER CERCARE I MESSAGGI {utente.Id}");
                    if(DAOMessaggi.GetInstance().FindByUtente(utente.Id) == null)
                    {
                        Console.WriteLine("MESSAGGI NULLI");
                    }
                    foreach (var v in DAOMessaggi.GetInstance().FindByUtente(utente.Id))
                    {
                        chat.Messaggi.Add((Messaggio)v);
                    }
                //}
                var utente_e_chat = new {
                    utente = utente,
                    chat = chat,
                };
                return View(utente_e_chat);
            }
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
            if (DAOMessaggi.GetInstance().FindByUtente(int.Parse(idutente)) != null) // ramo con messaggi
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
                else
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
            return Content("TUTTO ROTTO PORCO DIO"); // DA CANCELLARE !!!!!!!!!

        }
    }
}

