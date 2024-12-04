using Utility;
using WebAppPlayshphere.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WebAppPlayshphere.Settings;

namespace WebAppPlayshphere.DAO
{
    public class DAOCarrello : IDAO
    {
        private IDatabase db;

        private DAOCarrello()
        {

            db = new Database("Playsphere2", "localhost");

        }
        private static DAOCarrello istance = null;

        public static DAOCarrello GetIstance()
        {
            if (istance == null)
            {
                istance = new DAOCarrello();
            }
            return istance;
        }

        public bool Create(Entity e)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            return db.Update("DELETE FROM Carrelli WHERE idUtente=" + id);
        }



        public List<Entity> Read()
        {
            throw new NotImplementedException();
        }
        public Entity Find(int id)
        {
            string query = "SELECT Videogiochi.* " +
                           "FROM Carrelli JOIN Videogiochi on Carrelli.idVideogioco=Videogiochi.id " +
                           $"WHERE Carrelli.idUtente={id}";
          
            var righe = db.Read(query);
            var righe2 = db.Read("SELECT Quantita, idPiattaforma FROM Carrelli WHERE idUtente="+id);
            
            if (righe == null || righe.Count == 0)
            {
                return new Carrello(id, new Dictionary<Videogioco, int>(), new Dictionary<Videogioco, int>());
            }

            int idUtente = id;
            int quantita = 0;
            int idPiattaforma = 0;
            Dictionary<Videogioco, int> giochi = new Dictionary<Videogioco, int>();
            Dictionary<Videogioco, int> piattaforme = new Dictionary<Videogioco, int>();
            int count = 0;
            foreach (var riga in righe)
            {
                Console.WriteLine("Riga: " + string.Join(", ", riga.Select(kvp => $"{kvp.Key}: {kvp.Value}")));

                /*idUtente = riga.ContainsKey("idutente") ? int.Parse(riga["idutente"] ?? "0") : 0;*/
                quantita = int.Parse(righe2[count]["quantita"]);
                idPiattaforma = int.Parse(righe2[count]["idpiattaforma"]);
                count++;

                
                
                Entity gioco = new Videogioco();
                gioco.FromDictionary(riga);
               

                giochi.Add((Videogioco)gioco, quantita);
                piattaforme.Add((Videogioco)gioco, idPiattaforma);
                //giochi.Add((Videogioco)gioco, 10);
            }

            Entity carrello = new Carrello(idUtente, giochi, piattaforme);
            //Entity carrello = new Carrello(2, giochi);
            Console.WriteLine("Carrello costruito correttamente.");
            return carrello;
        }

        public bool Insert(int idUtente, int idVideogioco, int quantita, int idPiattaforma)
        {
            Carrello carrello = (Carrello)Find(idUtente);
            
            Videogioco videogioco = (Videogioco)DAOVideogioco.GetIstance().Find(idVideogioco);
            
            int quantitaInCarrello = 0;
           
            if (carrello.Videogiochi.ContainsKey(videogioco))
            {
                quantitaInCarrello = carrello.Videogiochi[videogioco];
            }

            string query = "";
            if (videogioco.Quantita < quantita + quantitaInCarrello)
            {
                Console.WriteLine($"Quantità insufficiente per il videogioco con ID {idVideogioco}. " +
                                  $"Disponibile: {videogioco.Quantita}, richiesta: {quantita + quantitaInCarrello}");
                return false;
            }
            if (db.ReadOne("SELECT * FROM Carrelli WHERE idUtente=" + idUtente + " AND idVideogioco=" + idVideogioco+" AND idPiattaforma="+idPiattaforma) == null)
                {
                    query = $"INSERT INTO Carrelli(idUtente, idVideogioco, quantita, idPiattaforma) VALUES (" +
                        $"{idUtente},{idVideogioco},{quantita}, {idPiattaforma})";
                }
                else
                {
                    query = $"UPDATE Carrelli SET quantita=quantita+{quantita} WHERE idUtente={idUtente} AND idVideogioco={idVideogioco} AND idPiattaforma={idPiattaforma}";
                }
                return db.Update(query);

            

        }
       
        public bool Remove(int idVideogioco, int idUtente, int idPiattaforma, int quantita = -1)
        {
            string query = "";
            if (quantita == -1)
            {
                query = $"DELETE FROM Carrelli WHERE idUtente={idUtente} AND idVideogioco={idVideogioco}  AND idPiattaforma={idPiattaforma}";
            }
            else
            {
                query = $"UPDATE Carrelli SET quantita=quantita-{quantita} WHERE idUtente={idUtente} AND idVideogioco={idVideogioco} AND idPiattaforma={idPiattaforma}";
            }
            return db.Update(query);
        }
        public bool Update(Entity e)
        {
            throw new NotImplementedException();
        }



        public Ordine Ordina(Entity e)
        {
            Carrello c = (Carrello)e;
            Console.WriteLine("ID DEL FARMER: "+e.Id);
            Delete(c.Id);
            Utente u = ((Utente)DAOUtente.GetInstance().Find(e.Id));
            Console.WriteLine(u.ToString());
            Ordine o = new Ordine(0, c.Videogiochi, "In preparazione", u, DateTime.Now);
            DAOOrdine.GetInstance().Create(o);
            Console.WriteLine("ORDINE TROVATO: "+((Ordine)DAOOrdine.GetInstance().Read().Last()).ToString());
            return (Ordine)DAOOrdine.GetInstance().Read().Last();

        }

    }
}
