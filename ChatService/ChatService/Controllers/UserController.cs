using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using ChatService.DTO;
using ChatService.Entities;

namespace ChatService.Controllers
{
    [RoutePrefix("users")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ControllerBase
    {
        [Route("")]
        [HttpGet]
        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            return (await Repo.GetAllUsers()).Select(this.ToUserDto);
        }

        [Route("{rowKey}")]
        [HttpGet]
        public async Task<UserDTO> GetUser(string rowKey)
        {
            var user = await Repo.GetUser(rowKey);
            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return this.ToUserDto(user);
        }

        [Route("{rowKey}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateUser(string rowKey, FormDataCollection formData)
        {
            var user = await this.GetUserAndThrowIfInvalid();
            var name = formData.Get("name");
            if (string.IsNullOrWhiteSpace(name))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Missing name" });
            if (name.Length > 20)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Name too long" });

            user.Name = name;
            await Repo.UpdateUser(user);

            return this.Ok();
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> AddUser()
        {
            var name = await this.GetName();
            var geoLocation = await this.GetGeoLocation();

            var entity = await Repo.CreateUser(name, geoLocation);

            return Created(Request.RequestUri + "/users/" + entity.RowKey, entity);
        }

        private UserDTO ToUserDto(User user)
        {
            return new UserDTO
            {
                Name = user.Name,
                UserId = user.RowKey,
                LastCity = user.LastCity,
                LastCountryCode = user.LastCountryCode
            };
        }
    }
}
