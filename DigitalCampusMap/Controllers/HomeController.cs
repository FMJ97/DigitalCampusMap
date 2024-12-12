using DigitalCampusMap.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

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

                    PrepareAndSendMail(model);

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












        public void PrepareAndSendMail(Contactform model)
        {
            var apiId = "";
            var apiSecret = "";

            
            var senderEmail = "fjukic@algebra.hr";
            var recipientEmail = model.Email;
            var subject = "Submit form confirmation"; // subject
            var bodyHtml = $"<p>Form submission - Issue number:{2}</p>"; //actual msg
            var bodyText = "Your form has been successfully submited!";

            
            var token = GetAccessToken(apiId, apiSecret);

            
            SendEmail(token, senderEmail, recipientEmail, subject, bodyHtml, bodyText);
        }






        // Get the access token synchronously
        static string GetAccessToken(string apiId, string apiSecret)
        {
            using (var client = new HttpClient())
            {
                var url = "https://api.sendpulse.com/oauth/access_token";
                var payload = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", apiId },
                { "client_secret", apiSecret }
            };

                var response = client.PostAsync(url, new FormUrlEncodedContent(payload)).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;

                // Parse the access token from the response
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
                return jsonResponse.access_token;
            }
        }

        
        static void SendEmail(string accessToken, string senderEmail, string recipientEmail, string subject, string bodyHtml, string bodyText)
        {
            using (var client = new HttpClient())
            {
                var url = "https://api.sendpulse.com/smtp/emails";

                // Prepare the email data
                var emailData = new
                {
                    email = new
                    {
                        html = Convert.ToBase64String(Encoding.UTF8.GetBytes(bodyHtml)),
                        text = bodyText,
                        subject = subject,
                        from = new
                        {
                            name = "Sender Name",
                            email = senderEmail
                        },
                        to = new[]
                        {
                        new { name = "Recipient Name", email = recipientEmail }
                    }
                    }
                };

                var json = JsonConvert.SerializeObject(emailData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                
                var response = client.PostAsync(url, content).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;

                
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Email sent successfully!");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {responseString}");
                }
            }
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
