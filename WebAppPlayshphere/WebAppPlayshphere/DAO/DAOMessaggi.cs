using Utility;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.DAO
{
    public class DAOMessaggi : IDAO
    {
        private IDatabase db;

        private DAOMessaggi()
        {

            db = new Database("Playsphere2", "localhost");

        }
        private static DAOMessaggi istance = null;

        public static DAOMessaggi GetInstance()
        {
            if (istance == null)
            {
                istance = new DAOMessaggi();
            }
            return istance;
        }
        public bool Create(Entity e)
        {
            return db.Update($"INSERT INTO Messaggi (idChat, idUtente, contenuto) VALUES ({((Messaggio)e).IdChat}, {((Messaggio)e).IdUtente}, '{((Messaggio)e).Contenuto}');");
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Entity Find(int id)
        {
            throw new NotImplementedException();
        }
        public List<Entity> FindByUtente(int idutente)
        {
            string query = $"SELECT * FROM Messaggi WHERE idUtente = {idutente}";
            var righe = db.Read(query);
            if(righe == null)
            {
                return null;
            }
            List<Entity> ris = new List<Entity>();
            foreach (var riga in righe)
            {
                Entity e = new Messaggio();
                e.FromDictionary(riga);
                ris.Add(e);
            }
            return ris;
        }

        public List<Entity> Read()
        {
            throw new NotImplementedException();
        }

        public bool Update(Entity e)
        {
            throw new NotImplementedException();
        }
    }
}
