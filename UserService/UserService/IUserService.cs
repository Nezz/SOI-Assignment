using System.Runtime.Serialization;
using System.ServiceModel;

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

        [DataMember]
        public string CountryCode { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public int Latitude { get; set; }

        [DataMember]
        public int Longitude { get; set; }
    }
}
