using System;

namespace DDDEastAnglia.Controllers.JsonFeed
{
    public class TimeSlot
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Info { get; set; }
    }
}
