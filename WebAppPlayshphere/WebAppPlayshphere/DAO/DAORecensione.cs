using System.Xml.Linq;
using Utility;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.DAO
{
    public class DAORecensione : IDAO
    {
        private  readonly IDatabase db;
        private readonly DAOUtente _daoUtente;

        public DAORecensione(IDatabase database,DAOUtente daoUtente)
        {
            db = database;
            _daoUtente = daoUtente;
        }
        
        public bool Create(Entity e)
        {
            throw new NotImplementedException();
        }
        public bool CreateRecensione(Entity e, int idVideogioco)
        {
            Recensione r = (Recensione)e;
            string commento = r.Commento;
            commento.Replace("'", "''");

            return db.Update("INSERT INTO Recensioni (valutazione, commento, valido, idUtente, idVideogioco) VALUES (" +
                             $"{r.Valutazione}," +
                             $"'{commento}'," +
                             $"{(r.Valido ? 1 : 0)}," +
                             $"{r.Utente.Id}," +
                             $"{idVideogioco}");
        }

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
                Entity e = new Recensione();
                //e.FromDictionary(riga);
                ((Recensione)e).Utente = (Utente)_daoUtente.Find(int.Parse(riga["idutente"]));
                ((Recensione)e).Valutazione = int.Parse(riga["valutazione"]);
                ((Recensione)e).Commento = riga["commento"];
                ((Recensione)e).Valido = riga["valido"] == "1";
                ris.Add(e);
            }
            return ris;
        }
        public List<Recensione> RecensioniGioco(int id)
        {
            var righe = db.Read("SELECT * FROM Recensioni WHERE idVideogioco=" + id);
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
            var righe = db.ReadOne("SELECT * FROM Recensioni WHERE id=" + id);
            if (righe == null)
            {
                return null;
            }
            Entity e = new Recensione();
            e.FromDictionary(righe);
            return e;

        }

        public bool Update(Entity e)
        {
            throw new NotImplementedException();
        }
        public bool UpdateRecensione(Entity e, int idVideogioco)
        {
            Recensione r = (Recensione)e;
            string commento = r.Commento;
            commento.Replace("'", "''");
            return db.Update("UPDATE Recensioni SET " +
                            $"valutazione={r.Valutazione}," +
                            $"commento='{commento}'," +
                            $"valido={(r.Valido ? 1 : 0)}," +
                            $"idUtente={r.Utente.Id}," +
                            $"idVideogioco={idVideogioco} WHERE id={r.Id}");
        }

    }
}

