using System.Reflection.Metadata.Ecma335;
using Utility;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.DAO
{
    public class DAOAnagrafica : IDAO
    {
        private IDatabase db;

        private DAOAnagrafica()
        {

            db = new Database("Playsphere5", "LAPTOP-ANDREA");

        }
        private static DAOAnagrafica istance = null;

        public static DAOAnagrafica GetIstance()
        {
            if (istance == null)
            {
                istance = new DAOAnagrafica();
            }
            return istance;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

       
            public bool Update(Entity e)
            {
            return db.Update(
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

        }
        
        

        public bool Create(Entity e)
        {
            throw new NotImplementedException();
        }

        public Entity Find(int id)
        {
            //throw new NotImplementedException();
            var righe = db.ReadOne($"SELECT * FROM Anagrafiche WHERE idUtente = {id}");
            if (righe == null)
            {
                return null;
            }
            Entity e = new Anagrafica();
            e.FromDictionary(righe);
            return e;
        }

        public List<Entity> Read()
        {
            throw new NotImplementedException();
        }
    }
}
