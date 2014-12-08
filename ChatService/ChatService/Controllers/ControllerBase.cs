using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ChatService.Entities;
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

        protected async Task<User> GetUserAndThrowIfInvalid()
        {
            var user = await GetUser();
            if (user != null)
                return user;
            
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Invalid user" });
        }

        protected async Task<User> GetUser()
        {
            var userId = GetUserId();
            if (userId == null)
                return null;

            return await Repo.GetUser(userId.ToString());
        }
        protected Guid? GetUserId()
        {
            Guid result;

            if (Guid.TryParse(GetFirstHeaderValue("userId"), out result))
                return result;

            return null;
        }

        protected string GetFirstHeaderValue(string key)
        {
            IEnumerable<string> values;

            if (!ControllerContext.Request.Headers.TryGetValues(key, out values))
                return null;

            return values.FirstOrDefault();
        }
    }
}