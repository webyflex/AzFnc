using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace ToDoFunctions
{
    public static class GetItem
    {
        [FunctionName("GetItem")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            int id = 0;
            string idStr = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "id", true) == 0)
                .Value;

            if (string.IsNullOrEmpty(idStr) || !int.TryParse(idStr, out id))
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var repository = new ToDoRepository();
            var item = repository.GetById(id);

            if (item == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")
            };
        }
    }
}
