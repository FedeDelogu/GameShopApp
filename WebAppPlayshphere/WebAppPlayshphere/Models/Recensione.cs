using Utility;
using WebAppPlayshphere.DAO;

namespace WebAppPlayshphere.Models
{
    public class Recensione : Entity
    {
        public Recensione(int id, string commento, bool valido, int valutazione, Utente utente) : base (id)
        {
            Commento = commento;
            Valido = valido;
            Valutazione = valutazione;
            Utente = utente;
        }

        public Recensione() { }

        public string Commento { get; set; }
        public bool Valido { get; set; }
        public int Valutazione { get; set; }
        public Utente Utente { get; set; }
        public int IdVideogioco { get; set; } // aggiunto id videogioco

        // OVERRIDE TOSTRING
        public override string ToString()
        {
            return $"Commento: {Commento}\n" +
                   $"{(Valido ? "Approvato" : "Non Approvato")}\n" +
                   $"Valutazione: {Valutazione}\n" +
                   $"{Utente.ToString()}" +
                   $"-------------------------------------\n";
        }

    }
}
