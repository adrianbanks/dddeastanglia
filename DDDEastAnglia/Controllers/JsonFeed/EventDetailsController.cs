using System;
using System.Web.Http;

namespace DDDEastAnglia.Controllers.JsonFeed
{
    public class EventDetailsController : ApiController
    {
        public DDDEvent GetEventDetails()
        {
            var dddEvent = new DDDEvent
            {
                Id = 1,
                Version = 5,
                IsActive = true,
                Name = "DDD East Anglia 2014",
                City = "Cambridge",
                Date = new DateTime(2014, 9, 13, 08, 30, 00)
            };
            return dddEvent;
        }
    }
}
