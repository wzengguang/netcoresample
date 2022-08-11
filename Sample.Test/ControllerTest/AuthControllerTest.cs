using Sample.Application.Accounts;
using Sample.Application.Accounts.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Test.ControllerTest
{
    [TestClass]
    public class AuthControllerTest : TestBase
    {
        [TestInitialize]
        public override async Task TestInitialize()
        {
            await Task.Run(() =>
            {
                InitServer();
            });
        }

        [TestMethod]
        public async Task RegistTest()
        {
            var registModel = new RegisterModel() { Password = "User1@1234", Email = "user1@user.com", Username = "User1" };

            var requestMessage = CreateHttpRequestMessage(HttpMethod.Post, "api/auth/Register", registModel);

            var response = await _client.SendAsync(requestMessage);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            var responseResult = response.Content.ReadFromJsonAsync<RegisterResult>();
        }
    }
}
