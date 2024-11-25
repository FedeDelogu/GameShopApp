using Utility;
using WebAppPlayshphere.DAO;
namespace WebAppPlayshphere.Models
{
    public class Videogioco : Entity
    {
        public Videogioco() { }
        public Videogioco(int id, string titolo, string generi, string descrizione, int pegi, string piattaforme, double prezzo,
                          string publisher, int quantita, DateTime rilascio,
                          string sviluppatori, List<Entity> recensioni, string link) : base(id)
        {
            Titolo = titolo;
            Generi = generi;
            Descrizione = descrizione;
            Pegi = pegi;
            Piattaforme = piattaforme;
            Prezzo = prezzo;
            Publisher = publisher;
            Quantita = quantita;
            Rilascio = rilascio;
            Sviluppatori = sviluppatori;
            Recensioni = recensioni;
            Link = link;
        }

        public string Titolo { get; set; }
        public string Generi { get; set; }
        public string Descrizione { get; set; }
        public int Pegi { get; set; }
        public string Piattaforme { get; set; }
        public double Prezzo { get; set; }
        public string Publisher { get; set; }
        public int Quantita { get; set; }
        public DateTime Rilascio { get; set; }
        public string Sviluppatori { get; set; }
        public List<Entity> Recensioni { get; set; }
        public string Link { get; set; }


        public string ToString()
        {
            return base.ToString() +
                $"Titolo: {Titolo}\n" +
                $"Genere: {Generi}\n" +
                $"pegi: {Pegi}\n" +
                $"Piattaforme: {Piattaforme}\n" +
                $"Prezzo: {Prezzo}\n" +
                $"Publisher: {Publisher}\n" +
                $"Quantita: {Quantita}\n" +
                $"Rilascio: {Rilascio.ToString("dd/MM/yyyy")}\n" +
                $"Sviluppatore: {Sviluppatori}\n";// +
                /*
                $"Recensioni: {tutteRecensioni((Recensione)Recensioni)}\n" +
                $"Valutazione: {Valutazione(Recensioni)}\n";*/
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
            return Math.Round(ris / rec.Count, 1);
        }
        /*
        public override void FromDictionary(Dictionary<string, string> riga)
        {
            //List<Recensione> recensioni = (List<Recensione>)DAORecensione.GetIstance().RecensioniGioco(Id);

            Recensioni = DAORecensione.GetIstance().RecensioniGioco(Id);

            base.FromDictionary(riga);
        }*/
    }
}