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
    [RoutePrefix("messages")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MessageController : ControllerBase
    {
        [Route("")]
        [HttpGet]
        public async Task<IEnumerable<MessageDTO>> GetAllMessages()
        {
            return (await Repo.GetAllMessages()).Select(this.ToMessageDto);
        }

        [Route("{countryCode}")]
        [HttpGet]
        public async Task<IEnumerable<MessageDTO>> GetCountryMessages(string countryCode)
        {
            return (await Repo.GetCountryMessages(countryCode)).Select(this.ToMessageDto);
        }

        [Route("{countryCode}/{city}")]
        [HttpGet]
        public async Task<IEnumerable<MessageDTO>> GetCityMessages(string countryCode, string city)
        {
            return (await Repo.GetCityMessages(countryCode, city)).Select(this.ToMessageDto);
        }

        [Route("{countryCode}/{city}/{rowKey}")]
        [HttpGet]
        public async Task<MessageDTO> GetMessage(string countryCode, string city, string rowKey)
        {
            var message = await Repo.GetMessage(countryCode, city);
            if (message == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return this.ToMessageDto(message);
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> AddMessage(FormDataCollection formData)
        {
            var text = formData.Get("text");
            if (string.IsNullOrWhiteSpace(text))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Missing message" });

            var user = await this.GetUserAndThrowIfInvalid();
            var name = user.Name;
            var geoLocation = await this.GetGeoLocation();

            var entity = await Repo.CreateMessage(name, text, geoLocation);

            return Created(Request.RequestUri + "/messages/" + entity.PartitionKey + "/" + entity.City + "/" + entity.RowKey, entity);
        }

        private MessageDTO ToMessageDto(Message message)
        {
            return new MessageDTO
            {
                Author = message.Author,
                City = message.City,
                Country = message.Country,
                CreatedDate = message.CreatedDate,
                Text = message.Text
            };
        }
    }
}
