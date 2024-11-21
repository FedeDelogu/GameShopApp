
using Utility;
namespace WebAppPlayshphere.Models
{
    public class Videogioco : Entity
    {
        public Videogioco(int id, string titolo, string genere, int pegi, List<string> piattaforme, double prezzo,
                          string publisher, int quantita, DateTime rilascio,
                          List<string> sviluppatori, List<Recensione> recensioni) : base(id)
        {
            Titolo = titolo;
            Genere = genere;
            Pegi = pegi;
            Piattaforme = piattaforme;
            Prezzo = prezzo;
            Publisher = publisher;
            Quantita = quantita;
            Rilascio = rilascio;
            Sviluppatori = sviluppatori;
            Recensioni = recensioni;
        }

        public string Titolo { get; set; }
        public string Genere { get; set; }
        public int Pegi { get; set; }
        public List<string> Piattaforme { get; set; }
        public double Prezzo { get; set; }
        public string Publisher { get; set; }
        public int Quantita { get; set; }
        public DateTime Rilascio { get; set; }
        public List<string> Sviluppatori { get; set; }
        public List<Recensione> Recensioni { get; set; }


        public string ToString()
        {
            return base.ToString() +
                $"Titolo: {Titolo}\n" +
                $"Genere: {Genere}\n" +
                $"pegi: {Pegi}\n" +
                $"Piattaforme: {tuttePiattaforme(Piattaforme)}\n" +
                $"Prezzo: {Prezzo}\n" +
                $"Publisher: {Publisher}\n" +
                $"Quantita: {Quantita}\n" +
                $"Rilascio: {Rilascio.ToString("dd/MM/yyyy")}\n" +
                $"Sviluppatore: {tuttiSviluppatori(Sviluppatori)}\n" +
                $"Recensioni: {tutteRecensioni(Recensioni)}\n" +
                $"Valutazione: {Valutazione(Recensioni)}\n";
        }
        public string tuttePiattaforme(List<string> p)
        {
            string ris = "";
            foreach (var item in p)
            {
                ris += item + ",";
            }
            return ris;
        }
        public string tuttiSviluppatori(List<string> p)
        {
            string ris = "";
            foreach (var item in p)
            {
                ris += item + ",";
            }
            return ris;
        }
        public string tutteRecensioni(List<Recensione> p)
        {
            string ris = "";
            foreach (var item in p)
            {
                ris += item.Valutazione + ",";
            }
            return ris;
        }
        public double Valutazione(List<Recensione> rec)
        {
            double ris = 0;
            foreach (var item in rec)
            {
                ris += item.Valutazione;
            }
            return ris / rec.Count;
        }
    }
}
