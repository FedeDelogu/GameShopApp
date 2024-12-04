using Utility;

namespace WebAppPlayshphere.Models
{
    public class Messaggio : Entity
    {
        /*
         * idChat INT NOT NULL,
           idUtente INT NOT NULL, -- UserId del mittente
           Contenuto TEXT NOT NULL,
         */

        public int IdUtente { get; set; }
        public int IdChat { get; set; }
        public string Contenuto { get; set; }
    }
}
