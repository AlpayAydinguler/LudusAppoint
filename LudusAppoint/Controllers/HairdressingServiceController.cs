using Entities.Models;
using Entities.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Repositories.Contracts;

namespace LudusAppoint.Controllers
{
    public class offeredServiceController : Controller
    {
        private readonly IStringLocalizer<OfferedService> _localizer;

        public offeredServiceController(IStringLocalizer<OfferedService> localizer, IRepositoryManager repositoryManager)
        {
            _localizer = localizer;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IEnumerable<OfferedService> GetAll()
        {
            var ag1 = new AgeGroup { AgeGroupId = 1, MinAge = 0, MaxAge = 18 };
            var ag2 = new AgeGroup { AgeGroupId = 2, MinAge = 18, MaxAge = 75 };
            var ag = new List<AgeGroup>();
            ag.Add(ag1);
            ag.Add(ag2);

            var ge = new List<Gender>();
            ge.Add(Gender.m);
            ge.Add(Gender.f);

            List<OfferedService> dummy = new List<OfferedService>();
            dummy.Add(new OfferedService { OfferedServiceId = 1, OfferedServiceName = "HairCut", Genders = ge, AgeGroups = ag, ApproximateDuration = new TimeSpan(0, 40, 0) });
            dummy.Add(new OfferedService { OfferedServiceId = 2, OfferedServiceName = "RazorShave", Genders = ge, AgeGroups = ag, ApproximateDuration = new TimeSpan(0, 20, 0) });

            foreach (var ditem in dummy)
            {
                ditem.OfferedServiceName = _localizer[ditem.OfferedServiceName];
            }
            return dummy;
        }

    }
}
