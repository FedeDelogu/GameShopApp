using WebAppPlayshphere.DAO;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.Factory
{
    public class RecensioneFactory
    {
        public static Recensione CreateRecensione(Dictionary<string, string> recensione)
        {
            Recensione r = new Recensione();
            r.Id = (recensione.ContainsKey("id") ? Convert.ToInt32(recensione["id"]) : 0);
            r.Commento = recensione["commento"];
            r.Valutazione = Convert.ToInt32(recensione["valutazione"]);
            r.Valido = Convert.ToBoolean(recensione["valido"]);
            r.Utente = (Utente)DAOUtente.GetInstance().Find(Convert.ToInt32(recensione["idutente"]));
            r.IdVideogioco = Convert.ToInt32(recensione["idvideogioco"]);

            return r;
        }
    }
}