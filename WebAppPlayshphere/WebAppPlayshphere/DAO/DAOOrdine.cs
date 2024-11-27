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
            db = new Database("Playsphere", "CIMO");
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
        public bool update(string stato, int id)
        {
            return db.Update($"UPDATE Ordini SET stato = '{stato}' WHERE id = {id}");
        }
            //metodi che verranno utilizzati
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
                    Console.WriteLine($"STATO ORDINE : {((Ordine)o).Stato}");

                }
                lista.Add(o);
            }
            return lista;
        }
        public List<Entity> FilterPerGiorno(DateTime data)
        {
            List<Entity> lista = new();
            var righe = db.Read($"Select * from Ordini where dataOrdine = {data.ToString("yyyy-MM-dd")}");
            foreach (var riga in righe)
            {
                Entity e = new Ordine();
                e.FromDictionary(riga);
                lista.Add(e);
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