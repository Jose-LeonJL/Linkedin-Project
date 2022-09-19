using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Linkedin.Net.Estructuras
{
    public struct getProfileStruct
    {
        public string lastName { get; set; }
        public string locationName { get; set; }
        public bool student { get; set; }
        public string geoCountryName { get; set; }
        public string geoCountryUrn { get; set; }
        public string firstName { get; set; }
        public string entityUrn { get; set; }
        public geoLocationStruct geoLocation { get; set;}
        public string geoLocationName { get; set; }
        public locationStruct location { get; set; }
        public bool elt { get; set; }
        public miniProfilesStruct miniProfile { get; set; }
        public miniProfilesStruct profile { get; set; }
        public string headline { get; set; }


        public string sumary { get; set; }
        public string industryName { get; set; }
        public bool geoLocationBackfilled { get; set; }
        public string displayPictureUrl { get; set; }
     
        public string profile_id { get; set; }
        public string profile_urn { get; set; }
        public List<experienceStruct> experience { get; set; }
        public List<educationStruct> education { get; set; }
        public List<languagesStruct> languages { get; set; }
        public List<publicationsStruct> publications { get; set; }
        public List<certificationsStruct> certifications { get; set; }
        public List<volunteerStruct> volunteer { get; set; }
        public List<honorsStruct> honors { get; set; }
        public List<projectsStruct> projects { get; set; }
    }

    public struct miniProfilesStruct
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string dashEntityUrn { get; set; }
        public string occupation { get; set; }
        public string objectUrn { get; set; }
        public string entityUrn { get; set; }
        public string publicIdentifier { get; set; }
        public PictureProfileStruct picture { get; set; }
        public string trackingId { get; set; }
    }
    public struct PictureProfileStruct
    {
        [JsonPropertyName("com.linkedin.common.VectorImage")]
        [JsonProperty("com.linkedin.common.VectorImage")]
        public VectorImageStruct ComLinkedinCommonVectorImage { get; set; }
    }
    public struct VectorImageStruct
    {
        public List<ArtifactStruct> artifacts { get; set; }
        public string rootUrl { get; set; }
    }
    public struct ArtifactStruct
    {
        public int width { get; set; }
        public string fileIdentifyingUrlPathSegment { get; set; }
        public object expiresAt { get; set; }
        public int height { get; set; }
    }

    public struct geoLocationStruct
    {
        public string geoUrn { get; set; }
    }

    public struct locationStruct
    {
        public basicLocationStruct basicLocation { get; set; }
    }

    public struct basicLocationStruct
    {
        public string countryCode { get; set; }
    }

    //experience
    public struct experienceStruct
    {
        public string locationName { get; set; }
        public string entityUrn { get; set; }
        public string geoLocationName { get; set; }
        public string geoUrn { get; set; }
        public string companyName { get; set; }
        public timePeriodStruct timePeriod { get; set; }
        public companyStruct company { get; set; }
        public string title { get; set; }
        public string region { get; set; }
        public string companyUrn { get; set; }
        public string companyLogoUrn { get; set; }
    }

    public struct timePeriodStruct
    {
        public startDateStruct endDate { get; set; }
        public startDateStruct startDate { get; set; }
    }

    public struct startDateStruct
    {
        public int month { get; set; }
        public int year { get; set; }
    }

    public struct companyStruct
    {
        public employeeCountRangeStruct employeeCountRange { get; set; }
        public List<string> indutries { get; set; }
    }

    public struct employeeCountRangeStruct
    {
        public int start { get; set; }
        public int end { get; set; }
    }

    //education
    public struct educationStruct
    {
        public string entityUrn { get; set; }
        public schoolStruct school { get; set; }
        public string grade { get; set; }
        public timePeriodStructE timeperiod { get; set; }
        public string degreeName { get; set; }
        public string schoolName { get; set; }
        public string flieldOfStudy { get; set; }
        public string degreeUrn { get; set; }
        public string schoolUrn { get; set; }
    }
    public struct schoolStruct
    {
        public string objectUrn { get; set; }
        public string entityUrn { get; set; }
        public bool active { get; set; } 
        public string schoolName { get; set; }
        public string trakingId { get; set; }
        public string logoUrl { get; set; }
    }

    public struct timePeriodStructE
    {
        public endDateStructE endD { get; set; }
        public startDateStructE startDate { get; set; }
    }

    public struct endDateStructE
    {
        public int year { get; set; }
    }
    public struct startDateStructE
    {
        public int year { get; set; }
    }

    //Languajes
    public struct languagesStruct
    {
        public string name { get; set; }
        public string proficiency { get; set; }
    }

    //publications
    public struct publicationsStruct
    {
        //encontrar un perfil que tenga publicaciones para ver los campos
    }

    //certifications
    public struct certificationsStruct
    {
        public string authority { get; set; }
        public string name { get; set; }
        public timePeriodStruct timePeriod { get; set; }
        public companyCStruct company { get; set; }
        public string companyUrn { get; set; }
    }

    public struct companyCStruct
    {
        public string objectUrn { get; set; }
        public string entityUrn { get; set; }
        public string name { get; set; }
        public bool showcase { get; set; }
        public bool active { get; set; }
        public logoStruct logo { get; set; }
        public string universalName { get; set; }
        public string dashCompanyUrn { get; set; }
        public string trackingId { get; set; }
    }
    public struct logoStruct
    {
        public comlinkedincommonVectorImageStruct com_linkedin_common_VectorImage { get; set;}
    }
    public struct comlinkedincommonVectorImageStruct
    {
        public List<artifactsStruct> artifacts { get; set; }
        public string rootUrl { get; set; }
    }

    //volunteer
    public struct volunteerStruct
    {
        //encontrar un perfil que tenga estos datos
    }

    //honors
    public struct honorsStruct
    {
        //encontrar un perfil que tenga estos datos campos
    }

    //projects
    public struct projectsStruct
    {
        public string ocupation { get; set;}
        public List<membersStruct> members { get; set; }
        public timePeriodStruct timePeriod { get; set; }
        public string title { get; set; }
    }

    public struct membersStruct
    {
        public memberStruct member { get; set; }
        public string entityUrn { get; set; }
        public string profileUrn { get; set; }
    }

    public struct memberStruct
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string dashEntityUrn { get; set; }
        public string ocupation { get; set; }
        public string objectUrn { get; set; }
        public backgroundImageStructs backgroundImage { get; set; }
        public string publicIdentifier { get; set; }
        public pictureStruct picture { get; set; }
        public string trackingId { get; set; }
    }
    public struct backgroundImageStructs
    {
        public comlinkedincommonVectorImageStruct com_linkedin_common_VectorImage { get; set; }
    }
    /*public struct pictureStruct
    {
        public comlinkedincommonVectorImageStruct com_linkedin_common_VectorImage { get; set; }
    }*/

}
