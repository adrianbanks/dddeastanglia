using System;
using System.Web.Http;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia.Controllers.JsonFeed
{
    public class EventDetailsController : ApiController
    {
        public const int DDDEventId = 2; //TODO: where can this come from?

        private readonly IConferenceLoader conferenceLoader;
        private readonly ICalendarItemRepository calendarItemRepository;

        public EventDetailsController(ICalendarItemRepository calendarItemRepository, IConferenceLoader conferenceLoader)
        {
            if (conferenceLoader == null)
            {
                throw new ArgumentNullException("conferenceLoader");
            }
            
            if (calendarItemRepository == null)
            {
                throw new ArgumentNullException("calendarItemRepository");
            }
            
            this.calendarItemRepository = calendarItemRepository;
            this.conferenceLoader = conferenceLoader;
        }

        public DDDEvent GetEventDetails()
        {
            var conference = conferenceLoader.LoadConference();
            var conferenceDate = calendarItemRepository.GetFromType(CalendarEntryType.Conference);
            var dddEvent = new DDDEvent
            {
                Id = DDDEventId,
                Version = 5,    //TODO: why is this 5?
                IsActive = true,
                Name = conference.Name,
                City = "Cambridge",  //TODO: where can we get this from?
                Date = conferenceDate.StartDate.DateTime
            };
            return dddEvent;
        }
    }
}
