using System;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;

namespace DDDEastAnglia.Helpers.Sessions
{
    public interface ISessionLoaderFactory
    {
        ISessionLoader Create(IConference conference);
    }

    public sealed class SessionLoaderFactory : ISessionLoaderFactory
    {
        private readonly SessionRepositoryFactory sessionRepositoryFactory;

        public SessionLoaderFactory(SessionRepositoryFactory sessionRepositoryFactory)
        {
            if (sessionRepositoryFactory == null)
            {
                throw new ArgumentNullException(nameof(sessionRepositoryFactory));
            }

            this.sessionRepositoryFactory = sessionRepositoryFactory;
        }

        public ISessionLoader Create(IConference conference)
        {
            ISessionLoader sessionLoader;

            if (conference.CanPublishAgenda())
            {
                sessionLoader = new SelectedSessionsLoader(sessionRepositoryFactory, SelectedSessions.SessionIds);
            }
            else
            {
                sessionLoader = new AllSessionsLoader(sessionRepositoryFactory);
            }

            return sessionLoader;
        }
    }
}
