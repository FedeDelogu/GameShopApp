using Utility;

namespace WebAppPlayshphere.Models
{
    public class Ordine : Entity
    {
        public Ordine() {
            Videogiochi = new Dictionary<Videogioco, int>();
        }
        public Ordine(int id, Dictionary<Videogioco, int> videogiochi, string stato, Utente utente, DateTime dataOrdine) : base(id)
        {
            Videogiochi = videogiochi;
            Stato = stato;
            Utente = utente;
            DataOrdine = dataOrdine;
        }

        public Dictionary<Videogioco, int> Videogiochi { get; set; } // oggetto Videogioco e int per la quantità
        public string Stato { get; set; }
        public Utente Utente { get; set; }
        public DateTime DataOrdine { get; set; }

        // TODO: aggiungere Utente al ToString 
        public override string ToString()
        {
            return base.ToString() +
                $"Stato dell'ordine: {Stato}\n" +
                $"Data ordine: {DataOrdine}\n" +
                $"------------------------------\n";
        }

        public double Totale()
        {
            return Videogiochi.Sum(v => v.Key.Prezzo * v.Value);
        }
    }
}