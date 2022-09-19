using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace Linkedin.Net.Estructuras
{
    public struct pictureStruct
    {
        [JsonProperty(PropertyName = "com.linkedin.common.VectorImage")] 
        public linkedinVectorImageStruct linkedinVectorImage { get; set; }
    }

    public struct linkedinVectorImageStruct
    {
        List<artifactsStruct> artifacts { get; set; }
        public string rootUrl { get; set; }
    }

    public struct artifactsStruct
    {
        public int width { get; set; }
        public string fileIdentifyingUrlPathSegment { get; set; }
        public string expiresAt { get; set; }
        public int height { get; set; }
    }
}
