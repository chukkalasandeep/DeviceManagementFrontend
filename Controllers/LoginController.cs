using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

using DeviceManagementWebsite.Models;
using Microsoft.Extensions.Configuration;

namespace DeviceManagementWebsite.Controllers
{
    public class LoginController : Controller
    {
        private IConfiguration configuration;
        string apiBaseUrl = "";

        public LoginController(IConfiguration iConfig)
        {
            configuration = iConfig;
            apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserInfo user)
        {
            ClaimsIdentity identity = null;
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                string endpoint = apiBaseUrl + "/login";

                using (var Response = await client.PostAsync(endpoint, content))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string token = Response.Content.ReadAsStringAsync().Result;
                        TempData["Profile"] = JsonConvert.SerializeObject(user);
                        identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.UserData, token)
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var login = HttpContext.SignInAsync(principal);
                        return RedirectToAction("Index", "Device");
                        //User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.UserData)?.Value
                    }
                    else                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                        return View();
                    }
                }
            }
        }
    }
}
