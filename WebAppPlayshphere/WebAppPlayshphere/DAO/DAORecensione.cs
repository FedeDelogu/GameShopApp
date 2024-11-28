using System.Xml.Linq;
using Utility;
using WebAppPlayshphere.Factory;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.DAO

{

    public class DAORecensione : IDAO
    {
        private IDatabase db;

        private DAORecensione()
        {


            db = new Database("Playsphere5", "LAPTOP-ANDREA");



        }
        private static DAORecensione istance = null;

        public static DAORecensione GetIstance()
        {
            if (istance == null)
            {
                istance = new DAORecensione();
            }
            return istance;
        }
        public bool Create(Entity e)
        {
            Recensione r = (Recensione)e;
            string commento = r.Commento;
            commento.Replace("'", "''");

            return db.Update("INSERT INTO Recensioni (valutazione, commento, valido, idUtente, idVideogioco) VALUES (" +
                             $"{r.Valutazione}," +
                             $"'{commento}'," +
                             $"{(r.Valido ? 1 : 0)}," +
                             $"{r.Utente.Id}," +
                             $"{r.IdVideogioco});");
        } // OK

        public bool Delete(int id)
        {
            return db.Update("DELETE FROM Recensioni WHERE id=" + id);
        }

        public List<Entity> Read()
        {
            var righe = db.Read("SELECT * FROM Recensioni");
            if (righe == null)
            {
                Console.WriteLine("riga nulla");
                return null;
            }
            List<Entity> ris = new List<Entity>();
            foreach (var riga in righe)
            {
                Console.WriteLine($"ID GIOCO : {riga["idvideogioco"]}");
                Entity e = RecensioneFactory.CreateRecensione(riga);
                ris.Add(e);
            }
            //foreach (var rec in ris)
            //    Console.WriteLine(rec.ToString());
            return ris;
        } // OK
        public List<Entity> RecensioniGioco(int id)
        {
            var righe = db.Read("SELECT * FROM Recensioni WHERE idVideogioco=" + id);
            if (righe == null)
            {
                return null;
            }
            List<Entity> ris = new List<Entity>();
            foreach (var riga in righe)
            {
                Recensione r = RecensioneFactory.CreateRecensione(riga);
                //r.FromDictionary(riga);
                ris.Add(r);
            }
            
            return ris;
        }
        public List<Recensione> RecensioniUtente(int id)
        {
            var righe = db.Read("SELECT * FROM Recensioni WHERE idUtente=" + id);
            if (righe == null)
            {
                return null;
            }
            List<Recensione> ris = new List<Recensione>();
            foreach (var riga in righe)
            {
                Recensione r = new Recensione();
                r.FromDictionary(riga);
                ris.Add(r);
            }
            return ris;
        }
        public Entity Find(int id)
        {
            var riga = db.ReadOne("SELECT * FROM Recensioni WHERE id=" + id);
            if (riga == null)
            {
                return null;
            }
            Entity e = RecensioneFactory.CreateRecensione(riga);
            return e;

        } // OK

        public bool Update(Entity e)
        {
            Recensione r = (Recensione)e;
            return db.Update("UPDATE Recensioni SET " +
                            $"valutazione={r.Valutazione}," +
                            $"commento='{r.Commento}'," +
                            $"valido={(r.Valido ? 1 : 0)}," +
                            $"idUtente={r.Utente.Id}," +
                            $"idVideogioco={r.IdVideogioco} WHERE id={r.Id}");
        } // OK

        public List<Entity> FindBy(string colonna, string valore)
        {
            string query = "";
            switch (colonna)
            {
                case "id":
                    query = $"SELECT * FROM Recensioni WHERE id = {valore};";
                    break;
                case "valutazione":
                    query = $"SELECT * FROM Recensioni WHERE valutazione = {valore};";
                    break;
                case "utente":
                    query = $"SELECT * FROM Recensioni WHERE idUtente = {valore};";
                    break;
                case "videogioco":

                    query = $"SELECT * FROM Recensioni WHERE idVideogioco = {valore};";
                    break;
                default:
                    return null;
            }
            var righe = db.Read(query);
            if (righe == null)
            {
                return null;
            }
            List<Entity> ris = new List<Entity>();
            foreach (var riga in righe)
            {
                Recensione r = RecensioneFactory.CreateRecensione(riga);
                ris.Add(r);
            }
            if(ris.Count == 0)
            {
                Console.WriteLine("nessuna recensione trovata");
            }
            return ris;
        }

        /* RISERVATO ALL'ADMIN */

        public bool Approve(int id) // APPROVA RECENSIONE
        {

            var riga = db.Update($"UPDATE Recensioni SET valido = 1 WHERE id = " + id);
            if (riga)
                return true;

            return false;
        }
        
        public bool Disapprove(int id) // APPROVA RECENSIONE
        {

            var riga = db.Update($"UPDATE Recensioni SET valido = 0 WHERE id = " + id);
            if (riga)
                return true;

            return false;
        }
    }
}