using Utility;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.DAO
{
    public class DAOUtente : IDAO
    {
        private IDatabase db;

        //private DAOUtente()
        //{
        //    db = new Database("Playsphere", "FEDUCCINI");
        //}

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
                // QUI RECUPERO L ANAGRAFICA
                Entity anagrafica = DAOAnagrafica.GetIstance().Find(id);
                if (anagrafica != null) // SE L ANAGRAFICA ESISTE LA ASSEGNO ALLA PROPRIETA' ANAGRAFICA DELL UTENTE
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
                $"(email, passwordUtente,ruolo)" +
                $"values" +
                $"(" +
                $"'{((Utente)e).Email.Replace("'", "''")}'," +
                $"HASHBYTES('SHA2_512','{((Utente)e).Password}'),"+
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

                $"passwordUtente = HASHBYTES('SHA2_512','{((Utente)e).Password}');" +
                $"passwordUtente = {((Utente)e).Password}," +
                $"ruolo = {((Utente)e).Ruolo}" +
                $"where = {e.Id};" +
                $"update Anagrafiche set " +
                $"nome = {(((Utente)e).Anagrafica != null ? ((Utente)e).Anagrafica.Nome.Replace("'", "''") : "null")}," +
                $"cognome = {(((Utente)e).Anagrafica != null ? ((Utente)e).Anagrafica.Cognome.Replace("'", "''") : "null")}," +
                $"indirizzo = {(((Utente)e).Anagrafica != null ? ((Utente)e).Anagrafica.Indirizzo.Replace("'", "''") : "null")}," +
                $"telefono = {(((Utente)e).Anagrafica != null ? ((Utente)e).Anagrafica.Telefono.Replace("'", "''") : "null")}," +
                $"citta = {(((Utente)e).Anagrafica != null ? ((Utente)e).Anagrafica.Citta.Replace("'", "''") : "null")}," +
                $"stato = {(((Utente)e).Anagrafica != null ? ((Utente)e).Anagrafica.Stato.Replace("'", "''") : "null")}," +
                $"cap = {(((Utente)e).Anagrafica != null ? ((Utente)e).Anagrafica.Cap.Replace("'", "''") : "null")}," +
                $"idUtente = {e.Id} " +
                $"where idUtente = {e.Id}" +
                $";");
            return false;
        }
        public List<Entity> Read()
        {
            List<Entity> ris = new();
            return ris;
        }
        public bool Find(string user, string password)
        {
            var riga = db.ReadOne($"SELECT * FROM Utenti " +
                                   $"WHERE " +
                                   $"email = '{user}' AND " +
                                   $"passwordUtente = HASHBYTES('SHA2_512','{password}');");

            return riga != null;
        }
        public Entity Find(string user)
        {
            var riga = db.ReadOne($"SELECT * FROM Utenti WHERE email = '{user}';");

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