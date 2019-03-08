using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WorkflowManager.Exceptions;

namespace WorkflowManager.Services.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {
        private readonly JsonSerializerSettings _serializerSettings;
        public RequestProvider()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }
        public async Task<TResult> GetAsync<TResult>(string uri, string token = "")
        {
            try
            {
                using (var httpClient = this.CreateHttpClient(token))
                {
                    try
                    {
                        HttpResponseMessage response = await httpClient.GetAsync(uri);

                        await this.HandleResponse(response);

                        string serializedData = await response.Content.ReadAsStringAsync();

                        TResult result = await Task.Run(() => 
                            JsonConvert.DeserializeObject<TResult>(serializedData, _serializerSettings));

                        return result;
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine(e.InnerException.Message);
                        throw e;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw e;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error in RequestProvider.GetAsync(): \n{0}", e));
                throw e;
            }
        }
        public async Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "", string header = "")
        {
            try
            {
                using (var httpClient = this.CreateHttpClient(token))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(header))
                        {
                            this.AddHeaderParameter(httpClient, header);
                        }

                        var content = new StringContent(JsonConvert.SerializeObject(data));
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        HttpResponseMessage response = await httpClient.PostAsync(uri, content);

                        await this.HandleResponse(response);
                        string serializedData = await response.Content.ReadAsStringAsync();

                        TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serializedData, _serializerSettings));

                        return result;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw e;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error in RequestProvider.PostAsync(): {0}", e));
                throw e;
            }
        }
        public async Task<TResult> PostAsync<TResult>(string uri, string data, string clientId, string clientSecret)
        {
            try
            {
                using (var httpClient = this.CreateHttpClient(string.Empty))
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret))
                        {
                            this.AddBasicAuthenticationHeader(httpClient, clientId, clientSecret);
                        }

                        var content = new StringContent(JsonConvert.SerializeObject(data));
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        HttpResponseMessage response = await httpClient.PostAsync(uri, content);

                        await this.HandleResponse(response);
                        string serializedData = await response.Content.ReadAsStringAsync();

                        TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serializedData, _serializerSettings));

                        return result;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw e;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error in RequestProvider.PostAsync(): {0}", e));
                throw e;
            }
        }
        public async Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", string header = "")
        {
            try
            {
                using (var httpClient = this.CreateHttpClient(token))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(header))
                        {
                            this.AddHeaderParameter(httpClient, header);
                        }

                        var content = new StringContent(JsonConvert.SerializeObject(data));
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        HttpResponseMessage response = await httpClient.PutAsync(uri, content);

                        await this.HandleResponse(response);
                        string serializedData = await response.Content.ReadAsStringAsync();

                        TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serializedData, _serializerSettings));

                        return result;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw e;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error in RequestProvider.PutAsync(): {0}", e));
                throw e;
            }
        }
        public async Task DeleteAsync(string uri, string token = "")
        {
            try
            {
                using (var httpClient = this.CreateHttpClient(token))
                {
                    await httpClient.DeleteAsync(uri);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error in RequestProvider.DeleteAsync(): {0}", e));
                throw e;
            }
        }
        private HttpClient CreateHttpClient(string token = "")
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return httpClient;
        }
        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(content);
                }

                throw new HttpRequestExceptionEx(response.StatusCode, content);
            }
        }
        private void AddHeaderParameter(HttpClient httpClient, string parameter)
        {
            if (httpClient == null || string.IsNullOrEmpty(parameter)) return;

            httpClient.DefaultRequestHeaders.Add(parameter, Guid.NewGuid().ToString());
        }
        private void AddBasicAuthenticationHeader(HttpClient httpClient, string clientId, string clientSecret)
        {
            if (httpClient == null) return;

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret)) return;

            httpClient.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(clientId, clientSecret);
        }
    }
}
