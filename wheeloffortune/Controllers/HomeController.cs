using System.Collections.Generic;
using System.Web.Mvc;
using wheeloffortune.Library;
using wheeloffortune.Models;
using Engineer = wheeloffortune.Library.Model.Engineer;

namespace wheeloffortune.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShiftsService _service;
        private IList<Shift>Shifts => _service.GetShiftsViewData();
        public HomeController()
        {
            var engine = new Engine(2,10,10);
            var shiftStorage = new StorageBase<int[,]>("WheelsoffortuneShifts.json");
            var engineerStorage = new StorageBase<IList<Engineer>>("WheelsoffortuneEngineers.json");
            _service = new ShiftsService(engine,shiftStorage,engineerStorage);
        }
        public ActionResult Index()
        {
            var shifts = _service.GetShiftsViewData();
            return View(shifts);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Application description page.";

            return View();
        }

        public ActionResult Engineers()
        {
            ViewBag.Message = "Engineers";
            
            return View(_service.Engineers);
        }

   
        public ActionResult GenerateNewSchedule(string date)
        {
            _service.GenerateNewSchedule();
            
            return PartialView("Shifts",Shifts);
        }

        public ActionResult SaveSchedule(string date)
        {
            _service.SaveSchedule();

            return PartialView("Shifts", Shifts);
        }

        public ActionResult LoadSchedule(string date)
        {
            _service.LoadSchedule();

            return PartialView("Shifts", Shifts);
        }
    }
}