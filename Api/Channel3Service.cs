using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace Recommender.Api
{
    public class Channel3Service
    {
        private static readonly HttpClient client = new HttpClient();
        public async Task<string[]> getPeopleAlsoSearchForAsync(string workOfArt)
        {
            return new string[0];
            var response = await client.GetStringAsync($"https://app.zenserp.com/api/v2/search?apikey=72e89f30-8369-11ea-b8c1-27fe76fdaa39&q={workOfArt}&lr=lang_tr&hl=tr&location=Turkey&gl=tr");
            var x = JObject.Parse(response);
            string[] result;
            if (x["knowledge_graph"][0]["people_also_search_for"] != null)
                result = x["knowledge_graph"][0]["people_also_search_for"].Select(kv => kv["name"].ToString()).ToArray();
            else
                return new string[0];
            return result;
        }
    }
}
