using Utility;

namespace WebAppPlayshphere.Models
{
    public class Chat : Entity
    {
        public int IdUtente { get; set; }
        public DateTime DataCreazione { get; set; }
        public List<Messaggio> Messaggi { get; set; }

        public int Notifica { get; set; }
        public Chat()
        {
            Messaggi = new List<Messaggio>();  
        }
    }
}
