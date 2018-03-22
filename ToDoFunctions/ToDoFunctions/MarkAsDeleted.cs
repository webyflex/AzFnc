using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ToDoFunctions
{
    public static class MarkAsDeleted
    {
        [FunctionName("MarkAsDeleted")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            string idStr = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "id", true) == 0)
                .Value;

            int id = 0;

            if (string.IsNullOrEmpty(idStr) || !int.TryParse(idStr, out id))
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var repository = new ToDoRepository();

            if (!repository.MarkAsDeleted(id))
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
