using Utility;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.DAO
{
    public class DAOVideogioco : IDAO
    {
        private IDatabase db;

        private DAOVideogioco()
        {

            db = new Database("Playsphere", "CIMO");


        }
        private static DAOVideogioco istance = null;

        public static DAOVideogioco GetIstance()
        {
            if (istance == null)
            {
                istance = new DAOVideogioco();
            }
            return istance;
        }

        public bool Create(Entity e)
        {
            Videogioco v = (Videogioco)e;
            string descrizione = v.Descrizione;
            descrizione.Replace("'", "''");

            return db.Update("INSERT INTO Videogiochi (titolo, prezzo, descrizione, rilascio, piattaforme, generi, pegi, sviluppatori, publisher, quantita)" +
                             "VALUES(" +
                             $"'{v.Titolo}'," +
                             $"{v.Prezzo}," +
                             $"'{descrizione}'," +
                             $"'{v.Rilascio.ToString("yyyy-mm-dd")}'," +
                             $"'{v.Piattaforme}'," +
                             $"'{v.Generi}'," +
                             $"{v.Pegi}," +
                             $"'{v.Sviluppatori}'," +
                             $"'{v.Publisher}'," +
                             $"{v.Quantita}");
        }

        public bool Delete(int id)
        {
            return db.Update("DELETE FROM Videogiochi WHERE id=" + id);
        }



        public List<Entity> Read()
        {
            var righe = db.Read("SELECT * FROM Videogiochi;");
            if (righe == null)
            {
                Console.WriteLine("riga nulla");
                return null;
            }
            List<Entity> ris = new List<Entity>();
            foreach (var riga in righe)
            {
                Entity e = new Videogioco();
                e.FromDictionary(riga);
                ris = DAOPiattaforma.GetIstance().FindByGioco(((Videogioco)e).Id);
                foreach (var item in ris)
                {
                    ((Videogioco)e).Piattaforme.Add()
                }
                ris.Add(e);
            }
            return ris;
        }
        public Entity Find(int id)
        {
            var righe = db.ReadOne("SELECT * FROM Videogiochi WHERE id=" + id);
            if (righe == null)
            {
                return null;
            }
            Entity e = new Videogioco();
            e.FromDictionary(righe);
            ((Videogioco)e).Recensioni = DAORecensione.GetIstance().RecensioniGioco(id);
            return e;

        }


        public bool Update(Entity e)
        {
            Videogioco v = (Videogioco)e;
            string descrizione = v.Descrizione;
            descrizione.Replace("'", "''");
            // PROBLEMA PREZZO 
            string prezzo = ((Videogioco)v).Prezzo.ToString().Replace(",", ".");
            //
            bool ris = db.Update("UPDATE Videogiochi SET " +
                             $"titolo='{v.Titolo}'," +
                             $"prezzo={prezzo}," +
                             $"descrizione='{descrizione}'," +
                             $"rilascio='{v.Rilascio.ToString("yyyy-MM-dd")}'," +
                             //$"piattaforme='{v.Piattaforme}'," +
                             $"generi='{v.Generi}'," +
                             $"pegi={v.Pegi}," +
                             $"sviluppatori='{v.Sviluppatori}'," +
                             $"publisher='{v.Publisher}'," +
                             $"quantita={v.Quantita} WHERE id={v.Id}");
            if (!ris)
            {
                Console.WriteLine("ERRORE UPDATE SULLA TABELLA VIDEOGIOCHI");
                return ris;
            }
            else
            {
                ris = db.Update("DELETE FROM PiattaformeVideogiochi WHERE idVideogioco=" + v.Id);
                if (!ris)
                {
                    Console.WriteLine("ERRORE DELETE SULLA TABELLA PiattaformeVideogiochi");
                }
                foreach(var item in v.Piattaforme)
                {
                    ris = db.Update("INSERT INTO PiattaformeVideogiochi (idVideogioco, piattaforma) VALUES (" + v.Id + ",'" + item + "')");
                    if (!ris)
                    {
                        Console.WriteLine("ERRORE INSERT SULLA TABELLA PiattaformeVideogiochi");
                    }
                }
            }
        }

    }
}