﻿using Microsoft.AspNetCore.Mvc;

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
