using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ChatService.Repository;
using ChatService.ServiceReference1;

namespace ChatService.Controllers
{
    public class ControllerBase : ApiController
    {
        private static TableStorageRepo repo;

        protected static TableStorageRepo Repo
        {
            get
            {
                if (repo == null)
                    repo = new TableStorageRepo();

                return repo;
            }
        }

        private static UserServiceClient userServiceClient;

        protected static UserServiceClient UserServiceClient
        {
            get
            {
                if (userServiceClient == null)
                    userServiceClient = new UserServiceClient();

                return userServiceClient;
            }
        }

        protected async Task<string> GetName()
        {
            return await UserServiceClient.GetNameAsync();
        }

        protected async Task<GeoLocation> GetGeoLocation()
        {
            return await UserServiceClient.GetGeoLocationAsync(HttpContext.Current.Request.UserHostAddress);
        }
    }
}