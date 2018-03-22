using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ToDoFunctions
{
    public static class UpdateItem
    {
        [FunctionName("UpdateItem")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "put", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            ToDo item = await req.Content.ReadAsAsync<ToDo>();
            string errorMessages = GetValideErrorMessages(item);
            var repository = new ToDoRepository();

            if (!string.IsNullOrEmpty(GetValideErrorMessages(item)))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, errorMessages);
            }

            if (!repository.Update(item))
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            return req.CreateResponse(HttpStatusCode.OK);
        }

        private static string GetValideErrorMessages(ToDo item)
        {
            string errorMessages = string.Empty;

            if (string.IsNullOrEmpty(item.Title))
            {
                errorMessages = "Title is required! \n\r ";
            }

            if (string.IsNullOrEmpty(item.Description))
            {
                errorMessages += "Description is required! \n\r ";
            }

            if (item.UniqueID <= 0)
            {
                errorMessages += "UniqueID is required!";
            }

            return errorMessages;
        }
    }
}
