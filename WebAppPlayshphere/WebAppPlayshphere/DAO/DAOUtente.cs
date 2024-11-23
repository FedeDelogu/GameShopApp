using Utility;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.DAO
{
    public class DAOUtente : IDAO
    {
        private IDatabase db;
        private DAOUtente()
        {
            db = new Database("Playsphere", "CIMO");
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
            var riga = db.ReadOne($"SELECT * FROM Utenti WHERE id = {id}");
            if (riga != null && riga.Count > 0)
            {
                Entity e = new Utente();
                //e.FromDictionary(riga);
                Console.WriteLine(
                    $"{riga["email"]}\n")
                    ;
                ((Utente)e).Email = riga["email"];
                ((Utente)e).Password = riga["passwordutente"];
                ((Utente)e).Ruolo = int.Parse(riga["ruolo"]);
                Entity anagrafica = DAOAnagrafica.GetIstance().Find(id);
                if(anagrafica != null)
                {
                    ((Utente)e).Anagrafica = (Anagrafica)anagrafica;
                }
                return e;
            }
            else
                Console.WriteLine("utente null");
                return null;
        }
        public bool Create(Entity e)
        {
            return db.Update($"insert into Utenti" +
                $"(email, passwordUtente, dob, ruolo)" +
                $"values" +
                $"(" +
                $"{((Utente)e).Email.Replace("'", "''")}," +
                $"{((Utente)e).Password}," +
                $"{((Utente)e).Ruolo}" +
                $")");
        }
        public bool Delete(int id)
        {
            return db.Update($"Delete from utenti where id = {id}");
        }
        public bool Update(Entity e)
        {
            /*return db.Update($"Update Utenti set " +
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
                $";");*/
            return false;
        }
        public List<Entity> Read()
        {
            List<Entity> ris = new();
            return ris;
        }
        public bool Find(string user, string password)
        {
            var riga = db.ReadOne($"SELECT * FROM Logins " +
                                   $"WHERE " +
                                   $"username = '{user}' AND " +
                                   $"passw = HASHBYTES('SHA2_512','{password}');");

            return riga != null;
        }
        public Entity Find(string user)
        {
            var riga = db.ReadOne($"SELECT * FROM Logins WHERE username = '{user}';");

            if (riga != null)
            {
                Entity e = new Utente();
                e.FromDictionary(riga);

                return e;
            }
            else
                return null;
        }

    }
}
