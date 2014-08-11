namespace DDDEastAnglia.Controllers.JsonFeed
{
    public class Session
    {
        public int Id { get; set; }
        public int DDDEventId { get; set; }
        public int TrackId { get; set; }
        public int TimeSlotId { get; set; }
        public string Title { get; set; }
        public string Speaker { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
    }
}
