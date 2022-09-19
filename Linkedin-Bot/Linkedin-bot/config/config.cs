using System;
using System.Collections.Generic;
using System.Text;

namespace Linkedin_bot.config
{
    class configuracion
    {

        private static linkedin _linkedin = new linkedin();
        public static linkedin linkedin { get { return _linkedin; } }

    }
    class linkedin
    {
        private static string _url = "https://www.linkedin.com/login/";
        private static login _login = new login();
        private static verificationcode _verificationcode = new verificationcode();
        private static string _busqueda = "#global-nav-typeahead.input[type='text']";
        public string url { get { return _url; } }
        public login login { get { return _login; } }
        public string txtbusqueda { get { return _busqueda; } }
        public verificationcode verificationcode { get { return _verificationcode; } }
        public string urlloginsuccess { get { return "https://www.linkedin.com/feed/"; } }
        public string urlbanned { get { return "https://www.linkedin.com/checkpoint/lg/login-submit"; } }
        public string urlchallange { get { return "https://www.linkedin.com/checkpoint/challenge"; } }      
    }
    class login
    {
        public string txtuser { get { return "#username"; } }
        public string txtpass { get { return "#password"; } }
        public string btnlogin { get { return "button[type='submit']"; } }
    }
    class verificationcode
    {
        private static string _url = "https://www.linkedin.com/checkpoint/challenge/";
        private static string _txtpin = "#input__email_verification_pin";
        private static string _btnpinenviar = "#input__email_verification_pin";
        public string url { get { return _url; } }
        public string txtpin { get { return _txtpin; } }
        public string btnpinenviar { get { return _btnpinenviar; } }
    }
}
