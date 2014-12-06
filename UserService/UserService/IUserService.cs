using System.Runtime.Serialization;
using System.ServiceModel;
using Newtonsoft.Json;

namespace UserService
{
    [ServiceContract]
    public interface IUserService
    {

        [OperationContract]
        string GetName();

        [OperationContract]
        GeoLocation GetGeoLocation(string ipAddress);
    }

    [DataContract]
    public class GeoLocation
    {
        [JsonProperty("country_code")]
        [DataMember]
        public string CountryCode { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public float Latitude { get; set; }

        [DataMember]
        public float Longitude { get; set; }
    }
}
