using Microsoft.AspNetCore.Mvc;
using sm_coding_challenge.Models;
using sm_coding_challenge.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace sm_coding_challenge.Controllers
{
    public class HomeController : Controller
    {
        private IDataProvider _dataProvider;

        public HomeController(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Player(string id)
        {
            var player = await _dataProvider.GetPlayerById(id);
            return Json(player);
        }

        [HttpGet]
        public async Task<IActionResult> Players(string ids)
        {
            var idList = ids.Split(',').Distinct();
            var returnList = new List<object>();
            foreach (var id in idList)
            {
                var player = await _dataProvider.GetPlayerById(id);
                returnList.Add(player);
            }

            return Json(returnList);
        }

        [HttpGet]
        public async Task<IActionResult> LatestPlayers(string ids)
        {
            var idList = ids.Split(',').Distinct();

            var latestPlayers = await _dataProvider.GetLatestPlayers(idList.ToArray());

            return Json(latestPlayers,
                new JsonSerializerOptions()
                {
                    IgnoreNullValues = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
