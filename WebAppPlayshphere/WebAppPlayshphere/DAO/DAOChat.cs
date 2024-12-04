using Utility;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.DAO
{
    public class DAOChat : IDAO
    {
        private IDatabase db;

        private DAOChat()
        {

            db = new Database("Playsphere2", "localhost");

        }
        private static DAOChat istance = null;

        public static DAOChat GetInstance()
        {
            if (istance == null)
            {
                istance = new DAOChat();
            }
            return istance;
        }
        public bool Create(Entity e)
        {
            return db.Update($"INSERT INTO Chat (dataCreazione) VALUES ('{((Chat)e).DataCreazione}');");
        }
        public List<Entity> Read()
        {
            List<Entity> ris = new List<Entity>();
            return ris;
        }
        public bool AddPartecipante(int idchat, int idutente)
        {
            return db.Update($"INSERT INTO PartecipantiChat (idChat, idUtente) VALUES ({idchat}, {idutente});");
        }
        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Entity Find(int id)
        {
            //throw new NotImplementedException
            string query = $"SELECT * FROM Chat WHERE id = {id}";
            var righe = db.ReadOne(query);
            if (righe == null)
            {
                return null;
            }
            Entity e = new Chat();
            e.FromDictionary(righe);
            return e;
        }
        public Entity FindByUtente(int idutente)
        {
            string query = $"SELECT * FROM PartecipantiChat WHERE idUtente = {idutente}";
            var righe = db.ReadOne(query);
            if (righe == null)
            {
                return null;
            }
            Entity e = new Chat();
            ((Chat)e).Id = int.Parse(righe["idchat"]);
            string query2 = $"SELECT * FROM Chat WHERE id = {((Chat)e).Id}";
            var righe2 = db.ReadOne(query2);
            if (righe == null)
            {
                return null;
            }
            ((Chat)e).DataCreazione = DateTime.Parse(righe2["datacreazione"]);
            return e;
        }

        public List<dynamic> ReadLista()
        {
            //throw new NotImplementedException();
            string query = "SELECT * FROM Chat";
            var righe = db.Read(query);
            if (righe == null)
            {
                Console.WriteLine("ERRORE ELENCO CHAT ARRIVATO NULL");
                return null;
            }
            List<Entity> ris = new List<Entity>();
            List<dynamic> list = new List<dynamic>();
            foreach (var riga in righe)
            {
                Entity e = new Chat();
                e.FromDictionary(riga);
                Console.WriteLine($"ID CHAT : {e.Id}");
                //ris.Add(e);
                int idutente = DAOPartecipantiChat.GetInstance().FindByIdUtente(e.Id);
                Utente utente = (Utente)DAOUtente.GetInstance().Find(idutente);
                var v = new
                {
                    chat = e,
                    utente = utente
                };
                list.Add(v);
            }

            return list;
        }

        public bool Update(Entity e)
        {
            throw new NotImplementedException();
        }
    }
}
