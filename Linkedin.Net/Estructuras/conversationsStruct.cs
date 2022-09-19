using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Linkedin.Net.Estructuras
{
    public struct conversationsStruct
    {
        public metadataStruct metadata { get; set; }
        public List<messageStruct> elements { get; set; }
        public pagingStruct paging { get; set; }

    }

    //Estructura Metadata 
    public struct metadataStruct
    {
        public int unreadCount { get; set; }
    }

    //Estructura Mensajes
    public struct messageStruct
    {
        public bool read { get; set; }
        public string entityUrn { get; set; }
        public int totalEventCount { get; set; }
        public bool muted { get; set; }
        public List<eventsStruct> events { get; set; }
        public List<participantsStruct> participants { get; set; }

    }

    // Estructura de eventos 
    public struct eventsStruct
    {
        public long createdAt { get; set; }
        public string entityUrn { get; set; }
        public string subtype { get; set; }
        public eventContentStruct eventContent { get; set; }
        public fromStruct from { get; set; }

    }

    //Estructura de contenedor de eventos
    public struct eventContentStruct
    {
        [JsonPropertyName("com.linkedin.voyager.messaging.event.MessageEvent")]
        [JsonProperty("com.linkedin.voyager.messaging.event.MessageEvent")]
        public EventMessageEventStruct EventMessageEvent { get; set; }
        [JsonPropertyName("com.linkedin.voyager.messaging.event.StickerEvent")]
        [JsonProperty("com.linkedin.voyager.messaging.event.StickerEvent")]
        public EventStickerEventStruct EventStickerEvent { get; set; }
    }
    
    //Estructura de evento MensajeEvento
    public struct EventMessageEventStruct
    {
        public List<object> attachments { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public attributedBodyStruct attributedBody { get; set; }
    }

    public struct attributedBodyStruct
    {
        public string text { get; set; }
        public List<object> attributes { get; set; }
    }
    //Estructuras de evento StickerEvent 
    public struct EventStickerEventStruct
    {
        public stickerStruct sticker { get; set; }
    }

    //Estructura de Sticker
    public struct stickerStruct
    {
        public imageStruct image { get; set; }
        public string entityUrn { get; set; }
    }
    public struct imageStruct
    {
        [JsonPropertyName("com.linkedin.voyager.common.MediaProcessorImage")]
        [JsonProperty("com.linkedin.voyager.common.MediaProcessorImage")]
        public MediaProcessorImageStruct MediaProcessorImage { get; set; }
        public string entityUrn { get; set; }
    }
    public struct MediaProcessorImageStruct
    {
        public string id { get; set; }
    }

    //Estructura de from
    public struct fromStruct
    {
        [JsonPropertyName("com.linkedin.voyager.messaging.MessagingMember")]
        [JsonProperty("com.linkedin.voyager.messaging.MessagingMember")]
        public MessagingMemberStruct MessagingMember { get; set; }
    }

    //Estructura messaging.MessagingMember
    public struct MessagingMemberStruct
    {
        public string entityUrn { get; set; }
        public miniProfileStruct miniProfile { get; set; }
    }
    public struct miniProfileStruct
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string occupation { get; set; }
        public string objectUrn { get; set; }
        public string entityUrn { get; set; }
        public string publicIdentifier { get; set; }
        public string trackingId { get; set; }
        public PictureStruct picture { get; set; }
        public backgroundImageStruct backgroundImage { get; set; }
    }
    public struct PictureStruct
    {
        [JsonPropertyName("com.linkedin.voyager.common.MediaProcessorImage")]
        [JsonProperty("com.linkedin.voyager.common.MediaProcessorImage")]
        public MediaProcessorImageStruct MediaProcessorImage { get; set; }
    }
    public struct backgroundImageStruct
    {
        [JsonPropertyName("com.linkedin.voyager.common.MediaProcessorImage")]
        [JsonProperty("com.linkedin.voyager.common.MediaProcessorImage")]
        public MediaProcessorImageStruct MediaProcessorImage { get; set; }
    }

    //Estructura participants
    public struct participantsStruct
    {
        [JsonPropertyName("com.linkedin.voyager.messaging.MessagingMember")]
        [JsonProperty("com.linkedin.voyager.messaging.MessagingMember")]
        public MessagingMemberStruct MessagingMember { get; set; }
    }

    //Estructura Pading
    public struct pagingStruct
    {
        public int total { get; set; }
        public int count { get; set; }
        public int start { get; set; }
        public List<object> links { get; set; }

    }
}
