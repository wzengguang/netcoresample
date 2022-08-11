using Sample.Application.Accounts;
using Sample.Application.Accounts.Models;
using Sample.Web;
using Sample.Web.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using Xunit;

namespace Sample.Test
{
    [TestClass]
    public class TestBase
    {
        WebApplicationFactory<Program> _server;

        protected HttpClient _client;

        protected string token;

        protected LoginModel _loginModel;

        public TestBase()
        {
            _loginModel = new LoginModel() { Password = "User1@1234", Username = "User1" };
        }

        [TestInitialize]
        public virtual async Task TestInitialize()
        {
            try
            {
                InitServer();

                token = await GetToken(_loginModel.Username, _loginModel.Password);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void InitServer()
        {
            _server = new WebApplicationFactory<Program>();
            _client = _server.CreateClient();
            _client.BaseAddress = new Uri("http://localhost");
        }

        protected async Task<string> GetToken(string userName, string password)
        {
            var loginModel = new LoginModel() { Username = userName, Password = password };

            var requestMessage = CreateHttpRequestMessage(HttpMethod.Post, "api/Auth/Login", loginModel);

            var response = await _client.SendAsync(requestMessage);

            var result = await response.Content.ReadFromJsonAsync<LoginResult>();

            Assert.IsTrue(!string.IsNullOrEmpty(result.Token));
            Assert.IsTrue(result.IsSuccess);

            return result.Token;
        }

        protected HttpRequestMessage CreateGetHttpRequestMessage(string uri)
        {
            return CreateHttpRequestMessage<string>(HttpMethod.Get, uri);
        }

        protected HttpRequestMessage CreatePostHttpRequestMessage<T>(string uri, T content) where T : class
        {
            return CreateHttpRequestMessage<T>(HttpMethod.Post, uri, content);
        }

        protected HttpRequestMessage CreateHttpRequestMessage<T>(HttpMethod httpMethod, string uri, T content = null) where T : class
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage()
            {
                Method = httpMethod,
                RequestUri = new Uri("http://localhost/" + uri),
            };

            if (content != null)
            {
                var json = JsonConvert.SerializeObject(content);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                requestMessage.Content = stringContent;
            }

            if (token != null)
            {
                requestMessage.Headers.Add("authorization", "Bearer " + token);
            }
            return requestMessage;
        }
    }
}