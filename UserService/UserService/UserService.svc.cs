﻿using System;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UserService
{
    public class UserService : IUserService
    {
        public string GetName()
        {
            return "User" + Guid.NewGuid().ToString().Substring(0, 8);
        }

        public GeoLocation GetGeoLocation(string ipAddress)
        {
            return this.GetGeoLocationAsync(ipAddress).Result;
        }

        private async Task<GeoLocation> GetGeoLocationAsync(string ipAddress)
        {
            var client = new HttpClient();

            var response = await client.GetAsync("http://www.telize.com/geoip/" + ipAddress);
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new FaultException(content);

            return await JsonConvert.DeserializeObjectAsync<GeoLocation>(content);
        }
    }
}
