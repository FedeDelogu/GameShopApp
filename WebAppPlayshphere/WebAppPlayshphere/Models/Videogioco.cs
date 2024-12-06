﻿using Utility;
using WebAppPlayshphere.DAO;
namespace WebAppPlayshphere.Models
{
    public class Videogioco : Entity
    {
        public Videogioco() {
            Piattaforme = new List<Piattaforma>();
            Recensioni = new List<Entity>();
        }
        public Videogioco(int id, string titolo, string generi, string descrizione, int pegi, double prezzo,
                          string publisher, int quantita, DateTime rilascio,
                          string sviluppatori, List<Entity> recensioni, string link) : base(id)
        {
            Titolo = titolo;
            Generi = generi;
            Descrizione = descrizione;
            Pegi = pegi;
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
        public List<Piattaforma> Piattaforme { get; set; }
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
        public string GetPiattaforme()
        {
            string ris = "";
            foreach (var item in Piattaforme)
            {
                ris += item.Nome + ", ";
            }
            if(ris.Length > 0)
            {
                ris = ris.Remove(ris.Length - 2);
            }
            return ris;
        }
        public double Valutazione()
        {
            double ris = 0;
            foreach (var item in Recensioni)
            {
                ris += ((Recensione)item).Valutazione;
            }
            return Recensioni.Count>0?Math.Round(ris / Recensioni.Count, 1):0;
        }
        
        public override void FromDictionary(Dictionary<string, string> riga)
        {
            base.FromDictionary(riga);
            var ris = DAOPiattaforma.GetIstance().FindByGioco(int.Parse(riga["id"]));
            Recensioni = DAORecensione.GetIstance().RecensioniGioco(Id);
            // per ogni piattaforma trovata la aggiungo alla lista delle piattaforme del gioco
            /*foreach (var item in ris)
            {
                Piattaforme.Add(
                    new Piattaforma()
                    {
                        Id = ((Piattaforma)item).Id,
                        Nome = ((Piattaforma)item).Nome
                    }
                    );
            }*/
            
        }
    }
}