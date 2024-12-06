using Utility;
using WebAppPlayshphere.Factory;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.DAO
{
    public class DAOPiattaforma : IDAO
    {
        private IDatabase db;

        private DAOPiattaforma()
        {
            db = new Database("Playsphere3", "localhost");
        }
        private static DAOPiattaforma istance = null;

        public static DAOPiattaforma GetIstance()
        {
            if (istance == null)
            {
                istance = new DAOPiattaforma();
            }
            return istance;
        }
        public bool Create(Entity e)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Entity Find(int id)
        {
            var riga = db.ReadOne("SELECT * FROM Piattaforme WHERE id="+id);
            Entity e = new Piattaforma();
            e.FromDictionary(riga);
            return e;
        }

        public List<Entity> Read()
        {
            var righe = db.Read("SELECT * FROM Piattaforme");
            if (righe == null)
            {
                Console.WriteLine("riga nulla");
                return null;
            }
            List<Entity> ris = new List<Entity>();
            foreach (var riga in righe)
            {
                Entity e = new Piattaforma();
                //Entity e = RecensioneFactory.CreateRecensione(riga);
                ((Piattaforma)e).Id = int.Parse(riga["id"]);
                ((Piattaforma)e).Nome = riga["nome"];
                ris.Add(e);
            }
            return ris;
        }
        public List<Entity> FindByGioco(int idGioco)
        {
            List<Entity> ris = new List<Entity>();

            // Esegue la query per ottenere le piattaforme del videogioco
            var righe = db.Read(
                $"SELECT p.*, pl.nome " +
                $"FROM PiattaformeVideogiochi p " +
                $"LEFT JOIN Piattaforme pl ON p.idPiattaforma = pl.id " +
                $"WHERE p.idVideogioco = {idGioco}");

            // Itera sulle righe ottenute e aggiunge i risultati alla lista
            foreach (var riga in righe)
            {
                Piattaforma piattaforma = new Piattaforma
                {
                    Id = Convert.ToInt32(riga["idpiattaforma"]),
                    Nome = riga["nome"].ToString()
                };
                ris.Add(piattaforma);
            }

            return ris;
        }

        public bool Update(Entity e)
        {
            throw new NotImplementedException();
        }

    }
}
