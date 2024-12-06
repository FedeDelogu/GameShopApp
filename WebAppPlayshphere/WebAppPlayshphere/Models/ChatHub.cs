using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using Utility;
using WebAppPlayshphere.DAO;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly Dictionary<string, (string? Utente, string? Admin)> chatAttive = new();
        private static readonly SemaphoreSlim semaphore = new(1, 1);

        public async Task AccediChat(string userId, string chatId)
        {
            int idchat = int.Parse(chatId); 
            Entity? entity = DAOUtente.GetInstance().Find(int.Parse(userId));
            if (entity is not Utente utente)
            {
                await Clients.Caller.SendAsync("AccessoNegato", "Utente non trovato.");
                return;
            }

            string ruolo = utente.Ruolo == 0 ? "admin" : "utente";
            Console.WriteLine($"UTENTE CON RUOLO : {ruolo}.");
            string connectionId = Context.ConnectionId;

            Console.WriteLine($"CONNECTIONID = {connectionId}");

            await semaphore.WaitAsync();
            try
            {
                if (!chatAttive.ContainsKey(chatId))
                {
                    chatAttive[chatId] = (null, null);
                }

                var (utenteConnesso, adminConnesso) = chatAttive[chatId];

                // Se è un utente e l'utente è già connesso (controllo con userId)
                if (ruolo == "utente" && utenteConnesso != null && utenteConnesso != userId)
                {
                    await Clients.Caller.SendAsync("AccessoNegato", "Un altro utente è già connesso a questa chat.");
                    return;
                }
                /*else
                {
                    
                }*/

                // Se è un admin e c'è già un altro admin connesso (consideriamo admin unico per il sito)
                if (ruolo == "admin" && adminConnesso != null && adminConnesso != connectionId)
                {
                    await Clients.Caller.SendAsync("AccessoNegato", "Un admin è già connesso.");
                    return;
                }

                // Aggiorna le connessioni nel dizionario
                chatAttive[chatId] = ruolo == "utente"
                    ? (userId, adminConnesso)
                    : (utenteConnesso, connectionId);


                await Groups.AddToGroupAsync(connectionId, chatId);
                await Clients.Group(chatId).SendAsync("ReceiveMessage", "Sistema", $"{ruolo} si è connesso.");
                if (chatAttive[chatId].Admin == null)
                {
                    Console.WriteLine("NESSUN ADMIN CONNESSO");
                    Chat chat = (Chat)DAOChat.GetInstance().FindByUtente(utente.Id);
                    if (chat == null)
                    {
                        Console.WriteLine("ERRORE RECUPERO CHAT");
                        return;
                    }
                    chat.Notifica = 1;
                    bool ris = DAOChat.GetInstance().Update(chat);
                    if (!ris)
                    {
                        Console.WriteLine("ERRORE AGGIORNAMENTO CHAT");
                    }
                }
                else if (chatAttive[chatId].Admin != null)
                {
                    Chat chat = (Chat)DAOChat.GetInstance().Find(idchat);
                    if (chat == null)
                    {
                        Console.WriteLine("ERRORE RECUPERO CHAT QUANDO L ADMIN E' CONNESSO");
                        return;
                    }
                    chat.Notifica = 0;
                    bool ris = DAOChat.GetInstance().Update(chat);
                    if (!ris)
                    {
                        Console.WriteLine("ERRORE AGGIORNAMENTO CHAT");
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task SendMessage(string user, string chatId, string userId, string message)
        {
            Console.WriteLine($"CHAT ID PASSATO A SENDMESSAGE : {chatId}, id utente : {userId}");
            bool ris = DAOMessaggi.GetInstance().Create(new Messaggio
            {
                IdChat = int.Parse(chatId),
                IdUtente = int.Parse(userId),
                Contenuto = message
            });
            if (!ris)
            {
                Console.WriteLine("ERRORE NELL INSERIMENTO DEL MESSAGGIO");
            }
            if (!chatAttive.ContainsKey(chatId))
            {
                await Clients.Caller.SendAsync("AccessoNegato", "Chat non trovata.");
                return;
            }

            await Clients.Group(chatId).SendAsync("ReceiveMessage", user, message);
        }
    }
}
