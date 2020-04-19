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
            var response = await client.GetStringAsync($"https://app.zenserp.com/api/v2/search?apikey=e73735c0-80df-11ea-b12e-c7804e1da203&q={workOfArt}");
            var x = JObject.Parse(response);
            var result = x["knowledge_graph"][0]["people_also_search_for"].Select(kv => kv["name"].ToString()).ToArray();
            return result;
        }

    }
}
