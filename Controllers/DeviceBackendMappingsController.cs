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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

using DeviceManagementWebsite.Models;

namespace DeviceManagementWebsite.Controllers
{
    [Authorize]
    public class DeviceBackendMappingsController : Controller
    {
        private IConfiguration configuration;
        string apiBaseUrl = "";

        public DeviceBackendMappingsController(IConfiguration iConfig)
        {
            configuration = iConfig;
            apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");
        }

        // GET: DeviceBackendMappingsController
        public async Task<IActionResult> Index()
        {
            List<DeviceBackendDTO> liData = null;
            var token = User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.UserData)?.Value;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.SetBearerToken(token.Split(" ")[1]);
                string endpoint = apiBaseUrl + "/DeviceBackend";

                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = Response.Content.ReadAsStringAsync().Result;
                        liData = JsonConvert.DeserializeObject<List<DeviceBackendDTO>>(data); //JsonSerializer.Deserialize<List<DeviceDTO>>(data);
                    }
                }
            }
            return View(liData);
        }

        // GET: DeviceBackendMappingsController/Create
        public async Task<IActionResult> Create()
        {
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
                        var liDevices = JsonConvert.DeserializeObject<List<DeviceDTO>>(data);
                        ViewBag.devices = liDevices;
                    }
                }

                string endpoint1 = apiBaseUrl + "/Backend";
                using (var Response = await client.GetAsync(endpoint1))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = Response.Content.ReadAsStringAsync().Result;
                        var liBackends = JsonConvert.DeserializeObject<List<BackendDTO>>(data);
                        ViewBag.backends = liBackends;
                    }
                }
            }
            return View();
        }

        // POST: DeviceBackendMappingsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeviceBackendDTO model)
        {
            try
            {
                var token = User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.UserData)?.Value;
                using (HttpClient client = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                    string endpoint = apiBaseUrl + "/DeviceBackend";
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.SetBearerToken(token.Split(" ")[1]);

                    using (var Response = await client.GetAsync(endpoint))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var data = Response.Content.ReadAsStringAsync().Result;
                            var liData = JsonConvert.DeserializeObject<List<DeviceBackendDTO>>(data);
                            if (!liData.Exists(p => p.DeviceId == model.DeviceId && p.BackendId == model.BackendId))
                            {
                                using (var Response1 = await client.PostAsync(endpoint, content))
                                {
                                    if (Response1.StatusCode == System.Net.HttpStatusCode.OK)
                                    {
                                        //return RedirectToAction("index");
                                        ViewBag.RetVal = 1;
                                    }
                                }
                            }
                            else
                            {
                                ViewBag.RetVal = -2;
                            }

                            endpoint = apiBaseUrl + "/Device";

                            using (var Response2 = await client.GetAsync(endpoint))
                            {
                                if (Response2.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var d_data = Response2.Content.ReadAsStringAsync().Result;
                                    var liDevices = JsonConvert.DeserializeObject<List<DeviceDTO>>(d_data);
                                    ViewBag.devices = liDevices;
                                }
                            }

                            string endpoint1 = apiBaseUrl + "/Backend";
                            using (var Response3 = await client.GetAsync(endpoint1))
                            {
                                if (Response3.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var b_data = Response3.Content.ReadAsStringAsync().Result;
                                    var liBackends = JsonConvert.DeserializeObject<List<BackendDTO>>(b_data);
                                    ViewBag.backends = liBackends;
                                }
                            }
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

        // GET: DeviceBackendMappingsController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Int32? RetVal = null;
            try
            {
                var token = User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.UserData)?.Value;

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.SetBearerToken(token.Split(" ")[1]);
                    string endpoint = apiBaseUrl + "/DeviceBackend/" + id;

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
        }
    }
}
