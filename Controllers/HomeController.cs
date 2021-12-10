using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext _context;

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.allDishes = _context.Dishes.OrderByDescending(d => d.CreatedAt).ToList();
            return View();
        }

        [HttpGet("New")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost("addDish")]
        public IActionResult AddDish(Dish ndish)
        {
            if(ModelState.IsValid)
            {
                _context.Add(ndish);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View("New");
            }
        }

        [HttpGet("Edit/{dishId}")]
        public IActionResult Edit(int dishId)
        {
            Dish oneDish = _context.Dishes.SingleOrDefault(d => d.DishId == dishId);
            return View(oneDish);
        }


        [HttpPost("editDish/{dishId}")]
        public IActionResult EditDish(int dishId, Dish edited)
        {
            if(ModelState.IsValid)
            {
                Dish original = _context.Dishes.SingleOrDefault(d => d.DishId == dishId);
                original.Name = edited.Name;
                original.Chef = edited.Chef;
                original.Calories = edited.Calories;
                original.Tastiness = edited.Tastiness;
                original.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                Dish original = _context.Dishes.SingleOrDefault(d => d.DishId == dishId);
                return View("Edit", original);
            }
        }
        
        [HttpGet("dish/{dishId}")]
        public IActionResult ShowDish(int dishId)
        {
            Dish onedish = _context.Dishes.SingleOrDefault(d => d.DishId == dishId);
            return View(onedish);
        }

        [HttpGet("delete/{dishId}")]
        public IActionResult deleteOne(int dishId)
        {
            Dish onedish = _context.Dishes.SingleOrDefault(a => a.DishId == dishId);

            _context.Dishes.Remove(onedish);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }










        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
