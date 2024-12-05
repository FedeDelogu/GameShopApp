using Utility;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.DAO
{
    public class DAOPartecipantiChat : IDAO
    {
        private IDatabase db;

        private DAOPartecipantiChat()
        {

            db = new Database("Playsphere2", "localhost");

        }
        private static DAOPartecipantiChat istance = null;

        public static DAOPartecipantiChat GetInstance()
        {
            if (istance == null)
            {
                istance = new DAOPartecipantiChat();
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
            throw new NotImplementedException();
        }
        public Entity FindByUtente(int idutente)
        {
            string query = $"SELECT * FROM PartecipantiChat WHERE idUtente = {idutente}";
            var righe = db.Read(query);
            if (righe == null)
            {
                return null;
            }
            Entity e = new Chat();
            ((Chat)e).Id = int.Parse(righe[0]["idchat"]);
            ((Chat)e).DataCreazione = DateTime.Parse(righe[0]["datacreazione"]);
            return e;
        }
        public int FindByIdUtente(int idchat)
        {
            Console.WriteLine($"ID UTENTE NEL METODO FIND BY ID CHAT: {idchat}");
            string query = $"SELECT * FROM PartecipantiChat WHERE idChat = {idchat}";
            var righe = db.ReadOne(query);
            if (righe == null)
            {
                return 0;
            }
            int id = int.Parse(righe["idutente"]);
            Console.WriteLine($"ID UTENTE NEL METODO FIND BY ID UTENTE: {id}");
            return id;
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
