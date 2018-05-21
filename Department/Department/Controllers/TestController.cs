using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Department.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index1()
        {
            return View();
        }
        public IActionResult Index2()
        {
            return View();
        }
        public IActionResult Edit1()
        {
            return View();
        }
        public IActionResult Edit2()
        {
            return View();
        }
        public IActionResult Applications()
        {
            return View();
        }
        public IActionResult Activities()
        {
            return View();
        }
        public IActionResult Apply()
        {
            return View();
        }
        public IActionResult NewMember()
        {
            return View();
        }
        public IActionResult Departs()
        {
            return View();
        }
    }
}