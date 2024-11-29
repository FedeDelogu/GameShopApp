using Utility;
namespace WebAppPlayshphere.Models
{
    public class Carrello : Entity
    {
        public Dictionary<Videogioco, int> Videogiochi { get; set; }

        public Dictionary<Videogioco, int> Piattaforme { get; set; }
        public Carrello() { }


        public Carrello(int id, Dictionary<Videogioco, int> videogiochi, Dictionary<Videogioco,int> piattaforme) : base(id)

        {
            Videogiochi = videogiochi;
            Piattaforme = piattaforme;
        }

        public double Totale()
        {
            double totale = 0;
            foreach (var item in Videogiochi)
            {
                totale += (item.Key.Prezzo * item.Value);
            }
            return totale;

        }
        public override string ToString()
        {
            string ris = "";
            foreach(var v in Videogiochi)
            {
                ris+=$"Gioco: {v.Key.Titolo}\nQuantità: {v.Value}\n---------\n";
            }
            return ris;
        }

        
    }
}
