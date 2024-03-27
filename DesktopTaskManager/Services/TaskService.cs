using DesktopTaskManager.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DesktopTaskManager.Services
{
    public class TaskService : ITaskService
    {
        private string _connectionString;

        public TaskService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<(TaskModel result, string message)> AddTask(TaskModel newTask)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (HttpClient client = new HttpClient(handler))
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, _connectionString + "/task/add") { Content = new StringContent(JsonSerializer.Serialize(newTask), Encoding.UTF8, MediaTypeNames.Application.Json) };
                HttpResponseMessage responce = await client.SendAsync(message);
                string jsonResponce = await responce.Content.ReadAsStringAsync();

                if (responce.IsSuccessStatusCode)
                {
                    TaskModel result = JsonSerializer.Deserialize<TaskModel>(jsonResponce, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return (result, "ok");
                }

                return (null, jsonResponce);
            }
        }

        public async Task<(bool result, string message)> DeleteTask(int taskId)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (HttpClient client = new HttpClient(handler))
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, _connectionString + "/task/delete/" + taskId);
                HttpResponseMessage responce = await client.SendAsync(message);
                string jsonResponce = await responce.Content.ReadAsStringAsync();

                if (responce.IsSuccessStatusCode)
                {
                    bool result = JsonSerializer.Deserialize<bool>(jsonResponce);
                    return result ? (true, "ok") : (false, jsonResponce);
                }

                return (false, jsonResponce);
            }
        }

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
