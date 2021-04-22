using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPINSampleCode.DTOs;
using System;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace QPINSampleCode.Service
{
    public class QPINClient : IQPINClient
    {
        private readonly IConfiguration _config;
        private static CredentialModel _credential;
        
        public QPINClient(IConfiguration config, CredentialModel credential)
        {
            _config = config;            
            _credential = credential;            
        }

        private static HttpClient GenerateHttpClient()
        {
            string customerCode = _credential.CustomerCode;
            string username = _credential.Username;
            string password = _credential.Password;

            string UsernamePassword = string.Format("{0}/{1}:{2}", customerCode, username, password);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var credentialBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(UsernamePassword));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentialBase64);
            return client;
        }

        public async Task<PINOffsetGenerationResponse> GeneratePINOffset(QPINInput input)
        {
            string name = Constants.QPIN_URL_GENERATE_PINOFFSET;
            string urlName = _config.GetValue<string>(name);
            Uri Host = new Uri(urlName);
            HttpClient client = GenerateHttpClient();
            PINOffsetGenerationResponse pinoffsetGenResp = new PINOffsetGenerationResponse();
            try
            {
                string sc = JsonConvert.SerializeObject(input);

                using (StringContent strContent = new StringContent(sc, Encoding.UTF8, "application/json"))
                {
                    HttpResponseMessage response = client.PostAsync(Host, strContent).Result;
                    PrintOutRequestResponse(response, sc);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        HttpContent content = response.Content;
                        string pinOffset = await content.ReadAsStringAsync();
                        Console.WriteLine(TryFormatJson(pinOffset));
                        if (pinOffset.Contains("\\", StringComparison.Ordinal))
                            pinOffset = JsonConvert.DeserializeObject<string>(pinOffset);
                        pinoffsetGenResp = JsonConvert.DeserializeObject(pinOffset, typeof(PINOffsetGenerationResponse)) as PINOffsetGenerationResponse;
                        
                    }
                    else
                    {
                        string content = response.Content.ReadAsStringAsync().Result;
                        var err = JsonConvert.DeserializeObject(content, typeof(QwickPINError)) as QwickPINError;
                        Console.WriteLine("Error Code: " + err.Code + ", Error Message: " + err.Message);
                    }
                }
            }
            catch (Exception ex) when (ex is CommunicationException || ex is ProtocolException || ex is FaultException || ex is Exception)
            {
                throw ex;
            }
            finally
            {
                client.Dispose();
            }
            return pinoffsetGenResp;
        }
        
        public async Task<PINOffsetVerificationResponse> VerifyPINOffset(QPINInput input)
        {
            string name = Constants.QPIN_URL_VERIFY_PINOFFSET;
            string urlName = _config.GetValue<string>(name);
            Uri Host = new Uri(urlName);
            HttpClient client = GenerateHttpClient();
            PINOffsetVerificationResponse pinoffsetVerifyResp = new PINOffsetVerificationResponse();
            try
            {
                string sc = JsonConvert.SerializeObject(input);

                using (StringContent strContent = new StringContent(sc, Encoding.UTF8, "application/json"))
                {
                    HttpResponseMessage response = client.PostAsync(Host, strContent).Result;
                    PrintOutRequestResponse(response, sc);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        HttpContent content = response.Content;
                        string pinOffset = await content.ReadAsStringAsync();
                        Console.WriteLine(TryFormatJson(pinOffset));
                        if (pinOffset.Contains("\\", StringComparison.Ordinal))
                            pinOffset = JsonConvert.DeserializeObject<string>(pinOffset);
                        pinoffsetVerifyResp = JsonConvert.DeserializeObject(pinOffset, typeof(PINOffsetVerificationResponse)) as PINOffsetVerificationResponse;

                    }
                    else
                    {
                        string content = response.Content.ReadAsStringAsync().Result;
                        var err = JsonConvert.DeserializeObject(content, typeof(QwickPINError)) as QwickPINError;
                        Console.WriteLine("Error Code: " + err.Code + ", Error Message: " + err.Message);
                    }
                }
            }
            catch (Exception ex) when (ex is CommunicationException || ex is ProtocolException || ex is FaultException || ex is Exception)
            {
                throw ex;
            }
            finally
            {
                client.Dispose();
            }
            return pinoffsetVerifyResp;
        }

        private static string TryFormatJson(string json)
        {
            try
            {
                dynamic parsedJson = JsonConvert.DeserializeObject(json);
                return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            }
            catch (Exception)
            {
                return json;
            }
        }
		
		// Printing out Request with Header and Response with Header.
        private void PrintOutRequestResponse(HttpResponseMessage response, string sc)
        {
            Console.WriteLine("--- REQUEST ---");
            Console.WriteLine("{0} {1} HTTP/{2}", response?.RequestMessage?.Method, response?.RequestMessage?.RequestUri, response?.RequestMessage?.Version);
            foreach (var header in response?.RequestMessage?.Headers)
                Console.WriteLine("{0}: {1}", header.Key, string.Join(',', header.Value));
            Console.WriteLine();
            Console.WriteLine(TryFormatJson(sc));
            Console.WriteLine("--- RESPONSE ---");
            Console.WriteLine("HTTP/{0} {1} {2}", response?.Version, (int)response?.StatusCode, response?.ReasonPhrase);
            foreach (var header in response?.Headers)
                Console.WriteLine("{0}: {1}", header.Key, string.Join(',', header.Value));
            Console.WriteLine();
        }
    }
}
