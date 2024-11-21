using Utility;

namespace WebAppPlayshphere.Models
{
    public class Utente : Entity
    {
        string username;
        string password;
        string email;
        string indirizzo;
        string telefono;
        string citta;
        string cap;
        string stato;
        int ruolo;
        DateTime dob;
        Carrello carrello;

        public Utente() { }
        public Utente(int id,string username, string password, string email, string indirizzo, string telefono, string citta, string cap, string stato, int ruolo, DateTime dob, Carrello carrello)
            :base (id)
        {
            Username = username;
            Password = password;
            Email = email;
            Indirizzo = indirizzo;
            Telefono = telefono;
            Citta = citta;
            Cap = cap;
            Stato = stato;
            Ruolo = ruolo;
            Dob = dob;
            Carrello = carrello;
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Email { get => email; set => email = value; }
        public string Indirizzo { get => indirizzo; set => indirizzo = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Citta { get => citta; set => citta = value; }
        public string Cap { get => cap; set => cap = value; }
        public string Stato { get => stato; set => stato = value; }
        public int Ruolo { get => ruolo; set => ruolo = value; }
        public DateTime Dob { get => dob; set => dob = value; }
        public Carrello Carrello { get => carrello; set => carrello = value; }

        public override string ToString()
        {
            return $"Id : {base.ToString()}\n" +
                   $"Password : {Password}\n" +
                   $"Email : {Email}\n" +
                   $"Stato : {Email}\n" +
                   $"Citta : {Citta}\n" +
                   $"Via : {Indirizzo}\n" +
                   $"Caap : {Cap}\n" +
                   $"Stato : {Stato}\n" +
                   $"Ruolo : {(Ruolo == 1 ? "Admin" : "Utente")}\n" +
                   $"Data di nascita : {Dob.ToString("dd/MM/yyyy")}\n" +
                   $"Carrello : {Carrello.ToString()}" +
                   $"{(Ruolo == -1 ? "\nUtente bannato" : "")}";
        }
        public int Eta()
        {
            DateTime oggi = DateTime.Now;
            int eta = oggi.Year - Dob.Year;

            // SE IL COMPLEANNO NON E' ANCORA PASSATO TOLGO 1 DALL 'ETA'
            if (oggi < Dob.AddYears(eta))
            {
                eta--;
            }

            return eta;
        }
    }
}
