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

        [HttpPost("train")]
        public IActionResult Train([FromBody] KeyValuePair<int, string>[] data) {
            indexCache = new Dictionary<int, int>();
            documentCache = new Dictionary<int, string>();
            for (int i = 0; i < data.Length; i++) {
                indexCache[i] = data[i].Key;
                documentCache[data[i].Key] = data[i].Value;
            }

            string path = System.IO.Directory.GetCurrentDirectory() + "/ch2.txt";
            System.IO.File.WriteAllText(path, string.Join(Environment.NewLine, data.Select(x => x.Value)));

            var web = new WebClient();
            web.Proxy = null;
            web.UploadString($"{HOST}/train", "POST", JsonConvert.SerializeObject(path));

            return Ok("trained");
        }

        [HttpGet("similarity/{docId}")]
        public IActionResult Recommend([FromRoute] int docId) {
            var indexes = indexCache;
            var documents = documentCache;

            var web = new WebClient();
            web.Proxy = null;
            var json = web.UploadString($"{HOST}/similartiy", "POST", JsonConvert.SerializeObject(new {
                data = documents.GetValueOrDefault(docId) ?? ""
            }));

            int index = (int)(JObject.Parse(json)["id"]);
            int id = indexes[index];

            return Ok(id);
        }
    }
}
