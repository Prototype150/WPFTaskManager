using DesktopTaskManager.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DesktopTaskManager.Services
{
    public class TaskService : ITaskService
    {
        private string _connectionString = "https://26.218.3.87:7025";
        public async Task<(IEnumerable<TaskModel>? tasks, string message)> GetAccountTasks(int accountId)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (HttpClient client = new HttpClient(handler))
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, _connectionString + "/task/" + accountId);
                var responce = await client.SendAsync(message);
                var jsonResponce = await responce.Content.ReadAsStringAsync();

                if (responce.IsSuccessStatusCode)
                {
                    var tasks = JsonSerializer.Deserialize<IEnumerable<TaskModel>>(jsonResponce, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if(tasks != null)
                        return (tasks, "ok");
                }
                return (null, jsonResponce);
            }
        }

        public async Task<(bool result, string message)> UpdateTask(TaskModel task)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (HttpClient client = new HttpClient(handler))
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Put, new Uri(_connectionString + "/task/update")) { Content = new StringContent(JsonSerializer.Serialize(task), Encoding.UTF8, MediaTypeNames.Application.Json) };
                var responce = await client.SendAsync(message);
                var stringContent = await responce.Content.ReadAsStringAsync();

                if (responce.IsSuccessStatusCode)
                {
                    bool result = JsonSerializer.Deserialize<bool>(stringContent);
                    return (result, result ? "ok" : stringContent);
                }
                return (false, stringContent);
            }
        }
    }
}
