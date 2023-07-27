using AccentureWatch.Enum;
using AccentureWatch.Helper;
using AccentureWatch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Microsoft.Data.SqlClient.Server;
using Newtonsoft.Json;
using NuGet.Protocol;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace AccentureWatch.Controllers
{
    public class WatchesController : Controller
    {
        private readonly string? _WatchApiUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["WatchApiUrl"];
        private readonly string? _WatchApiUrlPut = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["WatchApiUrlPut"];


        public async Task<IActionResult> Index()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_WatchApiUrl))
                    {
                        string? apiResponse = await response.Content.ReadAsStringAsync();
                        var list = JsonConvert.DeserializeObject<List<Watch>>(apiResponse);


                        if (TempData.ContainsKey("successMessage"))
                        {
                            ViewBag.successMessage = TempData["successMessage"];
                        }

                        return View(list);
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }


        public IActionResult _Watches()
        {
            try
            {
            
                using (var httpClient = new HttpClient())
                {
                    using (var response =  httpClient.GetAsync(_WatchApiUrl).Result)
                    {
                        List<Watch> watchList;

                        string? apiResponse =  response.Content.ReadAsStringAsync().Result;
                        watchList = JsonConvert.DeserializeObject<List<Watch>>(apiResponse)!;

                        Random random = new Random();
                        watchList =  watchList.OrderBy(r => random.Next()).ToList();

                        int count = 8;
                        List<Watch> randomWatches = watchList.Take(count).ToList();

                        return View(randomWatches);
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();

        }

        [HttpPost, ActionName("Add")]

        public async Task<ActionResult> AddWatch(Watch watch)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent(watch.ItemName ?? ""), "ItemName");
                    formData.Add(new StringContent(watch.ItemNumber ?? ""), "ItemNumber");
                    formData.Add(new StringContent(watch.ShortDescription ?? ""), "ShortDescription");
                    formData.Add(new StringContent(watch.FullDescription ?? ""), "FullDescription");
                    formData.Add(new StringContent(watch.Price.ToString() ?? ""), "Price");
                    formData.Add(new StringContent(watch.Caliber ?? ""), "Caliber");
                    formData.Add(new StringContent(watch.Movement ?? ""), "Movement");
                    formData.Add(new StringContent(watch.Chronograph ?? ""), "Chronograph");
                    formData.Add(new StringContent(watch.Weight.ToString() ?? ""), "Weight");
                    formData.Add(new StringContent(watch.Height.ToString() ?? ""), "Height");
                    formData.Add(new StringContent(watch.Diameter.ToString() ?? ""), "Diameter");
                    formData.Add(new StringContent(watch.Thickness.ToString() ?? ""), "Thickness");
                    formData.Add(new StringContent(watch.Jewel.ToString() ?? ""), "Jewel");
                    formData.Add(new StringContent(watch.CaseMaterial ?? ""), "CaseMaterial");
                    formData.Add(new StringContent(watch.StrapMaterial ?? ""), "StrapMaterial");
                    formData.Add(new StringContent(watch.URL ?? ""), "URL");

                    if (watch.FormFile is not null)
                    {
                        var fileContent = new StreamContent(watch.FormFile.OpenReadStream());

                        formData.Add(fileContent, "FormFile", watch.FormFile.FileName);
                        var httpClient = new HttpClient();
                        var response = httpClient.PostAsync(_WatchApiUrl, formData).Result;
                        string? apiResponse = await response.Content.ReadAsStringAsync();

                        int nId = JsonConvert.DeserializeObject<int>(apiResponse);

                        int newId = nId;

                        if (response.IsSuccessStatusCode)
                        {
                            ViewBag.ID = newId;
                            ViewBag.successMessage = "You have added new watch successfully.";
                            ModelState.Clear();
                            return View();
                        }
                        else
                        {
                            ViewBag.Alert = AlertService.ShowAlert(Alerts.Danger, "Can't proceed request. Please try again.");
                            return View();
                        }

                    }
                    else 
                    {
                        ViewBag.Alert = AlertService.ShowAlert(Alerts.Danger, "Watch image is required.");
                        return View();
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }

        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == 0 || id == null)
                {
                    return View("Error");
                }

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_WatchApiUrl + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        Watch watch = JsonConvert.DeserializeObject<Watch>(apiResponse)!;

                        if (watch == null)
                        {
                            return View("Error");
                        }

                        return View(watch);

                    }
                }
            }
            catch (Exception)
            {

                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            try
            {
                if (id == 0)
                {
                    return View("Error");
                }

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_WatchApiUrl + id))
                    {

                        string apiResponse = await response.Content.ReadAsStringAsync();

                        Watch watch = JsonConvert.DeserializeObject<Watch>(apiResponse)!;

                        if (watch == null)
                        {
                            return View("Error");
                        }

                        return View(watch);

                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }


        [HttpPost, ActionName("Edit")]

        public ActionResult EditWatch(Watch watch)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                using (var formData = new MultipartFormDataContent())
                {

                    formData.Add(new StringContent(watch.ID.ToString() ?? ""), "ID");
                    formData.Add(new StringContent(watch.ItemName ?? ""), "ItemName");
                    formData.Add(new StringContent(watch.ItemNumber ?? ""), "ItemNumber");
                    formData.Add(new StringContent(watch.ShortDescription ?? ""), "ShortDescription");
                    formData.Add(new StringContent(watch.FullDescription ?? ""), "FullDescription");
                    formData.Add(new StringContent(watch.Price.ToString() ?? ""), "Price");
                    formData.Add(new StringContent(watch.Caliber ?? ""), "Caliber");
                    formData.Add(new StringContent(watch.Movement ?? ""), "Movement");
                    formData.Add(new StringContent(watch.Chronograph ?? ""), "Chronograph");
                    formData.Add(new StringContent(watch.Weight.ToString() ?? ""), "Weight");
                    formData.Add(new StringContent(watch.Height.ToString() ?? ""), "Height");
                    formData.Add(new StringContent(watch.Diameter.ToString() ?? ""), "Diameter");
                    formData.Add(new StringContent(watch.Thickness.ToString() ?? ""), "Thickness");
                    formData.Add(new StringContent(watch.Jewel.ToString() ?? ""), "Jewel");
                    formData.Add(new StringContent(watch.CaseMaterial ?? ""), "CaseMaterial");
                    formData.Add(new StringContent(watch.StrapMaterial ?? ""), "StrapMaterial");
                    formData.Add(new StringContent(watch.URL ?? ""), "URL");

                    if (watch.FormFile is not null)
                    {
                        var fileContent = new StreamContent(watch.FormFile.OpenReadStream());

                        formData.Add(fileContent, "FormFile", watch.FormFile.FileName);

                        var httpClient = new HttpClient();
                        var response = httpClient.PutAsync(_WatchApiUrlPut, formData).Result;


                        if (response.IsSuccessStatusCode)
                        {
                            TempData["id"] = watch.ID;
                            TempData["Success"] = "You have updated the watch successfully.";
                            return RedirectToAction("Edit", new { id = watch.ID });
                        }
                        else
                        {
                            return View("Error");
                        }
                    }
                    else
                    {
                        var httpClient = new HttpClient();
                        var response = httpClient.PutAsync(_WatchApiUrlPut, formData).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            TempData["id"] = watch.ID;
                            TempData["Success"] = "You have updated the watch successfully.";
                            return RedirectToAction("Edit", new { id = watch.ID });
                        }
                        else
                        {
                            ViewBag.Alert = AlertService.ShowAlert(Alerts.Danger, "Can't proceed request. Please try again.");
                            return View();
                        }
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    await httpClient.DeleteAsync(_WatchApiUrl + id);
                    TempData["successMessage"] = "The watch has been deleted.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
