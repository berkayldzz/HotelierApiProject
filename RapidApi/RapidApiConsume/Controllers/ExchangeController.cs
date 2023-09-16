using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using RapidApiConsume.Models;
using Newtonsoft.Json;

namespace RapidApiConsume.Controllers
{
    public class ExchangeController : Controller
    {
		public async Task<IActionResult> Index()
		{
			List<ExchangeViewModel> exchangeViewModels = new List<ExchangeViewModel>();
			
			
			var client = new HttpClient();
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri("https://exchangerate-api.p.rapidapi.com/rapid/latest/TRY"),
				Headers =
	{
		{ "X-RapidAPI-Key", "7d4fbc2db2msh2d20bc77ece018fp1d252ejsn082f4d950691" },
		{ "X-RapidAPI-Host", "exchangerate-api.p.rapidapi.com" },
	},
			};
			using (var response = await client.SendAsync(request))
			{
				response.EnsureSuccessStatusCode();
				var body = await response.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<ExchangeViewModel>(body);
				return View(values);
				
			}
		}
    }
}
