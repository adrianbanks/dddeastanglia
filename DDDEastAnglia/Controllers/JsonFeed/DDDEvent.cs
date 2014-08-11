using System;

namespace DDDEastAnglia.Controllers.JsonFeed
{
    public class DDDEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
        public int Version { get; set; }
        public bool IsActive { get; set; }
    }
}
