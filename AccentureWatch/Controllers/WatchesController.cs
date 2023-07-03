using AccentureWatch.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AccentureWatch.Controllers
{
    public class WatchesController : Controller
    {


        public async Task<IActionResult> Index()
        {
            string? _GetWatchApiUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["GetWatchApiUrl"];

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_GetWatchApiUrl))
                {
                    string? apiResponse = await response.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<List<Watch>>(apiResponse);
                    return View(list);
                }
            }

        }

        [HttpGet]
        public IActionResult Add(Watch watch)
        {
            return View();

        }

        [HttpPost, ActionName("Add")]
        public IActionResult AddWatch(Watch watch)
        {
            return View();

        }

    }
}
