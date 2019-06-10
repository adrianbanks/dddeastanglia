using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Sessions
{
    public sealed class SelectedSessionsLoader : ISessionLoader
    {
        private readonly ISessionRepositoryFactory sessionRepositoryFactory;
        private readonly IEnumerable<int> sessionIds;

        public SelectedSessionsLoader(ISessionRepositoryFactory sessionRepositoryFactory, IEnumerable<int> selectedSessionIds)
        {
            if (sessionRepositoryFactory == null)
            {
                throw new ArgumentNullException("sessionRepositoryFactory");
            }

            if (selectedSessionIds == null)
            {
                throw new ArgumentNullException(nameof(selectedSessionIds));
            }

            this.sessionRepositoryFactory = sessionRepositoryFactory;
            this.sessionIds = selectedSessionIds;
        }

        public IEnumerable<Session> LoadSessions()
        {
            var sessions = sessionRepositoryFactory.Create().GetAllSessions().ToList();
            return Filter(sessions);
        }

        public IEnumerable<Session> LoadSessions(UserProfile profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            var sessions = sessionRepositoryFactory.Create().GetSessionsSubmittedBy(profile.UserName);
            return Filter(sessions);
        }

        private IEnumerable<Session> Filter(IEnumerable<Session> sessions)
        {
            return sessions.Where(s => sessionIds.Contains(s.SessionId)).ToList();
        }
    }
}
