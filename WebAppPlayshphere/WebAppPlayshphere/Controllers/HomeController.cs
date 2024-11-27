using Microsoft.AspNetCore.Mvc;
using Utility;
using WebAppPlayshphere.DAO;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.Controllers
{
    public class HomeController :Controller
    {
        public IActionResult Index(int id)
        {
            Entity e = (Utente)DAOUtente.GetInstance().Find(id);
           return View((Utente)e);   
                        }
    }
}
