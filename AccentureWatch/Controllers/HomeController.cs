using AccentureWatch.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AccentureWatch.Controllers
{
    public class HomeController : Controller
    {
      
        private readonly string? _CarouselApiUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["CarouselApiUrl"];

        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_CarouselApiUrl))
                {
                    string? apiResponse = await response.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<List<Carousel>>(apiResponse);
                    return View(list);
                }
            }

        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}