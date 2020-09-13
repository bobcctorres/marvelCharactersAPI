using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MarvelAngularApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<MarvelCharacter>> Get()        
        {
            string offset = Request.Query["offset"];
            string limit = Request.Query["limit"];
            var resp = await getCharacters(offset, limit);
            //System.Diagnostics.Debug.WriteLine(resp);

            //string total
            string results = resp.Substring(resp.IndexOf("results")+9);
            string total = resp.Substring(resp.IndexOf("total")+7, 4);
            results = "{ total: " + total + ", results: " + results.Substring(0, results.Length - 1);
            
            //var k = JsonConvert.DeserializeObject<List<WeatherForecast>>(results);
          
            var resultsObj = JsonConvert.DeserializeObject<CharacterData>(results);
            //return resultsObj.results;
            //return new CharacterData { results = resultsObj.results, total = 1000 };

            List<MarvelCharacter> resultsRet = new List<MarvelCharacter>();
            resultsObj.results.ForEach(res => resultsRet.Add(new MarvelCharacter
            {
                id = res.id,
                description = res.description,
                name = res.name,
                thumbnail = res.thumbnail,
                total = Int16.Parse(total),
            }));

            return resultsRet;
            /*var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)] + " - " + results // JsonConvert.SerializeObject(test)
            })
            .ToArray();*/
        }

        private static string getMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }

        public class Character
        {
            public string id;
            public string name;
        }

        [JsonArray]
        public class CharacterResults { public List<MarvelCharacter> JSON; }

        public class CharacterData
        {
            public int total;
            public List<MarvelCharacter> results;
        }
        public class CharacterResponse
        {
            public int code { get; set; }
            public CharacterData data { get; set; }
        }

        /*static HttpClient client = new HttpClient();
        static async Task<Character> GetCharactersAsync(string path)
        {
            Character product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Character>();
            }
            return product;

        }*/

        //public async Task<List<Character>> getCharacters()
        public async Task<string> getCharacters(string offset, string limit)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://gateway.marvel.com:443"); // ("https://gateway.marvel.com:443/v1/public");
                //HTTP GET

                string ts = "2";
                string privateKey = Environment.GetEnvironmentVariable("MARVEL_PRIVATE_KEY");
                string publicKey = Environment.GetEnvironmentVariable("MARVEL_PUBLIC_KEY");
                string API_KEY = Environment.GetEnvironmentVariable("MARVEL_API_KEY");
                string hash = getMD5 (ts + privateKey + publicKey);

                var responseTask = client.GetAsync("v1/public/characters?ts="+ts+"&hash=" + hash + "&apikey=" + API_KEY + "&offset="+ offset + "&limit="+limit);               
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync(); //<IList<Character>>();

                    try
                    {
                        /*CharacterResponse resp = JsonConvert.DeserializeObject<CharacterResponse>(readTask); // JsonConvert.DeserializeObject<List<Character>>(readTask);

                        System.Diagnostics.Debug.WriteLine("--RESULTS! " + resp.data.results);

                        return resp.data.results.ToString();*/
                        return readTask;
                    }
                    catch (JsonReaderException)
                    {
                        System.Diagnostics.Debug.WriteLine("error json deserialization!");
                    }
                        
                }
                return null;
            }
        }


        }
    }
