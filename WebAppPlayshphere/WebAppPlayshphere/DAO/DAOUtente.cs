using Utility;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.DAO
{
    public class DAOUtente : IDAO
    {
        private IDatabase db;
        private DAOUtente()
        {
            db = new Database("Playsphere", "DESKTOP-IB2WLV5");
        }
        private static DAOUtente instance = null;
        public static DAOUtente GetInstance()
        {
            if (instance == null)
            {
                instance = new DAOUtente();
            }
            return instance;
        }
        public Entity Find(int id)
        {
            var riga = db.ReadOne($"SELECT Utenti.*, Anagrafiche.* FROM" +
                $" Utenti inner join Anagrafiche on Utenti.Id = Anagrafiche.idUtente where id = {id}");
            if (riga != null)
            {
                Entity e = new Utente();
                e.FromDictionary(riga);
                return e;
            }
            else return null;
        }
        public bool Create(Entity e)
        {
            return db.Update($"insert into Utenti" +
                $"(email, passwordUtente, dob, ruolo)" +
                $"values" +
                $"(" +
                $"{((Utente)e).Email.Replace("'", "''")}," +
                $"{((Utente)e).Password}," +
                $"{((Utente)e).Dob.ToString("yyyy-MM-dd")}," +
                $"{((Utente)e).Ruolo}" +
                $")");
        }
        public bool Delete(int id)
        {
            return db.Update($"Delete from utenti where id = {id}");
        }
        public bool Update(Entity e)
        {
            return db.Update($"Update Utenti set " +
                $"email = {((Utente)e).Email.Replace("'", "''")}," +
                $"passwordUtente = {((Utente)e).Password}," +
                $"dob = {((Utente)e).Dob.ToString("yyyy-MM-dd")}," +
                $"ruolo = {((Utente)e).Ruolo}" +
                $"where = {e.Id};" +
                $"update Anagrafiche set " +
                $"nome = {((Utente)e).Nome.Replace("'", "''")}," +
                $"cognome = {((Utente)e).Cognome.Replace("'", "''")}," +
                $"indirizzo = {((Utente)e).Indirizzo.Replace("'", "''")}," +
                $"telefono = {((Utente)e).Telefono}," +
                $"citta = {((Utente)e).Citta.Replace("'", "''")}," +
                $"stato = {((Utente)e).Stato.Replace("'", "''")}," +
                $"cap = {((Utente)e).Cap}," +
                $"idUtente = {e.Id} " +
                $"where = {e.Id}" +
                $";");
        }
        public List<Entity> Read()
        {
            List<Entity> ris = new();
            return ris;
        }
    }
}
