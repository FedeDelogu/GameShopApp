﻿using Microsoft.AspNetCore.Mvc;
using Utility;
using WebAppPlayshphere.DAO;
using WebAppPlayshphere.Models;

namespace WebAppPlayshphere.Controllers
{
    public class HomeController :Controller
    {
        public IActionResult Index()
        {
            
           return View();   
                        }
    }
}
