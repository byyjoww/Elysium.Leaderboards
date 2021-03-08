using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;

namespace Elysium.Networking
{
    public class HttpSender
    {
        private HttpClient _client;

        public struct Response
        {
            public string content;
            public bool isSuccess;
            public HttpStatusCode code;
        }

        public async Task<Response> SendHttpsRequest(HttpMethod _method, string _url, string _user, string _password, StringContent _body)
        {
            if (_client == null) { _client = new HttpClient(); }

            var httpRequestMessage = new HttpRequestMessage();
            if (_method != HttpMethod.Get)
            {
                httpRequestMessage.Content = _body;
            }
            httpRequestMessage.Method = _method;
            httpRequestMessage.RequestUri = new Uri(_url);
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{_user}:{_password}")));

            var response = await _client.SendAsync(httpRequestMessage);
            var responseContent = response.Content;
            var jsonContent = await responseContent.ReadAsStringAsync();

            httpRequestMessage.Dispose();
            responseContent.Dispose();

            return new Response
            {
                content = jsonContent,
                isSuccess = response.IsSuccessStatusCode,
                code = response.StatusCode
            };
        }
    }
}
