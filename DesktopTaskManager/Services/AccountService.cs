using DesktopTaskManager.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DesktopTaskManager.Services
{
    public class AccountService : IAccountService
    {
        private string _connectionString;

        public AccountService(string connectionString) {
            _connectionString = connectionString;
        }

        public async Task<(AccountModel? account, string message)> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                return (null, "empty:username");
            if (string.IsNullOrWhiteSpace(password))
                return (null, "empty:password");

            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (HttpClient client = new HttpClient(handler))
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, new Uri(_connectionString + "/account/login")) { Content = new StringContent(JsonSerializer.Serialize(new AccountModel(username, password)), Encoding.UTF8, MediaTypeNames.Application.Json) };
                var responce = await client.SendAsync(message);

                if (responce.IsSuccessStatusCode)
                {
                    var jsonAccount = await responce.Content.ReadAsStringAsync();
                    var account = JsonSerializer.Deserialize<AccountModel>(jsonAccount, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (account != null)
                        return (account, "ok");
                    return (null, "Something went wrong");
                }
                return (null, await responce.Content.ReadAsStringAsync());
            }
        }

        public async Task<(AccountModel? account, string message)> Register(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                return (null, "empty:username");
            if (string.IsNullOrWhiteSpace(password))
                return (null, "empty:password");

            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (HttpClient client = new HttpClient(handler))
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, new Uri(_connectionString + "/account/register")) { Content = new StringContent(JsonSerializer.Serialize(new AccountModel(username, password)), Encoding.UTF8, MediaTypeNames.Application.Json) };
                var responce = await client.SendAsync(message);
                var jsonAccount = await responce.Content.ReadAsStringAsync();

                if (responce.IsSuccessStatusCode)
                {
                    var account = JsonSerializer.Deserialize<AccountModel>(jsonAccount, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (account != null)
                        return (account, "ok");
                }
                return (null, jsonAccount);
            }
        }
    }
}
