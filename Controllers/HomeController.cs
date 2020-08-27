using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ListORama.Models;
using ListORama.DataAccess;
using System.Net.Http;

namespace ListORama.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private ApplicationDBContext dbContext;

        static string BASE_URL = "https://developer.nps.gov/api/v1/";

        static string API_KEY = "RaUgx7BbI1b7lfyYTEwdFSbfpOKfFce1mHNpCHsX";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List myList = new List();
            myList.listID = 1;
            myList.listName = "Shopping List";
            myList.listStatus = "Complete";


            List[] allLists = { myList};
            Lists listsToDisplay = new Lists();
            listsToDisplay.list = allLists;
            return View(listsToDisplay);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Settings()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Soon()
        {
            return View();
        }

        public IActionResult Chart()
        {
            Parks parks = SearchPark(null);
            Dictionary<String, int> parkCountByState = new Dictionary<string, int>();
            foreach (Park park in parks.data)
            {
                String[] stateCodes = park.states.Split(",");
                foreach (String stateCode in stateCodes)
                {
                    if (parkCountByState.ContainsKey(stateCode))
                    {
                        parkCountByState[stateCode] = parkCountByState[stateCode] + 1;
                    }
                    else
                    {
                        parkCountByState.Add(stateCode, 1);
                    }
                }
            }


            string[] chartColors = new string[] { "rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)",
                                                  "rgb(54, 162, 235)", "rgb(153, 102, 255)", "rgb(201, 203, 207)", "#4dc9f6",
                                                "#f67019", "#f53794","#537bc4", "#acc236", "#166a8f", "#00a950", "#58595b","#8549ba" };
            Chart Model = new Chart
            {
                chartType = "doughnut",
                labels = String.Join(",", parkCountByState.Keys.Select(d => "'" + d + "'")),
                data = String.Join(",", parkCountByState.Values.Select(d => d)),
                chartTitle = "No. of parks by state",
                backgroundColors = String.Join(",", chartColors.Select(d => "'" + d + "'"))
            };
            return View(Model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static Parks SearchPark(String param)
        {
            HttpClient httpClient;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string NATIONAL_PARK_API_PATH = BASE_URL + "/parks?limit=20&" + param;
            string parksData = "";

            Models.Parks parks = null;

            httpClient.BaseAddress = new Uri(NATIONAL_PARK_API_PATH);

            try
            {
                HttpResponseMessage response = httpClient.GetAsync(NATIONAL_PARK_API_PATH).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    parksData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                if (!parksData.Equals(""))
                {
                    parks = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Parks>(parksData);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return parks;
        }

        public IActionResult ChartOffers()
        {
            Stores stores = SearchOffers();
            List<Store> usStoresWithOffers = new List<Store>();
            Dictionary<String, int> storesByCountry = new Dictionary<string, int>();
            foreach (Store store in stores.stores)
            {
                String country = store.location;
                if(!String.IsNullOrWhiteSpace(country))
                {
                    String[] countries = country.Split(",");
                    foreach (String countryCode in countries)
                    {
                        if (storesByCountry.ContainsKey(countryCode))
                        {
                            storesByCountry[countryCode] = storesByCountry[countryCode] + 1;
                        }
                        else
                        {
                            storesByCountry.Add(countryCode, 1);
                        }
                        if ("United States".Equals(countryCode) || "USA".Equals(countryCode))
                        {
                            usStoresWithOffers.Add(store);
                        }
                    }
                }
            }

            Dictionary<String, int> sampledStores = new Dictionary<String, int>();
            int i = 1;
            foreach (KeyValuePair<String, int> element in storesByCountry.OrderByDescending(key => key.Value))
            {
                sampledStores.Add(element.Key, element.Value);
                i++;
                if(i == 10)
                {
                    break;
                }
            }

            string[] chartColors = new string[] { "rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)",
                                                  "rgb(54, 162, 235)", "rgb(153, 102, 255)", "rgb(201, 203, 207)", "#4dc9f6",
                                                "#f67019", "#f53794","#537bc4", "#acc236", "#166a8f", "#00a950", "#58595b","#8549ba" };
            Chart Model = new Chart
            {
                chartType = "horizontalBar",
                labels = String.Join(",", sampledStores.Keys.Select(d => "'" + d + "'")),
                data = String.Join(",", sampledStores.Values.Select(d => d)),
                chartTitle = "No. of offers by Country",
                backgroundColors = String.Join(",", chartColors.Select(d => "'" + d + "'"))
            };
            return View(Model);
        }

        public static Stores SearchOffers()
        {
            HttpClient httpClient;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string COUPON_STORES_API_PATH = "https://couponapi.org/api/getStores/";
            string storesData = "";

            Stores stores = null;

            httpClient.BaseAddress = new Uri(COUPON_STORES_API_PATH);

            try
            {
                HttpResponseMessage response = httpClient.GetAsync(COUPON_STORES_API_PATH).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    storesData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                if (!storesData.Equals(""))
                {
                    stores = Newtonsoft.Json.JsonConvert.DeserializeObject<Stores>(storesData);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

/*            if(null != stores && stores.stores.Length > 0)
            {
                foreach(Store store in stores.stores)
                {
                    StorePreview preview = GetPreview(store);
                    store.preview = preview;
                }
            }*/
            return stores;
        }

        public static StorePreview GetPreview(Store store)
        {
            StorePreview preview = null;
            if (null == store)
                return preview;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string PREVIEW_API_PATH = "https://api.linkpreview.net?key=563b2c1eaf79c62ccabc5c1decfc62ad&q=" + store.URL;
            string previewData = "";

            httpClient.BaseAddress = new Uri(PREVIEW_API_PATH);

            try
            {
                HttpResponseMessage response = httpClient.GetAsync(PREVIEW_API_PATH).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    previewData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                if (!previewData.Equals(""))
                {
                    preview = Newtonsoft.Json.JsonConvert.DeserializeObject<StorePreview>(previewData);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return preview;
        }
    }
}


