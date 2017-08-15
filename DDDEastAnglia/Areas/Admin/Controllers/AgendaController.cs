using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    public class AgendaController : Controller
    {
        private readonly ISessionRepository sessionRepository;
        private readonly ISpeakerRepository speakerRepository;

        public AgendaController(ISessionRepository sessionRepository, ISpeakerRepository speakerRepository)
        {
            if (sessionRepository == null)
            {
                throw new ArgumentNullException(nameof(sessionRepository));
            }

            if (speakerRepository == null)
            {
                throw new ArgumentNullException(nameof(speakerRepository));
            }

            this.sessionRepository = sessionRepository;
            this.speakerRepository = speakerRepository;
        }

        public ActionResult Index()
        {
            var sessions = sessionRepository.GetAllSessions();
            var speakers = speakerRepository.GetAllSpeakerProfiles().ToDictionary(s => s.UserName, s => s);
            var sessionsWithSpeakers = PairSpeakersWithSessions(sessions, speakers).OrderBy(s => s.Speaker.Name);

            return View(sessionsWithSpeakers);
        }

        private IEnumerable<SessionWithSpeaker> PairSpeakersWithSessions(IEnumerable<Session> sessions, Dictionary<string, SpeakerProfile> speakers)
        {
            foreach (var session in sessions)
            {
                var speaker = speakers[session.SpeakerUserName];
                yield return new SessionWithSpeaker()
                {
                    Session = session,
                    Speaker = speaker
                };
            }
        }
    }

    public class SessionWithSpeaker
    {
        public Session Session { get; set; }
        public SpeakerProfile Speaker { get; set; }
    }
}
