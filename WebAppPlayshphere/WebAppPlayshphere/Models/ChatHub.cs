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
            Entity? entity = DAOUtente.GetInstance().Find(int.Parse(userId));
            if (entity is not Utente utente)
            {
                await Clients.Caller.SendAsync("AccessoNegato", "Utente non trovato.");
                return;
            }

            string ruolo = utente.Ruolo == 0 ? "admin" : "utente";
            Console.WriteLine($"UTENTE CON RUOLO : {ruolo}.");
            string connectionId = Context.ConnectionId;

            await semaphore.WaitAsync();
            try
            {
                if (!chatAttive.ContainsKey(chatId))
                {
                    chatAttive[chatId] = (null, null);
                }

                var (utenteConnesso, adminConnesso) = chatAttive[chatId];

                if (ruolo == "utente" && utenteConnesso != null ||
                    ruolo == "admin" && adminConnesso != null)
                {
                    await Clients.Caller.SendAsync("AccessoNegato", $"Un {ruolo} è già connesso.");
                    return;
                }

                chatAttive[chatId] = ruolo == "utente"
                    ? (connectionId, adminConnesso)
                    : (utenteConnesso, connectionId);

                await Groups.AddToGroupAsync(connectionId, chatId);
                await Clients.Group(chatId).SendAsync("ReceiveMessage", "Sistema", $"{ruolo} si è connesso.");
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task SendMessage(string user, string chatId, string message)
        {
            if (!chatAttive.ContainsKey(chatId))
            {
                await Clients.Caller.SendAsync("AccessoNegato", "Chat non trovata.");
                return;
            }

            await Clients.Group(chatId).SendAsync("ReceiveMessage", user, message);
        }
    }
}
