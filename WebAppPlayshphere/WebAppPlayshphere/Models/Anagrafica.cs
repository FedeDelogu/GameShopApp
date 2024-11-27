using Utility;

namespace WebAppPlayshphere.Models
{
    public class Anagrafica : Entity
    {
        string nome;
        string cognome;
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
        public string Nome { get => nome; set => nome = value; }
        public string Cognome { get => cognome; set => cognome = value; }

        public override string ToString()
        {
            return
                $"Nome : {Nome}\n" +
                $"Cognome : {Cognome}\n" +
                $"Indirizzo : {Indirizzo}\n" +
                $"Telefono : {Telefono}\n" +
                $"Citta : {Citta}\n" +
                $"Cap : {Cap}\n" +
                $"Stato : {Stato}\n" +
                $"Ruolo : {Ruolo}\n";
        }


    }
}