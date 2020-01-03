using APITests.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace APITests.Base
{
    public class Settings
    {
        public Uri BaseUrl { get; set; }
        public IRestResponse Response { get; set; }
        public IRestRequest Request { get; set; }
        public RestClient RestClient { get; set; } = new RestClient();

    }
}
