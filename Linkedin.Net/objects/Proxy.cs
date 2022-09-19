using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;

namespace Linkedin.Net.objects
{
    public class Proxy
    {
        //Variables
        private WebProxy _Proxy = null;
        private HttpClientHandler _httpClientHandler = null;
        private CookieContainer _cookieContainer;

        //Propiedades
        public string Ip { get; set; }
        public string Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public HttpClientHandler HttpHandler 
        {
            get
            {
                return this._httpClientHandler;
            }
        }
        
        //Constructor
        public Proxy(string Ip, string Port, string User, string Password)
        {
            this.Ip = Ip;
            this.Port = Port;
            this.User = User;
            this.Password = Password;

            //Cofiguracion del proxy 
            this._Proxy = new WebProxy
            {
                Address = new Uri($"http://{this.Ip}:{this.Port}/"),
                UseDefaultCredentials = false,
                BypassProxyOnLocal = false,
                Credentials = new NetworkCredential
                {
                    UserName = this.User,
                    Password = this.Password
                }
            };
            Console.WriteLine(_Proxy.Address);
            //Configuramos la cookie
            this._cookieContainer = new CookieContainer();

            //Configuracion del HttpHandler
            this._httpClientHandler = new HttpClientHandler()
            {
                Proxy = this._Proxy,
                PreAuthenticate = false,
                UseDefaultCredentials = false,
                UseProxy = true,
                CookieContainer = this._cookieContainer,
                UseCookies = true
            };
        }

    }
}
