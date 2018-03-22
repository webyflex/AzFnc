using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ToDoFunctions
{
    public static class CreateItem
    {
        [FunctionName("CreateItem")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            ToDo item = await req.Content.ReadAsAsync<ToDo>();

            string errorMessages = GetValideErrorMessages(item);
            var repository = new ToDoRepository();

            if (!string.IsNullOrEmpty(errorMessages))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, errorMessages);
            }

            repository.Insert(item);

            return req.CreateResponse(HttpStatusCode.Created);
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
                errorMessages += "Description is required!";
            }

            return errorMessages;
        }
    }
}
