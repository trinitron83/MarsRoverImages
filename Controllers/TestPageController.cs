using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MarsRoverImages.Controllers
{
    [Route("api")]
    public class TestPageController : Controller
    {
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}