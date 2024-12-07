using DigitalCampusMap.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DigitalCampusMap.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        private readonly DigitalCampusMapContext _context;

        public HomeController(DigitalCampusMapContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }
       

        [HttpPost]
        public IActionResult SubmitContactForm(Contactform model)
        {
            if (ModelState.IsValid)
            {
                var contact = new Contactform
                {
                    Email = model.Email,
                    Subject = model.Subject,
                    Description = model.Description,
                    CreatedAt = DateTime.Now 
                };

                _context.Contactforms.Add(contact);
                _context.SaveChanges();  

                return Content("Contact form submitted successfully!");
            }

            return View(model);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
