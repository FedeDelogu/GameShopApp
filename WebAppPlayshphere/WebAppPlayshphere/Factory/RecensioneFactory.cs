using Microsoft.Extensions.Logging.Abstractions;
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
            // controllo se l id videogioco è presente
            string idVideogiocoStr = recensione["idvideogioco"];
            r.IdVideogioco = (int.TryParse(idVideogiocoStr, out int idVideogioco)) ? idVideogioco : 0;

            return r;
        }
    }
}