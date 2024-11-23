using Utility;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.DAO
{
    public class DAOAnagrafica : IDAO
    {
        private readonly IDatabase db;

        public DAOAnagrafica(IDatabase database)
        {
            db = database;
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
