using Utility;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.DAO
{
    public class DAOAnagrafica : IDAO
    {
        private IDatabase db;

        private DAOAnagrafica()
        {


            db = new Database("Playsphere", "localhost");

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
            throw new NotImplementedException();
        }

        public bool Create(Entity e)
        {
            throw new NotImplementedException();
        }

        public Entity Find(int id)
        {
            //throw new NotImplementedException();
            var righe = db.ReadOne($"SELECT * FROM Anagrafiche WHERE id = {id}");
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
