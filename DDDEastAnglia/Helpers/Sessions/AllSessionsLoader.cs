using System;
using System.Collections.Generic;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Sessions
{
    public class AllSessionsLoader : ISessionLoader
    {
        private readonly SessionRepositoryFactory sessionRepositoryFactory;

        public AllSessionsLoader(SessionRepositoryFactory sessionRepositoryFactory)
        {
            if (sessionRepositoryFactory == null)
            {
                throw new ArgumentNullException("sessionRepositoryFactory");
            }

            this.sessionRepositoryFactory = sessionRepositoryFactory;
        }

        public IEnumerable<Session> LoadSessions()
        {
            return sessionRepositoryFactory.Create().GetAllSessions();
        }

        public IEnumerable<Session> LoadSessions(UserProfile profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            return sessionRepositoryFactory.Create().GetSessionsSubmittedBy(profile.UserName);
        }
    }
}
