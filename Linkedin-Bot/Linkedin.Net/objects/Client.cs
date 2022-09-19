using Flurl;
using System;
using System.IO;
using Flurl.Http;
using System.Net;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Linkedin.Net.Estructuras;
using System.Collections.Generic;

namespace Linkedin.Net.objects
{
    public class Client
    {
        //Headers
        private Dictionary<string, string> REQUEST_HEADERS = new Dictionary<string, string>
        {
            {"user-agent","Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36"},
            {"accept-language","en-AU,en-GB;q=0.9,en-US;q=0.8,en;q=0.7" },
            {"x-li-lang","en_US"},
            {"x-restli-protocol-version","2.0.0" },
            {"Accept-Encoding","gzip, deflate" },
            {"Accept", "*/*"},
            {"Connection","keep-alive" }
        };
        private Dictionary<string, string> AUTH_REQUEST_HEADERS = new Dictionary<string, string>
        {
            {"X-Li-User-Agent","LIAuthLibrary:3.2.4 \n com.linkedin.LinkedIn:8.8.1 \n iPhone:8.3" },
            {"User-Agent","LinkedIn/8.8.1 CFNetwork/711.3.18 Darwin/14.0.0" },
            {"X-User-Language","en" },
            {"X-User-Locale","en_US" },
            {"Accept-Language","en-us" }
        };

        //Variables
        private const string LINKEDIN_BASE_URL = "https://www.linkedin.com";
        private readonly string API_BASE_URL = $"{LINKEDIN_BASE_URL}/voyager/api";
        private FlurlClient _httpClient;
        private string _csrf_token;
        private CookieRepository _cookieRepository;
        private List<FlurlCookie> _cookies;
        private Login _Credenciales;
        private bool _UseCookieCache;

        //Propiedades
        public FlurlClient HttpClient
        {
            get
            {
                return this._httpClient;
            }
        }

        public string csrf_token
        {
            get
            {
                return _csrf_token;
            }
        }

        public List<FlurlCookie> cookies
        {
            get
            {
                return _cookies;
            }
        }

        //construtor
        public Client(Login Credenciales,Proxy proxy,bool useCookieCache=false)
        {
            //Instanceamos el cliente http
            this._httpClient = new FlurlClient(new HttpClient(proxy.HttpHandler));
            this._httpClient.BaseUrl = API_BASE_URL;

            //Agregamos configuraciones
            this._Credenciales = Credenciales;
            this._UseCookieCache = useCookieCache;

            //Utilizamos cookies
            this._cookieRepository = new CookieRepository(Credenciales.email);

        }

        public Client(Login Credenciales, bool useCookieCache = false)
        {
            //Instanceamos el cliente http
            this._httpClient = new FlurlClient(new HttpClient());
            this._httpClient.BaseUrl = API_BASE_URL;

            this._Credenciales = Credenciales;
            //Agregamos configuraciones
            this._Credenciales = Credenciales;
            this._UseCookieCache = useCookieCache;
            //Utilizamos cookies
            this._cookieRepository = new CookieRepository(Credenciales.email);
        }

        public async Task<IReadOnlyList< FlurlCookie>> getCookies()
        {
            Console.WriteLine(this._UseCookieCache);
            Console.WriteLine(this._cookieRepository.CookieLoad);

            if (this._UseCookieCache==false | this._cookieRepository.CookieLoad==false)
            {
                Console.WriteLine("-> Obteniendo las cookies");
                var response = await $"{LINKEDIN_BASE_URL}"
                    .WithClient(this._httpClient)
                    .WithHeaders(AUTH_REQUEST_HEADERS)
                    .GetAsync();
                return response.Cookies;
            }
            return this._cookieRepository.Data;
        }
        
        public void SetCookies(IReadOnlyList<FlurlCookie> cookies)
        {
            this._cookies = cookies.ToList();
            this._csrf_token = cookies.ToList().Find(x => x.Name== "JSESSIONID").Value.Replace("\u0022", "");
        }

        public async Task Authenticate()
        {
            if (this._UseCookieCache)
            {
                Console.WriteLine("-> Cargado cookies cache");
                this._cookieRepository = new CookieRepository(this._Credenciales.email);
                if (this._cookieRepository.CookieLoad)
                {
                    SetCookies(this._cookieRepository.Data);
                    await FetchMetadata();
                    return;
                }

                await AuthenticateRequest();
                await FetchMetadata();
            }
        }

        private async Task FetchMetadata()
        {
            Console.WriteLine("-> Solicitando metadata");
            await Task.Delay(1);
        }

        public async Task AuthenticateRequest()
        {

            this._cookies = getCookies().GetAwaiter().GetResult().ToList();
            var payload = $"session_key={this._Credenciales.email}&session_password={this._Credenciales.password}&JSESSIONID={_cookies.Find(x => x.Name == "JSESSIONID").Value}";

            var response = await $"{LINKEDIN_BASE_URL}/uas/authenticate"
                .WithCookies(this._cookies)
                .WithHeaders(AUTH_REQUEST_HEADERS)
                .WithHeader("csrf-token", this._csrf_token)
                .WithHeader("ContentType", "application/x-www-form-urlencoded")
                .WithHeader("ContentLength", Encoding.UTF8.GetBytes(payload).Length)
                .SendUrlEncodedAsync(HttpMethod.Post, payload);

            var data = await response.GetJsonAsync();
            if (data.login_result != "PASS")
            {
                throw new Exception($"-> Error {data.login_result}");
            }
            SetCookies(response.Cookies);
            this._cookieRepository.Save(response.Cookies.ToList());
        }
         
        public async Task<string> GetIp()
        {
            var r = await "https://api.ipify.org"
                .WithClient(this._httpClient)
                .WithHeaders(AUTH_REQUEST_HEADERS)
                .GetAsync();
            return await r.ResponseMessage.Content.ReadAsStringAsync();
        }

    }
}
