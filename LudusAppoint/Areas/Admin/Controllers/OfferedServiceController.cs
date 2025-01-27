using Entities.Models;
using Entities.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services.Contracts;

namespace LudusAppoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OfferedServiceController : Controller
    {
        private readonly IServiceManager _serviceManager;
        private readonly RequestLocalizationOptions _localizationOptions;

        public OfferedServiceController(IServiceManager serviceManager, IOptions<RequestLocalizationOptions> localizationOptions)
        {
            _serviceManager = serviceManager;
            _localizationOptions = localizationOptions.Value;
        }
        public IActionResult Index()
        {
            var model = _serviceManager.OfferedServiceService.GetAllOfferedServices(false, System.Globalization.CultureInfo.CurrentCulture.Name);

            return View(model);
        }
        public IActionResult Create()
        {
            PopulateAgeGroups();
            PopulateSupportedCultures();
            var model = new OfferedService(); // Ensure a valid model is created
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create([FromForm] OfferedService offeredService, int[] AgeGroupIds, string[] Genders, Dictionary<string, string> Translations)
        {
            // Initialize collections
            offeredService.AgeGroups = new List<AgeGroup>();
            offeredService.Genders = new List<Gender>();

            // Associate selected age groups
            foreach (var ageGroupId in AgeGroupIds)
            {
                var ageGroup = _serviceManager.AgeGroupService.GetAgeGroup(ageGroupId, true);
                if (ageGroup != null)
                {
                    offeredService.AgeGroups.Add(ageGroup);
                }
            }

            // Associate selected genders
            foreach (var gender in Genders)
            {
                if (Enum.TryParse(typeof(Gender), gender, out var parsedGender) && parsedGender is Gender validGender)
                {
                    offeredService.Genders.Add(validGender);
                }
            }

            // Handle translations
            offeredService.OfferedServiceLocalizations = Translations
                .Select(t => new OfferedServiceLocalization
                {
                    Language = t.Key,
                    OfferedServiceLocalizationName = t.Value,
                    OfferedServiceId = offeredService.OfferedServiceId
                })
                .ToList();

            if (!ModelState.IsValid)
            {
                PopulateAgeGroups();
                PopulateSupportedCultures();
                return View(offeredService);
            }

            try
            {
                _serviceManager.OfferedServiceService.CreateofferedService(offeredService);
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception.InnerException?.Source.ToString(), exception.Message);
                }
                PopulateAgeGroups();
                PopulateSupportedCultures();
                return View(offeredService);
            }
        }

        private void PopulateSupportedCultures()
        {
            ViewBag.SupportedCultures = _localizationOptions.SupportedCultures.Select(c => c.Name).ToList();
        }

        private void PopulateAgeGroups()
        {
            var ageGroups = _serviceManager.AgeGroupService.GetAllAgeGroups(false).ToList();
            ViewBag.AllAgeGroups = ageGroups;
        }
    }
}
