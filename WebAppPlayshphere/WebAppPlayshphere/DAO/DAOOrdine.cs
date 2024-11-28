using System.Diagnostics.Contracts;
using Utility;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.DAO
{
    public class DAOOrdine : IDAO
    {
        private IDatabase db;
        private DAOOrdine()
        {



            db = new Database("Playsphere", "localhost");



        }
        private static DAOOrdine instance = null;
        public static DAOOrdine GetInstance()
        {
            if (instance == null)
            {
                instance = new DAOOrdine();
            }
            return instance;
        }
        //metodi da non usare
        public bool Create(Entity e)
        {
            Console.WriteLine("HIIII"+((Ordine)e).Utente.Id);
            // se l'ordine è stato creato correttamente deve avere almeno un videogioco altrimenti non eseguo l insert
            if (((Ordine)e).Videogiochi.Count == 0)
            {
                Console.WriteLine("ERRORE ORDINE SENZA VIDEOGIOCHI");
                return false;
            }
            // inserisco l'ordine
            bool risultatoQuery = 
                db.Update($"INSERT INTO Ordini (stato, idUtente) VALUES( " +
                          $"'{((Ordine)e).Stato}', {((Ordine)e).Utente.Id});");
            if (!risultatoQuery)
            {
                Console.WriteLine("ERRORE INSERT INTO SULLA TABELLA ORDINI");
                return risultatoQuery;
            }
            // inserisco i dettagli dell'ordine
            foreach(var gioco in ((Ordine)e).Videogiochi)
            {
                risultatoQuery = 
                    db.Update(
                        $"INSERT INTO DettagliOrdini(quantitaTotale, idVideogioco, idOrdine)VALUES(" +
                        $"{gioco.Value}, {gioco.Key.Id}, {e.Id}");
                if (!risultatoQuery)
                {
                    Console.WriteLine("ERRORE INSERT INTO SULLA TABELLA DettagliOrdini");
                    return risultatoQuery;
                }
            }

            return true;
        }
        public bool Delete(int id)
        {
            return true;
        }
        public bool Update(Entity e)
        {
            return true;
        }
        public bool Update(string stato, int id)
        {
            return db.Update($"UPDATE Ordini SET stato = '{stato}' WHERE id = {id}");
        }
        public Entity Find(int id)
        {
            var riga = db.ReadOne($"SELECT * FROM Ordini where id = {id}");
            if (riga != null)
            {
                Entity e = new Ordine();
                e.FromDictionary(riga);
                return e;
            }
            else return null;
        }
        public List<Entity>FindByUtente(int id)
        {
            List<Entity> lista = new List<Entity>();
            var righe = db.Read($"SELECT * FROM Ordini WHERE idUtente = {id}");
            if (righe == null)
            {
                Console.WriteLine("Errore metodo read tabella Ordini");
                return null;
            }
            foreach (var riga in righe)
            {
                Entity o = new Ordine();
                o.FromDictionary(riga);
                // AGGIUNGERE LA LISTA DI VIDEOGIOCHI DELL ORDINE
                // devo fare una select * from DettagliOrdine where idOrdine = o.Id
                var righeDettagli = db.Read($"select * from DettagliOrdini where idOrdine = {o.Id}");
                foreach (var rigaDettaglio in righeDettagli)
                {
                    Entity v = DAOVideogioco.GetIstance().Find(int.Parse(rigaDettaglio["idvideogioco"]));
                    int quantita = int.Parse(rigaDettaglio["quantitatotale"]);
                    ((Ordine)o).Videogiochi.Add((Videogioco)v, quantita);

                }
                lista.Add(o);
            }
            return lista;
        }

        public List<Ordine> OrdiniUtente(int id)
        {
            List<Ordine> lista = new List<Ordine>();
            var righe = db.Read($"SELECT * FROM Ordini WHERE idUtente = {id}");
            if (righe == null)
            {
                Console.WriteLine("Errore metodo read tabella Ordini");
                return null;
            }
            foreach (var riga in righe)
            {
                Ordine o = new Ordine();
                o.FromDictionary(riga);
                // AGGIUNGERE LA LISTA DI VIDEOGIOCHI DELL ORDINE
                // devo fare una select * from DettagliOrdine where idOrdine = o.Id
                var righeDettagli = db.Read($"select * from DettagliOrdini where idOrdine = {o.Id}");
                foreach (var rigaDettaglio in righeDettagli)
                {
                    Entity v = DAOVideogioco.GetIstance().Find(int.Parse(rigaDettaglio["idvideogioco"]));
                    int quantita = int.Parse(rigaDettaglio["quantitatotale"]);
                    o.Videogiochi.Add((Videogioco)v, quantita);

                }
                lista.Add(o);
            }
            return lista;
        }

        public List<Entity> Read()
        {
            List<Entity> lista = new List<Entity>();
            var righe = db.Read($"select * from Ordini");
            if(righe == null)
            {
                Console.WriteLine("Errore metodo read tabella Ordini");
                return null;
            }
            foreach (var riga in righe)
            {
                Entity o = new Ordine();
                o.FromDictionary(riga);
                // AGGIUNGERE LA LISTA DI VIDEOGIOCHI DELL ORDINE
                // devo fare una select * from DettagliOrdine where idOrdine = o.Id
                var righeDettagli = db.Read($"select * from DettagliOrdini where idOrdine = {o.Id}");
                foreach (var rigaDettaglio in righeDettagli)
                {
                    Entity v = DAOVideogioco.GetIstance().Find(int.Parse(rigaDettaglio["idvideogioco"]));
                    int quantita = int.Parse(rigaDettaglio["quantitatotale"]);
                    ((Ordine)o).Videogiochi.Add((Videogioco)v, quantita);

                }
                lista.Add(o);
            }
            return lista;
        }
        public List<Entity> FindByData(string data)
        {
            List<Entity> lista = new();
            DateTime d = DateTime.Parse(data);
            var righe = db.Read($"Select * from Ordini where dataOrdine = '{d.ToString("yyyy-MM-dd")}'");
            foreach (var riga in righe)
            {
                Entity ordine = new Ordine();
                ordine.FromDictionary(riga);
                var righeDettagli = db.Read($"select * from DettagliOrdini where idOrdine = {ordine.Id}");
                foreach (var rigaDettaglio in righeDettagli)
                {
                    Entity v = DAOVideogioco.GetIstance().Find(int.Parse(rigaDettaglio["idvideogioco"]));
                    int quantita = int.Parse(rigaDettaglio["quantitatotale"]);
                    ((Ordine)ordine).Videogiochi.Add((Videogioco)v, quantita);

                }
                lista.Add(ordine);
            }
            return lista;
        }
        public List<Entity> FilterPerVidegioco(Videogioco vg)
        {
            List<Entity> lista = new();
            var righe = db.Read($"Select * from Ordini where idvideogioco = {vg.Id}");
            foreach (var riga in righe)
            {
                Entity e = new Ordine();
                e.FromDictionary(riga);
                lista.Add(e);
            }
            return lista;
        }
    }
}