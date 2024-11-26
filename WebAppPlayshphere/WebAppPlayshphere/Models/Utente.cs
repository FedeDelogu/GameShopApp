using System.ComponentModel.DataAnnotations;
using Utility;
using WebAppPlayshphere.DAO;

namespace WebAppPlayshphere.Models
{
    public class Utente : Entity
    {
        //string password;
        //string email;
        //int ruolo;
        //Anagrafica anagrafica;

        //Carrello carrello;

        public Utente() { }
        /*public Utente(int id,string nome, string cognome, string password, string email, string indirizzo, string telefono, string citta, string cap, string stato, int ruolo, DateTime dob, Carrello carrello)
            :base (id)
        {
            Nome = nome;
            Cognome = cognome;
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
        }*/

        [Required(ErrorMessage = "L'email è obbligatoria.")]
        [EmailAddress(ErrorMessage = "L'email non è valida.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La password è obbligatoria.")]
        [StringLength(100, ErrorMessage = "La password deve avere almeno 8 caratteri.", MinimumLength = 8)]
        public string Password { get; set; }
        [Required(ErrorMessage = "La password di conferma è obbligatoria.")]
        [Compare("Password", ErrorMessage = "Le password non corrispondono.")]
        public string ConfermaPassword { get; set; }

        [Range(0, 10, ErrorMessage = "Ruolo non valido.")]
        public int Ruolo { get; set; }
        public Anagrafica Anagrafica { get; set; }



        /*
        public string Indirizzo { get => indirizzo; set => indirizzo = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Citta { get => citta; set => citta = value; }
        public string Cap { get => cap; set => cap = value; }
        public string Stato { get => stato; set => stato = value; }
        public int Ruolo { get => ruolo; set => ruolo = value; }
        public string Nome { get; set; }
        public string Cognome {  get; set; }
        public DateTime Dob { get => dob; set => dob = value; }*/
        //public Carrello Carrello { get => carrello; set => carrello = value; }
        public string Username
        {
            get
            {
                if (!string.IsNullOrEmpty(Email) && Email.Contains("@"))
                {
                    return Email.Split('@')[0];
                }
                return string.Empty;
            }
        }
        public override string ToString()
        {
            return $"Id : {base.ToString()}\n" +
                   $"Password : {Password}\n" +
                   $"Email : {Email}\n" +
                   $"Username : {Username}\n" +
                   $"Ruolo : {(Ruolo == 1 ? "Admin" : "Utente")}\n" +
                   //$"Carrello : {Carrello.ToString()}" +
                   $"{(Ruolo == -1 ? "\nUtente bannato" : "")}\n" +
                   $"Dati anagrafici : {(this.Anagrafica != null ? this.Anagrafica.ToString() : "")}";
        }
        public int Eta()
        {
            // SE NON HA UN ANAGRAFICA RESTITUISCO -1 
            if (this.Anagrafica == null)
            {
                return -1;
            }
            DateTime oggi = DateTime.Now;
            int eta = oggi.Year - this.Anagrafica.Dob.Year;

            // SE IL COMPLEANNO NON E' ANCORA PASSATO TOLGO 1 DALL 'ETA'
            if (oggi < this.Anagrafica.Dob.AddYears(eta))
            {
                eta--;
            }

            return eta;
        }
        public override void FromDictionary(Dictionary<string, string> riga)
        {
            Entity anagrafica = (Anagrafica)DAOAnagrafica.GetIstance().Find(int.Parse(riga["id"]));
            if (anagrafica != null) {
                Anagrafica = (Anagrafica)anagrafica;
            }
            base.FromDictionary(riga);
        }
    }
}