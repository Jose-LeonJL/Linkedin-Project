using System;
using Flurl;
using Flurl.Http;
using System.Linq;
using System.Web;
using System.Text;
using Linkedin.Net;
using Newtonsoft.Json;
using System.Net.Http;
using Linkedin.Net.objects;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Linkedin.Net.Estructuras;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Linkedin.Net
{
    public class Linkedin
    {
        //variables
        private const string LINKEDIN_BASE_URL = "https://www.linkedin.com";
        private readonly string API_BASE_URL = $"{LINKEDIN_BASE_URL}/voyager/api";
        private Client _client;
        public dynamic _result;
        public int _MAX_POST_COUNT = 100;

        public int _MAX_UPDATE_COUNT = 100;

        public int _MAX_SEARCH_COUNT = 49;

        public int _MAX_REPEATED_REQUESTS = 200;

        public Client Cliente
        {
            get
            {
                return _client;
            }
        }

        //constructores
        public Linkedin(Login credenciales, Proxy proxy, bool useCookieCache = false)
        {
            this._client = new Client(credenciales, proxy, useCookieCache);
            this._client.Authenticate().Wait();

        }

        public Linkedin(Login credenciales, bool useCookieCache = false)
        {
            this._client = new Client(credenciales, useCookieCache);
            this._client.Authenticate().Wait();

        }

        //metodos
        public async Task<getProfileStruct> GetProfile(string publicID)
        {
            var response = await $"{API_BASE_URL}/identity/profiles/{publicID}/profileView"
                .WithCookies(_client.cookies)
                .WithHeader("csrf-token", _client.csrf_token)
                .GetJsonAsync<getProfileStruct>();
          
            return response;
        }

        public async Task<dynamic> GetProfilePost(string publicID = null, string urnID = null, int postCount = 10)
        {
            List<dynamic> Datas = new List<dynamic>(); 
            var urlParams = new Dictionary<object, object>
            {
                { "count",Math.Min(postCount, this._MAX_POST_COUNT) },
                { "start", 0},
                { "q", "memberShareFeed" },
                { "moduleKey", "member-shares:phone" },
                { "includeLongTermHistory", true },
            };

            string profile_urn;

            if (urnID != null)
            {
                profile_urn = $"urn:li:fsd_profile:{urnID}";
            }
            else
            {
                var profile = await GetProfile(publicID);
                string profile_ur = Convert.ToString(profile.entityUrn);
                profile_urn = profile_ur.Replace("urn:li:fs_profile", "urn:li:fsd_profile");
            };
            urlParams.Add("profileUrn", profile_urn);

            var response = await $"{API_BASE_URL}/identity/profileUpdatesV2"
                .WithCookies(_client.cookies)
                .WithHeader("csrf-token", _client.csrf_token)
                .SetQueryParams(urlParams)
                .GetJsonAsync();

            var responseJson = JsonConvert.SerializeObject(response);
            dynamic data = JValue.Parse(responseJson);
   
            Console.WriteLine(data["elements"]);
            return data;
        }

        public async Task<dynamic> GetPostComments(string postUrn, int commentCount = 100)
        {
            var urlParams = new Dictionary<object, object>
            {
                {"count", Math.Min(commentCount, this._MAX_POST_COUNT)},
                {"start", 0},
                {"q", "comments"},
                {"sortOrder", "REVELANCE"}
            };

            urlParams.Add("updateId", "activity:" + postUrn);
            var response = await $"{API_BASE_URL}/feed/comments"
                .WithCookies(this._client.cookies)
                .WithHeader("csrf-token", _client.csrf_token)
                .SetQueryParams(urlParams)
                .GetJsonAsync();

            var responseJson = JsonConvert.SerializeObject(response);
            dynamic data = JValue.Parse(responseJson);

            Console.WriteLine(data.elements);
            return data;
        }
        public async Task<dynamic> Search_Prueba()
        {
            List<string> regions = new List<string>();
            regions.Add("102571732");
            var filters = new List<Dictionary<object, object>>();
            var geoUrn = new Dictionary<object, object>
            {
                { "geoUrn", regions}
            };
            filters.Add(geoUrn);
            var parametros = new Dictionary<object, object>
            {
                //{"id", 3},
                {"keywords", "Nigel Shamash"},
                {"filters", filters},

                {"q", "all"},
                {"origin", "TYPEAHEAD_ESCAPE_HATCH"},
                {"start", 0},
                {"count", 10 },
                { "primaryHitType", "PEOPLE" }
            };

            try
            {
                var response = await $"{API_BASE_URL}/search/blended"
              .WithCookies(this._client.cookies)
              .WithHeader("csrf-token", this._client.csrf_token)
              .SetQueryParams(parametros)
              .WithHeader("accept", "application/vnd.linkedin.normalized+json+2.1")
              .GetJsonAsync();

                var responseJson = JsonConvert.SerializeObject(response);
                dynamic data = JValue.Parse(responseJson);
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dynamic info = "no dio";
                return info;
            }


        }

        //Importante
        public async Task<List<dynamic>> Search(Dictionary<string, object> parametros, int limit = -1, int offset = 0)
        {
            var count = _MAX_SEARCH_COUNT;

            
            List<dynamic> results = new List<dynamic>();
            int i = 0;
            while (i < 1)
            {

                if (limit > -1 & limit - results.Count < count)
                {
                    count = limit - results.Count;
                }
                var default_params = new Dictionary<string, object> {
                        {
                            "count",
                            count.ToString()},
                        {
                            "filters",
                            parametros["filters"]},
                        {
                            "origin",
                            "GLOBAL_SEARCH_HEADER"},
                        {
                            "q",
                            "all"},
                        {
                            "start",
                            results.Count + offset},
     
                        {
                            "queryContext",
                            "List(spellCorrectionEnabled->true,relatedSearchesEnabled->true,kcardTypes->PROFILE|COMPANY)"}};


                //default_params["filters"] = parametros["filters"];

               string url = $"{API_BASE_URL}/search/blended?{string.Join("&", default_params.Select(kvp => $"{kvp.Key}={kvp.Value}"))}";

                var response = await $"{API_BASE_URL}/search/blended?{HttpUtility.ParseQueryString(string.Join("&", default_params.Select(kvp => $"{kvp.Key}={kvp.Value}")))}"
                  .WithCookies(this._client.cookies)
                  .WithHeader("csrf-token", this._client.csrf_token)
                  //.SetQueryParams(default_params)
                  .WithHeader("accept", "application/vnd.linkedin.normalized+json+2.1")
                  .GetJsonAsync();

                var responseJson = JsonConvert.SerializeObject(response);
                dynamic data = JValue.Parse(responseJson);
                

                try
                {

                    Console.WriteLine(data.data);
                    results.Add(data);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }


                i++;
                if(((-1<limit & limit <=results.Count) | results.Count / count >= _MAX_SEARCH_COUNT) | results.Count == 0)
                {
                    break;
                }
            }

               
            
            return results;
        }

        //Importante
        public async Task<dynamic> SearchPeople(
            string keywords = null,
            string connection_of = null,
            List<string> network_depths = null,
            List<string> current_company = null,
            List<string> past_companies = null,
            string nonprofit_interests = null,
            List<string> profile_languages = null,
            List<string> regions = null,
            List<string> industries = null,
            List<string> schools = null,
            List<string> contact_interests = null,
            List<string> service_categories = null,
            bool include_private_profiles = false,
            string keyword_first_name = null,
            string keyword_last_name = null,
            string keyword_title = null,
            string keyword_company = null,
            string keyword_school = null,
            List<string> network_depth = null,
            string title = null
    )
        {
            List<object> filters = new List<object> {
                    "resultType->PEOPLE"
                };
            if (connection_of != null)
            {
                filters.Add($"connectionOf->{connection_of}");
            }
            if (network_depths != null)
            {
                filters.Add($"network->|{string.Join('|',network_depths)}");
            }
            else if (network_depth != null)
            {
                filters.Add($"network->{network_depth}");
            }
            if (regions != null)
            {
                filters.Add($"geoUrn->|{string.Join('|',regions)}");
            }
            if (industries != null)
            {
                filters.Add($"industry->{string.Join('|', industries)}");
            }
            if (current_company != null)
            {
                filters.Add($"currentCompany->{string.Join('|', current_company)}");
            }
            if (past_companies != null)
            {
                filters.Add($"pastCompany->{string.Join('|', past_companies)}");
            }
            if (profile_languages != null)
            {
                filters.Add($"profileLanguage->{string.Join('|', profile_languages)}");
            }
            if (nonprofit_interests != null)
            {
                filters.Add($"nonprofitInterest->{string.Join('|', nonprofit_interests)}");
            }
            if (schools != null)
            {
                filters.Add($"schools->{string.Join('|', schools)}");
            }
            if (service_categories != null)
            {
                filters.Add($"serviceCategory->{string.Join('|', service_categories)}");
            }
            // `Keywords` filter
            if(keyword_title == null)
            {
                keyword_title = title;
            }
           
            if (keyword_first_name != null)
            {
                filters.Add($"firstName->{keyword_first_name}");
            }
            if (keyword_last_name != null)
            {
                filters.Add($"lastName->{keyword_last_name}");
            }
            if (keyword_title != null)
            {
                filters.Add($"title->{keyword_title}");
            }
            if (keyword_company != null)
            {
                filters.Add($"company->{keyword_company}");
            }
            if (keyword_school != null)
            {
                filters.Add($"school->{keyword_school}");
            }

            var parametros = new Dictionary<string, object>
            {
                {"filters", $"List({string.Join(',', filters)})"}
            };

            if(keywords != null)
            {
                parametros["keywords"] = keywords;
            }

            List<dynamic> data = await Search(parametros);
            Console.WriteLine(data);
            return data;
        }

        public async Task<dynamic> SearchCompany()
        {
            string message = "no terminado";
            return message;
        }

        public async Task<dynamic> Searchjobs()
        {
            string message = "no terminado";
            return message;
        }

        public async Task<Dictionary<object, object>> GetProfileContact(string publicID)
        {

            var response = await $"{API_BASE_URL}/identity/profiles/{publicID}/profileContactInfo"
            .WithCookies(_client.cookies)
            .WithHeader("csrf-token", _client.csrf_token)
                .GetJsonAsync();

            var responseJson = JsonConvert.SerializeObject(response);
            dynamic data = JValue.Parse(responseJson);

            var contactInfo = new Dictionary<object, object>
            {
                {"emailAddress", data.emailAddress},
                {"websites", data.websites},
                {"twitter", data.twitterHandles},
                {"birthdate", data.birthDateOn},
                {"ims",data.ims},
                {"phoneNumbers", data.phoneNumbers}
            };

            Console.WriteLine(JsonConvert.SerializeObject(contactInfo));
            return contactInfo;
        }

        public async Task<dynamic> GetProfileSkills(string publicID)
        {
            var parametros = new Dictionary<object, object>
            {
                {"count", 100},
                {"start", 0}
            };
            var response = await $"{API_BASE_URL}/identity/profiles/{publicID}/skills"
                .WithCookies(_client.cookies)
                .WithHeader("csrf-token", _client.csrf_token)
                .WithHeaders(parametros)
                .GetJsonAsync();

            var responseJson = JsonConvert.SerializeObject(response);
            dynamic data = JValue.Parse(responseJson);

            Console.WriteLine(data.elements);
            return data.elements;
        }

        //Medio
        public async Task<dynamic> GetProfileConnections(string urnId)
        {
            return "no terminado";
        }

        public async Task<dynamic> GetCompanyUpdates(string publicIdOrUrnId = null, int maxResults = 0, Array result = null)
        {
            var parametros = new Dictionary<object, object>
            {
                {"companyUniversalName", publicIdOrUrnId},
                {"q", "companyFeedByUniversalName"},
                {"moduleKey","member-share"},
                {"count", this._MAX_UPDATE_COUNT},
                {"start", result.Length}
            };

            var response = await $"{API_BASE_URL}/feed/updates"
                .WithCookies(this._client.cookies)
                .WithHeader("csrf-token", this._client.csrf_token)
                .SetQueryParams(parametros)
                .GetJsonAsync();

            var responseJson = JsonConvert.SerializeObject(response);
            dynamic data = JValue.Parse(responseJson);

            if (data.elements.Lenght == 0 |
            (maxResults != 0 & result.Length >= maxResults) |
                maxResults != 0 & result.Length / maxResults >= this._MAX_REPEATED_REQUESTS)
            {
                return result;
            }

            result = data.elements;
            Console.WriteLine($"RESULTS GREW: {result.Length}");

            return this.GetCompanyUpdates(
                publicIdOrUrnId,
                maxResults,
                result
                );
        }

        public async Task<dynamic> GetProfileUpdates(string publicIdOrUrnId = null, int maxResults = 0, Array result = null)
        {
            var parametros = new Dictionary<object, object>
            {
                {"profileId", publicIdOrUrnId},
                {"q", "memberShareFeed"},
                {"moduleKey","member-share"},
                {"count", this._MAX_UPDATE_COUNT},
                {"start", result.Length}
            };

            var response = await $"{API_BASE_URL}/feed/updates"
                .WithCookies(this._client.cookies)
                .WithHeader("csrf-token", this._client.csrf_token)
                .SetQueryParams(parametros)
                .GetJsonAsync();

            var responseJson = JsonConvert.SerializeObject(response);
            dynamic data = JValue.Parse(responseJson);

            if (data.elements.Lenght == 0 |
            (maxResults != 0 & result.Length >= maxResults) |
                maxResults != 0 & result.Length / maxResults >= this._MAX_REPEATED_REQUESTS)
            {
                return result;
            }

            result = data.elements;
            Console.WriteLine($"RESULTS GREW: {result.Length}");

            return this.GetCompanyUpdates(
                publicIdOrUrnId,
                maxResults,
                result
                );
        }

        public async Task<dynamic> GetCurrentProfileViews()
        {
            var response = await $"{API_BASE_URL}/identity/wvmpCards"
                .WithCookies(this._client.cookies)
                .WithHeader("csrf-token", this._client.csrf_token)
                .GetJsonAsync();

            var responseJson = JsonConvert.SerializeObject(response);
            dynamic data = JValue.Parse(responseJson);

            var retorno = data.elements[0].value["com.linkedin.voyager.identity.me.wvmpOverview.WvmpViewersCard"]["insightCards"][0]["value"][
            "com.linkedin.voyager.identity.me.wvmpOverview.WvmpSummaryInsightCard"
        ][
            "numViews"
        ];
            Console.WriteLine(retorno);
            return retorno;
        }

        public async Task<dynamic> GetSchool(string publicId)
        {
            var parametros = new Dictionary<string, string>
            {
                {"decorationId", "com.linkedin.voyager.deco.organization.web.WebFullCompanyMain-12"},
                {"q", "universalName"},
                {"universalName", publicId}
            };

            var response = await $"{API_BASE_URL}/organization/companies?"
                .WithCookies(this._client.cookies)
                .WithHeader("csrf-token", this._client.csrf_token)
                .SetQueryParams(parametros)
                .GetJsonAsync();

            var responseJson = JsonConvert.SerializeObject(response);
            dynamic data = JValue.Parse(responseJson);

            var school = data["elements"][0];
            Console.WriteLine(school);
            return school;
        }

        public async Task<dynamic> GetCompany(string publicID)
        {
            var parametros = new Dictionary<string, string>
            {
                { "decorationId", "com.linkedin.voyager.deco.organization.web.WebFullCompanyMain-12"},
                { "q", "universalName"},
                { "universalName", publicID},
            };

            var response = await $"{API_BASE_URL}/organization/companies"
                .WithCookies(_client.cookies)
                .WithHeader("csrf-token", _client.csrf_token)
                .SetQueryParams(parametros)
                .GetJsonAsync();

            var responseJson = JsonConvert.SerializeObject(response);
            dynamic data = JValue.Parse(responseJson);

            Console.WriteLine(data["elements"][0]);
            return data["elements"][0];
        }

        //public conversationsStruct getconversation
        //Importante
        public async Task<dynamic> GetConversationDetails(string profileUrnId)
        {
            var parametros = new Dictionary<string, string>
            {
                {"keyVersion", "LEGACY_INBOX"},
                {"q", "participants"},
                {"recipients", $"List({profileUrnId})"}
            };


            dynamic item;
            var response = await $"{API_BASE_URL}/messaging/conversations?{HttpUtility.UrlEncode($"keyVersion=LEGACY_INBOX&q=participants&recipients=List({profileUrnId})")}"
                .WithCookies(this._client.cookies)
                .WithHeader("csrf-token", this._client.csrf_token)
                //.SetQueryParams(parametros)
                .GetAsync();

            var responseJson = JsonConvert.SerializeObject(response.GetJsonAsync());
            dynamic data = JValue.Parse(responseJson);




            Console.WriteLine(data.Result.elements);
            return data; 
            }

        //Importante
        public async Task<conversationsStruct> GetConversations()
        {
            var parametros = new Dictionary<string, string>
            {
                {"keyVersion", "LEGACY_INBOX"}
            };

            var response = await $"{API_BASE_URL}/messaging/conversations"
                .WithCookies(this._client.cookies)
                .WithHeader("csrf-token", this._client.csrf_token)
                .SetQueryParams(parametros)
                .GetJsonAsync<conversationsStruct>();

            return response;
        }
        //Importante
        public async Task<conversationsStruct> GetConversation(string conversationUrnId)
        {
            var response = await $"{API_BASE_URL}/messaging/conversations/{conversationUrnId}/events"
                .WithCookies(this._client.cookies)
                .WithHeader("csrf-token", this._client.csrf_token)
                .GetJsonAsync<conversationsStruct>();
            
            return response;
        }

        public string generateTrackingIdAsCharString()
        {
            var randomIntArray = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                randomIntArray.Add(new Random().Next(256));
            }
            var randByteArray = new List<byte>();
            foreach(var numero in randomIntArray)
            {
                randByteArray.Add(Convert.ToByte(numero));
            }

            var result = (from rd in randByteArray select Convert.ToChar(rd));
            return new String(result.ToArray());
        }

        //importante
        public async Task<bool> sendMessage(string messageBody, string conversationUrnId=null,  List<string> recipients=null)
        {
            var result = false;
            var parametros = new Dictionary<string, string>
            {
                {"action", "create"}
            };
            
            if (!(conversationUrnId != null | recipients != null))
            {
                Console.WriteLine("Must provide [conversation_urn_id] or [recipients].");
                return false;
            }

            var messageEvent = new Dictionary<string, object>()
            {
                {"eventCreate", new Dictionary<string, object>()
                {
                    {"originToken",Guid.NewGuid()},
                    {"value",new Dictionary<string, object>()
                    {
                        {"com.linkedin.voyager.messaging.create.MessageCreate",new Dictionary<string, object>()
                        {
                            {"attributedBody",new Dictionary<string,object>()
                            {
                                {"text",messageBody },
                                {"attributes" ,new List<object>()}
                            } },
                            {"attachments",new List<object>() }
                        } 
                        }
                    } },
                    {"trackingId" ,generateTrackingIdAsCharString()}
                }},
                {"dedupeByClientGeneratedToken",false }
            };
            Console.WriteLine((recipients != null & !(conversationUrnId == null)));
            if(conversationUrnId != null && !(recipients != null))
            {
                var response = await $"/messaging/conversations/{conversationUrnId}/events"
                    .WithCookies(_client.cookies)
                    .WithHeader("csrf-token", _client.csrf_token)
                    .SetQueryParams(parametros)
                    .PostJsonAsync(messageEvent);
                Console.WriteLine("xd");
                result = response.StatusCode != 201;
            }
            else if(recipients != null && !(conversationUrnId == null))
            {
                Console.WriteLine("no xd");
                messageEvent["recipients"] = recipients;
                messageEvent["subtype"] = "MEMBER_TO_MEMBER";
                var payload = new Dictionary<string, object>()
                {
                    {"keyVersion","LEGACY_INBOX" },
                    {"conversationCreate",messageEvent }
                };
                var response = await $"/messaging/conversations"
                    .WithCookies(_client.cookies)
                    .WithHeader("csrf-token", _client.csrf_token)
                    .SetQueryParams(parametros)
                    .PostJsonAsync(payload);
                result = response.StatusCode != 201;
            }
            //falta terminar
            
            return result;
        }

        //Importante
        public async Task <bool>MarkConversationAsSeen(string conversationUrnId)
        {
            var payload = new Dictionary<object, object> {
                    {
                        "patch",
                        new Dictionary<object, object> {
                            {
                                "$set",
                                new Dictionary<object, object> {
                                    {
                                        "read",
                                        true}}}}}};
            string data = JsonConvert.SerializeObject(payload);

            var response = await $"{API_BASE_URL}/messaging/conversations/{conversationUrnId}"
                .WithCookies(this._client.cookies)
                .WithHeader("csrf-token", this._client.csrf_token)
                .SendStringAsync(HttpMethod.Post, data);

            return response.StatusCode == 200;
        }

        public async Task<dynamic> GetUserProfile()
        {
            var response = await $"{API_BASE_URL}/me"
                .WithCookies(_client.cookies)
                .WithHeader("csrf-token", _client.csrf_token)
                .GetJsonAsync();

            var responseJson = JsonConvert.SerializeObject(response);
            dynamic data = JValue.Parse(responseJson);

            Console.WriteLine(data);
            return data;

        }

        //Medio <Listo>
        public async Task<getInvitationsStruct> GetInvitations(int start = 0, int limit = 3)
        {
            getInvitationsStruct resp = new getInvitationsStruct();
            var parametros = new Dictionary<object, object>
            {
                {"start", start},
                {"count", limit},
                {"includeInsights", true},
                {"q", "receivedInvitation"}
            };

            try
            {
                var response = await $"{API_BASE_URL}/relationships/invitationViews"
                .WithCookies(_client.cookies)
                .WithHeader("csrf-token", _client.csrf_token)
                .SetQueryParams(parametros)
                .GetJsonAsync<getInvitationsStruct>();

                return response;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return resp;
            }

        }
        //Medio
        public async Task<bool> ReplyInvitation(string invitation_entity_urn, string invitation_shared_secret, string action= "accept")
        {
            var invitationId = invitation_entity_urn.Split(":")[3];
            var parametros = new Dictionary<string, string>
            {
                {"action", action}
            };
            var response = await $"{API_BASE_URL}/relationships/invitations/{invitationId}"
                .WithCookies(_client.cookies)
                .WithHeader("csrf-token", _client.csrf_token)
                .SetQueryParams(parametros)
                .PostJsonAsync(new
                {
                    invitationId = invitation_entity_urn,
                    invitationSharedSecret= invitation_shared_secret,
                    isGenericInvitation= false
                });

            return response.StatusCode == 200;
        }

        public string GenerateTRackingId()
        {
            var randomIntArray = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                randomIntArray.Add(new Random().Next(256));
            }
            var randByteArray = new List<byte>();
            foreach (var numero in randomIntArray)
            {
                randByteArray.Add(Convert.ToByte(numero));
            }
            return Convert.ToBase64String(randByteArray.ToArray());

        }

        public async Task<getInvitationsStruct> GetSentInvitations(int start = 0, int limit = 100)
        {
            var parametros = new Dictionary<object, object>
            {
                {"start", start},
                {"count", limit},
                {"invitationType", "CONNECTION"},
                {"q", "invitationType"}
            };
            var response = await $"{API_BASE_URL}/relationships/sentInvitationViewsV2"
                .WithCookies(_client.cookies)
                .WithHeader("csrf-token", _client.csrf_token)
                .SetQueryParams(parametros)
                .GetJsonAsync<getInvitationsStruct>();

            var data = JsonConvert.SerializeObject(response);

            return response;
        }

        //Importante
        public async Task<bool> AddConnection(string profilePublicId, string message = "", string profileUrn = "")
        {
            if (message.Length > 300)
            {
                Console.WriteLine("Message too long. Max size is 300 characters");
                throw new Exception("Message too long. Max size is 300 characters");
            }
            if (profileUrn == string.Empty | profileUrn == null)
            {
                var profileUrnString = await this.GetProfile(profilePublicId);
                profileUrn = profileUrnString.profile.entityUrn.Replace("urn:li:fs_profile:", "");
                string trackingId = GenerateTRackingId();

                string payload = "{" +
                    $"\u0022trackingId\u0022:\u0022{trackingId}\u0022," +
                    $"\u0022message\u0022:\u0022{message}\u0022," +
                    $"\u0022invitations\u0022:[]," +
                    "\u0022excludeInvitations\u0022:[],\u0022invitee\u0022:{\u0022com.linkedin.voyager.growth.invitation.InviteeProfile\u0022:{" +
                    $"\u0022profileId\u0022:\u0022{profileUrn}\u0022" +
                    "}}}";
                var response = await $"{API_BASE_URL}/growth/normInvitations"
                    .WithCookies(this._client.cookies)
                    .WithHeader("csrf-token", this._client.csrf_token)
                    .WithHeader("accept", "application/vnd.linkedin.normalized+json+2.1")
                    .SendStringAsync(HttpMethod.Post,payload);
                return response.StatusCode == 201;
            }
            return false;
         }
        //Importante
        public async Task<bool> RemoveConnection(string publicProfileId)
        {
            var response = await $"{API_BASE_URL}/identity/profiles/{publicProfileId}/profileActions"
                .WithCookies(this._client.cookies)
                .WithHeader("csrf-token", this._client.csrf_token)
                .SetQueryParam("action", "disconnect")
                .WithHeader("accept", "application/vnd.linkedin.normalized+json+2.1")
                .PostAsync();

            return response.StatusCode == 200;
        }

        public async Task<bool> DeleteConnection(string invitation_entity_urn, string invitation_shared_secret)
        {
            var invitationId = invitation_entity_urn.Split(":")[3];
            var response = await $"{API_BASE_URL}/relationships/invitations/{invitationId}"
                .WithCookies(_client.cookies)
                .WithHeader("csrf-token", _client.csrf_token)
                .SetQueryParam("action", "withdraw")
                .PostJsonAsync(new
                {
                    invitationId = invitation_entity_urn,
                    invitationSharedSecret = invitation_shared_secret,
                    isGenericInvitation = false
                });

            return response.StatusCode == 200;
        }

        public async Task<bool> Track(Dictionary<object, object> eventBody, Dictionary<object, object> eventInfo)
        {
            var payload = new Dictionary<object, object>
            {
                {"eventBody", eventBody},
                {"eventInfo", eventInfo}
            };

            var data = JsonConvert.SerializeObject(payload);

            var headers = new Dictionary<string, string>
            {
                {"accept", "*/*"},
                {"content-type", "text/plain;charset=UTF-8"}
            };

            var response = await $"{API_BASE_URL}/li/track"
                 .WithCookies(_client.cookies)
                 .WithHeader("csrf-token", _client.csrf_token)
                 .WithHeaders(headers)
                 .PostJsonAsync(data);

            return response.StatusCode == 200;
        }

        public async Task<dynamic> GetProfilePrivacySettings(string publicId)
        {

            var response = await $"{API_BASE_URL}/identity/profiles/{publicId}/privacySettings"
                 .WithCookies(_client.cookies)
                 .WithHeader("csrf-token", _client.csrf_token)
                 .WithHeader("accept", "application/vnd.linkedin.normalized+json+2.1")
                 .GetAsync();

            if (response.StatusCode != 200)
            {
                return "{}";
            }
            else
            {
                var responseJson = JsonConvert.SerializeObject(response.GetJsonAsync().Result);
                dynamic data = JToken.Parse(responseJson);

                Console.WriteLine(data["data"]);
                return data["data"];
            }
        }

        public async Task<dynamic> GetProfileMemberBadges(string publicProfileId)
        {

            var response = await $"{API_BASE_URL}/identity/profiles/{publicProfileId}/memberBadges"
               .WithCookies(_client.cookies)
               .WithHeader("csrf-token", _client.csrf_token)
               .WithHeader("accept", "application/vnd.linkedin.normalized+json+2.1")
               .GetJsonAsync();

            var responseJson = JsonConvert.SerializeObject(response);
            dynamic data = JValue.Parse(responseJson);

            Console.WriteLine(data);
            return data;

        }

        public async Task<dynamic> GetProfileNetworkInfo(string publicProfileId)
        {

            var response = await $"{API_BASE_URL}/identity/profiles/{publicProfileId}/networkinfo"
               .WithCookies(_client.cookies)
               .WithHeader("csrf-token", _client.csrf_token)
               .WithHeader("accept", "application/vnd.linkedin.normalized+json+2.1")
               .GetAsync();

            if (response.StatusCode != 200)
            {
                return Array.Empty<string>();
            }
            var responseJson = JsonConvert.SerializeObject(response.GetJsonAsync());
            dynamic data = JValue.Parse(responseJson);

            Console.WriteLine(data);
            return data;

        }

        public async Task<bool> unfollowEntity(string urnId)
        {
            var payload = new
            {
                urn = $"urn:li:fs_followingInfo:{urnId}"
            };

            var data = JsonConvert.SerializeObject(payload);
            var response = await $"{API_BASE_URL}/feed/follows?action=unfollowByEntityUrn"
              .WithCookies(_client.cookies)
              .WithHeader("csrf-token", _client.csrf_token)
              .WithHeader("accept", "application/vnd.linkedin.normalized+json+2.1")
              .PostJsonAsync(new { urn = $"urn:li:fs_followingInfo:{urnId}"});

            bool err;
            if (response.StatusCode != 200)
            {
                Console.WriteLine(response.StatusCode.ToString());
                err = true;
            }
            else
            {
                err = false;
            }
            Console.WriteLine(err);
            return err; 
         }   

       
    }
}
