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

            db = new Database("Playsphere", "FEDUCCINI");

        }
        private static DAOAnagrafica istance = null;

        public static DAOAnagrafica GetInstance()
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
            Console.WriteLine("sto entrando nell'update");
            if (e is Utente utente && utente.Anagrafica != null)
            {
                Console.WriteLine("sono nell'if");
                // Costruzione della query SQL con i valori da inserire
                string query = $"UPDATE Anagrafiche SET " +
                               $"nome = '{utente.Anagrafica.Nome?.Replace("'", "''")}', " +
                               $"cognome = '{utente.Anagrafica.Cognome?.Replace("'", "''")}', " +
                               $"indirizzo = '{utente.Anagrafica.Indirizzo?.Replace("'", "''")}', " +
                               $"telefono = '{utente.Anagrafica.Telefono?.Replace("'", "''")}', " +
                               $"citta = '{utente.Anagrafica.Citta?.Replace("'", "''")}', " +
                               $"stato = '{utente.Anagrafica.Stato?.Replace("'", "''")}', " +
                               $"cap = '{utente.Anagrafica.Cap}' " +
                               $"WHERE idUtente = {e.Id}";

                Console.WriteLine(query);
                // Passa la query come unico parametro al metodo Update del database
                return db.Update(query);
            }
            return false;
        }





        public bool Create(Entity e)
        {
            return db.Update($"Inserti into anagrafiche" +
                $"(nome,cognome, indirizzo, telefono, citta , stato, cap, idUtente)" +
                $"values" +
                $"(" +
                $"{((Utente)e).Anagrafica.Nome}," +
                $"{((Utente)e).Anagrafica.Cognome}," +
                $"{((Utente)e).Anagrafica.Indirizzo}," +
                $"{((Utente)e).Anagrafica.Telefono}," +
                $"{((Utente)e).Anagrafica.Citta}," +
                $"{((Utente)e).Anagrafica.Stato}," +
                $"{((Utente)e).Anagrafica.Cap}," +
                $"{((Utente)e).Id}" +
                $")");
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
