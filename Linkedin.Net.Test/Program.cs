using System;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Linkedin.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Linkedin.Net.Estructuras;
using Linkedin.Net.objects;

namespace Linkedin.Net.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string publicIdHenoc = "henoc-hernandez-b00373222";
            string publicIdPaladin = "paladin-oscuro-48384a242";
            string publicIdJose = "jose-leon-176195119";//ACwAADfwVXABGqPNMXnv6OcoItmLCkrkvsw-n7o
            string invUrn = "ACoAAB1kl6oBsziexw7YVNo2S06uzwHAhDGnoN4";
            //var proxy = new Proxy("185.245.25.197", "6458", "usaczprx", "qo7waas0");
            var linkedin = new Linkedin(new Estructuras.Login { email= "asdasdasd", password= "asdasfacsa" },true);
            //var linkedin = new Linkedin(new Estructuras.Login { email = "asdasdasdasd", password = "asdassdasdassd" }, true);
            //Console.WriteLine("Estado de conexion "+ reslut);
            //Console.WriteLine("...");
            //Console.ReadLine();
            //var reslut = linkedin.RemoveConnection("paladin-oscuro-48384a242").GetAwaiter().GetResult();

            //Console.WriteLine("Estado de desconexion "+ reslut);

            //var reslut = linkedin.GetSentInvitations().GetAwaiter().GetResult();
            //Console.WriteLine("Estado de desconexion "+ reslut);

            //var r = linkedin.ReplyInvitation("urn:li:invitation:6944679252411768832", "dummy", "withdraw").GetAwaiter().GetResult();
            //Console.WriteLine("Estado de desconexion " + r);


            //linkedin.sendMessage("f","fgt",new System.Collections.Generic.List<string>()).Wait();

            //Console.WriteLine(linkedin.GetConversation(chats.elements[2].entityUrn.Split(":")[3]).GetAwaiter().GetResult().paging.count);
            //var linkedin = new Linkedin(new Estructuras.Login { email = "asdasdasdasd", password = "asdasdasd" });
            //linkedin.GetProfilePost("howardpinsky").Wait();
            //var result = linkedin.GetProfile("david-andino-28b478234").GetAwaiter().GetResult();
            //linkedin.GetProfilePost("enara-sanchez-24556613a").Wait();
            //linkedin.GetPostComments("6939938851935186944").Wait();
            //linkedin.GetProfileContact("enara-sanchez-24556613a").Wait();
            //linkedin.GetProfileSkills("enara-sanchez-24556613a").Wait();
            //linkedin.GetCurrentProfileViews().Wait();
            //linkedin.GetSchool("universidad-politecnica-de-madrid").Wait();
            //linkedin.GetConversationDetails("ACoAADfwVXABt5lJtrkPEC9kQYFN26Ag26D8aW4").Wait();
            //var inv = linkedin.ReplyInvitation("urn:li:invitation:6943585428012728320", "A").GetAwaiter().GetResult();
            //linkedin.GenerateTRackingId();
            //linkedin.GetConversation("2-MTllYmRjMGQtZjZiMi00ZWU4LWI3MTctNDE2MjU1ZWQ1YWIyXzAxMA==").Wait();
            // linkedin.GetConversationDetails("ACoAADpyHosBwdq-DvtgjNheOtoSENNFL2hcByo").Wait();
            //linkedin.GetProfileMemberBadges(publicIdHenoc).Wait();
            //linkedin.GetProfilePrivacySettings(publicIdHenoc).Wait();
            //linkedin.GetProfilePrivacySettings("henoc-hernandez-b00373222").Wait();
            //var result = linkedin.AddConnection(publicIdPaladin,"hola").GetAwaiter().GetResult();
            //var result = linkedin.Search_Prueba().GetAwaiter().GetResult();
            //var result = linkedin.GetInvitations().GetAwaiter().GetResult();
            //Console.WriteLine(result);//linkedin.AddConnection("henoc-hernandez-b00373222", "Te interesaria un cafe con pan?").Wait();
            //var result = linkedin.GetInvitations().GetAwaiter().GetResult();
            //linkedin.GetCompany("").Wait();
            //linkedin.unfollowEntity("urn:li:company:7046940").Wait();
            //linkedin.GetProfileNetworkInfo(publicIdHenoc).Wait();
            var result = linkedin.GetConversationDetails("ACoAADpyHosBwdq-DvtgjNheOtoSENNFL2hcByo").GetAwaiter().GetResult();
            //linkedin.SetCookies(linkedin.getCookies().GetAwaiter().GetResult());
            //linkedin.AuthenticateRequest().Wait();

            var json = JsonConvert.SerializeObject(result);
            var jo = JToken.Parse(json);

            //var rso = result.elements.Find((obj) => obj.entityUrn=="ACoAADpyHosBwdq-DvtgjNheOtoSENNFL2hcByo");
            //var json = JsonConvert.SerializeObject(rso);
            //var jo = JToken.Parse(json);
            Console.WriteLine(jo);
        }
    }
}

