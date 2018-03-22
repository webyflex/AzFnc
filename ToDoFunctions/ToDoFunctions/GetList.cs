using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ToDoFunctions
{
    public static class GetList
    {
        [FunctionName("GetList")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            var repository = new ToDoRepository();

            return req.CreateResponse(HttpStatusCode.OK, repository.List());
        }
    }
}
