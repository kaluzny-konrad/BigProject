using BigProject.Data;
using BigProject.Models;
using BigProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BigProject.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService mailService;
        private readonly IDutchRepository repository;

        public AppController(IMailService mailService, IDutchRepository repository)
        {
            this.mailService = mailService;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactModel model)
        {
            if(ModelState.IsValid)
            {
                mailService.SendMessage("konrad.kaluzny@hotmail.com", model.Subject, 
                    $"From: {model.Name} - {model.Email}, Message: {model.Message}");
                ViewBag.UserMessage = "Mail Sent";
                ModelState.Clear();
            }
            return View();
        }

        [HttpGet("about")]
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Shop()
        {
            var results = repository.GetAllProducts();
            return View(results);
        }
    }
}
