using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess.Sessionize
{
    internal sealed class SessionizeSessionRepository : ISessionRepository
    {
        private readonly IConferenceLoader conferenceLoader;
        private readonly SessionizeDataLoader sessionizeDataLoader;

        public SessionizeSessionRepository(IConferenceLoader conferenceLoader, SessionizeDataLoader sessionizeDataLoader)
        {
            this.conferenceLoader = conferenceLoader;
            this.sessionizeDataLoader = sessionizeDataLoader;
        }

        public IEnumerable<Session> GetAllSessions()
        {
            var conference = conferenceLoader.LoadConference();
            var sessionizeInfo = conference.SessionizeInfo;
            var data = sessionizeDataLoader.Load(sessionizeInfo.ConferenceId);

            return data.Sessions.Select(s => new Session()
            {
                ConferenceId = conference.Id,
                SessionId = s.Id,
                Title = s.Title,
                Abstract = s.Description,
                SpeakerUserName = s.Speakers.First().ToString()
            });
        }

        public Session Get(int id)
        {
            return GetAllSessions()
                .FirstOrDefault(s => s.SessionId == id);
        }

        public IEnumerable<Session> GetSessionsSubmittedBy(string speakerName)
        {
            return GetAllSessions()
                .Where(s => s.SpeakerUserName == speakerName);
        }

        public bool Exists(int id)
        {
            return GetAllSessions().Any(s => s.SessionId == id);
        }

        public Session AddSession(Session session)
        {
            throw new NotSupportedException();
        }

        public void UpdateSession(Session session)
        {
            throw new NotSupportedException();
        }

        public void DeleteSession(int id)
        {
            throw new NotSupportedException();
        }
    }
}
