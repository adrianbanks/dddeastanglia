using System;
using System.Web.Http;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia.Controllers.JsonFeed
{
    public class AgendaController : ApiController
    {
        private readonly ISessionRepository sessionRepository;
        private readonly IUserProfileRepository userProfileRepository;
        private readonly ICalendarItemRepository calendarItemRepository;

        public AgendaController(ISessionRepository sessionRepository, IUserProfileRepository userProfileRepository, ICalendarItemRepository calendarItemRepository)
        {
            if (sessionRepository == null)
            {
                throw new ArgumentNullException("sessionRepository");
            }

            if (userProfileRepository == null)
            {
                throw new ArgumentNullException("userProfileRepository");
            }
            
            if (calendarItemRepository == null)
            {
                throw new ArgumentNullException("calendarItemRepository");
            }
            
            this.sessionRepository = sessionRepository;
            this.userProfileRepository = userProfileRepository;
            this.calendarItemRepository = calendarItemRepository;
        }

        public DDDEventDetail GetAgenda()
        {
            var tracks = CreateTracks();
            var timeSlots = CreateTimeSlots();
            var sessions = CreateSessions();

            var dddEventDetail = new DDDEventDetail
            {
                DDDEventId = EventDetailsController.DDDEventId,
                Tracks = tracks,
                TimeSlots = timeSlots,
                Sessions = sessions
            };
            return dddEventDetail;
        }

        private Track[] CreateTracks()
        {
            return new[]
            {
                new Track {Id = 1, Name = "Track 1", RoomName = "Room 1"},
                new Track {Id = 2, Name = "Track 2", RoomName = "Room 2"},
                new Track {Id = 3, Name = "Track 3", RoomName = "Room 3"},
                new Track {Id = 7, Name = "Track 7", RoomName = "Room 7"},
                new Track {Id = 8, Name = "Track 8", RoomName = "Room 8"}
            };
        }

        private TimeSlot[] CreateTimeSlots()
        {
            return new[]
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
                    To = ToDateTime(18, 00),
                    Info = "Drinks Reception (sponsored by Red Gate)"
                },
                new TimeSlot
                {
                    Id = 15,
                    From = ToDateTime(18, 00),
                    To = ToDateTime(18, 00),
                    Info = "Close"
                }
            };
        }

        private Session[] CreateSessions()
        {
            return new[]
            {
                CreateSession(1109, 3, 1, 3),
                CreateSession(104, 2, 2, 3),
                CreateSession(4124, 1, 3, 3),
                CreateSession(2117, 4, 4, 3),
                CreateSession(3132, 5, 5, 3),

                CreateSession(2115, 8, 1, 5),
                CreateSession(2107, 7, 2, 5),
                CreateSession(1108, 6, 3, 5),
                CreateSession(102, 9, 4, 5),
                CreateSession(1106, 10, 5, 5),

                CreateSession(3140, 13, 1, 7),
                CreateSession(1107, 12, 2, 7),
                CreateSession(3139, 11, 3, 7),
                CreateSession(1114, 14, 4, 7),
                CreateSession(2113, 15, 5, 7),

                CreateSession(2109, 18, 1, 9),
                CreateSession(107, 17, 2, 9),
                CreateSession(3125, 16, 3, 9),
                CreateSession(3119, 19, 4, 9),
                CreateSession(3127, 20, 5, 9),

                CreateSession(108, 23, 1, 11),
                CreateSession(2111, 22, 2, 11),
                CreateSession(4133, 21, 3, 11),
                CreateSession(4135, 24, 4, 11),
                CreateSession(3129, 25, 5, 11)
            };
        }

        private Session CreateSession(int sessionId, int id, int trackId, int timeSlotId)
        {
            var session = sessionRepository.Get(sessionId);
            var speaker = userProfileRepository.GetUserProfileByUserName(session.SpeakerUserName);

            return new Session
            {
                DDDEventId = 1,
                Id = id,
                TrackId = trackId,
                TimeSlotId = timeSlotId,
                Title = session.Title,
                Speaker = speaker.Name,
                ShortDescription = session.Abstract,
                FullDescription = session.Abstract
            };
        }

        private DateTime ToDateTime(int hour, int minute)
        {
            var conferenceDate = calendarItemRepository.GetFromType(CalendarEntryType.Conference).StartDate.Date;
            return new DateTime(conferenceDate.Year, conferenceDate.Month, conferenceDate.Day, hour, minute, 00);
        }
    }
}
