namespace DDDEastAnglia.Areas.Admin.Models.Eventbrite
{
    public sealed class EventbriteUrl
    {
        public string Url{get {return url;}}
        private readonly string url;

        public EventbriteUrl(string userKey, string appKey, long eventId)
        {
            this.url = string.Format("https://www.eventbrite.com/json/event_list_attendees?user_key={0}&app_key={1}&id={2}",
                                     userKey, appKey, eventId);
        }
    }
}
