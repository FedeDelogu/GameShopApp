using Utility;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.DAO
{
    public class DAOCarrello : IDAO
    {
        private IDatabase db;

        private DAOCarrello()
        {
            db = new Database("Playsphere", "DESKTOP-S0KBKL3");
        }
        private static DAOCarrello istance = null;

        public static DAOCarrello GetIstance()
        {
            if (istance == null)
            {
                istance = new DAOCarrello();
            }
            return istance;
        }

        public bool Create(Entity e)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            return db.Update("DELETE FROM Carrelli WHERE idUtente=" + id);
        }



        public List<Entity> Read()
        {
            throw new NotImplementedException();
        }
        public Entity Find(int id)
        {
            var righe = db.Read("SELECT Carrelli.idUtente as idutente, Carrelli.Quantita as quantita, Videogiochi.* " +
                                  "FROM  Carrelli JOIN Videogiochi on Videogiochi.idVideogioco=Videogiochi.id " +
                                  "WHERE Carrelli.idUtente=" + id);
            if (righe == null)
            {
                return new Carrello(id, new Dictionary<Videogioco, int>());
            }
            int idUtente = 0;
            int quantita = 0;
            Dictionary<Videogioco, int> giochi = new Dictionary<Videogioco, int>();
            foreach (var riga in righe)
            {
                idUtente = int.Parse(riga["idutente"]);
                quantita = int.Parse(riga["quantita"]);
                riga.Remove("idutente");
                riga.Remove("quantita");
                Entity gioco = new Videogioco();
                gioco.FromDictionary(riga);
                giochi.Add((Videogioco)gioco, quantita);
            }

            Entity e = new Carrello(idUtente, giochi);
            return e;

        }
        public bool Insert(int idUtente, int idVideogioco, int quantita)
        {
            string query = "";
            if (((Videogioco)DAOVideogioco.GetIstance().Find(idVideogioco)).Quantita >= quantita + ((Carrello)Find(idUtente)).Videogiochi[(Videogioco)DAOVideogioco.GetIstance().Find(idVideogioco)])
            {
                if (db.ReadOne("SELECT * FROM Carrelli WHERE idUtente=" + idUtente + " AND idVideogioco=" + idVideogioco) == null)
                {
                    query = $"INSERT INTO Carrelli(idUtente, idVideogioco, quantita) VALUES (" +
                        $"{idUtente},{idVideogioco},{quantita})";
                }
                else
                {
                    query = $"UPDATE Carrelli SET quantita=quantita+{quantita} WHERE idUtente={idUtente} AND idVideogioco={idVideogioco}";
                }
                return db.Update(query);

            }
            else
            {
                return false;
            }

        }
        private bool Remove(int idVideogioco, int idUtente, int quantita = -1)
        {
            string query = "";
            if (quantita == -1)
            {
                query = $"DELETE FROM Carrelli WHERE idUtente={idUtente} AND idVideogioco={idVideogioco}";
            }
            else
            {
                query = $"UPDATE Carrelli SET quantita=quantita-{quantita} WHERE idUtente={idUtente} AND idVideogioco={idVideogioco}";
            }
            return db.Update(query);
        }
        public bool Update(Entity e)
        {
            throw new NotImplementedException();
        }

        public bool Ordina(Entity e)
        {
            Carrello c = (Carrello)e;
            Delete(c.Id);
            Ordine o = new Ordine(0, c.Videogiochi, "In preparazione", null, DateTime.Now);
            return DAOOrdine.GetInstance().Create(o);

        }

    }
}

