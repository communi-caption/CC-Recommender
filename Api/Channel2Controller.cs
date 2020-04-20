using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Recommender.Api {

    [Route("/ch2")]
    public class Channel2Controller : ControllerBase {

        private const string HOST = "http://37.148.210.36:8083";

        private static Dictionary<int, int> indexCache;
        private static Dictionary<int, string> documentCache;

        public Channel2Controller() {
        }

        public class TrainTuple {
            public int[] Item1 { get; set; }
            public string[] Item2 { get; set; }
        }

        [HttpPost("train")]
        public IActionResult Train([FromBody] TrainTuple data) {
            indexCache = new Dictionary<int, int>();
            documentCache = new Dictionary<int, string>();
            for (int i = 0; i < data.Item1.Length; i++) {
                indexCache[i] = data.Item1[i];
                documentCache[data.Item1[i]] = data.Item2[i];
            }

            string path = System.IO.Directory.GetCurrentDirectory() + "/ch2.txt";
            System.IO.File.WriteAllText(path, string.Join(Environment.NewLine, data.Item2).Trim());
            
            var web = new WebClient();
            web.Proxy = null;
            web.Headers[HttpRequestHeader.ContentType] = "application/json";
            web.UploadString($"{HOST}/train", "POST", JsonConvert.SerializeObject(path));

            return Ok("trained");
        }

        [HttpGet("similarity/{docId}")]
        public IActionResult Recommend([FromRoute] int docId) {
            var indexes = indexCache;
            var documents = documentCache;

            var web = new WebClient();
            web.Headers[HttpRequestHeader.ContentType] = "application/json";
            web.Proxy = null;
            var json = web.UploadString($"{HOST}/similarity", "POST", JsonConvert.SerializeObject(new {
                data = documents[docId]
            }));


            var res = (JArray)(JObject.Parse(json)["id"]);
            var list = new List<int>();

            Console.Error.WriteLine(JsonConvert.SerializeObject(res));

            foreach (var item in res) {
                list.Add(indexes[(int)(item)]);
            }
            Console.Error.WriteLine(JsonConvert.SerializeObject(indexes));
            Console.Error.WriteLine(JsonConvert.SerializeObject(list));

            return Ok(list);
        }
    }
}
