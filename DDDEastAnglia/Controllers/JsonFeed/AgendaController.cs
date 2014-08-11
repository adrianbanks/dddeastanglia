using System;
using System.Web.Http;

namespace DDDEastAnglia.Controllers.JsonFeed
{
    public class AgendaController : ApiController
    {
        public DDDEventDetail GetAgenda()
        {
            var tracks = new[]
            {
                new Track {Id = 1, Name = "Track 1", RoomName = "Room 1"},
                new Track {Id = 2, Name = "Track 2", RoomName = "Room 2"},
                new Track {Id = 3, Name = "Track 3", RoomName = "Room 3"},
                new Track {Id = 4, Name = "Track 4", RoomName = "Room 4"},
                new Track {Id = 5, Name = "Track 5", RoomName = "Room 5"}
            };

            var timeSlots = new[]
            {
                new TimeSlot
                {
                    Id = 1,
                    From = ToDateTime(08, 30),
                    To = ToDateTime(09, 00),
                    Info = "Registration, Refreshments (sponsored by Compare the Market)"
                },
                new TimeSlot
                {
                    Id = 2,
                    From = ToDateTime(09, 00),
                    To = ToDateTime(09, 30),
                    Info = "Welcome & Housekeeping"
                },
                new TimeSlot
                {
                    Id = 3,
                    From = ToDateTime(09, 15),
                    To = ToDateTime(10, 15),
                    Info = null
                },
                new TimeSlot
                {
                    Id = 4,
                    From = ToDateTime(10, 15),
                    To = ToDateTime(10, 35),
                    Info = "Break"
                },
                new TimeSlot
                {
                    Id = 5,
                    From = ToDateTime(10, 35),
                    To = ToDateTime(11, 35),
                    Info = null
                },
                new TimeSlot
                {
                    Id = 6,
                    From = ToDateTime(11, 35),
                    To = ToDateTime(11, 55),
                    Info = "Break"
                },
                new TimeSlot
                {
                    Id = 7,
                    From = ToDateTime(11, 55),
                    To = ToDateTime(12, 55),
                    Info = null
                },
                new TimeSlot
                {
                    Id = 8,
                    From = ToDateTime(12, 55),
                    To = ToDateTime(14, 00),
                    Info = "Lunch, Grok Talks (sponsored by Huddle)"
                },
                new TimeSlot
                {
                    Id = 9,
                    From = ToDateTime(14, 00),
                    To = ToDateTime(15, 00),
                    Info = null
                },
                new TimeSlot
                {
                    Id = 10,
                    From = ToDateTime(15, 00),
                    To = ToDateTime(15, 20),
                    Info = "Break"
                },
                new TimeSlot
                {
                    Id = 11,
                    From = ToDateTime(15, 20),
                    To = ToDateTime(16, 20),
                    Info = null
                },
                new TimeSlot
                {
                    Id = 12,
                    From = ToDateTime(16, 20),
                    To = ToDateTime(16, 30),
                    Info = "Break"
                },
                new TimeSlot
                {
                    Id = 13,
                    From = ToDateTime(16, 30),
                    To = ToDateTime(17, 00),
                    Info = "Wrap-up"
                },
                new TimeSlot
                {
                    Id = 14,
                    From = ToDateTime(17, 00),
                    To = ToDateTime(17, 00),
                    Info = "Close"
                }
            };

            var sessions = new[]
            {
                new Session
                {
                    DDDEventId = 1,
                    Id = 1,
                    TrackId = 1,
                    TimeSlotId = 3,
                    Title = "OWIN, Katana and ASP.NET vNext: eliminating the pain of IIS ",
                    Speaker = "David Simner",
                    ShortDescription = "",
                    FullDescription = ""
                },
                new Session
                {
                    DDDEventId = 1,
                    Id = 2,
                    TrackId = 2,
                    TimeSlotId = 3,
                    Title = "Github Automation ",
                    Speaker = "Forbes Lindsay",
                    ShortDescription = "",
                    FullDescription = ""
                },
                new Session
                {
                    DDDEventId = 1,
                    Id = 3,
                    TrackId = 3,
                    TimeSlotId = 3,
                    Title =
                        "An Actor's Life for Me – An introduction to the TPL Dataflow Library and asynchronous programming blocks ",
                    Speaker = "Liam Westley",
                    ShortDescription = "",
                    FullDescription = ""
                },
                new Session
                {
                    DDDEventId = 1,
                    Id = 4,
                    TrackId = 4,
                    TimeSlotId = 3,
                    Title = "A Unit Testing Swiss Army Knife ",
                    Speaker = "Adam Kosinski",
                    ShortDescription = "",
                    FullDescription = ""
                },
                new Session
                {
                    DDDEventId = 1,
                    Id = 5,
                    TrackId = 5,
                    TimeSlotId = 3,
                    Title = "Taking your craft seriously with F# ",
                    Speaker = "Tomas Petricek",
                    ShortDescription = "",
                    FullDescription = ""
                },

//                new Session { DDDEventId = 1, Id = 6, TrackId = 1, TimeSlotId = 5, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//                new Session { DDDEventId = 1, Id = 7, TrackId = 2, TimeSlotId = 5, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//                new Session { DDDEventId = 1, Id = 8, TrackId = 3, TimeSlotId = 5, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//                new Session { DDDEventId = 1, Id = 9, TrackId = 4, TimeSlotId = 5, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//                new Session { DDDEventId = 1, Id = 10, TrackId = 5, TimeSlotId = 5, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//
//                new Session { DDDEventId = 1, Id = 11, TrackId = 1, TimeSlotId = 7, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//                new Session { DDDEventId = 1, Id = 12, TrackId = 2, TimeSlotId = 7, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//                new Session { DDDEventId = 1, Id = 13, TrackId = 3, TimeSlotId = 7, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//                new Session { DDDEventId = 1, Id = 14, TrackId = 4, TimeSlotId = 7, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//                new Session { DDDEventId = 1, Id = 15, TrackId = 5, TimeSlotId = 7, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//
//                new Session { DDDEventId = 1, Id = 16, TrackId = 1, TimeSlotId = 9, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//                new Session { DDDEventId = 1, Id = 17, TrackId = 2, TimeSlotId = 9, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//                new Session { DDDEventId = 1, Id = 18, TrackId = 3, TimeSlotId = 9, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//                new Session { DDDEventId = 1, Id = 19, TrackId = 4, TimeSlotId = 9, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//                new Session { DDDEventId = 1, Id = 20, TrackId = 5, TimeSlotId = 9, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//
//                new Session { DDDEventId = 1, Id = 21, TrackId = 1, TimeSlotId = 1, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//                new Session { DDDEventId = 1, Id = 22, TrackId = 2, TimeSlotId = 1, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//                new Session { DDDEventId = 1, Id = 23, TrackId = 3, TimeSlotId = 1, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//                new Session { DDDEventId = 1, Id = 24, TrackId = 4, TimeSlotId = 1, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
//                new Session { DDDEventId = 1, Id = 25, TrackId = 5, TimeSlotId = 1, Title = "", Speaker = "" , ShortDescription = "", FullDescription = "" },
            };

            var dddEventDetail = new DDDEventDetail
            {
                DDDEventId = 1,
                Tracks = tracks,
                TimeSlots = timeSlots,
                Sessions = sessions
            };
            return dddEventDetail;
        }

        private DateTime ToDateTime(int hour, int minute)
        {
            return new DateTime(2014, 9, 13, hour, minute, 00);
        }
    }
}
