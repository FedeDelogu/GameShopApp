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
            bool ris = false;
            Videogioco v = (Videogioco)e;
            string descrizione = v.Descrizione;
            descrizione.Replace("'", "''");

            ris = db.Update("INSERT INTO Videogiochi (titolo, prezzo, descrizione, rilascio, generi, pegi, sviluppatori, publisher, quantita)" +
                             "VALUES(" +
                             $"'{v.Titolo}'," +
                             $"{v.Prezzo}," +
                             $"'{descrizione}'," +
                             $"'{v.Rilascio.ToString("yyyy-mm-dd")}'," +
                             $"'{v.Generi}'," +
                             $"{v.Pegi}," +
                             $"'{v.Sviluppatori}'," +
                             $"'{v.Publisher}'," +
                             $"{v.Quantita}");
            if (!ris)
            {
                Console.WriteLine("ERRORE UPDATE SULLA TABELLA VIDEOGIOCHI");
                return ris;
            }

            ris = db.Update("DELETE FROM PiattaformeVideogiochi WHERE idVideogioco=" + v.Id);
            if (!ris)
            {
                Console.WriteLine("ERRORE DELETE SULLA TABELLA PiattaformeVideogiochi");
            }
            foreach (var item in v.Piattaforme)
            {
                ris = db.Update("INSERT INTO PiattaformeVideogiochi (idVideogioco, idPiattaforma) VALUES (" + v.Id + ",'" + item.Id + "')");
                if (!ris)
                {
                    Console.WriteLine("ERRORE INSERT SULLA TABELLA PiattaformeVideogiochi");
                    return ris;
                }
            }
            return ris;
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
                // recupero tutte le piattaforme del gioco
                List<Entity> piattaforme = DAOPiattaforma.GetIstance().FindByGioco(((Videogioco)e).Id);
                if(DAOPiattaforma.GetIstance().FindByGioco(((Videogioco)e).Id) == null)
                {
                    Console.WriteLine("ERRORE RECUPERO PIATTAFORME");
                }
                // per ogni piattaforma trovata la aggiungo alla lista delle piattaforme del gioco
                foreach (var item in piattaforme)
                {
                    ((Videogioco)e).Piattaforme.Add(
                        new Piattaforma()
                        {
                            Id = ((Piattaforma)item).Id,
                            Nome = ((Piattaforma)item).Nome
                        }
                        );
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
            // recupero tutte le piattaforme del gioco
            var ris = DAOPiattaforma.GetIstance().FindByGioco(((Videogioco)e).Id);
            // per ogni piattaforma trovata la aggiungo alla lista delle piattaforme del gioco
            foreach (var item in ris) {
                ((Videogioco)e).Piattaforme.Add(
                    new Piattaforma()
                    {
                        Id = ((Piattaforma)item).Id,
                        Nome = ((Piattaforma)item).Nome
                    }
                    );
            }
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
                /*ris = db.Update($"DELETE FROM PiattaformeVideogiochi WHERE idVideogioco = {v.Id}");//+ v.Id
                if (!ris)
                {
                    Console.WriteLine("ERRORE DELETE SULLA TABELLA PiattaformeVideogiochi");
                }*/
                foreach(var item in v.Piattaforme)
                {
                    ris = db.Update("INSERT INTO PiattaformeVideogiochi (idVideogioco, idPiattaforma) VALUES (" + v.Id + ",'" + item.Id + "')");
                    if (!ris)
                    {
                        Console.WriteLine("ERRORE INSERT SULLA TABELLA PiattaformeVideogiochi");
                        return ris;
                    }
                }
            }
            return ris;
        }

    }
}