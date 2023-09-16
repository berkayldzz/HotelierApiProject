using HotelProject.WebUI.Models.Staff;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HotelProject.WebUI.Controllers
{
    public class StaffController : Controller
    {
        //IHttpClientFactory, HTTP istemcisi oluşturma ve yönetme işlevselliği sağlayan bir servisdir ve genellikle dış API'lere veya hizmetlere HTTP istekleri göndermek için kullanılır.

        private readonly IHttpClientFactory _httpClientFactory;

        public StaffController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();  // Bir tane istemci oluşturdum.Bu istemci, dış API'ye HTTP istekleri göndermek için kullanılacaktır.
            var responseMessage = await client.GetAsync("http://localhost:60444/api/Staff");  // bu adrese get isteğinde bulundum.
            if (responseMessage.IsSuccessStatusCode)
            {
                // Gelen json türündeki veriyi listelemek için deserialize ettik.

                var jsondata = await responseMessage.Content.ReadAsStringAsync();
                // Gelen json datayı deserialize etmemiz gerek.Gelen veriyi deserialize ederek benim tablomda gösterebilecek formata getirdim.
                // Consume ettiğimiz sırada jsondaki(gelen veri json türünde) datalarla entitydeki dataların propertyleri bire bir aynı olmalı.

                var values = JsonConvert.DeserializeObject<List<StaffViewModel>>(jsondata);

                return View(values);
            }
            return View();
        }
        [HttpGet]
        public IActionResult AddStaff()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddStaff(AddStaffViewModel model)
        {
            // Bir data göndericez ve bu data jsona dönüşücek o yüzden serialize edicez.Yukarıdakinin tam tersi.

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(model);
            // StringContent bizim dönüşümümüz için kullanacağımız sınıfımız.
            //jsonData adlı JSON verisi, StringContent sınıfını kullanarak bir HTTP isteği gövdesine dönüştürülür. 
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("http://localhost:60444/api/Staff", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> DeleteStaff(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"http://localhost:60444/api/Staff/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateStaff(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"http://localhost:60444/api/Staff/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateStaffViewModel>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStaff(UpdateStaffViewModel model)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(model);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("http://localhost:60444/api/Staff/", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}
