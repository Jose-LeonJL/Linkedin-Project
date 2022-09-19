using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Linkedin.Net.Estructuras
{
    public struct getInvitationsStruct
    {
        public List<elementsStruct> elements { get; set; }
    }

    public struct elementsStruct
    {
        public string entityUrn { get; set; }
        public invitationStruct invitation { get; set; }

    } 

    public struct invitationStruct
    {
        public string mailboxItemId { get; set; }
        public string toMemberId { get; set; }
        public memberStruct fromMember { get; set; }
        public string message { get; set; }
        public inviteeStruct invitee { get; set; }
        public string invitationType { get; set; }
        public string entityUrn { get; set; }
        public memberStruct toMember { get; set; }
        public string sentTime { get; set; }
        public bool customMessage { get; set; }
        public string sharedSecret { get; set; }
        public bool unseen { get; set; }
    }


    public struct MemberStruct
    {
        public bool normalized { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string dashEntityUrn { get; set; }
        public string ocupation { get; set; }
        public string objectUrn { get; set; }
        public string entityUrn { get; set; }
        public string publicIdentifier { get; set; }
        public pictureStruct picture { get; set; }
        public string trackingId { get; set; }
    }

    public struct inviteeStruct
    {
        [JsonProperty(PropertyName = "com.linkedin.voyager.relationships.invitation.ProfileInvitee")] public profileInviteeStruct profileInvitee { get; set; }
    }

    public struct profileInviteeStruct
    {
        public memberStruct miniProfile { get; set; }
    }
}