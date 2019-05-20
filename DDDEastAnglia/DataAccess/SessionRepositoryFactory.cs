using DDDEastAnglia.DataAccess.Sessionize;
using DDDEastAnglia.DataAccess.SimpleData;

namespace DDDEastAnglia.DataAccess
{
    public sealed class SessionRepositoryFactory
    {
        private readonly IConferenceLoader conferenceLoader;
        private readonly SessionizeDataLoader sessionizeDataLoader;

        public SessionRepositoryFactory(IConferenceLoader conferenceLoader, SessionizeDataLoader sessionizeDataLoader)
        {
            this.conferenceLoader = conferenceLoader;
            this.sessionizeDataLoader = sessionizeDataLoader;
        }

        public ISessionRepository Create()
        {
            var conference = conferenceLoader.LoadConference();

            if (conference.IsUsingSessionize())
            {
                return new SessionizeSessionRepository(conferenceLoader, sessionizeDataLoader);
            }

            return new SessionRepository();
        }
    }
}
