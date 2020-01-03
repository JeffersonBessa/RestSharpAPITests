using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serialization.Json;
using FluentAssertions;
using APITests.Model;
using System.Threading.Tasks;
using System;
using APITests.Utilities;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using System.IO;

namespace APITests
{

    [TestClass]
    public class RestSharpDemo
    {

        [TestMethod]
        public void GetMethod()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts/{postid}", Method.GET);

            request.AddUrlSegment("postid", 1);

            var response = client.Execute(request);

            //Lib 1 - Dictionary based response
            var deserialize = new JsonDeserializer();
            var output = deserialize.Deserialize<Dictionary<string, string>>(response);
            var result = output["author"];

            //Lib 2 - JSON based response
            JObject obs = JObject.Parse(response.Content);
            var Nome = (obs["author"].ToString());
            Nome.Should().Be("Karthik KK");
        }

        [TestMethod]
        public void PostWihtAnonymousBody()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts/{postid}/profile", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { name = "Raj" });

            request.AddUrlSegment("postid", 1);

            var response = client.Execute(request);

            var result = response.DeserializeResponse()["name"];

            JObject obs = JObject.Parse(response.Content);
            var Nome = (obs["name"].ToString());
            Nome.Should().Be("Raj");
        }

        [TestMethod]
        public void PostWihtTypeClassBody()
        {

            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(new Posts() { id = "56", author = "Execute Automation", title = "RestSharp demo course" });

            var response = client.Execute<Posts>(request);

            //var deserialize = new JsonDeserializer();
            //var output = deserialize.Deserialize<Dictionary<string, string>>(response);
            //var result = output["author"];

            Assert.That(response.Data.author, Is.EqualTo("Execute Automation"), "Author is not correct");
        }

        [TestMethod]
        public void PostWihAsync()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts", Method.POST);

            request.RequestFormat = DataFormat.Json;

            request.AddBody(new Posts() { id = "39", author = "Execute Automation", title = "RestSharpDemo course" });

            //var response = client.Execute<Posts>(request);

            var response = client.ExecutePostTaskAsync<Posts>(request).GetAwaiter().GetResult();

            //var deserialize = new JsonDeserializer();
            //var output = deserialize.Deserialize<Dictionary<string, string>>(response);
            //var result = output["author"];

            var result = response.DeserializeResponse()["author"];

            Assert.That(response.Data.author, Is.EqualTo("Execute Automation"), "Author is not correct");
        }

        [TestMethod]
        public void AuthenticationMechanism() {

            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("auth/login", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { email = "karthik@email.com", password = "haha123" });

            var response = client.ExecutePostTaskAsync(request).GetAwaiter().GetResult();

            var access_token = response.DeserializeResponse()["access_token"];

            var jwtAuth = new JwtAuthenticator(access_token);
            client.Authenticator = jwtAuth;

            var getRequest = new RestRequest("posts/{postid}", Method.GET);
            getRequest.AddUrlSegment("postid", 5);

            //Perform the Get Operation
            var result = client.ExecuteAsyncRequest<Posts>(getRequest).GetAwaiter().GetResult();
            //Assert.That(result.Data.author, Is.EqualTo("Karthik KK"), "The author is not correct");
        }

        [TestMethod]
        public void AuthenticationMechanismWithJSONFile()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("auth/login", Method.POST);

            var file = @"TestData\Data.json";

            request.RequestFormat = DataFormat.Json;
            var jsonData = JsonConvert.DeserializeObject<User>(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file)).ToString());
            request.AddJsonBody(jsonData);

            var response = client.ExecutePostTaskAsync(request).GetAwaiter().GetResult();
            var access_token = response.DeserializeResponse()["access_token"];

            var jwtAuth = new JwtAuthenticator(access_token);
            client.Authenticator = jwtAuth;

            var getRequest = new RestRequest("posts/{postid}", Method.GET);
            getRequest.AddUrlSegment("postid", 5);

            //Perform Get operation
            var result = client.ExecuteAsyncRequest<Posts>(getRequest).GetAwaiter().GetResult();
            //Assert.That(result.Data.author, Is.EqualTo("Karthik KK"), "The author is not correct");
        }
       
        private class User
        {
            public string email { get; set; }
            public string password { get; set; }
        }

    }
}
