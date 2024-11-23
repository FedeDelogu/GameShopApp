using Utility;

namespace WebAppPlayshphere.Models
{
    public class Anagrafica : Entity
    {
        string indirizzo;
        string telefono;
        string citta;
        string cap;
        string stato;
        int ruolo;
        DateTime dob;

        public Anagrafica() { }

        public string Indirizzo { get => indirizzo; set => indirizzo = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Citta { get => citta; set => citta = value; }
        public string Cap { get => cap; set => cap = value; }
        public string Stato { get => stato; set => stato = value; }
        public int Ruolo { get => ruolo; set => ruolo = value; }
        public DateTime Dob { get => dob; set => dob = value; }

        public override string ToString()
        {
            return 
                $"Indirizzo : {Indirizzo}\n" +
                $"Telefono : {Telefono}\n" +
                $"Citta : {Citta}\n" +
                $"Cap : {Cap}\n" +
                $"Stato : {Stato}\n" +
                $"Ruolo : {Ruolo}\n" +
                $"Data di nascita : {Dob}\n";
        }
    }
}
