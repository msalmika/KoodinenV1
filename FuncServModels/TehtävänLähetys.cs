using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KoodinenV1.FuncServModels
{
    public class TehtävänLähetys
    {
        const string URL = "https://funclogickoodinen.azurewebsites.net/api/TehtavaSuoritusTietoKantaan?code=NudohY8CkjsRXFxqHZgBaLKia5Oko0SAmidmBjCEShYaLxY9B3hIZQ==";

        public static void Tarkista(Suoritus s)
        {
            var data = JsonConvert.SerializeObject(s);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(data, UTF8Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync(URL, content).Result;
                var json = response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
