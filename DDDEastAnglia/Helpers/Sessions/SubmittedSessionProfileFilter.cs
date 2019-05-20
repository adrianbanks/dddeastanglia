using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Sessions
{
    public class SubmittedSessionProfileFilter : IUserProfileFilter
    {
        private readonly SessionRepositoryFactory sessionRepositoryFactory;

        public SubmittedSessionProfileFilter(SessionRepositoryFactory sessionRepositoryFactory)
        {
            if (sessionRepositoryFactory == null)
            {
                throw new ArgumentNullException("sessionRepositoryFactory");
            }

            this.sessionRepositoryFactory = sessionRepositoryFactory;
        }

        public IEnumerable<UserProfile> FilterProfiles(IEnumerable<UserProfile> profiles)
        {
            return profiles.Where(profile =>
            {
                var submittedSessions = sessionRepositoryFactory.Create().GetSessionsSubmittedBy(profile.UserName);
                return submittedSessions != null && submittedSessions.Any(s => s.SpeakerUserName == profile.UserName);
            });
        }
    }
}
