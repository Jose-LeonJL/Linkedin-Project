using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Linkedin.Net.Estructuras
{
    public struct cookiesStruct
    {
        public DateTimeOffset DateReceived { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTimeOffset? Expires { get; set; }
        public int? MaxAge { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public bool Secure { get; set; }
        public bool HttpOnly { get; set; }
        public SameSite? SameSite { get; set; }

        public override string ToString()
        {
            var name = Name + "=" + Value + ";";
            

            return name;
        }
    }

    public static class CookiesToString
    {
        public static string ToCookieString(this IEnumerable<cookiesStruct> values )
        {
            var result = "";
            foreach (var item in values)
            {
                result += item.ToString();
            }

            return result;
        }
    }
}
