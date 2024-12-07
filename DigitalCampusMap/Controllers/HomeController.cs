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


                try
                {
                    _context.Contactforms.Add(contact);
                    _context.SaveChanges();

                    return Content("<div style='text-align: center; margin-top: 50px;'>" +
                           "<h2>Your inquiry was submitted successfully and will be reviewed as soon as we can.</h2>" +
                           "</div>", "text/html");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while submitting contact form.");

                    return Content("<div style='text-align: center; margin-top: 50px; color: red;'>" +
                           "<h2>Something went wrong while submitting. Please try again later.</h2>" +
                           "</div>", "text/html");
                }
            }
            return Content("Good");
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
