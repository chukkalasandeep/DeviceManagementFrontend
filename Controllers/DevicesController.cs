using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

using DeviceManagementWebsite.Models;


namespace DeviceManagementWebsite.Controllers
{
    [Authorize]
    public class DeviceController : Controller
    {
        private IConfiguration configuration;
        string apiBaseUrl = "";

        public DeviceController(IConfiguration iConfig)
        {
            configuration = iConfig;
            apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");
        }
        // GET: DevicesController
        public async Task<IActionResult> Index()
        {
            List<DeviceDTO> liData = null;
            var token = User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.UserData)?.Value;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.SetBearerToken(token.Split(" ")[1]);
                string endpoint = apiBaseUrl + "/Device";

                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = Response.Content.ReadAsStringAsync().Result;
                        liData = JsonConvert.DeserializeObject<List<DeviceDTO>>(data); //JsonSerializer.Deserialize<List<DeviceDTO>>(data);
                    }
                }
            }
            return View(liData);
        }
        // GET: DvController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            DeviceDTO model = null;
            var token = User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.UserData)?.Value;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.SetBearerToken(token.Split(" ")[1]);
                string endpoint = apiBaseUrl + "/Device/" + id;

                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = Response.Content.ReadAsStringAsync().Result;
                        var li = JsonConvert.DeserializeObject<List<DeviceDTO>>(data); //JsonSerializer.Deserialize<List<DeviceDTO>>(data);
                        if (li.Count > 0)
                            model = li[0];
                    }
                }
            }
            return View(model);
        }

        // GET: DeviceController/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: DevicesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DeviceDTO model)
        {
            try
            {
                var token = User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.UserData)?.Value;
                using (HttpClient client = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                    string endpoint = apiBaseUrl + "/Device";
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.SetBearerToken(token.Split(" ")[1]);
                    using (var Response = await client.PostAsync(endpoint, content))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string data = Response.Content.ReadAsStringAsync().Result;
                            ViewBag.RetVal = 1;
                            //return RedirectToAction("index");
                        }
                    }
                }
            }
            catch
            {
                ViewBag.RetVal = -1;
            }
            return View(model);
        }

        // GET: DevicesController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            DeviceDTO model = null;
            var token = User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.UserData)?.Value;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.SetBearerToken(token.Split(" ")[1]);
                string endpoint = apiBaseUrl + "/Device/" + id;

                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = Response.Content.ReadAsStringAsync().Result;
                        var li = JsonConvert.DeserializeObject<List<DeviceDTO>>(data); //JsonSerializer.Deserialize<List<DeviceDTO>>(data);
                        if (li.Count > 0)
                            model = li[0];
                    }
                }
            }
            return View(model);
        }

        // POST: DevicesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(DeviceDTO model)//(int id, IFormCollection collection)
        {
            try
            {
                var token = User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.UserData)?.Value;
                using (HttpClient client = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                    string endpoint = apiBaseUrl + "/Device";
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.SetBearerToken(token.Split(" ")[1]);
                    using (var Response = await client.PutAsync(endpoint, content))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string data = Response.Content.ReadAsStringAsync().Result;
                            ViewBag.RetVal = 1;
                            //return RedirectToAction("index");
                        }
                    }
                }
            }
            catch
            {
                ViewBag.RetVal = -1;
            }
            return View();
        }

        // GET: DevicesController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Int32? RetVal = null;
            try
            {
                var token = User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.UserData)?.Value;

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.SetBearerToken(token.Split(" ")[1]);
                    string endpoint = apiBaseUrl + "/Device/" + id;

                    using (var Response = await client.DeleteAsync(endpoint))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var data = Response.Content.ReadAsStringAsync().Result;
                            RetVal = 1;
                        }
                        else
                        {
                            RetVal = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RetVal = -1;
            }
            TempData["RetVal"] = RetVal;
            return RedirectToAction("index");
            //return View();
        }
    }
}
