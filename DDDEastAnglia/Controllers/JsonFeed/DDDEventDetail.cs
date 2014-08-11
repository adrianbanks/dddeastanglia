using System.Collections.Generic;

namespace DDDEastAnglia.Controllers.JsonFeed
{
    public class DDDEventDetail
    {
        public int DDDEventId { get; set; }
        public IList<Track> Tracks { get; set; }
        public IList<TimeSlot> TimeSlots { get; set; }
        public IList<Session> Sessions { get; set; }
    }
}
