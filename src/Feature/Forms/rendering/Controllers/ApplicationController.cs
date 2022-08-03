using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mvp.Feature.Forms.Models;
using System.Collections.Generic;

namespace Mvp.Feature.Forms.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(IConfiguration configuration, ILogger<ApplicationController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult GetApplicationLists()
        {
            if (!User.Identity.IsAuthenticated)
                return null;

            return Json(new ApplicationLists
            {
                Country = new List<Country>() { new Country { Name = "Australia", Description = "Australia", ID = System.Guid.Empty } },
                EmploymentStatus = new List<EmploymentStatus>() { new EmploymentStatus { Name = "Employed", Description = "Employed", ID = System.Guid.Empty } },
                MvpCategory = new List<MvpCategory>() { new MvpCategory { Name = "Technology", Active = true } }
            });
        }

        public IActionResult GetApplicationInfo()
        {
            return Json(new 
            {
                UserIsAuthenticated = User.Identity.IsAuthenticated
            });
        }
    }
}
