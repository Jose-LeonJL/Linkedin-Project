using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Flurl.Http;
using Linkedin.Net.Estructuras;

namespace Linkedin.Net.objects
{
    public class CookieRepository
    {
        //Variables
        private readonly string CookiesDir = Path.Join(Directory.GetCurrentDirectory(), "Cookies");
        private string username;
        private readonly bool _cookieLoad;
        private List<FlurlCookie> _data;
        

        //Propiedades
        public string CookieDirectory
        {
            get
            {
                return Path.Join( this.CookiesDir , this.username +".json");
            }
        }
        public bool CookieLoad
        {
            get
            {
                return this._cookieLoad;
            }
        }
        public List<FlurlCookie> Data
        {
            get
            {
                return this._data;
            }
        }

        //Constructor
        public CookieRepository(string userName)
        {
            //Cargamos Datos
            this.username = userName;

            //Verificamos existencia de la cookie
            if (VerificationCookieExist())
            {
                this._data = new List<FlurlCookie>();
                Console.WriteLine("-> La cookie existe en el fichero de archivo");
                foreach(var cookie in JsonSerializer.Deserialize<List<cookiesStruct>>(File.ReadAllText(CookieDirectory)))
                {
                    this._data.Add(new FlurlCookie(cookie.Name, cookie.Value)
                    {
                        Domain = cookie.Domain,
                        HttpOnly = cookie.HttpOnly,
                        Expires = cookie.Expires,
                        MaxAge = cookie.MaxAge,
                        Path = cookie.Path,
                        SameSite = cookie.SameSite,
                        Secure = cookie.Secure
                    });
                }
               
                this._cookieLoad = true;
            }
            else
            {
                Console.WriteLine("-> La cookie no existe en el fichero de archivo");
            }


            //Verificamos que la cookie no este vencida
            if (!CookieValidate())
            {
                Console.WriteLine("-> La cookie no expirado");
                this._cookieLoad = true;
            }
            else
            {
                Console.WriteLine("-> La cookie expiro");
                this._cookieLoad = false;
            }

        }
        
        //Metodos
        /// <summary>
        /// Metodo encargado de guardar la cookie 
        /// </summary>
        /// <returns></returns>
        public bool Save(List<FlurlCookie> cookies)
        {
            var result = false;
            try
            {
                if (!Directory.Exists(this.CookiesDir)) Directory.CreateDirectory(this.CookiesDir);
                File.WriteAllText(this.CookieDirectory, JsonSerializer.Serialize(cookies));
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Metodo encargado de devolver un valor de la cookie almacinada
        /// </summary>
        /// <param name="key">Llave del valor que se desea obtener</param>
        /// <returns></returns>
        public FlurlCookie Get(string key)
        {
            try
            {
                var result = this._data.Find(x=> x.Name == key);
                return result;
            }
            catch (Exception)
            {
                Console.WriteLine("-> Error no se pudo obtener el objeto deseado");
            }
            return null;
        }
 
        /// <summary>
        /// Metido para verificar la existencia del archivo de la cookie
        /// </summary>
        /// <returns></returns>
        public bool VerificationCookieExist()
        {
            return File.Exists(CookieDirectory);
        }

        /// <summary>
        /// Metodo encargado de validar la fecha de expiracion de la cookie
        /// </summary>
        /// <returns></returns>
        public bool CookieValidate()
        {
            try
            {
                var Reason = "";
                return CookieCutter.IsExpired(Get("JSESSIONID"),out Reason);
            }catch(Exception ex)
            {
                return true;
            }
        }

        public bool LoadCookie(List<FlurlCookie> data)
        {
            var result = false;
            try
            {
                if (!Directory.Exists(this.CookiesDir)) Directory.CreateDirectory(this.CookiesDir);
                File.WriteAllText(CookieDirectory,JsonSerializer.Serialize(data));
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("-> Error guardando la cookie");
                Console.WriteLine(e.ToString());
                result = false;
            }
            return result;
            
        }
    }
}
