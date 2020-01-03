using APITests.Base;
using APITests.Utilities;
using RestSharp;
using RestSharp.Authenticators;
using APITests.Base;
using APITests.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace RestSharpDemo.Steps
{

    [Binding]
    public class CommonSteps
    {

        //Context injection
        private Settings _settings;
        public CommonSteps(Settings settings)
        {
            _settings = settings;
        }


        [Given(@"I get JWT authentication of User with following details")]
        public void GivenIGetJWTAuthenticationOfUserWithFollowingDetails(Table table)
        {
            dynamic data = table.CreateDynamicInstance();

            _settings.Request = new RestRequest("auth/login", Method.POST);

            _settings.Request.RequestFormat = DataFormat.Json;
            _settings.Request.AddBody(new { email = (string)data.Email, password = (string)data.Password });

            //Get access token
            _settings.Response = _settings.RestClient.ExecutePostTaskAsync(_settings.Request).GetAwaiter().GetResult();
            var access_token = _settings.Response.GetResponseObject("access_token");

            //Authentication
            var authenticator = new JwtAuthenticator(access_token);
            _settings.RestClient.Authenticator = authenticator;
        }


    }
}
